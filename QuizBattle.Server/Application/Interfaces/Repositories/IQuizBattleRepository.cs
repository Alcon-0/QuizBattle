using System;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IQuizBattleRepository : IRepository<QuizBattle, Guid>
{
    Task<QuizBattle?> GetByIdWithAnswersAsync(Guid battleId);
}
