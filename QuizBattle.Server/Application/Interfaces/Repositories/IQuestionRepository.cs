using System;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IQuestionRepository : IRepository<Question, Guid> { }