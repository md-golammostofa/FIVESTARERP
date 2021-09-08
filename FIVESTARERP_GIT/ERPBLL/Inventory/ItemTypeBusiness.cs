﻿using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.DomainModels;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
   public class ItemTypeBusiness:IItemTypeBusiness
    {
        /// <summary>
        ///  BC Stands for          - Business Class
        ///  db Stands for          - Database
        ///  repo Stands for        - Repository
        /// </summary>
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly ItemTypeRepository itemTypeRepository; // repo
        public ItemTypeBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            itemTypeRepository = new ItemTypeRepository(this._inventoryDb);
        }

        public IEnumerable<ItemType> GetAllItemTypeByOrgId(long orgId)
        {
            return itemTypeRepository.GetAll(item => item.OrganizationId == orgId).ToList();
        }

        public bool SaveItemType(ItemTypeDTO itemTypeDTO, long userId, long orgId)
        {
            ItemType itemType = new ItemType();
            if (itemTypeDTO.ItemId == 0)
            {
                itemType.ItemName = itemTypeDTO.ItemName;
                itemType.Remarks = itemTypeDTO.Remarks;
                itemType.IsActive = itemTypeDTO.IsActive;
                itemType.ShortName = itemTypeDTO.ShortName;
                itemType.EUserId = userId;
                itemType.EntryDate = DateTime.Now;
                itemType.OrganizationId = orgId;
                itemType.WarehouseId = itemTypeDTO.WarehouseId;
                itemTypeRepository.Insert(itemType);
            }
            else
            {
                itemType = GetItemType(itemTypeDTO.ItemId, orgId);
                //itemType.ItemName = itemTypeDTO.ItemName;
                itemType.Remarks = itemTypeDTO.Remarks;
                itemType.IsActive = itemTypeDTO.IsActive;
                itemType.ItemName = itemTypeDTO.ItemName;
                itemType.ShortName = itemTypeDTO.ShortName;
                itemType.UpUserId = userId;
                itemType.UpdateDate = DateTime.Now;
                itemType.OrganizationId = orgId;
                itemType.WarehouseId = itemTypeDTO.WarehouseId;
                itemTypeRepository.Update(itemType);
            }
            return itemTypeRepository.Save();
        }
        public bool IsDuplicateItemTypeName(string itemTypeName, long id, long orgId, long warehouseId)
        {
            return itemTypeRepository.GetOneByOrg(item => item.ItemName == itemTypeName && item.ItemId != id && item.OrganizationId == orgId && item.WarehouseId == warehouseId) != null ? true : false;
        }
        public bool IsDuplicateShortName(string shortName, long id, long orgId)
        {
            return itemTypeRepository.GetOneByOrg(item => item.ShortName == shortName && item.ItemId != id && item.OrganizationId == orgId ) != null ? true : false;
        }

        public ItemType GetItemTypeOneByOrgId(long id, long warehouseId, long orgId)
        {
            return itemTypeRepository.GetOneByOrg(item => item.ItemId == id && item.OrganizationId == orgId && item.WarehouseId == warehouseId);
        }

        public ItemType GetItemType(long id, long orgId)
        {
            return itemTypeRepository.GetOneByOrg(item => item.ItemId == id && item.OrganizationId == orgId);
        }
    }
}
