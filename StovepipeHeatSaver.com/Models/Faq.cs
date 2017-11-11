using System.ComponentModel.DataAnnotations;

namespace StovepipeHeatSaver.Models
{
    public class Faq
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Question { get; set; }

        [StringLength(1000)]
        public string Answer { get; set; }
    }
}