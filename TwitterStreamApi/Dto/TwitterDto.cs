using System.ComponentModel.DataAnnotations;

namespace TwitterStreamApi.Dto
{
    public record TwitterDto
    {
        // public string? Id { get; set; }
        //public string? AuthorId { get; set; }
        //public DateTimeOffset TwitterPublished { get; set; }

        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }

    }
}
