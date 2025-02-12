namespace AnyCodeHub.Domain.Exceptions;
public static class TagException
{
    public class TagExistsException : ExistsException
    {
        public TagExistsException(string TagName) : base($"The tag name [{TagName}] already exists.")
        {

        }
    }

    public class TagNameContainsWhiteSpaceCharacterException : Exception
    {
        public TagNameContainsWhiteSpaceCharacterException(string TagName) : base($"The tag name [{TagName}] shouldn't contain white spaces.")
        {

        }
    }
    public class NotFoundTagByIdException : NotFoundException
    {
        public NotFoundTagByIdException(Guid Id) : base($"The tag with Id [{Id}] was not found.")
        {

        }
    }

    public class TagHasBeenDeletedException : AlreadyDeletedException
    {
        public TagHasBeenDeletedException(Guid Id) : base($"The tag with Id [{Id}] has been deleted.")
        {

        }
    }
}
