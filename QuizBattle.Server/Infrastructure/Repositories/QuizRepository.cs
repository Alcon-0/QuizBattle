using System;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuizRepository : Repository<Quiz, Guid>, IQuizRepository
{
    public QuizRepository(AppDbContext context) : base(context)
    {
    }
}

