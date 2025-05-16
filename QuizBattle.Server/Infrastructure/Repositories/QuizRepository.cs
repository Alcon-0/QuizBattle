using System;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuizRepository : Repository<Quiz, Guid>, IQuizRepository
{
    private readonly AppDbContext _context;
    public QuizRepository(AppDbContext context) : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this._context = context;
    }

    public Task<Quiz?> GetByIdWithQuestionsAsync(Guid quizId)
    {
        return _context.Set<Quiz>()
            .Include(q => q.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == quizId);
    }
}

