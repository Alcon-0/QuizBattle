using System;

namespace Application.DTOs;

public record QuestionDto(Guid Id, string Text, List<AnswerOptionDto> Options, int CorrectAnswerIndex);