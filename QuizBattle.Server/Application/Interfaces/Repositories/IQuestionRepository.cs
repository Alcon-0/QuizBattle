using System;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IQuestionRepository : IRepository<Question, Guid>
{
    Task<Question?> GetByIdWithOptionsAsync(Guid questionId);
    Task<Question?> GetByIdWithQuizAsync(Guid questionId);
}