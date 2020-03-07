namespace DNI.Shared.Contracts.Options
{
    public interface IPagerResultOptions
    {
        int PageNumber { get; set; }
        int MaximumRowsPerPage { get; set; }
        bool UseAsync { get; set; }
    }
}
