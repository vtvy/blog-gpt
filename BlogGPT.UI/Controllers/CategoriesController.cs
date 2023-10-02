using BlogGPT.Application.Categories.Commands.CreateCategory;
using BlogGPT.Domain.Constants;
using BlogGPT.Domain.Entities;
using BlogGPT.Infrastructure.Data;
using BlogGPT.UI.Models.Category;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogGPT.UI.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CategoriesController(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categoriesQuery = (from cate in _context.Categories select cate)
                                  .Include(cate => cate.Parent)
                                  .Include(cate => cate.ChildrenCategories);

            var categories = (await categoriesQuery.ToListAsync())
                             .ToList()
                             .Where(cate => cate.Parent == null);
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public async Task<IActionResult> Create()
        {
            var categoriesQuery = (from cate in _context.Categories select cate)
                     .Include(cate => cate.Parent)
                     .Include(cate => cate.ChildrenCategories);

            var categories = (await categoriesQuery.ToListAsync())
                             .Where(cate => cate.Parent == null)
                             .ToList();

            categories.Insert(0, new Category()
            {
                Name = "Not having parent category",
                Slug = ""
            });

            var selectList = new List<Category>();

            CreatePrefixForSelect(categories, selectList, 0);

            ViewData["ParentId"] = new SelectList(selectList, "Id", "Title");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Category.CreateCategoryModel command)
        {

            var result = await _mediator.Send(command);
            //if (ModelState.IsValid)
            //{
            //    if (category.ParentId == -1) category.ParentId = null;
            //    _context.Add(category);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewBag.ParentId = new SelectList(_context.Categories, "Id", "Slug", category.ParentId);

            return View();
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var categoriesQuery = (from cate in _context.Categories select cate)
                     .Include(cate => cate.Parent)
                     .Include(cate => cate.ChildrenCategories);

            var categories = (await categoriesQuery.ToListAsync())
                             .Where(cate => cate.Parent == null)
                             .ToList();

            categories.Insert(0, new Category()
            {
                Id = -1,
                Name = "Not having parent category",
                Slug = ""
            });

            var selectList = new List<Category>();

            CreatePrefixForSelect(categories, selectList, 0, category);
            ViewData["ParentId"] = new SelectList(selectList, "Id", "Title");
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentId,Title,Content,Slug")] Category category)
        {
            if (id != category.Id || category.ParentId == category.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    if (category.ParentId == -1) category.ParentId = null;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Slug", category.ParentId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.Categories
                           .Include(c => c.ChildrenCategories)
                           .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            foreach (var childCategory in category.ChildrenCategories)
            {
                childCategory.ParentId = category.ParentId;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        private void CreatePrefixForSelect(List<Category> rawCategories, List<Category> outCategories, int level, Category curCategory = null)
        {
            string prefix = string.Concat(Enumerable.Repeat("|---", level));
            foreach (var category in rawCategories)
            {
                if (curCategory != null && category.Id == curCategory.Id) continue;
                outCategories.Add(new Category()
                {
                    Id = category.Id,
                    Name = prefix + " " + category.Name,
                    Slug = ""
                });
                if (category.ChildrenCategories?.Count > 0)
                {
                    CreatePrefixForSelect(category.ChildrenCategories.ToList(), outCategories, ++level, curCategory);
                }
            }
        }
    }
}
