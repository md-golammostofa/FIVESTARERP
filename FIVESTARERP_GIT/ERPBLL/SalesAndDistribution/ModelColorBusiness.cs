﻿using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using ERPDAL.SalesAndDistributionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution
{
    public class ModelColorBusiness : IModelColorBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistribution;
        private readonly ModelColorsRepository _modelColorsRepository;

        public ModelColorBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution)
        {
            this._salesAndDistribution = salesAndDistribution;
            this._modelColorsRepository = new ModelColorsRepository(this._salesAndDistribution);
        }
        public ModelColors GetModelColors(long modelId, long colorId, long orgId)
        {
            return _modelColorsRepository.GetOneByOrg(s => s.DescriptionId == modelId && s.ColorId == colorId && s.OrganizationId == orgId);
        }
        public IEnumerable<ModelColors> GetModelColors(long orgId)
        {
            return _modelColorsRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }
        public IEnumerable<ModelColor> GetModelColorsByModel(long modelId, long orgId)
        {
            var query = string.Format(@"Select c.ColorId,c.ColorName From [SalesAndDistribution].dbo.tblColors c
Inner Join [SalesAndDistribution].dbo.tblModelColors mc on c.ColorId = mc.ColorId and mc.DescriptionId={0}
Where mc.OrganizationId = {1}", modelId, orgId);
            return _salesAndDistribution.Db.Database.SqlQuery<ModelColor>(query).ToList();
        }
        public IEnumerable<ModelColors> GetModelColorsByOrg(long orgId)
        {
            return _modelColorsRepository.GetAll(s => s.OrganizationId == orgId);
        }
        public bool SaveModelColors(long modelId, List<long> colors, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<ModelColors> modelColorslist = new List<ModelColors>();
            foreach (var color in colors)
            {
                var modelColorInDb = this.GetModelColors(modelId, color, orgId);
                if (modelColorInDb == null)
                {
                    ModelColors modelColors = new ModelColors()
                    {
                        DescriptionId = modelId,
                        ColorId = color,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        OrganizationId = orgId
                    };
                    modelColorslist.Add(modelColors);
                }
            }
            if(modelColorslist.Count > 0)
            {
                _modelColorsRepository.InsertAll(modelColorslist);
                IsSuccess = _modelColorsRepository.Save();
            }
            else
            {
                IsSuccess = true;
            }
            return IsSuccess;
        }
    }
}
