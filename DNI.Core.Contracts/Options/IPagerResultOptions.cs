namespace DNI.Core.Contracts.Options
{
    /// <summary>
    /// Represents pager result options to be used by a IPager instance
    /// </summary>
    public interface IPagerResultOptions
    {
        int PageNumber { get; set; }
        int MaximumRowsPerPage { get; set; }
        bool UseAsync { get; set; }
    }
}
