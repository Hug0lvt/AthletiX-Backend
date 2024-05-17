using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PaginationResult<TItem>
        where TItem : class
    {
        /// <summary>
        /// The current page items.
        /// </summary>
        public IEnumerable<TItem> Items { get; set; } = new List<TItem>();

        /// <summary>
        /// The total number of items.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// The query next page Url.
        /// </summary>
        public int NextPage { get; set; } = 0;
    }
}
