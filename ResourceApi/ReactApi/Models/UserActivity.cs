using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApi.Models
{
    public class UserActivity
    {
        [Required(ErrorMessage = "Некорректный идентификатор пользователя")]
        public int UserID { get; set; }

        [DateFormatValidate(ErrorMessage = "Некорректный формат даты")]
        public DateTime DateRegistration { get; set; }

        [DateFormatValidate(ErrorMessage = "Некорректный формат даты")]
        public DateTime DateLastActivity { get; set; }
    }

    internal class DateFormatValidate : ValidationAttribute
    {
        public override bool IsValid(object data)
        {
            if (!(data is DateTime date)) return false;

            return DateTime.TryParseExact((string)data, "dd.MM.yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out _);
        }
    }
}
