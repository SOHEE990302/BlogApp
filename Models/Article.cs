using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Article
    {
        [Key] // Primary Key
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Body is required.")]
        public string Body { get; set; } = string.Empty;

        public string ContributorUsername { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; } // Optional Start Date

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; } // Optional End Date
    }
}
