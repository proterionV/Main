using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainApi.Models
{
    public class UserActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "Некорректный идентификатор пользователя")]
        public int UserID { get; set; }

        [DateFormatValidate(ErrorMessage = "Некорректный формат даты")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateRegistration { get; set; }

        [DateFormatValidate(ErrorMessage = "Некорректный формат даты")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateLastActivity { get; set; }

        public int DaysByActivity => (DateLastActivity - DateRegistration).Days;
    }

    internal class DateFormatValidate : ValidationAttribute
    {
        public override bool IsValid(object data)
        {
            return data is DateTime _;
        }
    }
}
