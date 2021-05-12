using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Brokers.Repositories
{
    public class SortAndPagingParameters
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public bool IsAscending { get; set; }
        public string SortCloumnName { get; set; } = "Id";
    }
}
