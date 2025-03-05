using AnyCodeHub.Domain.Entities.Identity;
using AutoMapper;

namespace AnyCodeHub.Application.Mappers;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        // V1
        CreateMap<Contract.Services.V1.Authentication.Command.RegisterCommand, AppUser>().ReverseMap();
        //CreateMap<Post, Contract.Services.V1.Post.Response.PostResponse>().ReverseMap();
        //CreateMap<PagedResult<Post>, PagedResult<Contract.Services.V1.Post.Response.PostResponse>>().ReverseMap();

        //CreateMap<Category, Contract.Services.V1.Category.Response.CategoryResponse>().ReverseMap();
        //CreateMap<PagedResult<Category>, PagedResult<Contract.Services.V1.Category.Response.CategoryResponse>>().ReverseMap();

        //CreateMap<Response.AuthenticatedResponse, AuthToken>().ReverseMap();
        ////CreateMap<List<Post>, List<Response.PostResponse>>().ReverseMap();

        //// V2
        //CreateMap<Post, Contract.Services.V2.Post.Response.PostResponse>().ReverseMap();
        //CreateMap<Result<List<Post>>, Result<List<Contract.Services.V2.Post.Response.PostResponse>>>().ReverseMap();

        //CreateMap<Category, Contract.Services.V2.Category.Response.CategoryResponse>().ReverseMap();
        //CreateMap<Result<List<Category>>, Result<List<Contract.Services.V2.Category.Response.CategoryResponse>>>().ReverseMap();

        //// Map command entity
        //CreateMap<Post, Contract.Services.V1.Post.DomainEvent.PostCreated>().ConstructUsing(t => new Contract.Services.V1.Post.DomainEvent.PostCreated(Guid.NewGuid(), t.Id, t.AuthorId, t.ParentId, t.Title, t.Slug, t.Summary, t.Content, t.CreatedBy, t.CreatedAt));
        //CreateMap<Contract.Services.V1.Post.DomainEvent.PostCreated, Post>().ConstructUsing(t => new Post());

        //CreateMap<Command.RegisterCommand, AppUser>().ReverseMap();

        //CreateMap<Tag, Contract.Services.V1.Tag.Response.TagResponse>().ReverseMap();
    }
}
