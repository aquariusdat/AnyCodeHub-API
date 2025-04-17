using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Contract.Services.V1.Course;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Course;

public class CreateCourseCommandHandler : ICommandHandler<Command.CreateCourseCommand, Response.CourseResponse>
{
    private readonly ILogger<CreateCourseCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Lesson, Guid> _lessonRepository;
    private readonly IRepositoryBase<SectionLesson, Guid> _sectionLessonRepository;
    private readonly IRepositoryBase<CourseBenefit, Guid> _courseBenefitRepository;
    private readonly IRepositoryBase<CourseTechnology, Guid> _courseTechnologyRepository;
    private readonly IRepositoryBase<CourseCategory, Guid> _courseCategoryRepository;
    private readonly IRepositoryBase<CourseQA, Guid> _courseQARepository;
    private readonly IRepositoryBase<CourseRequirement, Guid> _courseRequirementRepository;
    private readonly IMapper _mapper;

    public CreateCourseCommandHandler(
        ILogger<CreateCourseCommandHandler> logger, 
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IRepositoryBase<Section, Guid> sectionRepository,
        IRepositoryBase<Lesson, Guid> lessonRepository,
        IRepositoryBase<SectionLesson, Guid> sectionLessonRepository,
        IRepositoryBase<CourseBenefit, Guid> courseBenefitRepository,
        IRepositoryBase<CourseTechnology, Guid> courseTechnologyRepository,
        IRepositoryBase<CourseCategory, Guid> courseCategoryRepository,
        IRepositoryBase<CourseQA, Guid> courseQARepository,
        IRepositoryBase<CourseRequirement, Guid> courseRequirementRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseRepository = courseRepository;
        _sectionRepository = sectionRepository;
        _lessonRepository = lessonRepository;
        _sectionLessonRepository = sectionLessonRepository;
        _courseBenefitRepository = courseBenefitRepository;
        _courseTechnologyRepository = courseTechnologyRepository;
        _courseCategoryRepository = courseCategoryRepository;
        _courseQARepository = courseQARepository;
        _courseRequirementRepository = courseRequirementRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.CourseResponse>> Handle(Command.CreateCourseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if a course with the same name or slug already exists
            var existingCourse = _courseRepository.FindAll(c =>
                c.Name.ToLower() == request.name.ToLower() ||
                (!string.IsNullOrEmpty(request.slug) && !string.IsNullOrEmpty(c.Slug) && c.Slug.ToLower() == request.slug.ToLower())
            ).FirstOrDefault();

            if (existingCourse != null)
            {
                // A course with the same name or slug already exists
                string duplicateField = existingCourse.Name.ToLower() == request.name.ToLower() ? "name" : "slug";
                return Result.Failure<Response.CourseResponse>(
                    new Error("Course.Duplicate", $"A course with this {duplicateField} already exists."));
            }

            // Create a new course entity
            var course = Domain.Entities.Course.Create(
                request.name,
                request.description,
                request.price,
                request.salePrice,
                request.imageUrl,
                request.videoUrl,
                request.slug,
                request.status,
                request.authorId,
                request.level,
                request.totalViews,
                request.totalDuration,
                request.rating,
                request.createdBy);

            // Add to repository
            _courseRepository.Add(course);

            // Create sections and lessons if provided
            if (request.sections != null && request.sections.Any())
            {
                foreach (var sectionDto in request.sections)
                {
                    // Create section
                    var section = Section.Create(
                        sectionDto.Name,
                        course.Id,
                        request.createdBy);
                    
                    _sectionRepository.Add(section);

                    // Create lessons for this section if provided
                    if (sectionDto.Lessons != null && sectionDto.Lessons.Any())
                    {
                        foreach (var lessonDto in sectionDto.Lessons)
                        {
                            // Create lesson
                            var lesson = Lesson.Create(
                                lessonDto.Title,
                                lessonDto.Description,
                                lessonDto.VideoUrl,
                                lessonDto.AttachmentUrl,
                                section.Id,
                                course.Id,
                                lessonDto.Duration,
                                lessonDto.CreatedBy);
                            
                            _lessonRepository.Add(lesson);

                            // Create section-lesson relationship
                            var sectionLesson = SectionLesson.Create(
                                section.Id,
                                lesson.Id,
                                lessonDto.CreatedBy);
                            
                            _sectionLessonRepository.Add(sectionLesson);
                        }
                    }
                }
            }

            // Create course benefits if provided
            if (request.benefits != null && request.benefits.Any())
            {
                foreach (var benefitDto in request.benefits)
                {
                    var benefit = CourseBenefit.Create(
                        benefitDto.Description,
                        course.Id,
                        benefitDto.Description,
                        benefitDto.CreatedBy);
                    
                    _courseBenefitRepository.Add(benefit);
                }
            }

            // Create course technologies if provided
            if (request.technologies != null && request.technologies.Any())
            {
                foreach (var technologyDto in request.technologies)
                {
                    var technology = CourseTechnology.Create(
                        course.Id,
                        technologyDto.TechnologyId);
                    
                    _courseTechnologyRepository.Add(technology);
                }
            }

            // Create course categories if provided
            if (request.categories != null && request.categories.Any())
            {
                foreach (var categoryDto in request.categories)
                {
                    var category = CourseCategory.Create(
                        course.Id,
                        categoryDto.CategoryId,
                        categoryDto.CreatedBy);
                    
                    _courseCategoryRepository.Add(category);
                }
            }

            // Create course Q&As if provided
            if (request.qAs != null && request.qAs.Any())
            {
                foreach (var qaDto in request.qAs)
                {
                    var qa = CourseQA.Create(
                        qaDto.Question,
                        qaDto.Answer,
                        course.Id);
                    
                    _courseQARepository.Add(qa);
                }
            }

            // Create course requirements if provided
            if (request.requirements != null && request.requirements.Any())
            {
                foreach (var requirementDto in request.requirements)
                {
                    var requirement = CourseRequirement.Create(
                        requirementDto.Description,
                        course.Id,
                        requirementDto.CreatedBy);
                    
                    _courseRequirementRepository.Add(requirement);
                }
            }

            // Map to response and return
            var response = _mapper.Map<Response.CourseResponse>(course);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating course: {ex.Message}");
            return Result.Failure<Response.CourseResponse>(
                new Error("Course.CreateFailed", $"Failed to create course: {ex.Message}"));
        }
    }
}
