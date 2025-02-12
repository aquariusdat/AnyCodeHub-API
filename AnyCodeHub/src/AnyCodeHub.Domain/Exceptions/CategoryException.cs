namespace AnyCodeHub.Domain.Exceptions;
public static class CategoryException
{
    public class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(Guid postId)
            : base($"The category with the id {postId} was not found.") { }
    }
}
