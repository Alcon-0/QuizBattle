using System;
using System.Collections.Generic;

namespace Application.DTOs.Questions;

public record QuestionDto(
    Guid Id,
    string Text,
    List<AnswerOptionDto> Options,
    int CorrectAnswerIndex,
    string? ImageId)
{
    public string? ImageUrl => ImageId != null ? $"/api/images/{ImageId}" : null;
}