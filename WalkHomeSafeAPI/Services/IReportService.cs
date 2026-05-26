using WalkHomeSafeAPI.Models.Context;
using WalkHomeSafeAPI.Models.DTOs;
using WalkHomeSafeAPI.Models.DTOs.Save;

namespace WalkHomeSafeAPI.Services;

public interface IReportService
{
    IReadOnlyCollection<ReportDto> GetAll(LocationDto location);

    bool Create(AppUserContext user, SaveReportDto saveReport);

    bool Update(AppUserContext user, int id, SaveReportDto saveReport);

    bool Delete(AppUserContext user, int id);

    bool CreateOrUpdateVotes(AppUserContext user, IReadOnlyCollection<SaveReportVoteDto> votes);
}
