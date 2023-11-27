using Microsoft.AspNetCore.Mvc;

namespace BlogGPT.UI.Views.Shared.Components.CategorySidebar
{
    [ViewComponent]
    public class CategorySidebar : ViewComponent
    {
        public class CategorySidebarData
        {
            public IEnumerable<TreeModel<CategoryModel>> Categories { set; get; } = null!;
            public int Level { set; get; }
        }

        //public CategorySidebar() { }
        public IViewComponentResult Invoke(CategorySidebarData data)
        {
            return View(data);
        }
    }
}