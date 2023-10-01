using Microsoft.AspNetCore.Mvc;
using blog.Models;

namespace blog.Components
{
        [ViewComponent]
        public class CategorySidebar : ViewComponent
        {
            public class CategorySidebarData {
                public List<Category> Categories {set;get;}
                public int level {set; get;}
                public string categoryslug { set; get;}
            }
            public const string COMPONENTNAME = "CategorySidebar";
            
            public CategorySidebar() {}
            public IViewComponentResult Invoke(CategorySidebarData data) {
                return View(data);
            }
        }
}