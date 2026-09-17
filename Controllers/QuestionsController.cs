using InterviewApp.Data;
using InterviewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace InterviewApp.Controllers;

public class QuestionsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

   public QuestionsController(
    ApplicationDbContext context,
    UserManager<IdentityUser> userManager)
{
    _context = context;
    _userManager = userManager;
}

    // GET: /Questions
    public async Task<IActionResult> Index()
    {
        var questions = await _context.Questions
            .Include(q => q.Category)
            .ToListAsync();

        return View(questions);
    }

    // GET: /Questions/Details/1
    public async Task<IActionResult> Details(int? id)
    {   
        if (id == null)
     {
        return NotFound();
     }
     

    var question = await _context.Questions
    .Include(q => q.Category)
    .Include(q => q.Attempts)
    .FirstOrDefaultAsync(q => q.Id == id);

    if (question == null)
     {
        return NotFound();
      }

var userId = _userManager.GetUserId(User);

var myAttempts = await _context.Attempts
    .Where(a => a.QuestionId == id && a.UserId == userId)
    .OrderByDescending(a => a.AttemptedAt)
    .ToListAsync();

ViewBag.MyAttempts = myAttempts;
    return View(question);
    }

    // GET: /Questions/Create
    public IActionResult Create()
{
    ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
    return View();
}

    // POST: /Questions/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Id,Title,Content,CategoryId,Difficulty")] Question question)
{
    if (ModelState.IsValid)
    {
        _context.Add(question);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", question.CategoryId);
    return View(question);
}


    // GET: /Questions/Edit/1
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var question = await _context.Questions.FindAsync(id);

    if (question == null)
    {
        return NotFound();
    }
    ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", question.CategoryId);
    return View(question);
}

// POST: /Questions/Edit/1
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(
    int id,
    [Bind("Id,Title,Content,CategoryId,Difficulty")] Question question)
{
    if (id != question.Id)
    {
        return NotFound();
    }

    if (!ModelState.IsValid)
    {
        ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", question.CategoryId);
        return View(question);
    }

    _context.Update(question);
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}

// DELETE: /Questions/Delete/1
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var question = await _context.Questions.FindAsync(id);

    if (question == null)
    {
        return NotFound();
    }

    return View(question);
}

// POST: /Questions/Delete/1
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var question = await _context.Questions.FindAsync(id); 

    if (question != null)
    {
        _context.Questions.Remove(question);
         await _context.SaveChangesAsync();
    }
        return RedirectToAction(nameof(Index));

    }
    
}
