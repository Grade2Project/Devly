namespace Devly.Models;

public class ArrayDto<T>
{
    public ArrayDto(IReadOnlyList<T> source)
    {
        Container = source;
    }

    public IReadOnlyList<T> Container { get; set; }
}