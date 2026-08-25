using JokesWebApp.Data;
using JokesWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JokesWebApp.Controllers;

public class QuestionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public QuestionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Questions
    public async Task<IActionResult> Index()
    {
        var questions = await _context.Questions.ToListAsync();

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
        .FirstOrDefaultAsync(q => q.Id == id);

    if (question == null)
     {
        return NotFound();
      }

    return View(question);
    }

    // GET: /Questions/Create
    public IActionResult Create()
{
    return View();
}

    // POST: /Questions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Content,Category,Difficulty")] Question question)
    {
        if (ModelState.IsValid)
        {
            _context.Add(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
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

    return View(question);
}

// POST: /Questions/Edit/1
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(
    int id,
    [Bind("Id,Title,Content,Category,Difficulty")] Question question)
{
    if (id != question.Id)
    {
        return NotFound();
    }

    if (!ModelState.IsValid)
    {
        return View(question);
    }

    _context.Update(question);
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}
}