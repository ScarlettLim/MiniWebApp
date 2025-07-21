using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp1.Data;
using WebApp1.Models;

public class HomeController : Controller
{
    private readonly DatabaseContext _context;

    public HomeController(DatabaseContext context)
    {
        _context = context;
    }

    public IActionResult Setup()
    {
        var model = _context.Projects.Include(p => p.Tasks).ToList();
        return View(model);
    }

    public IActionResult Worksheet()
    {
        ViewBag.Projects = _context.Projects.ToList();
        var model = _context.WorkSheets
            .Include(w => w.Project)
            .Include(w => w.Task)
            .ToList();
        return View(model);
    }

    public IActionResult GetTasks(int projectId)
    {
        var tasks = _context.ProjectTasks
            .Where(t => t.ProjectId == projectId)
            .Select(t => new { t.Id, t.Name })
            .ToList();
        return Json(tasks);
    }

    [HttpPost]
    public IActionResult SaveWorksheet([FromBody] WorkSheet entry)
    {
        if (_context.WorkSheets.Any(w =>
            w.Date == entry.Date &&
            w.ProjectId == entry.ProjectId &&
            w.ProjectTaskId == entry.ProjectTaskId))
        {
            return BadRequest("Duplicate project-task combination for this date");
        }

        _context.WorkSheets.Add(entry);
        _context.SaveChanges();
        return Ok();
    }

    [HttpPost]
    public IActionResult AddProject([FromBody] Project project)
    {
        if (string.IsNullOrEmpty(project.Name))
        {
            return BadRequest("Project name is required");
        }

        _context.Projects.Add(project);
        _context.SaveChanges();
        return Json(new { id = project.Id, name = project.Name });
    }

    [HttpPost]
    public IActionResult AddTask([FromBody] ProjectTask task)
    {
        if (task.ProjectId == 0)
        {
            return BadRequest("Project ID is required");
        }
        if (string.IsNullOrEmpty(task.Name))
        {
            return BadRequest("Task name is required");
        }

        // Ensure the project exists
        var project = _context.Projects.Find(task.ProjectId);
        if (project == null)
        {
            return BadRequest("Invalid Project ID");
        }

        task.Project = project; // Set the navigation property
        _context.ProjectTasks.Add(task);
        _context.SaveChanges();

        return Json(new
        {
            id = task.Id,
            name = task.Name,
            projectName = project.Name
        });
    }

    [HttpPost]
    public IActionResult SaveWorksheet([FromBody] List<WorkSheet> entries)
    {
        if (!entries.Any())
        {
            return BadRequest("No entries to save");
        }

        var date = entries.First().Date;

        // Check for duplicates in the submitted data
        var duplicateCheck = entries
            .GroupBy(e => new { e.ProjectId, e.ProjectTaskId })
            .Any(g => g.Count() > 1);

        if (duplicateCheck)
        {
            return BadRequest("Duplicate project-task combination in submitted data");
        }

        // Check against existing database entries
        var existingCombinations = _context.WorkSheets
            .Where(w => w.Date == date)
            .Select(w => new { w.ProjectId, w.ProjectTaskId })
            .ToList();

        var hasDuplicates = entries.Any(e =>
            existingCombinations.Any(ec =>
                ec.ProjectId == e.ProjectId &&
                ec.ProjectTaskId == e.ProjectTaskId));

        if (hasDuplicates)
        {
            return BadRequest("Duplicate project-task combination for this date already exists in database");
        }

        // Clear existing entries for this date
        _context.WorkSheets.RemoveRange(
            _context.WorkSheets.Where(w => w.Date == date));

        // Add new entries
        _context.WorkSheets.AddRange(entries);
        _context.SaveChanges();

        return Ok();
    }

}