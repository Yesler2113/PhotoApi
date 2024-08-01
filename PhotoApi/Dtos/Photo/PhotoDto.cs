namespace PhotoApi.Dtos.Photo;

public class PhotoDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public DateTime Date { get; set; }
}