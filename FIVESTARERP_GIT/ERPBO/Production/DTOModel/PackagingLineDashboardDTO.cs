using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
   public class PackagingLineDashboardDTO
    {
        public long PackagingLineId { get; set; }
        public string PackagingLineName { get; set; }
        public int IMEIWrite { get; set; }
        public int Cartoon { get; set; }
        public int QCPassQty { get; set; }
        public int QCFailQty { get; set; }
    }
}
