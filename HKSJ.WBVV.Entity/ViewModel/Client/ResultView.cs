using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    [Serializable]
    public class ResultView<T>
    {
        public T Data { get; set; }

        public bool Success { get; set; }

        public string ExceptionMessage { get; set; }

    }
}
