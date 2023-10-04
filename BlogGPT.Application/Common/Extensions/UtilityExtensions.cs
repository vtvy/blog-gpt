using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BlogGPT.Application.Common.Extensions
{
    public static class UtilityExtensions
    {
        public static string GenerateSlug(this string title)
        {
            var slug = title.ToLower();

            string[] specialChars = new string[]
            {
                "à","á","ạ","ả","ã","â","ầ","ấ","ậ","ẩ","ẫ","ă","ằ","ắ","ặ","ẳ","ẵ",
                "è","é","ẹ","ẻ","ẽ","ê","ề","ế","ệ","ể","ễ","ì","í","ị","ỉ","ĩ","ò",
                "ó","ọ","ỏ","õ","ô","ồ","ố","ộ","ổ","ỗ","ơ","ò","ớ","ợ","ở","õ","ù",
                "ú","ụ","ủ","ũ","ư","ừ","ứ","ự","ử","ữ","ỳ","ý","ỵ","ỷ","ỹ","đ","À",
                "À","Ạ","Ả","Ã","Â","Ầ","Ấ","Ậ","Ẩ","Ẫ","Ă","Ằ","Ắ","Ặ","Ẳ","Ẵ","È",
                "É","Ẹ","Ẻ","Ẽ","Ê","Ề","Ế","Ệ","Ể","Ễ","Ì","Í","Ị","Ỉ","Ĩ","Ò","Ó",
                "Ọ","Ỏ","Õ","Ô","Ồ","Ố","Ộ","Ổ","Ỗ","Ơ","Ờ","Ớ","Ợ","Ở","Ỡ","Ù","Ú",
                "Ụ","Ủ","Ũ","Ư","Ừ","Ứ","Ự","Ử","Ữ","Ỳ","Ý","Ỵ","Ỷ","Ỹ","Đ"
            };

            string[] normalChars =
            {
                "a","a","a","a","a","a","a","a","a","a","a","a","a","a","a","a","a",
                "e","e","e","e","e","e","e","e","e","e","e","i","i","i","i","i","o",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","u",
                "u","u","u","u","u","u","u","u","u","u","y","y","y","y","y","d","a",
                "a","a","a","a","a","a","a","a","a","a","a","a","a","a","a","a","e",
                "e","e","e","e","e","e","e","e","e","e","i","i","i","i","i","o","o",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","u","u",
                "u","u","u","u","u","u","u","u","u","y","y","y","y","y","d"
            };

            // Convert culture special characters
            for (int i = 0; i < specialChars.Length; i++)
            {
                slug = slug.Replace(specialChars[i], normalChars[i]);
            }

            // Remove special characters
            slug = Regex.Replace(slug, @"[^a-z0-9]", " ").Trim();

            // Remove whitespaces
            slug = Regex.Replace(slug, @"\s+", " ").Replace(" ", "-");

            return slug;
        }
        public static bool IsValidEmail(this string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }

            try
            {
                var mail = new MailAddress(email);
                return mail.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
