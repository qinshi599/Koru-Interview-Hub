using InterviewApp.Data;
using InterviewApp.Models;
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

    public AttemptsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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

    _context.Attempts.Add(attempt);
    await _context.SaveChangesAsync();

    return RedirectToAction(
        "Details",
        "Questions",
        new { id = questionId });
}
}
