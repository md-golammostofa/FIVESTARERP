﻿using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModel;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.ViewModels;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class ItemPreparationInfoBusiness : IItemPreparationInfoBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly ItemPreparationInfoRepository _itemPreparationInfoRepository; // repo
        private readonly IItemBusiness _itemBusiness;
        public ItemPreparationInfoBusiness(IInventoryUnitOfWork inventoryDb, IItemBusiness itemBusiness)
        {
            this._inventoryDb = inventoryDb;
            this._itemPreparationInfoRepository = new ItemPreparationInfoRepository(this._inventoryDb);
            this._itemBusiness = itemBusiness;
        }

        public IEnumerable<ItemPreparationInfo> GetItemPreparationInfosByOrgId(long orgId)
        {
            return _itemPreparationInfoRepository.GetAll(i => i.OrganizationId == orgId).ToList();
        }

        public ItemPreparationInfo GetItemPreparationInfoOneByOrgId(long id, long orgId)
        {
            return _itemPreparationInfoRepository.GetOneByOrg(i => i.PreparationInfoId == id && i.OrganizationId == orgId);
        }

        public bool SaveItemPreparations(ItemPreparationInfoDTO info, List<ItemPreparationDetailDTO> details, long userId, long orgId)
        {
            bool IsSuccess = false;
            if (info.PreparationInfoId == 0)
            {
                ItemPreparationInfo model = new ItemPreparationInfo
                {
                    WarehouseId = info.WarehouseId,
                    ItemTypeId = info.ItemTypeId,
                    ItemId = info.ItemId,
                    UnitId = _itemBusiness.GetItemById(info.ItemId, orgId).UnitId,
                    Remarks = info.Remarks,
                    DescriptionId = info.DescriptionId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    PreparationType = info.PreparationType
                };
                List<ItemPreparationDetail> modelDetails = new List<ItemPreparationDetail>();
                foreach (var item in details)
                {
                    ItemPreparationDetail itemPreparationDetail = new ItemPreparationDetail()
                    {
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        UnitId = _itemBusiness.GetItemById(item.ItemId, orgId).UnitId,
                        Quantity = item.Quantity,
                        OrganizationId = orgId,
                        Remarks = item.Remarks,
                        EUserId = userId,
                        EntryDate = DateTime.Now
                    };
                    modelDetails.Add(itemPreparationDetail);
                }
                model.ItemPreparationDetails = modelDetails;

                _itemPreparationInfoRepository.Insert(model);
                IsSuccess = _itemPreparationInfoRepository.Save();
            }
            return IsSuccess;
        }

        public ItemPreparationInfo IsDuplicationItemPreparation(long itemId, long modelId, long orgId)
        {
            return _itemPreparationInfoRepository.GetOneByOrg(i => i.ItemId == itemId && i.DescriptionId == modelId && i.OrganizationId == orgId);
        }

        public bool DeleteItemPreparation(long id, long userId, long orgId)
        {
            _itemPreparationInfoRepository.DeleteAll(i => i.PreparationInfoId == id && i.OrganizationId == orgId);
            return _itemPreparationInfoRepository.Save();
        }

        public ItemPreparationInfo GetPreparationInfoByModelAndItem(long modelId, long itemId, long orgId)
        {
            return _itemPreparationInfoRepository.GetOneByOrg(i => i.DescriptionId == modelId && i.ItemId == itemId && i.OrganizationId == orgId);
        }

        public bool IsItemPreparationExistWithThistype(string type, long modelId, long itemId, long orgId)
        {
            return _itemPreparationInfoRepository.GetOneByOrg(s => s.PreparationType == type && s.DescriptionId == modelId && s.ItemId == itemId && s.OrganizationId == orgId) != null;
        }

        public ItemPreparationInfo GetPreparationInfoByModelAndItemAndType(string type, long modelId, long itemId, long orgId)
        {
            return _itemPreparationInfoRepository.GetOneByOrg(i => i.DescriptionId == modelId && i.ItemId == itemId && i.PreparationType == type && i.OrganizationId == orgId);
        }


        public async Task<ItemPreparationInfo> GetPreparationInfoByModelAndItemAndTypeAsync(string type, long modelId, long itemId, long orgId)
        {
            return await _itemPreparationInfoRepository.GetOneByOrgAsync(i => i.DescriptionId == modelId && i.ItemId == itemId && i.PreparationType == type && i.OrganizationId == orgId);
        }

        public IEnumerable<ItemPreparationInfoDTO> GetItemPreparationInfos(long orgId, long? modelId, long? itemTypeId, long? itemId, long? warehouseId, string type)
        {
            return _inventoryDb.Db.Database.SqlQuery<ItemPreparationInfoDTO>(string.Format(@"Exec spItemPreparationInfo @orgId,@modelId,@itemType,@item,@warehouseId,@type"), new SqlParameter("orgId", orgId), new SqlParameter("modelId", modelId ?? 0), new SqlParameter("itemType", itemTypeId ?? 0), new SqlParameter("item", itemId ?? 0), new SqlParameter("@warehouseId", warehouseId ?? 0), new SqlParameter("type", type ?? "")).ToList();
        }
    }
}
