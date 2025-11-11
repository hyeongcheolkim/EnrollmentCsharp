namespace Enrollment.Dtos;

public class PageableResponse<T>
{
    public List<T> Content { get; set; }
    public int Number { get; set; }
    public int Size { get; set; }
    public int TotalPages { get; set; }
    public long TotalElements { get; set; }
    public bool First { get; set; }
    public bool Last { get; set; }
    public bool Empty { get; set; }
    public SortInfo Sort { get; set; }
    
    public PageableResponse(List<T> content)
    {
        Content = content;

        Number = 0; 
        Size = content.Count; 
        TotalPages = 1; 
        TotalElements = content.Count; 
        First = true;
        Last = true;
        Empty = content.Count == 0;
        Sort = new SortInfo();
    }
}

public class SortInfo
{
    public bool Sorted { get; set; } = false;
    public bool Unsorted { get; set; } = true;
    public bool Empty { get; set; } = true;
}