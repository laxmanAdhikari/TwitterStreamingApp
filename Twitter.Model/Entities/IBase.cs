namespace Twitter.Model.Entities
{
    public interface IBase
    {
        int Id { get; set; }
        DateTimeOffset Created { get; set; }
        DateTimeOffset? Updated { get; set; }
    }
}
