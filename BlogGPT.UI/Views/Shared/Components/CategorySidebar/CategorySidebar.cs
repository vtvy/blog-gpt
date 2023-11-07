using BlogGPT.UI.Areas.Manage.Models.Category;
using BlogGPT.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogGPT.UI.Views.Shared.Components.CategorySidebar
{
    [ViewComponent]
    public class CategorySidebar : ViewComponent
    {
        public class CategorySidebarData
        {
            public List<TreeModel<IndexCategoryModel>> Categories { set; get; }
            public int level { set; get; }
            public string categoryslug { set; get; }
        }
        public const string COMPONENTNAME = "CategorySidebar";

        public CategorySidebar() { }
        public IViewComponentResult Invoke(CategorySidebarData data)
        {
            return View(data);
        }
    }
}