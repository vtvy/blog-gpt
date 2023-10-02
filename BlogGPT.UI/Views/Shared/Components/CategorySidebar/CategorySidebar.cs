using BlogGPT.UI.Models.Category;
using Microsoft.AspNetCore.Mvc;

namespace BlogGPT.UI.Components
{
    [ViewComponent]
    public class CategorySidebar : ViewComponent
    {
        public class CategorySidebarData
        {
            public List<CategoryModel> Categories { set; get; }
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