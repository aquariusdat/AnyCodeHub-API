using AnyCodeHub.Contract.Enumerations;

namespace AnyCodeHub.Contract.Extensions;
public static class SortOrderExtension
{
    public static SortOrder ConvertStringToSortOrder(this string? sortOrder) => !string.IsNullOrWhiteSpace(sortOrder)
                        ?
                            sortOrder.ToLower().Equals("asc")
                            ? SortOrder.Ascending
                            : SortOrder.Descending

                        : SortOrder.Descending;

    public static Dictionary<string, SortOrder> ConvertStringToSortColumnAndOrder(this string? sortColumnAndOrder)
    {
        Dictionary<string, SortOrder> dicColumnAndOrder = new Dictionary<string, SortOrder>();

        if (!string.IsNullOrEmpty(sortColumnAndOrder))
        {
            if (sortColumnAndOrder.Trim().Split(',').Length > 0)
            {
                foreach (var sortColumnAndOrderItem in sortColumnAndOrder.Trim().Split(','))
                {
                    if (!sortColumnAndOrderItem.Contains("-"))
                        throw new FormatException("Sort condition should be follow by format: Column1-ASC,Column2-DESC,...");

                    var property = sortColumnAndOrderItem.Trim().Split("-");
                    var key = property[0].GetSortProductProperty();
                    var value = property[1].ConvertStringToSortOrder();
                    dicColumnAndOrder[key] = value;
                }
            }
            else
            {
                if (!sortColumnAndOrder.Contains("-"))
                    throw new FormatException("Sort condition should be follow by format: Column1-ASC,Column2-DESC,...");

                var property = sortColumnAndOrder.Trim().Split("-");
                var key = property[0].GetSortProductProperty();
                var value = property[1].ConvertStringToSortOrder();
                dicColumnAndOrder[key] = value;
            }
        }

        return dicColumnAndOrder;
    }
}
