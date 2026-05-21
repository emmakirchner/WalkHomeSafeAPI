using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using WalkHomeSafeAPI.Data;
using WalkHomeSafeAPI.Models.DTOs;
using WalkHomeSafeAPI.Models.DTOs.Save;
using WalkHomeSafeAPI.Models.Entities;

namespace WalkHomeSafeAPI.Services
{
    public class ReportService(AppDbContext context) : IReportService
    {
        public IReadOnlyCollection<ReportDto> GetAll(LocationDto location)
        {
            var entities = GetBaseQuery(location: location);
            return ProjectToDto(entities).ToList();
        }

        public bool Create(SaveReportDto saveReport)
        {
            var entity = ProjectToEntity(saveReport);
            context.Reports.Add(entity);
            context.SaveChanges();

            return true;
        }

        public bool Update(int id, SaveReportDto saveReport)
        {
            var dbEntity = GetBaseQuery(id).SingleOrDefault();
            if (dbEntity is null) return false;

            dbEntity = ProjectToEntity(saveReport);
            context.Reports.Update(dbEntity);
            context.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var dbEntity = GetBaseQuery(id).SingleOrDefault();
            if (dbEntity is null) return false;

            context.Reports.Remove(dbEntity);
            context.SaveChanges();

            return true;
        }

        public bool CreateOrUpdateVotes(string username, IReadOnlyCollection<SaveReportVoteDto> votes)
        {
            var user = context.Users.SingleOrDefault(u => u.UserName == username);
            if (user is null) return false;

            var userVotes = context.ReportVotes.Where(v => v.UserId == user.Id).ToList();
            List<ReportVoteEntity> newVotes = [];
            foreach (var vote in votes)
            {
                var existing = userVotes.SingleOrDefault(v => v.ReportId == vote.ReportId);

                if (vote.IsUpvote == null)
                {
                    if (existing != null)
                    {
                        context.ReportVotes.Remove(existing);
                    }
                    continue;
                }

                if (existing != null)
                {
                    existing.IsUpvote = vote.IsUpvote.Value;
                }
                else
                {
                    newVotes.Add(new ReportVoteEntity
                    {
                        UserId = user.Id,
                        ReportId = vote.ReportId,
                        IsUpvote = vote.IsUpvote.Value
                    });
                }

            }

            if (newVotes.Count != 0)
            {
                context.AddRange(newVotes);
            }

            context.SaveChanges();
            return true;
        }

        private IQueryable<ReportEntity> GetBaseQuery(int id = default, LocationDto? location = null)
        {
            var query = context.Reports
                .Include(r => r.User)
                .Include(r => r.Ratings)
                .ThenInclude(r => r.Category)
                .Include(r => r.Votes)
                .AsQueryable();

            if (id != default)
            {
                query = query.Where(report => report.Id == id);
            }

            if (location is not null && location.Longitude.HasValue && location.Latitude.HasValue)
            {
                var point = new Point(location.Longitude.Value, location.Latitude.Value);
                query = query.Where(report => report.Location != null && report.Location.Distance(point) <= location.RadiusInMeters);
            }

            return query;
        }

        private IQueryable<ReportDto> ProjectToDto(IQueryable<ReportEntity> entities)
            => entities.Select(e => new ReportDto
            {
                Id = e.Id,
                UserName = e.User != null ? e.User.UserName : string.Empty,
                Title = e.Title,
                Description = e.Description,
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                RatingCategories = e.Ratings.Select(r => new ReportRatingDto
                {
                    Id = r.Category != null ? r.Category.Id : 0,
                    Name = r.Category != null ? r.Category.Name : string.Empty,
                    Rating = r.Rating,
                }).ToList(),
                UpvoteCount = e.Votes.Count(v => v.IsUpvote),
                DownvoteCount = e.Votes.Count(v => !v.IsUpvote)
            });

        private ReportEntity ProjectToEntity(SaveReportDto saveReport)
            => new ReportEntity // ToDo: get user id
            {
                Title = saveReport.Title,
                Description = saveReport.Description,
                Latitude = saveReport.Latitude,
                Longitude = saveReport.Longitude,
                Location = new Point(saveReport.Longitude, saveReport.Latitude) { SRID = 4326 },
                CreatedAt = DateTime.UtcNow,
                Ratings = saveReport.RatingCategories
                    .Select(r => new ReportRatingEntity
                    {
                        CategoryId = context.ReportCategories.SingleOrDefault(c => c.Name == r.Name)?.Id ?? 0,
                        Rating = r.Rating
                    }).ToList()
            };
    }
}
