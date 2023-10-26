using Service.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.PagingUriGenerator
{
    public interface IPagingUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
