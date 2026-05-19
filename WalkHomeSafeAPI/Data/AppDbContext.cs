using Microsoft.EntityFrameworkCore;
using WalkHomeSafeAPI.Models.Entities;

namespace WalkHomeSafeAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ReportEntity> Reports { get; set; }
    public DbSet<ReportCategoryEntity> ReportCategories { get; set; }
    public DbSet<ReportRatingEntity> ReportRatings { get; set; }
    public DbSet<ReportVoteEntity> ReportVotes { get; set; }
    public DbSet<UserEntity> Users { get; set; }
}