using System.Reflection;

namespace AnyCodeHub.Infrastructure.Auth;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
