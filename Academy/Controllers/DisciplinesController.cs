using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Models;

public class DisciplinesController : Controller
{
    private readonly AcademyContext _context;

    public DisciplinesController(AcademyContext context)
    {
        _context = context;
    }

    // GET: Disciplines
    public async Task<IActionResult> Index(string sortOrder)
    {
        ViewData["NameSortParam"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
        ViewData["LessonsSortParam"] = sortOrder == "lessons_desc" ? "lessons_asc" : "lessons_desc";

        var disciplines = _context.Disciplines.AsQueryable();

        disciplines = sortOrder switch
        {
            "name_asc" => disciplines.OrderBy(d => d.discipline_name),
            "name_desc" => disciplines.OrderByDescending(d => d.discipline_name),
            "lessons_asc" => disciplines.OrderBy(d => d.number_of_lessons),
            "lessons_desc" => disciplines.OrderByDescending(d => d.number_of_lessons),
            _ => disciplines.OrderBy(d => d.discipline_name)
        };

        return View(await disciplines.ToListAsync());
    }

    // GET: Disciplines/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var discipline = await _context.Disciplines
            .FirstOrDefaultAsync(m => m.discipline_id == id);

        if (discipline == null) return NotFound();

        return View(discipline);
    }

    // GET: Disciplines/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Disciplines/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("discipline_id,discipline_name,number_of_lessons")] Discipline discipline)
    {
        if (ModelState.IsValid)
        {
            _context.Add(discipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(discipline);
    }

    // GET: Disciplines/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var discipline = await _context.Disciplines.FindAsync(id);
        if (discipline == null) return NotFound();

        return View(discipline);
    }

    // POST: Disciplines/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("discipline_id,discipline_name,number_of_lessons")] Discipline discipline)
    {
        if (id != discipline.discipline_id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(discipline);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Disciplines.Any(e => e.discipline_id == discipline.discipline_id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(discipline);
    }

    // GET: Disciplines/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var discipline = await _context.Disciplines
            .FirstOrDefaultAsync(m => m.discipline_id == id);

        if (discipline == null) return NotFound();

        return View(discipline);
    }

    // POST: Disciplines/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var discipline = await _context.Disciplines.FindAsync(id);
        if (discipline != null) _context.Disciplines.Remove(discipline);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
