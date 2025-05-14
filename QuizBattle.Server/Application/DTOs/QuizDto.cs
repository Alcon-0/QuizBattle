namespace Application.DTOs;

public record QuizDto(Guid Id, string Title, string Description, DateTime CreatedAt);