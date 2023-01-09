using System.ComponentModel.DataAnnotations;

namespace Twitter.Service.Dto
{
    public record TwitterDto
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }

    }
}
