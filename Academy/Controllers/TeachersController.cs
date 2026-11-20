using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Models;

public class TeachersController : Controller
{
    private readonly AcademyContext _context;

    public TeachersController(AcademyContext context)
    {
        _context = context;
    }

    // GET: Teachers
    public async Task<IActionResult> Index(string sortOrder)
    {
        ViewData["NameSortParam"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
        ViewData["WorkSortParam"] = sortOrder == "work_desc" ? "work_asc" : "work_desc";
        ViewData["RateSortParam"] = sortOrder == "rate_desc" ? "rate_asc" : "rate_desc";

        var teachers = _context.Teachers.AsQueryable();

        teachers = sortOrder switch
        {
            "name_asc" => teachers.OrderBy(t => t.last_name).ThenBy(t => t.first_name),
            "name_desc" => teachers.OrderByDescending(t => t.last_name).ThenByDescending(t => t.first_name),
            "work_asc" => teachers.OrderBy(t => t.work_since),
            "work_desc" => teachers.OrderByDescending(t => t.work_since),
            "rate_asc" => teachers.OrderBy(t => t.rate),
            "rate_desc" => teachers.OrderByDescending(t => t.rate),
            _ => teachers.OrderBy(t => t.last_name)
        };

        return View(await teachers.ToListAsync());
    }

    // GET: Teachers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.teacher_id == id);

        if (teacher == null) return NotFound();

        return View(teacher);
    }

    // GET: Teachers/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Teachers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("teacher_id,work_since,rate,last_name,first_name,middle_name,birth_date,email,phone")] Teacher teacher)
    {
        if (ModelState.IsValid)
        {
            _context.Add(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(teacher);
    }

    // GET: Teachers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();

        return View(teacher);
    }

    // POST: Teachers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("teacher_id,work_since,rate,last_name,first_name,middle_name,birth_date,email,phone")] Teacher teacher)
    {
        if (id != teacher.teacher_id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Teachers.Any(e => e.teacher_id == teacher.teacher_id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(teacher);
    }

    // GET: Teachers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.teacher_id == id);

        if (teacher == null) return NotFound();

        return View(teacher);
    }

    // POST: Teachers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher != null) _context.Teachers.Remove(teacher);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
