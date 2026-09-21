using System.Text.Json;
using System.Text.Json.Serialization;
using InterviewApp.Models;

namespace InterviewApp.Services;

public class GeminiScoringService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly ILogger<GeminiScoringService> _logger;

    public GeminiScoringService(HttpClient http, IConfiguration config, ILogger<GeminiScoringService> logger)
    {
        _http = http;
        _config = config;
        _logger = logger;
    }

    public async Task<(int Score, string Feedback)?> ScoreAsync(Question question, string answerText)
    {
        var apiKey = _config["Gemini:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogWarning("Gemini:ApiKey is not configured; skipping AI scoring.");
            return null;
        }

        var model = _config["Gemini:Model"] ?? "gemini-flash-lite-latest";
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = BuildPrompt(question, answerText) } } }
            },
            generationConfig = new { responseMimeType = "application/json" }
        };

        try
        {
            using var response = await _http.PostAsJsonAsync(url, requestBody);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<GenerateContentResponse>();
            var jsonText = payload?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
            if (string.IsNullOrWhiteSpace(jsonText))
            {
                _logger.LogWarning("Gemini response had no text content.");
                return null;
            }

            var result = JsonSerializer.Deserialize<ScoreResult>(jsonText, JsonOptions);
            if (result is null)
            {
                return null;
            }

            return (Math.Clamp(result.Score, 0, 100), result.Feedback ?? string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Gemini AI scoring call failed.");
            return null;
        }
    }

    private static string BuildPrompt(Question question, string answerText)
    {
        var isBehavioral = question.Category?.Name == "Behavioral";
        var rubric = isBehavioral
            ? "assess STAR structure completeness (Situation, Task, Action, Result), specificity of details, and whether the result is quantified"
            : "assess technical accuracy, depth of explanation, clarity of communication, and structure of the answer";

        return $"""""
            You are an experienced technical interviewer conducting a {question.Category?.Name} interview at the {question.Difficulty} level.

            Question: "{question.Title}"
            Details: "{question.Content}"

            Candidate's answer:
            """
            {answerText}
            """

            Evaluate the answer: {rubric}.

            Give a composite score from 0 to 100 reflecting overall interview readiness for this specific answer.

            Then write feedback that:
            - Quotes at least one specific phrase from the candidate's answer
            - Points out concretely what is missing or weak (not generic advice like "be more specific")
            - Suggests a specific rewrite or addition for at least one weak part

            Respond only with a JSON object with exactly two fields: "score" (an integer 0-100) and "feedback" (a string).
            """"";
    }

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private record ScoreResult(int Score, string? Feedback);

    private record GenerateContentResponse([property: JsonPropertyName("candidates")] List<Candidate>? Candidates);
    private record Candidate([property: JsonPropertyName("content")] Content? Content);
    private record Content([property: JsonPropertyName("parts")] List<Part>? Parts);
    private record Part([property: JsonPropertyName("text")] string? Text);
}
