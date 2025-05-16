namespace Application.DTOs;

public record QuizDto(
    Guid Id,
    string Title,
    string Description,
    DateTime CreatedAt,
    string? ImageId)
{
    public string? ImageUrl => ImageId != null ? $"/api/images/{ImageId}" : null;
}
