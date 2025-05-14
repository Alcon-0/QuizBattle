using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/battles")]
    public class BattlesController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public BattlesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost]
        public async Task<ActionResult<QuizBattleDto>> StartBattle(
            [FromBody] StartBattleDto startBattleDto)
        {
            var battle = await _quizService.StartBattleAsync(
                startBattleDto.QuizId,
                startBattleDto.Player1Id,
                startBattleDto.Player2Id);
            
            return CreatedAtAction(nameof(GetBattleById), new { id = battle.Id }, battle);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizBattleDto>> GetBattleById(Guid id)
        {
            var battle = await _quizService.GetBattleByIdAsync(id);
            return Ok(battle);
        }

        [HttpPost("{battleId}/answers")]
        public async Task<ActionResult<QuizBattleDto>> SubmitAnswer(
            Guid battleId,
            [FromBody] SubmitAnswerDto submitAnswerDto)
        {
            var battle = await _quizService.SubmitAnswerAsync(
                battleId,
                submitAnswerDto.PlayerId,
                submitAnswerDto.QuestionId,
                submitAnswerDto.AnswerIndex);
            
            return Ok(battle);
        }
    }
}
