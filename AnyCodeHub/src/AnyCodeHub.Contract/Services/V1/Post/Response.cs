namespace AnyCodeHub.Contract.Services.V1.Post;

public static class Response
{
    public record PostResponse(
        Guid Id,
        string Title,
        string Slug,
        string? Summary,
        string? Content,
        string? FeaturedImageUrl,
        Guid AuthorId,
        string AuthorName,
        string AuthorAvatarUrl,
        List<CategoryInfo> Categories,
        int Views,
        int Likes,
        int CommentCount,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    public record PostDetailResponse(
        Guid Id,
        string Title,
        string Slug,
        string? Summary,
        string? Content,
        string? FeaturedImageUrl,
        Guid AuthorId,
        string AuthorName,
        string AuthorAvatarUrl,
        List<CategoryInfo> Categories,
        List<TagInfo> Tags,
        List<CommentInfo> Comments,
        int Views,
        int Likes,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    public record CategoryInfo(
        Guid Id,
        string Name,
        string? Description);

    public record TagInfo(
        Guid Id,
        string Name);

    public record CommentInfo(
        Guid Id,
        string Content,
        Guid AuthorId,
        string AuthorName,
        string AuthorAvatarUrl,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}