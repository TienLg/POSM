using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSManagement.Presenters
{
    interface IView<T>
    {
        string SearchCriteria { get; }
        List<T> Results { get; }
    }
}
