using System;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuestionRepository : Repository<Question, Guid>, IQuestionRepository
{
    private readonly AppDbContext  _context;
    
    public QuestionRepository(AppDbContext  context) : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this._context = context;
    }

    public Task<Question?> GetByIdWithQuizAsync(Guid questionId)
    {
        return _context.Set<Question>()
            .Include(q => q.Quiz)
            .FirstOrDefaultAsync(q => q.Id == questionId);
    }

    public async Task<Question?> GetByIdWithOptionsAsync(Guid questionId)
    {
        return await _context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == questionId);
    }
}
