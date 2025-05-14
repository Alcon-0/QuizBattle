using System;
using Domain.Enums;

namespace Domain.Entities;

public class QuizBattle
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; } = new();
    public string FirstPlayerId { get; set; } = string.Empty;
    public string SecondPlayerId { get; set; } = string.Empty;
    public int FirstPlayerScore { get; set; }
    public int SecondPlayerScore { get; set; }
    public BattleStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public ICollection<PlayerAnswer> PlayerAnswers { get; set; } = new List<PlayerAnswer>();
}
