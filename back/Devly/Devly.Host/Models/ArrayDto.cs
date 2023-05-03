namespace Devly.Models;

public class ArrayDto<T>
{
    public IReadOnlyList<T> Container { get; set; }
    
    public ArrayDto(IReadOnlyList<T> source)
    {
        Container = source;
    }
}