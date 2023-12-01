using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.Manage
{
    public class EditExtraProfileModel
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Địa chỉ email")]
        public string UserEmail { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(400)]
        public string Address { get; set; }


        [Display(Name = "Ngày sinh")]
        public DateTime? BirthDate { get; set; }
    }
}