using Microsoft.EntityFrameworkCore;
using WalkHomeSafeAPI.Data;
using WalkHomeSafeAPI.Models.DTOs;
using WalkHomeSafeAPI.Models.DTOs.Save;
using WalkHomeSafeAPI.Models.Entities;

namespace WalkHomeSafeAPI.Services;

public class CategoryService(AppDbContext context) : ICategoryService
{
    public IReadOnlyCollection<ReportCategoryDto> GetAll()
    {
        var entities = context.ReportCategories.AsQueryable();
        return ProjectToDto(entities).ToList();
    }

    public bool Create(SaveReportCategoryDto saveReportCategory)
    {
        var entity = ProjectToEntity(saveReportCategory);
        context.ReportCategories.Add(entity);
        context.SaveChanges();

        return true;
    }

    public bool Update(int id, SaveReportCategoryDto saveReportCategory)
    {
        var dbEntity = context.ReportCategories.SingleOrDefault(x => x.Id == id);
        if (dbEntity is null) return false;

        dbEntity = ProjectToEntity(saveReportCategory, dbEntity);
        context.ReportCategories.Update(dbEntity);
        context.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var dbEntity = context.ReportCategories.SingleOrDefault(x => x.Id == id);
        if (dbEntity is null) return false;

        context.ReportCategories.Remove(dbEntity);
        context.SaveChanges();

        return true;
    }

    private IQueryable<ReportCategoryDto> ProjectToDto(IQueryable<ReportCategoryEntity> entities)
            => entities.Select(e => new ReportCategoryDto
            {
                Id = e.Id,
                Name = e.Name
            });

    private ReportCategoryEntity ProjectToEntity(SaveReportCategoryDto saveReportCategory, ReportCategoryEntity? existing = null)
    {
        if (existing is not null)
        {
            context.Entry(existing).State = EntityState.Detached;
        }

        return new ReportCategoryEntity
        {
            Id = existing?.Id ?? 0,
            Name = saveReportCategory.Name
        };
    }
}
