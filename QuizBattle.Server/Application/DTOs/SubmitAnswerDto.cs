using System;

namespace Application.DTOs;

public class SubmitAnswerDto
{
    public string PlayerId { get; set; } = string.Empty;
    public Guid QuestionId { get; set; }
    public int AnswerIndex { get; set; }
}
