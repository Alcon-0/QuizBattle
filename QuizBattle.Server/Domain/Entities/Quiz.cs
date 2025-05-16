using System;

namespace Domain.Entities;

public class Quiz
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public DateTime CreatedAt { get; set; }
    public string? MongoCoverImageId { get; set; }
}
