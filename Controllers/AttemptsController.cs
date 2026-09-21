using InterviewApp.Data;
using InterviewApp.Models;
using InterviewApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewApp.Controllers;

[Authorize]
public class AttemptsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly GeminiScoringService _scoringService;

    public AttemptsController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        GeminiScoringService scoringService)
    {
        _context = context;
        _userManager = userManager;
        _scoringService = scoringService;
    }

    // GET: /Attempts  (我的练习记录)
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);

        var myAttempts = await _context.Attempts
            .Include(a => a.Question)
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.AttemptedAt)
            .ToListAsync();

        return View(myAttempts);
    }

    [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(int questionId, string answerText)
{
    var userId = _userManager.GetUserId(User);

    var attempt = new Attempt
    {
        QuestionId = questionId,
        UserId = userId!,
        AnswerText = answerText
    };

    var question = await _context.Questions
        .Include(q => q.Category)
        .FirstOrDefaultAsync(q => q.Id == questionId);

    if (question != null)
    {
        var result = await _scoringService.ScoreAsync(question, answerText);
        if (result != null)
        {
            attempt.Score = result.Value.Score;
            attempt.Feedback = result.Value.Feedback;
        }
    }

    _context.Attempts.Add(attempt);
    await _context.SaveChangesAsync();

    return RedirectToAction(
        "Details",
        "Questions",
        new { id = questionId });
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Delete(int id)
{
    var userId = _userManager.GetUserId(User);
    var attempt = await _context.Attempts.FindAsync(id);

    if (attempt == null || attempt.UserId != userId)
    {
        return NotFound();
    }

    var questionId = attempt.QuestionId;
    _context.Attempts.Remove(attempt);
    await _context.SaveChangesAsync();

    return RedirectToAction("Details", "Questions", new { id = questionId });
}
}
