using System;

namespace Application.DTOs;

public record CreateQuestionDto(string Text, List<CreateAnswerOptionDto> Options, int CorrectAnswerIndex);