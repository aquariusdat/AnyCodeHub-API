using System.ComponentModel.DataAnnotations;

namespace AnyCodeHub.Persistence.DependencyInjections.Options;
public record PostgreSqlRetryOptions
{
    [Required, Range(5, 20)]
    public int MaxRetryCount { get; set; }
    [Required, Timestamp]
    public TimeSpan MaxRetryDelay { get; set; }
    public ICollection<string>? ErrorNumbersToAdd { get; set; }

}
