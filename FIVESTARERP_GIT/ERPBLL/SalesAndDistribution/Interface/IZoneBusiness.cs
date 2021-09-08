﻿using ERPBO.Common;
using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IZoneBusiness
    {
        IEnumerable<Zone> GetZones(long orgId);
        Zone GetZoneById(long zoneId, long orgId);
        bool SaveZone(ZoneDTO dto, long userId, long orgId);
        IEnumerable<Dropdown> GetZoneWithDistrictAndDivision(long orgId);
        IEnumerable<Dropdown> GetZonesByDistrict(long districtId, long orgId);
        IEnumerable<Dropdown> GetRepresentativesByZone(long zoneId, long orgId);
        IEnumerable<Dropdown> GetZonesWithDistrict(long? districtId, long orgId);
    }
}
