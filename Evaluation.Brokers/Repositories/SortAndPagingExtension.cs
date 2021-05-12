using Evaluation.Brokers.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Linq.Dynamic.Core;

namespace Evaluation.Brokers.Repositories
{
    public static class SortAndPagingExtension
    {
        //public static Task<List<T>>  AddSortAndPagigToQueryAsync<T>
        //    (this IQueryable<T> sorce, SortAndPagingParameters paras = null)
        //{
        //    if(paras == null)
        //    {
        //        return sorce.ToListAsync();
        //    }

        //    if (paras.IsAscending)
        //        sorce =
        //            sorce.OrderBy(paras.SortCloumnName);
        //    else
        //        sorce =
        //            sorce.OrderBy(paras.SortCloumnName + " DESC");

        //    sorce.Skip(paras.PageIndex * paras.PageSize).Take(paras.PageSize);

        //    return sorce.ToListAsync();

        //}
    }
}
