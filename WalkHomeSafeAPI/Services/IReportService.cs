using WalkHomeSafeAPI.Models.DTOs;
using WalkHomeSafeAPI.Models.DTOs.Save;

namespace WalkHomeSafeAPI.Services;

public interface IReportService
{
    IReadOnlyCollection<ReportDto> GetAll(LocationDto location);

    bool Create(SaveReportDto saveReport);

    bool Update(int id, SaveReportDto saveReport);

    bool Delete(int id);

    bool CreateOrUpdateVotes(string username, IReadOnlyCollection<SaveReportVoteDto> votes);
}
