using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common
{
    public class PageHepler
    {
        public int   PageIndex { get; set; }
        public int  PageSize   { get; set; }
        public SortName SortName { get; set; }
        public int TotalNum { get; set; }
        
        
    }

    public enum SortName
    {

        CreateTime=1,
        PlayCount=2
    }
}
