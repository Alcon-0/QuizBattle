using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuizBattleRepository _battleRepository;

    public QuizService(
        IQuizRepository quizRepository,
        IQuestionRepository questionRepository,
        IQuizBattleRepository battleRepository)
    {
        _quizRepository = quizRepository;
        _questionRepository = questionRepository;
        _battleRepository = battleRepository;
    }

    public async Task<QuizDto> CreateQuizAsync(CreateQuizDto createQuizDto)
    {
        var quiz = new Quiz
        {
            Title = createQuizDto.Title,
            Description = createQuizDto.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _quizRepository.AddAsync(quiz);
        
        return new QuizDto(
            quiz.Id,
            quiz.Title,
            quiz.Description,
            quiz.CreatedAt);
    }

    public async Task<QuestionDto> AddQuestionToQuizAsync(Guid quizId, CreateQuestionDto createQuestionDto)
    {
        var quiz = await _quizRepository.GetByIdAsync(quizId);
        if (quiz == null)
            throw new KeyNotFoundException($"Quiz with ID {quizId} not found");

        var question = new Question
        {
            Text = createQuestionDto.Text,
            CorrectAnswerIndex = createQuestionDto.CorrectAnswerIndex,
            QuizId = quizId,
            Options = createQuestionDto.Options
                .Select(o => new AnswerOption { Text = o.Text })
                .ToList()
        };

        await _questionRepository.AddAsync(question);

        return new QuestionDto(
            question.Id,
            question.Text,
            question.Options.Select(o => new AnswerOptionDto(o.Id, o.Text)).ToList(),
            question.CorrectAnswerIndex);
    }

    public async Task<IEnumerable<QuizDto>> GetAllQuizzesAsync()
    {
        var quizzes = await _quizRepository.GetAllAsync();
        return quizzes.Select(q => new QuizDto(
            q.Id,
            q.Title,
            q.Description,
            q.CreatedAt));
    }

    public async Task<QuizDto> GetQuizByIdAsync(Guid id)
    {
        var quiz = await _quizRepository.GetByIdAsync(id);
        if (quiz == null)
            throw new KeyNotFoundException($"Quiz with ID {id} not found");

        return new QuizDto(
            quiz.Id,
            quiz.Title,
            quiz.Description,
            quiz.CreatedAt);
    }

    public async Task<QuizBattleDto> StartBattleAsync(Guid quizId, string player1Id, string player2Id)
    {
        var quiz = await _quizRepository.GetByIdAsync(quizId);
        if (quiz == null)
            throw new KeyNotFoundException($"Quiz with ID {quizId} not found");

        var battle = new QuizBattle
        {
            QuizId = quizId,
            FirstPlayerId = player1Id,
            SecondPlayerId = player2Id,
            Status = BattleStatus.WaitingForPlayers,
            StartedAt = DateTime.UtcNow,
            FirstPlayerScore = 0,
            SecondPlayerScore = 0
        };

        await _battleRepository.AddAsync(battle);

        return new QuizBattleDto(
            battle.Id,
            battle.QuizId,
            battle.FirstPlayerId,
            battle.SecondPlayerId,
            battle.FirstPlayerScore,
            battle.SecondPlayerScore,
            battle.Status);
    }

  public async Task<QuizBattleDto> SubmitAnswerAsync(Guid battleId, string playerId, Guid questionId, int answerIndex)
    {
        var battle = await _battleRepository.GetByIdWithAnswersAsync(battleId);
        if (battle == null)
            throw new KeyNotFoundException($"Battle with ID {battleId} not found");

        if (battle.Status != BattleStatus.InProgress)
            throw new InvalidOperationException("Battle is not in progress");

        var question = await _questionRepository.GetByIdAsync(questionId);
        if (question == null)
            throw new KeyNotFoundException($"Question with ID {questionId} not found");

        // Check if player has already answered this question
        if (battle.PlayerAnswers.Any(pa => pa.QuestionId == questionId && pa.PlayerId == playerId))
            throw new InvalidOperationException("Player has already answered this question");

        bool isCorrect = answerIndex == question.CorrectAnswerIndex;

        // Update player score
        if (battle.FirstPlayerId == playerId)
        {
            battle.FirstPlayerScore += isCorrect ? 1 : 0;
        }
        else if (battle.SecondPlayerId == playerId)
        {
            battle.SecondPlayerScore += isCorrect ? 1 : 0;
        }
        else
        {
            throw new UnauthorizedAccessException("Player is not part of this battle");
        }

        // Record the answer
        battle.PlayerAnswers.Add(new PlayerAnswer
        {
            BattleId = battleId,
            PlayerId = playerId,
            QuestionId = questionId,
            AnswerIndex = answerIndex,
            IsCorrect = isCorrect,
            AnsweredAt = DateTime.UtcNow
        });

        // Check if battle is complete
        var quiz = await _quizRepository.GetByIdAsync(battle.QuizId);
        var totalQuestions = quiz?.Questions.Count ?? 0;
        
        // Count unique questions answered by both players
        var player1Answers = battle.PlayerAnswers
            .Where(pa => pa.PlayerId == battle.FirstPlayerId)
            .Select(pa => pa.QuestionId)
            .Distinct()
            .Count();

        var player2Answers = battle.PlayerAnswers
            .Where(pa => pa.PlayerId == battle.SecondPlayerId)
            .Select(pa => pa.QuestionId)
            .Distinct()
            .Count();

        if (player1Answers >= totalQuestions && player2Answers >= totalQuestions)
        {
            battle.Status = BattleStatus.Completed;
            battle.FinishedAt = DateTime.UtcNow;
        }

        await _battleRepository.UpdateAsync(battle);

        return new QuizBattleDto(
            battle.Id,
            battle.QuizId,
            battle.FirstPlayerId,
            battle.SecondPlayerId,
            battle.FirstPlayerScore,
            battle.SecondPlayerScore,
            battle.Status);
    }

    public async Task<QuizBattleDto> GetBattleByIdAsync(Guid id)
    {
        var battle = await _battleRepository.GetByIdWithAnswersAsync(id);
        if (battle == null)
            throw new KeyNotFoundException($"Battle with ID {id} not found");

        return new QuizBattleDto(
            battle.Id,
            battle.QuizId,
            battle.FirstPlayerId,
            battle.SecondPlayerId,
            battle.FirstPlayerScore,
            battle.SecondPlayerScore,
            battle.Status);
    }
}