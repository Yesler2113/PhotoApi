using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoApi.entities
{
    [Table("Photos")]
    public class PhotoEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("title")]
        public string? Description { get; set; }
        [Column("url")]
        [Required]
        public string Url { get; set; }
        [Column("date")]
        [Required]
        public DateTime date { get; set; }
    } 
}

