using System;
using Application.DTOs;
using Application.DTOs.Questions;

namespace Application.Interfaces.Services;

public interface IQuizService
{
    Task<QuizDto> CreateQuizAsync(CreateQuizDto createQuizDto, string? imageId = null);
    Task<QuizDto> GetQuizByIdAsync(Guid id);
    Task<IEnumerable<QuizDto>> GetAllQuizzesAsync();
    Task<QuestionDto> AddQuestionToQuizAsync(Guid quizId, CreateQuestionDto createQuestionDto, string? imageId = null);
    Task<IEnumerable<QuestionDto>> GetQuizQuestionsAsync(Guid quizId);
    Task<QuestionDto> GetQuestionByIdAsync(Guid quizId, Guid questionId);
    Task UpdateQuestionAsync(Guid quizId, Guid questionId, UpdateQuestionDto updateQuestionDto);
    Task RemoveQuestionFromQuizAsync(Guid quizId, Guid questionId);
    Task SetQuestionImageAsync(Guid quizId, Guid questionId, string imageId);
    Task SetQuizCoverImageAsync(Guid id, string imageId);
}