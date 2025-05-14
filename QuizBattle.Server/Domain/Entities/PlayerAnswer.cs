using System;

namespace Domain.Entities;

public class PlayerAnswer
{
    public Guid Id { get; set; }
    public Guid BattleId { get; set; }
    public QuizBattle Battle { get; set; } = new();
    public string PlayerId { get; set; } = string.Empty;
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = new();
    public int AnswerIndex { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime AnsweredAt { get; set; }
}