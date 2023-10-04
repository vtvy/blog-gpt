using BlogGPT.UI.ViewModels.PostCategory;
using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.ViewModels.Category
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Display(Name = "Danh mục cha")]
        public int? ParentId { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} from {2} to {1}")]
        [Display(Name = "Category name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Need url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} from {2} to {1}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Url")]
        public string Slug { set; get; }

        public ICollection<CategoryModel> ChildrenCategories { get; set; }

        [Display(Name = "Danh mục cha")]
        public CategoryModel ParentCategory { set; get; }

        public List<PostCategoryModel> PostCategories { get; set; }

        public List<CategoryModel> ListParents()
        {
            List<CategoryModel> li = new();
            var parent = ParentCategory;
            while (parent != null)
            {
                li.Add(parent);
                parent = parent.ParentCategory;
            }


            li.Reverse();
            return li;
        }


        public static CategoryModel Find(ICollection<CategoryModel> lis, int CategoryId)
        {
            foreach (var c in lis)
            {
                if (c.Id == CategoryId) return c;
                if (c.ChildrenCategories != null)
                {
                    var c_in_child = Find(c.ChildrenCategories, CategoryId);

                    if (c_in_child != null)
                        return c_in_child;
                }
            }
            return null;
        }


        public List<int> ChildCategoryIDs(ICollection<CategoryModel> childcates = null, List<int> lists = null)
        {
            lists ??= new List<int>();

            childcates ??= ChildrenCategories;

            if (childcates == null)
                return lists;

            foreach (var item in childcates)
            {
                lists.Add(item.Id);
                ChildCategoryIDs(item.ChildrenCategories, lists);
            }

            return lists;
        }

    }

}
