using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Questions;
using Application.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/quizzes")]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly MongoImageService _mongoImageService;

        public QuizzesController(
            IQuizService quizService,
            MongoImageService mongoImageService)
        {
            _quizService = quizService;
            _mongoImageService = mongoImageService;
        }
        
        #region Quiz Endpoints
        
        [HttpPost]
        public async Task<ActionResult<QuizDto>> CreateQuiz([FromBody] CreateQuizDto createQuizDto)
        {
            var quiz = await _quizService.CreateQuizAsync(createQuizDto);
            return CreatedAtAction(nameof(GetQuizById), new { id = quiz.Id }, quiz);
        }

        [HttpPost("{id}/cover")]
        public async Task<IActionResult> UploadQuizCover(Guid id, IFormFile imageFile)
        {
            using var stream = imageFile.OpenReadStream();
            var imageId = (await _mongoImageService.UploadImageAsync(stream, imageFile.FileName)).ToString();
            await _quizService.SetQuizCoverImageAsync(id, imageId);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetAllQuizzes()
        {
            var quizzes = await _quizService.GetAllQuizzesAsync();
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDto>> GetQuizById(Guid id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            return Ok(quiz);
        }

        #endregion


        #region Question Endpoints

        [HttpGet("{quizId}/questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuizQuestions(Guid quizId)
        {
            var questions = await _quizService.GetQuizQuestionsAsync(quizId);
            return Ok(questions);
        }

        [HttpGet("{quizId}/questions/{questionId}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(
            Guid quizId,
            Guid questionId)
        {
            var question = await _quizService.GetQuestionByIdAsync(quizId, questionId);
            return Ok(question);
        }

        [HttpPost("{quizId}/questions")]
        public async Task<ActionResult<QuestionDto>> AddQuestionToQuiz(
            Guid quizId,
            [FromBody] CreateQuestionDto createQuestionDto)
        {
            if (createQuestionDto.Options == null || !createQuestionDto.Options.Any())
            {
                return BadRequest("At least one option is required");
            }

            var question = await _quizService.AddQuestionToQuizAsync(quizId, createQuestionDto);
            return CreatedAtAction(nameof(GetQuestionById), new { quizId, questionId = question.Id }, question);
        }

        [HttpPost("{quizId}/questions/{questionId}/image")]
        public async Task<IActionResult> UploadQuestionImage(
            Guid quizId,
            Guid questionId,
            IFormFile imageFile)
        {
            using var stream = imageFile.OpenReadStream();
            var imageId = (await _mongoImageService.UploadImageAsync(stream, imageFile.FileName)).ToString();
            await _quizService.SetQuestionImageAsync(quizId, questionId, imageId);
            return Ok();
        }

        [HttpPut("{quizId}/questions/{questionId}")]
        public async Task<IActionResult> UpdateQuestion(
            Guid quizId,
            Guid questionId,
            [FromBody] UpdateQuestionDto updateQuestionDto)
        {
            await _quizService.UpdateQuestionAsync(quizId, questionId, updateQuestionDto);
            return NoContent();
        }

        [HttpDelete("{quizId}/questions/{questionId}")]
        public async Task<IActionResult> RemoveQuestionFromQuiz(
            Guid quizId,
            Guid questionId)
        {
            await _quizService.RemoveQuestionFromQuizAsync(quizId, questionId);
            return NoContent();
        }

        #endregion
    }
}