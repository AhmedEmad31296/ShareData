using System.ComponentModel.DataAnnotations;

namespace ShareData.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}