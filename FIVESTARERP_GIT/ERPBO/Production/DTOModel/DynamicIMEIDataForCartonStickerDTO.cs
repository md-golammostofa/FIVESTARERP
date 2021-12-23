using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class DynamicIMEIDataForCartonStickerDTO
    {
        public int RowId { get; set; }
        public int? SL1 { get; set; }
        public string IMEI1 { get; set; }
        public int? SL2 { get; set; }
        public string IMEI2 { get; set; }
        public int? SL3 { get; set; }
        public string IMEI3 { get; set; }
        public int? SL4 { get; set; }
        public string IMEI4 { get; set; }
        public byte[] IMEI1Img { get; set; }
        public byte[] IMEI2Img { get; set; }
        public byte[] IMEI3Img { get; set; }
        public byte[] IMEI4Img { get; set; }
    }
}
