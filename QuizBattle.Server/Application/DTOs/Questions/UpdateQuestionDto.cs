using System;

namespace Application.DTOs.Questions;

public record UpdateQuestionDto(Guid Id, string Text, List<AnswerOptionDto> Options, int CorrectAnswerIndex);