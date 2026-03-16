using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keyzerov.Core.Models
{
    [Table("sales_history_keyzerov", Schema = "app")]
    public class SalesHistory
    {
        [Key]
        public int Id { get; set; }

        [Column("partner_id")]
        public int PartnerId { get; set; }

        [ForeignKey(nameof(PartnerId))]
        public Partner? Partner { get; set; }

        [Required, MaxLength(200)]
        [Column("product_name")]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column("sale_date")]
        public DateTime SaleDate { get; set; } = DateTime.Now;
    }
}