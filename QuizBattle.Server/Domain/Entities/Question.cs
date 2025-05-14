using System;

namespace Domain.Entities;

public class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public List<AnswerOption> Options { get; set; } = new();
    public int CorrectAnswerIndex { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; } = new();
}