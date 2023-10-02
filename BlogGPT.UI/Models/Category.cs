using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogGPT.UI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Danh mục cha")]
        public int? ParentId { get; set; }

        [Required(ErrorMessage = "Phải có tên danh mục")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Need url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Url")]
        public string Slug { set; get; }

        public ICollection<Category> ChildrenCategories { get; set; }

        [Display(Name = "Danh mục cha")]
        public Category ParentCategory { set; get; }

        public List<PostCategory> PostCategories { get; set; }

        public List<Category> ListParents()
        {
            List<Category> li = new();
            var parent = ParentCategory;
            while (parent != null)
            {
                li.Add(parent);
                parent = parent.ParentCategory;
            }


            li.Reverse();
            return li;
        }


        public static Category Find(ICollection<Category> lis, int CategoryId)
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


        public List<int> ChildCategoryIDs(ICollection<Category> childcates = null, List<int> lists = null)
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
