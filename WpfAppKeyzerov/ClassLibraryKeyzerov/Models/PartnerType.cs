using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keyzerov.Core.Models
{
    [Table("partner_types_keyzerov", Schema = "app")] 
    public class PartnerType
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Column("type_name")]
        public string TypeName { get; set; } = string.Empty;
    }
}