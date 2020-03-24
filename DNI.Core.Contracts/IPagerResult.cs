namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts.Options;

    public interface IPagerResult<T>
    {
        int Length { get; }

        Task<int> LengthAsync { get; }

        Task<int> GetTotalNumberOfPages(int maximumRowsPerPage, bool useAsync = true);

        [Obsolete("Use GetPagedItems instead.")]
        Task<IEnumerable<T>> GetItems(
            int pageNumber,
            int maximumRowsPerPage,
            bool useAsync = true,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetPagedItems(Action<IPagerResultOptions> pagerResultOptions, CancellationToken cancellationToken = default);
    }
}
