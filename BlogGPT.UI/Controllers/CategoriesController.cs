using BlogGPT.Application.Categories.Commands;
using BlogGPT.Application.Categories.Queries;
using BlogGPT.UI.Constants;
using BlogGPT.UI.ViewModels.Category;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogGPT.UI.Controllers
{
    [Authorize(Roles = Roles.Administrator + "," + Roles.Editor)]
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CategoriesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: Categories
        public async Task<IActionResult> IndexAsync()
        {
            var categories = await _mediator.Send(new GetAllCategoryQuery());

            var categoriesVM = _mapper.Map<IEnumerable<TreeModel<IndexCategoryModel>>>(categories);

            return View(categoriesVM);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> DetailAsync(int id)
        {
            var category = await _mediator.Send(new GetCategoryQuery { Id = id });
            if (category != null)
            {
                var categoryVM = _mapper.Map<DetailCategoryModel>(category);
                return View(categoryVM);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Categories/Create
        public async Task<IActionResult> CreateAsync()
        {
            var categories = await _mediator.Send(new GetSelectCategoryQuery());

            var categoriesList = _mapper.Map<IEnumerable<TreeModel<SelectCategoryModel>>>(categories);
            var selectList = new List<SelectCategoryModel>();

            CreatePrefixForSelect(categoriesList, selectList, 0);

            ViewData["ParentId"] = new SelectList(selectList, "Id", "Name");
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryModel category)
        {
            var command = _mapper.Map<CreateCategoryCommand>(category);
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);

                return RedirectToAction(nameof(Index));
            }

            var categories = await _mediator.Send(new GetSelectCategoryQuery());

            var categoriesList = _mapper.Map<IEnumerable<TreeModel<SelectCategoryModel>>>(categories);
            var selectList = new List<SelectCategoryModel>();

            CreatePrefixForSelect(categoriesList, selectList, 0);

            ViewData["ParentId"] = new SelectList(selectList, "Id", "Name");
            return View();
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _mediator.Send(new GetCategoryQuery { Id = id });
            if (category == null)
            {
                return NotFound();
            }

            var categories = await _mediator.Send(new GetSelectCategoryQuery { Id = id });

            var categoriesList = _mapper.Map<IEnumerable<TreeModel<SelectCategoryModel>>>(categories);

            var selectList = new List<SelectCategoryModel>();

            CreatePrefixForSelect(categoriesList, selectList, 0);

            ViewData["ParentId"] = new SelectList(selectList, "Id", "Name");

            var categoryVM = _mapper.Map<EditCategoryModel>(category);
            return View(categoryVM);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCategoryModel category)
        {
            if (id != category.Id || category.ParentId == category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var command = _mapper.Map<UpdateCategoryCommand>(category);

                await _mediator.Send(command);

                return RedirectToAction(nameof(Index));
            }

            var categories = await _mediator.Send(new GetSelectCategoryQuery());

            var categoriesList = _mapper.Map<IEnumerable<TreeModel<SelectCategoryModel>>>(categories);

            var selectList = new List<SelectCategoryModel>();

            CreatePrefixForSelect(categoriesList, selectList, 0);

            ViewData["ParentId"] = new SelectList(selectList, "Id", "Name");

            var categoryVM = _mapper.Map<EditCategoryModel>(category);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _mediator.Send(new GetCategoryQuery { Id = id });
            if (category == null)
            {
                return NotFound();
            }

            var categoryVM = _mapper.Map<DeleteCategoryModel>(category);
            return View(categoryVM);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteCategoryCommand { Id = id });

            return RedirectToAction(nameof(Index));
        }

        private void CreatePrefixForSelect(IEnumerable<TreeModel<SelectCategoryModel>> rawCategories, List<SelectCategoryModel> categoriesSelect, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("-----> ", level));
            foreach (var category in rawCategories)
            {
                categoriesSelect.Add(new SelectCategoryModel()
                {
                    Id = category.Item.Id,
                    Name = prefix + category.Item.Name,
                });

                if (category.Children != null)
                {
                    var childLevel = level + 1;
                    CreatePrefixForSelect(category.Children, categoriesSelect, childLevel);
                }
            }
        }
    }
}
