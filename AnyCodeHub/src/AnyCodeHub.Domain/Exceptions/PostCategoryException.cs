namespace AnyCodeHub.Domain.Exceptions;

public static class PostCategoryException
{
    public class PostCategoryNotFoundException : NotFoundException
    {
        public PostCategoryNotFoundException(Guid PostId, Guid CategoryId) : base($"The post's cateogry with PostId [{PostId}] and CategoryId [{CategoryId}] don't exists.") { }
    }

}
