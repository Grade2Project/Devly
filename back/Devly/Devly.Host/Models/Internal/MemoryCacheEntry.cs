namespace Devly.Models;

public class MemoryCacheEntry<T> where T : class
{
    public IReadOnlyList<T> Entries { get; set; }
    public int FilterHash { get; set; }
    private int CurrentIndex { get; set; }

    public bool IsEnded => CurrentIndex == Entries.Count - 1;

    public MemoryCacheEntry(IReadOnlyList<T> entries, int filterHash = 0)
    {
        Entries = entries;
        FilterHash = filterHash;
        CurrentIndex = -1;
    }

    public T? Next()
    {
        if (IsEnded)
            return null;
        CurrentIndex++;
        return Entries[CurrentIndex];
    }
}