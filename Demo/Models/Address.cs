using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public partial class Address
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Zip Code")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "The length must be 5 digits.")]
        public int ZipCode { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Country { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string City { get; set; }

        // One-to-One Relationship
        public virtual Company Company { get; set; } 
    }
}