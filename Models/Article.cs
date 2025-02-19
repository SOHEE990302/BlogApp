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
        public string Title { get; set; } = string.Empty; // 게시글 제목

        [Required(ErrorMessage = "Body is required.")]
        public string Body { get; set; } = string.Empty; // 게시글 내용

        [Required(ErrorMessage = "Contributor username is required.")]
        public string ContributorUsername { get; set; } = string.Empty; // 작성자 Username

        [Required(ErrorMessage = "Create date is required.")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // 생성 날짜

        [DataType(DataType.Date)]
        [CustomValidation(typeof(Article), nameof(ValidateStartDate))]
        public DateTime? StartDate { get; set; } // 게시글 시작 날짜 (선택 사항)

        [DataType(DataType.Date)]
        [CustomValidation(typeof(Article), nameof(ValidateEndDate))]
        public DateTime? EndDate { get; set; } // 게시글 종료 날짜 (선택 사항)

        /// <summary>
        /// StartDate와 EndDate 간의 검증 로직.
        /// </summary>
        public static ValidationResult? ValidateStartDate(DateTime? startDate, ValidationContext context)
        {
            if (startDate.HasValue && startDate.Value < DateTime.UtcNow)
            {
                return new ValidationResult("Start date cannot be in the past.");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateEndDate(DateTime? endDate, ValidationContext context)
        {
            var instance = (Article)context.ObjectInstance;
            if (endDate.HasValue && instance.StartDate.HasValue && endDate < instance.StartDate)
            {
                return new ValidationResult("End date cannot be earlier than the start date.");
            }
            return ValidationResult.Success;
        }
    }
}