using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Course.Query;
using static AnyCodeHub.Contract.Services.V1.Course.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Course;

public class GetCourseByIdQueryHandler : IQueryHandler<GetCourseByIdQuery, CourseDetailResponse>
{
    private readonly ILogger<GetCourseByIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IRepositoryBase<Domain.Entities.Rating, Guid> _ratingRepository;
    private readonly IRepositoryBase<Domain.Entities.UserCourse, Guid> _userCourseRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IRepositoryBase<Domain.Entities.CourseRequirement, Guid> _courseRequirementRepository;
    private readonly IRepositoryBase<Domain.Entities.CourseBenefit, Guid> _courseBenefitRepository;
    private readonly IRepositoryBase<Domain.Entities.CourseQA, Guid> _courseQARepository;
    private readonly IRepositoryBase<Domain.Entities.CourseCategory, Guid> _courseCategoryRepository;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IRepositoryBase<Domain.Entities.CourseTechnology, Guid> _courseTechnologyRepository;
    private readonly IRepositoryBase<Domain.Entities.Technology, Guid> _technologyRepository;

    public GetCourseByIdQueryHandler(
        ILogger<GetCourseByIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IRepositoryBase<Domain.Entities.Rating, Guid> ratingRepository,
        IRepositoryBase<Domain.Entities.UserCourse, Guid> userCourseRepository,
        UserManager<AppUser> userManager,
        IRepositoryBase<Domain.Entities.CourseRequirement, Guid> courseRequirementRepository,
        IRepositoryBase<Domain.Entities.CourseBenefit, Guid> courseBenefitRepository,
        IRepositoryBase<Domain.Entities.CourseQA, Guid> courseQARepository,
        IRepositoryBase<Domain.Entities.CourseCategory, Guid> courseCategoryRepository,
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository,
        IRepositoryBase<Domain.Entities.CourseTechnology, Guid> courseTechnologyRepository,
        IRepositoryBase<Domain.Entities.Technology, Guid> technologyRepository)
    {
        _logger = logger;
        _courseRepository = courseRepository;
        _sectionRepository = sectionRepository;
        _lessonRepository = lessonRepository;
        _ratingRepository = ratingRepository;
        _userCourseRepository = userCourseRepository;
        _userManager = userManager;
        _courseRequirementRepository = courseRequirementRepository;
        _courseBenefitRepository = courseBenefitRepository;
        _courseQARepository = courseQARepository;
        _courseCategoryRepository = courseCategoryRepository;
        _categoryRepository = categoryRepository;
        _courseTechnologyRepository = courseTechnologyRepository;
        _technologyRepository = technologyRepository;
    }

    public async Task<Result<CourseDetailResponse>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the course by ID
            var course = await _courseRepository.FindByIdAsync(request.Id, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<CourseDetailResponse>(
                    new Error("Course.NotFound", $"Course with ID {request.Id} not found."));
            }

            // Get instructor details
            var instructor = await _userManager.FindByIdAsync(course.AuthorId.ToString());
            if (instructor == null)
            {
                return Result.Failure<CourseDetailResponse>(
                    new Error("Course.InstructorNotFound", $"Instructor with ID {course.AuthorId} not found."));
            }

            // Calculate average rating
            var ratings = _ratingRepository.FindAll(r => r.CourseId == course.Id && !r.IsDeleted).ToList();
            double averageRating = ratings.Any() ? ratings.Average(r => r.Rate) : 0;

            // Count total students enrolled
            int totalStudents = _userCourseRepository.FindAll(uc => uc.CourseId == course.Id && !uc.IsDeleted).Count();

            // Get sections with their lessons
            var sections = _sectionRepository.FindAll(s => s.CourseId == course.Id && !s.IsDeleted)
                .OrderBy(s => s.CreatedAt)
                .ToList();

            var sectionInfos = new List<SectionInfo>();
            int totalLessons = 0;

            foreach (var section in sections)
            {
                // Get lessons for the section
                var lessons = _lessonRepository.FindAll(l =>
                    (l.SectionId == section.Id || l.CourseId == course.Id) && !l.IsDeleted)
                    .OrderBy(l => l.CreatedAt)
                    .ToList();

                totalLessons += lessons.Count;

                var lessonInfos = lessons.Select(l => new LessonInfo(
                    l.Id,
                    l.Title,
                    l.Description,
                    l.VideoUrl,
                    l.PdfUrl,
                    l.Duration
                )).ToList();

                sectionInfos.Add(new SectionInfo(
                    section.Id,
                    section.Name,
                    lessonInfos
                ));
            }

            // Get course requirements
            var requirements = _courseRequirementRepository.FindAll(r => r.CourseId == course.Id && !r.IsDeleted)
                .Select(r => new RequirementInfo(r.Id, r.RequirementContent))
                .ToList();

            // Get course benefits
            var benefits = _courseBenefitRepository.FindAll(b => b.CourseId == course.Id && !b.IsDeleted)
                .Select(b => new BenefitInfo(b.Id, b.BenefitContent, b.Description))
                .ToList();

            // Get course Q&As
            var qaList = _courseQARepository.FindAll(q => q.CourseId == course.Id && !q.IsDeleted)
                .Select(q => new QAInfo(q.Id, q.Question, q.Answer))
                .ToList();

            // Get course categories
            var courseCategories = _courseCategoryRepository.FindAll(cc => cc.CourseId == course.Id && !cc.IsDeleted).ToList();
            var categoryInfos = new List<CategoryInfo>();

            foreach (var cc in courseCategories)
            {
                var category = await _categoryRepository.FindByIdAsync(cc.CategoryId, cancellationToken);
                if (category != null && !category.IsDeleted)
                {
                    categoryInfos.Add(new CategoryInfo(
                        category.Id,
                        category.Name,
                        category.Description
                    ));
                }
            }

            // Get course technologies
            var courseTechnologies = _courseTechnologyRepository.FindAll(ct => ct.CourseId == course.Id && !ct.IsDeleted).ToList();
            var technologyInfos = new List<TechnologyInfo>();

            foreach (var ct in courseTechnologies)
            {
                var technology = await _technologyRepository.FindByIdAsync(ct.TechnologyId, cancellationToken);
                if (technology != null)
                {
                    technologyInfos.Add(new TechnologyInfo(
                        technology.Id,
                        technology.Name,
                        technology.Description
                    ));
                }
            }

            // Create the detailed course response
            var courseDetail = new CourseDetailResponse(
                course.Id,
                course.Name,
                course.Description,
                course.ImageUrl,
                course.Price,
                course.AuthorId,
                instructor.UserName,
                instructor.Avatar,
                averageRating,
                totalStudents,
                totalLessons,
                sections.Count,
                sectionInfos,
                requirements,
                benefits,
                qaList,
                technologyInfos,
                categoryInfos,
                course.CreatedAt,
                course.UpdatedAt);

            return Result.Success(courseDetail);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting course by ID: {ex.Message}");
            return Result.Failure<CourseDetailResponse>(
                new Error("Course.QueryFailed", $"Failed to query course: {ex.Message}"));
        }
    }
}