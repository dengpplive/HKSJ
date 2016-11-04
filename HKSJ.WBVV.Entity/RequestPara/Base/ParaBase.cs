using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity.RequestPara.Base
{
    public class ParaBase
    {
        private int _pageSize;
        /// <summary>
        /// 每页记录数
        /// </summary>
        [Display(Name = "每页记录数")]
        public int PageSize
        {
            get
            {
                return _pageSize == 0 ? 10 : _pageSize;
            }
            set { _pageSize = value; }
        }

        private int _pageIndex;
        /// <summary>
        /// 页码
        /// </summary>
        [Display(Name = "页码")]
        public int PageIndex
        {
            get
            {
                return _pageIndex == 0 ? 1 : _pageIndex;
            }
            set { _pageIndex = value; }
        }

    }
}
