using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keyzerov.Core.Models
{
    [Table("partners_keyzerov", Schema = "app")]  
    public class Partner
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Column("type_id")]
        public int TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public PartnerType? Type { get; set; }

        [MaxLength(20)]
        public string? INN { get; set; }

        public byte[]? Logo { get; set; }

        [Range(0, int.MaxValue)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(200)]
        public string? DirectorName { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? SalesPlaces { get; set; }

        public ICollection<SalesHistory> SalesHistory { get; set; } = new List<SalesHistory>();

        [NotMapped]
        public int CurrentDiscount { get; set; } = 0;
    }
}