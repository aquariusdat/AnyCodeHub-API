using System.Reflection;

namespace AnyCodeHub.Contract;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
