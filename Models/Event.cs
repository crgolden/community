using System;
using System.ComponentModel.DataAnnotations;

namespace Community.Models
{
    public class Event
    {
        public int ID { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }
        [Display(Name = "Release Date"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$"), Required, StringLength(30)]
        public string Location { get; set; }
        [Range(1, 100), DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$"), StringLength(5)]
        public string Creator { get; set; }
    }
}
