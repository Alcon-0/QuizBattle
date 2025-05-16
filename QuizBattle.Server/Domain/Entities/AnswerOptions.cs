using System;

namespace Domain.Entities;

public class AnswerOption
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}
