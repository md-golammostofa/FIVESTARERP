﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class DashboardRequestSparePartsDTO
    {
        public int Approved { get; set; }
        public int Current { get; set; }
        public int Pending { get; set; }
        public int Void { get; set; }
    }
}
