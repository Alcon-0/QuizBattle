using System;

namespace Application.DTOs;

public class StartBattleDto
{
    public Guid QuizId { get; set; }
    public string Player1Id { get; set; } = string.Empty;
    public string Player2Id { get; set; } = string.Empty;
}