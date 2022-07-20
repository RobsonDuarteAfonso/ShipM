using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipM.Models
{
    public class Ship
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Number { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ConstructionYear { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal GrossTonnage { get; set; }
        public decimal NetTonnage { get; set; }
        [ForeignKey("User")]
        public int IdUser { get; set; }
    }
}
