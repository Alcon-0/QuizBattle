using System;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Questions;

public record CreateQuestionDto(
    string Text,
    List<CreateAnswerOptionDto> Options,
    int CorrectAnswerIndex);