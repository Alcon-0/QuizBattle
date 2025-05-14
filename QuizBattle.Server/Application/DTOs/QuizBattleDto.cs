using System;
using Domain.Enums;

namespace Application.DTOs;

public record QuizBattleDto(Guid Id, Guid QuizId, string FirstPlayerId, string SecondPlayerId, 
    int FirstPlayerScore, int SecondPlayerScore, BattleStatus Status);