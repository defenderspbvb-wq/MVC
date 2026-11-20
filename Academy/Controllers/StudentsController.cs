using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academy.Models;

public class StudentsController : Controller
{
    private readonly AcademyContext _context;

    public StudentsController(AcademyContext context)
    {
        _context = context;
    }

    // GET: Students
    public async Task<IActionResult> Index(string sortOrder)
    {
        ViewData["NameSortParam"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
        ViewData["BirthSortParam"] = sortOrder == "birth_desc" ? "birth_asc" : "birth_desc";
        ViewData["GroupSortParam"] = sortOrder == "group_desc" ? "group_asc" : "group_desc";

        var students = _context.Students.Include(s => s.Group).AsQueryable();

        students = sortOrder switch
        {
            "name_asc" => students.OrderBy(s => s.last_name).ThenBy(s => s.first_name),
            "name_desc" => students.OrderByDescending(s => s.last_name).ThenByDescending(s => s.first_name),
            "birth_asc" => students.OrderBy(s => s.birth_date),
            "birth_desc" => students.OrderByDescending(s => s.birth_date),
            "group_asc" => students.OrderBy(s => s.Group!.group_name),
            "group_desc" => students.OrderByDescending(s => s.Group!.group_name),
            _ => students.OrderBy(s => s.last_name)
        };

        return View(await students.ToListAsync());
    }

    // GET: Students/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var student = await _context.Students
            .Include(s => s.Group)
            .FirstOrDefaultAsync(m => m.stud_id == id);

        if (student == null) return NotFound();

        return View(student);
    }

    // GET: Students/Create
    public IActionResult Create()
    {
        ViewData["group"] = new SelectList(_context.Groups, "group_id", "group_name");
        return View();
    }

    // POST: Students/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("stud_id,group,last_name,first_name,middle_name,birth_date,email,phone")] Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["group"] = new SelectList(_context.Groups, "group_id", "group_name", student.group);
        return View(student);
    }

    // GET: Students/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();

        ViewData["group"] = new SelectList(_context.Groups, "group_id", "group_name", student.group);
        return View(student);
    }

    // POST: Students/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("stud_id,group,last_name,first_name,middle_name,birth_date,email,phone")] Student student)
    {
        if (id != student.stud_id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.stud_id == student.stud_id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["group"] = new SelectList(_context.Groups, "group_id", "group_name", student.group);
        return View(student);
    }

    // GET: Students/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var student = await _context.Students
            .Include(s => s.Group)
            .FirstOrDefaultAsync(m => m.stud_id == id);

        if (student == null) return NotFound();

        return View(student);
    }

    // POST: Students/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null) _context.Students.Remove(student);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
