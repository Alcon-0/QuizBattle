using System;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuizBattleRepository : Repository<QuizBattle, Guid>, IQuizBattleRepository
{
    private readonly AppDbContext _context;

    public QuizBattleRepository(AppDbContext context) : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this._context = context;
    }
    
    public Task<QuizBattle?> GetByIdWithAnswersAsync(Guid battleId)
    {
        return _context.Set<QuizBattle>()
            .Include(q => q.Quiz)
            .ThenInclude(q => q.Questions)
            .ThenInclude(q => q.Options)
            .Include(q => q.PlayerAnswers)
            .FirstOrDefaultAsync(q => q.Id == battleId);
    }
}
