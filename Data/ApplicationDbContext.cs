using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InterviewApp.Models;

namespace InterviewApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
	public DbSet<Question> Questions { get; set; } = default!;
	public DbSet<Category> Categories { get; set; } = default!;
	public DbSet<Attempt> Attempts { get; set; } = default!;
}
