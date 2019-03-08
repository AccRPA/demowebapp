using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public partial class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public virtual Address Address { get; set; }
    }
}