using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Questions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Infrastructure.Services;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionRepository _questionRepository;

    public QuizService(
        IQuizRepository quizRepository,
        IQuestionRepository questionRepository)
    {
        _quizRepository = quizRepository;
        _questionRepository = questionRepository;
    }

    public async Task<QuizDto> CreateQuizAsync(CreateQuizDto createQuizDto, string? imageId = null)
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
            quiz.CreatedAt,
            quiz.MongoCoverImageId);
    }

    public async Task<QuestionDto> AddQuestionToQuizAsync(
        Guid quizId, CreateQuestionDto createQuestionDto, string? imageId = null)
    {
        var quiz = await _quizRepository.GetByIdAsync(quizId);
        if (quiz == null)
            throw new KeyNotFoundException($"Quiz with ID {quizId} not found");

        var question = new Question
        {
            Text = createQuestionDto.Text,
            CorrectAnswerIndex = createQuestionDto.CorrectAnswerIndex,
            QuizId = quizId,
            ImageId = imageId
        };

        await _questionRepository.AddAsync(question);

        for (int i = 0; i < createQuestionDto.Options.Count; i++)
        {
            var option = new AnswerOption
            {
                Id = i,
                Text = createQuestionDto.Options[i].Text,
                QuestionId = question.Id
            };
            question.Options.Add(option);
        }

        await _questionRepository.UpdateAsync(question);

        return new QuestionDto(
            question.Id,
            question.Text,
            question.Options
                .OrderBy(o => o.Id)
                .Select(o => new AnswerOptionDto(o.Id, o.Text))
                .ToList(),
            question.CorrectAnswerIndex,
            question.ImageId);
    }

    public async Task<IEnumerable<QuizDto>> GetAllQuizzesAsync()
    {
        var quizzes = await _quizRepository.GetAllAsync();
        return quizzes.Select(q => new QuizDto(
            q.Id,
            q.Title,
            q.Description,
            q.CreatedAt,
            q.MongoCoverImageId));
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
            quiz.CreatedAt,
            quiz.MongoCoverImageId);
    }

    public async Task<IEnumerable<QuestionDto>> GetQuizQuestionsAsync(Guid quizId)
    {
        var quiz = await _quizRepository.GetByIdWithQuestionsAsync(quizId);
        if (quiz == null)
            throw new KeyNotFoundException($"Quiz with ID {quizId} not found");

        return quiz.Questions.Select(q => new QuestionDto(
            q.Id,
            q.Text,
            q.Options.Select(o => new AnswerOptionDto(o.Id, o.Text)).ToList(),
            q.CorrectAnswerIndex,
            q.ImageId));
    }

    public async Task UpdateQuestionAsync(Guid quizId, Guid questionId, UpdateQuestionDto updateQuestionDto)
    {
        var question = await _questionRepository.GetByIdWithOptionsAsync(questionId);
        if (question == null || question.QuizId != quizId)
            throw new KeyNotFoundException($"Question with ID {questionId} not found in quiz {quizId}");

        // Update question properties
        question.Text = updateQuestionDto.Text;
        question.CorrectAnswerIndex = updateQuestionDto.CorrectAnswerIndex;

        // Remove existing options
        question.Options.Clear();

        // Add new options with explicit IDs
        for (int i = 0; i < updateQuestionDto.Options.Count; i++)
        {
            question.Options.Add(new AnswerOption
            {
                Id = i,
                Text = updateQuestionDto.Options[i].Text,
                QuestionId = questionId
            });
        }

        await _questionRepository.UpdateAsync(question);
    }

    public async Task RemoveQuestionFromQuizAsync(Guid quizId, Guid questionId)
    {
        var question = await _questionRepository.GetByIdWithQuizAsync(questionId);

        if (question == null || question.QuizId != quizId)
            throw new KeyNotFoundException($"Question with ID {questionId} not found in quiz {quizId}");

        await _questionRepository.DeleteAsync(question.Id);
    }

    public async Task<QuestionDto> GetQuestionByIdAsync(Guid quizId, Guid questionId)
    {
        var question = await _questionRepository.GetByIdWithQuizAsync(questionId);
        if (question == null || question.QuizId != quizId)
            throw new KeyNotFoundException($"Question with ID {questionId} not found in quiz {quizId}");

        return new QuestionDto(
            question.Id,
            question.Text,
            question.Options.Select(o => new AnswerOptionDto(o.Id, o.Text)).ToList(),
            question.CorrectAnswerIndex,
            question.ImageId);
    }

    public async Task SetQuestionImageAsync(Guid quizId, Guid questionId, string imageId)
    {
        var question = await _questionRepository.GetByIdWithQuizAsync(questionId);
        if (question == null || question.QuizId != quizId)
            throw new KeyNotFoundException($"Question with ID {questionId} not found in quiz {quizId}");

        question.ImageId = imageId;
        await _questionRepository.UpdateAsync(question);
    }

    public async Task SetQuizCoverImageAsync(Guid id, string imageId)
    {
        var quiz = await _quizRepository.GetByIdAsync(id);
        if (quiz == null)
            throw new KeyNotFoundException($"Quiz with ID {id} not found");

        quiz.MongoCoverImageId = imageId;
        await _quizRepository.UpdateAsync(quiz);
    }
}