using System;

namespace Domain.Entities;

public class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int CorrectAnswerIndex { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
    public ICollection<AnswerOption> Options { get; set; } = new List<AnswerOption>();
    public string? ImageId { get; set; } 
}