using Microsoft.EntityFrameworkCore;
using WebApp.Domain;

namespace WebApp.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Answer> Answers { get; set; } = default!;
    
    public DbSet<AnsweredQuiz> AnsweredQuizzes { get; set; } = default!;
    
    public DbSet<Question> Questions { get; set; } = default!;
    
    public DbSet<Quiz> Quizzes { get; set; } = default!;
    
    public DbSet<QuizAnswer> QuizAnswers { get; set; } = default!;
        
    
    protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);

        foreach (var relationship in modelBuilder.Model
                     .GetEntityTypes()
                     .Where(e => !e.IsOwned())
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}