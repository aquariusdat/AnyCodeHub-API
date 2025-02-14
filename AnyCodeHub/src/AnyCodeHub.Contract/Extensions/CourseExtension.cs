namespace AnyCodeHub.Contract.Extensions;
public static class CourseExtension
{
    public static string GetSortProductProperty(this string? PropertyName)
    {
        string propertyName = "CreatedAt";

        if (string.IsNullOrEmpty(PropertyName))
        {
            propertyName = PropertyName.Trim().ToLower() switch
            {
                "title" => "Title",
                _ => "CreatedAt"
            };
        }

        return propertyName;
    }
}
