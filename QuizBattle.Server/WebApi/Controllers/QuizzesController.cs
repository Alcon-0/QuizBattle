using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Questions;
using Application.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
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

        /// <summary>
        /// Creates a new quiz
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/quizzes
        ///     {
        ///         "title": "Geography Quiz",
        ///         "description": "Test your geography knowledge",
        ///         "category": "Geography",
        ///         "questions": []
        ///     }
        ///
        /// </remarks>
        /// <param name="createQuizDto">Quiz creation data</param>
        /// <returns>Newly created quiz</returns>
        /// <response code="201">Returns the newly created quiz</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="401">Unauthorized - User is not authenticated</response>
        [HttpPost]
        [ProducesResponseType(typeof(QuizDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Gets a quiz by ID
        /// </summary>
        /// <param name="id">The ID of the quiz to retrieve</param>
        /// <returns>Requested quiz</returns>
        /// <response code="200">Returns the requested quiz</response>
        /// <response code="404">If quiz is not found</response>
        /// <response code="401">If user is not authenticated</response>
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

        /// <summary>
        /// Adds a new question to a quiz
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/quizzes/{quizId}/questions
        ///     {
        ///         "text": "What is the capital of France?",
        ///         "options": [
        ///             {"text": "Paris", "isCorrect": true},
        ///             {"text": "London", "isCorrect": false},
        ///             {"text": "Berlin", "isCorrect": false}
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <param name="quizId">The ID of the quiz to add question to</param>
        /// <param name="createQuestionDto">Question data</param>
        /// <returns>Newly created question</returns>
        /// <response code="201">Returns the newly created question</response>
        /// <response code="400">If the request data is invalid or missing options</response>
        /// <response code="401">Unauthorized - User is not authenticated</response>
        /// <response code="404">If quiz is not found</response>
        [HttpPost("{quizId}/questions")]
        [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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