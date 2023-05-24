namespace Devly.Models;

public class MemoryCacheEntry<T>
{
    public IReadOnlyList<T> Entries { get; set; }
    public int FilterHash { get; set; }
}