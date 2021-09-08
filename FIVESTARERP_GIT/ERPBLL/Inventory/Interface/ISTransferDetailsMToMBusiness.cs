﻿using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
  public interface ISTransferDetailsMToMBusiness
    {
        IEnumerable<StockTransferDetailsMToM> GetDetailsDataOneByOrg(long id, long orgId);
    }
}
