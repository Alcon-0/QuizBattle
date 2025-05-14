using System;
using Application.DTOs;

namespace Application.Interfaces.Services;

public interface IQuizService
{
    Task<QuizDto> CreateQuizAsync(CreateQuizDto createQuizDto);
    Task<QuizDto> GetQuizByIdAsync(Guid id);
    Task<IEnumerable<QuizDto>> GetAllQuizzesAsync();
    Task<QuestionDto> AddQuestionToQuizAsync(Guid quizId, CreateQuestionDto createQuestionDto);
    Task<QuizBattleDto> StartBattleAsync(Guid quizId, string player1Id, string player2Id);
    Task<QuizBattleDto> SubmitAnswerAsync(Guid battleId, string playerId, Guid questionId, int answerIndex);
    Task<QuizBattleDto> GetBattleByIdAsync(Guid id);
}