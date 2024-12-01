namespace Application.Contracts.Common;

public abstract class PagedResult
{
    public int TotalPages { get; set; }
    public int? ItemsFrom { get; set; }
    public int? ItemsTo { get; set; }
    public int TotalItemsCount { get; set; }

    protected PagedResult(int totalCount, int? pageSize, int? pageNumber)
    {
        TotalItemsCount = totalCount;

        if (pageSize is null || pageNumber is null)
        {
            return;
        }

        ItemsFrom = (int)pageSize * ((int)pageNumber - 1) + 1;
        ItemsTo = ItemsFrom + (int)pageSize - 1;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}