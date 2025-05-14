using System;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

public class BattleHub : Hub
{
    private readonly IQuizService _quizService;

    public BattleHub(IQuizService quizService)
    {
        _quizService = quizService;
    }

    public async Task JoinBattle(Guid battleId, string playerId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, battleId.ToString());
        await Clients.Group(battleId.ToString()).SendAsync("PlayerJoined", playerId);
    }

    public async Task SubmitAnswer(Guid battleId, string playerId, Guid questionId, int answerIndex)
    {
        var battle = await _quizService.SubmitAnswerAsync(battleId, playerId, questionId, answerIndex);
        await Clients.Group(battleId.ToString()).SendAsync("AnswerProcessed", battle);
    }
}