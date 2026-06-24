using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using WalkHomeSafeAPI.Data;
using WalkHomeSafeAPI.Models.Context;
using WalkHomeSafeAPI.Models.DTOs;
using WalkHomeSafeAPI.Models.DTOs.Save;
using WalkHomeSafeAPI.Models.Entities;

namespace WalkHomeSafeAPI.Services
{
    public class ReportService(AppDbContext context, IUserService userService) : IReportService
    {
        public IReadOnlyCollection<ReportDto> GetAll(LocationDto location)
        {
            var entities = GetBaseQuery(location: location);
            return ProjectToDto(entities).ToList();
        }

        public IReadOnlyCollection<ReportDto> GetReportsByUser(AppUserContext user)
        {
            var userDbId = userService.GetUserIdFromDatabase(user);
            if (userDbId == 0) return [];

            var entities = GetBaseQuery(userId: userDbId);
            return ProjectToDto(entities).ToList();
        }

        public bool Create(AppUserContext user, SaveReportDto saveReport)
        {
            var userDbId = userService.GetUserIdFromDatabase(user);
            if (userDbId == 0) return false;

            var entity = ProjectToEntity(userDbId, saveReport);
            context.Reports.Add(entity);
            context.SaveChanges();

            return true;
        }

        public bool Update(AppUserContext user, int id, SaveReportDto saveReport)
        {
            var dbEntity = GetBaseQuery(id).SingleOrDefault();
            var userDbId = userService.GetUserIdFromDatabase(user);
            if (dbEntity is null || userDbId == 0 || dbEntity.UserId != userDbId) return false;

            dbEntity = ProjectToEntity(userDbId, saveReport, dbEntity);
            context.Reports.Update(dbEntity);
            context.SaveChanges();

            return true;
        }

        public bool Delete(AppUserContext user, int id)
        {
            var dbEntity = context.Reports.Find(id);
            var userDbId = userService.GetUserIdFromDatabase(user);
            if (dbEntity is null || userDbId == 0 || dbEntity.UserId != userDbId) return false;

            context.Reports.Remove(dbEntity);
            context.SaveChanges();

            return true;
        }

        public IReadOnlyCollection<ReportVoteDto> GetVotesByUser(AppUserContext user)
        {
            var userDbId = userService.GetUserIdFromDatabase(user);
            if (userDbId == 0) return [];

            return context.ReportVotes
                .Where(v => v.UserId == userDbId)
                .Select(v => new ReportVoteDto
                {
                    ReportId = v.ReportId,
                    IsUpvote = v.IsUpvote
                })
                .ToList();
        }

        public bool CreateOrUpdateVotes(AppUserContext user, IReadOnlyCollection<SaveReportVoteDto> votes)
        {
            var userDbId = userService.GetUserIdFromDatabase(user);
            if (userDbId == 0) return false;

            var userVotes = context.ReportVotes.Where(v => v.UserId == userDbId).ToList();
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
                        UserId = userDbId,
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

        private IQueryable<ReportEntity> GetBaseQuery(int id = default, int userId = default, LocationDto? location = null)
        {
            var query = context.Reports
                .Include(r => r.User)
                .Include(r => r.Ratings)
                .ThenInclude(r => r.Category)
                .Include(r => r.Votes)
                .AsNoTracking();

            if (id != default)
            {
                query = query.Where(report => report.Id == id);
            }

            if (userId != default)
            {
                query = query.Where(report => report.UserId == userId);
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
                UserName = e.User == null ? string.Empty : e.User.IsActive ? e.User.UserName : "Anonym",
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

        private ReportEntity ProjectToEntity(int userDbId, SaveReportDto saveReport, ReportEntity? existing = null)
        {
            return new ReportEntity
            {
                Id = existing?.Id ?? 0,
                UserId = userDbId,
                Title = saveReport.Title,
                Description = saveReport.Description,
                Latitude = saveReport.Latitude,
                Longitude = saveReport.Longitude,
                Location = new Point(saveReport.Longitude, saveReport.Latitude) { SRID = 4326 },
                CreatedAt = existing?.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = existing is not null ? DateTime.UtcNow : null,
                Ratings = saveReport.RatingCategories.Select(category =>
                {
                    var existingRating = existing?.Ratings.FirstOrDefault(r => r.Category != null && r.Category.Name == category.Name);
                    return new ReportRatingEntity
                    {
                        Id = existingRating?.Id ?? 0,
                        ReportId = existing?.Id ?? 0,
                        CategoryId = context.ReportCategories.SingleOrDefault(c => c.Name == category.Name)?.Id ?? 0,
                        Rating = category.Rating
                    };
                }).ToList()
            };
        }
    }
}
