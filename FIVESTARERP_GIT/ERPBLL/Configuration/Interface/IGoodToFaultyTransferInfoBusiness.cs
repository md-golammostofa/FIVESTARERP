﻿using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IGoodToFaultyTransferInfoBusiness
    {
        IEnumerable<GoodToFaultyTransferInfoDTO> GetGoodToFaultyInfoData(long orgId, long branchId);
    }
}