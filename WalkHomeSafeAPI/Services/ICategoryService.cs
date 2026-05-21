using WalkHomeSafeAPI.Models.DTOs;
using WalkHomeSafeAPI.Models.DTOs.Save;

namespace WalkHomeSafeAPI.Services;

public interface ICategoryService
{
    IReadOnlyCollection<ReportCategoryDto> GetAll();

    bool Create(SaveReportCategoryDto saveReportCategory);

    bool Update(int id, SaveReportCategoryDto saveReportCategory);

    bool Delete(int id);
}
