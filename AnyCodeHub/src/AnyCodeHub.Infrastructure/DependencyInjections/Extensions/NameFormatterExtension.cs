using System.Reflection;
using MassTransit;

namespace AnyCodeHub.Infrastructure.DependencyInjections.Extensions;
public static class NameFormatterExtension
{
    public static string ToKebabCaseString(this MemberInfo member) => KebabCaseEndpointNameFormatter.Instance.SanitizeName(member.Name);
}

internal class KebabCaseEntityNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>() => typeof(T).ToKebabCaseString();
}
