using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace JobDrop.Domain.Common;

public static class Guard
{
    public static T AgainstNull<T>(
        [NotNull] T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : class
    {
        if (value is null)
            throw new ArgumentNullException(paramName);
        return value;
    }

    public static string AgainstNullOrEmpty(
        [NotNull] string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or empty.", paramName);
        return value;
    }

    public static Guid AgainstEmpty(
        Guid value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("GUID cannot be empty.", paramName);
        return value;
    }

    public static int AgainstNegative(
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
        return value;
    }

    public static int AgainstNegativeOrZero(
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, "Value must be positive.");
        return value;
    }

    public static decimal AgainstNegative(
        decimal value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
        return value;
    }
}