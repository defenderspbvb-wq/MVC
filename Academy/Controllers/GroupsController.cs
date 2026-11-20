using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academy.Models;

public class GroupsController : Controller
{
    private readonly AcademyContext _context;

    public GroupsController(AcademyContext context)
    {
        _context = context;
    }

    // GET: Groups
    public async Task<IActionResult> Index(string sortOrder)
    {
        ViewData["NameSortParam"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
        ViewData["DirectionSortParam"] = sortOrder == "dir_desc" ? "dir_asc" : "dir_desc";
        ViewData["DateSortParam"] = sortOrder == "date_desc" ? "date_asc" : "date_desc";

        var groups = _context.Groups.Include(g => g.Direction).AsQueryable();

        groups = sortOrder switch
        {
            "name_asc" => groups.OrderBy(g => g.group_name),
            "name_desc" => groups.OrderByDescending(g => g.group_name),
            "dir_asc" => groups.OrderBy(g => g.Direction!.direction_name),
            "dir_desc" => groups.OrderByDescending(g => g.Direction!.direction_name),
            "date_asc" => groups.OrderBy(g => g.start_date),
            "date_desc" => groups.OrderByDescending(g => g.start_date),
            _ => groups.OrderBy(g => g.group_name)
        };

        return View(await groups.ToListAsync());
    }

    // GET: Groups/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var @group = await _context.Groups
            .Include(g => g.Direction)
            .FirstOrDefaultAsync(m => m.group_id == id);

        if (@group == null) return NotFound();

        return View(@group);
    }

    // GET: Groups/Create
    public IActionResult Create()
    {
        ViewData["direction"] = new SelectList(_context.Directions, "direction_id", "direction_name");
        return View();
    }

    // POST: Groups/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("group_id,group_name,direction,learning_days,start_time,start_date")] Group @group)
    {
        if (ModelState.IsValid)
        {
            _context.Add(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["direction"] = new SelectList(_context.Directions, "direction_id", "direction_name", @group.direction);
        return View(@group);
    }

    // GET: Groups/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var @group = await _context.Groups.FindAsync(id);
        if (@group == null) return NotFound();

        ViewData["direction"] = new SelectList(_context.Directions, "direction_id", "direction_name", @group.direction);
        return View(@group);
    }

    // POST: Groups/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("group_id,group_name,direction,learning_days,start_time,start_date")] Group @group)
    {
        if (id != @group.group_id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(@group);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Groups.Any(e => e.group_id == @group.group_id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["direction"] = new SelectList(_context.Directions, "direction_id", "direction_name", @group.direction);
        return View(@group);
    }

    // GET: Groups/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var @group = await _context.Groups
            .Include(g => g.Direction)
            .FirstOrDefaultAsync(m => m.group_id == id);

        if (@group == null) return NotFound();

        return View(@group);
    }

    // POST: Groups/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var @group = await _context.Groups.FindAsync(id);
        if (@group != null) _context.Groups.Remove(@group);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
