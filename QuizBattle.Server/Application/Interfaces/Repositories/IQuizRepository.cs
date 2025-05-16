using System;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IQuizRepository : IRepository<Quiz, Guid>
{
    Task<Quiz?> GetByIdWithQuestionsAsync(Guid quizId);
}
