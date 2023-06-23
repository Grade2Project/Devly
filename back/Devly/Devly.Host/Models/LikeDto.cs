namespace Devly.Models;

public class MutualityLikeDto<T>
{
    public bool IsMutual { get; set; }
    public T Data { get; set; }

}