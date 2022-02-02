using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.ConfigurationDAL;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class MobilePartStockDetailBusiness : IMobilePartStockDetailBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;//db
        private readonly MobilePartStockDetailRepository mobilePartStockDetailRepository; // repo
        private readonly MobilePartStockInfoRepository mobilePartStockInfoRepository; //repo
        private readonly RequsitionDetailForJobOrderRepository requsitionDetailForJobOrderRepository;
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;
        private readonly IMobilePartBusiness _mobilePartBusiness;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly IRequsitionInfoForJobOrderBusiness _requsitionInfoForJobOrderBusiness;
        private readonly IRequsitionDetailForJobOrderBusiness _requsitionDetailForJobOrderBusiness;
        private readonly ITechnicalServicesStockBusiness _technicalServicesStockBusiness;
        private readonly ITsStockReturnInfoBusiness _tsStockReturnInfoBusiness;
        private readonly IJobOrderBusiness _jobOrderBusiness;
        public MobilePartStockDetailBusiness(IConfigurationUnitOfWork configurationDb, IFrontDeskUnitOfWork frontDeskUnitOfWork, IServicesWarehouseBusiness servicesWarehouseBusiness, IMobilePartBusiness mobilePartBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, IRequsitionDetailForJobOrderBusiness requsitionDetailForJobOrderBusiness, ITechnicalServicesStockBusiness technicalServicesStockBusiness, ITsStockReturnInfoBusiness tsStockReturnInfoBusiness, IJobOrderBusiness jobOrderBusiness)
        {
            this._configurationDb = configurationDb;
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            mobilePartStockDetailRepository = new MobilePartStockDetailRepository(this._configurationDb);
            mobilePartStockInfoRepository = new MobilePartStockInfoRepository(this._configurationDb);
            requsitionDetailForJobOrderRepository = new RequsitionDetailForJobOrderRepository(this._frontDeskUnitOfWork);
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._requsitionInfoForJobOrderBusiness = requsitionInfoForJobOrderBusiness;
            this._requsitionDetailForJobOrderBusiness = requsitionDetailForJobOrderBusiness;
            this._technicalServicesStockBusiness = technicalServicesStockBusiness;
            this._tsStockReturnInfoBusiness = tsStockReturnInfoBusiness;
            this._jobOrderBusiness = jobOrderBusiness;
        }

        public IEnumerable<MobilePartStockDetail> GelAllMobilePartStockDetailByOrgId(long orgId, long branchId)
        {
            return mobilePartStockDetailRepository.GetAll(detail => detail.OrganizationId == orgId && detail.BranchId == branchId).ToList();
        }

        public MobilePartStockInfo GetPriceByModel(long modelId, long partsId, long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetOneByOrg(p => p.DescriptionId == modelId && p.MobilePartId == partsId && p.OrganizationId == orgId && p.BranchId == branchId);
        }

        public bool SaveMobilePartsStockOutByTSRequistion(long reqId, string status, long orgId, long userId, long branchId)
        {
            var reqInfo = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(reqId, orgId);
            var reqDetail = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(reqId, orgId, branchId);
            if (reqInfo != null && reqDetail.Count() > 0)
            {
                List<MobilePartStockDetailDTO> stockDetailDTOs = new List<MobilePartStockDetailDTO>();
                foreach (var item in reqDetail)
                {
                    MobilePartStockDetailDTO stockDetailDTO = new MobilePartStockDetailDTO
                    {
                        SWarehouseId = reqInfo.SWarehouseId,
                        MobilePartId = item.PartsId,
                        OrganizationId = item.OrganizationId,
                        CostPrice = item.CostPrice,
                        SellPrice = item.SellPrice,
                        Quantity = (int)item.Quantity,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        BranchId = branchId,
                        Remarks = "Stock Out By Production Requistion " + "(" + reqInfo.RequsitionCode + ")",
                        ReferrenceNumber = reqInfo.RequsitionCode,
                        StockStatus = StockStatus.StockOut
                    };
                    stockDetailDTOs.Add(stockDetailDTO);
                }
                if (SaveMobilePartStockOut(stockDetailDTOs, userId, orgId, branchId) == true)
                {
                    return _requsitionInfoForJobOrderBusiness.SaveRequisitionStatus(reqId, status, userId, orgId, branchId);
                }
            }
            return false;
        }

        public bool SaveMobilePartStockIn(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId)
        {
            List<MobilePartStockDetail> mobilePartStockDetails = new List<MobilePartStockDetail>();
            foreach (var item in mobilePartStockDetailDTO)
            {
                MobilePartStockDetail StockDetail = new MobilePartStockDetail();
                StockDetail.MobilePartStockDetailId = item.MobilePartStockDetailId;
                StockDetail.MobilePartId = item.MobilePartId;
                StockDetail.SWarehouseId = item.SWarehouseId;
                StockDetail.DescriptionId = item.DescriptionId;
                StockDetail.CostPrice = item.CostPrice;
                StockDetail.SellPrice = item.SellPrice;
                StockDetail.Quantity = item.Quantity;
                StockDetail.Remarks = "Stock-In By Warehouse";
                StockDetail.OrganizationId = orgId;
                StockDetail.BranchId = branchId;
                StockDetail.EUserId = userId;
                StockDetail.EntryDate = DateTime.Now;
                StockDetail.StockStatus = StockStatus.StockIn;
                StockDetail.BranchFrom = item.BranchFrom;
                StockDetail.ReferrenceNumber = item.ReferrenceNumber;

                var warehouseInfo = _mobilePartStockInfoBusiness.GetMobilePartStockInfoByModelAndMobilePartsAndCostPrice(item.DescriptionId.Value,item.MobilePartId.Value, item.CostPrice, orgId, branchId);
                
                if (warehouseInfo != null)
                {
                    warehouseInfo.StockInQty += item.Quantity;
                    warehouseInfo.UpUserId = userId;
                    warehouseInfo.UpdateDate = DateTime.Now;
                    mobilePartStockInfoRepository.Update(warehouseInfo);
                }
                else
                {
                    MobilePartStockInfo mobilePartStockInfo = new MobilePartStockInfo();
                    mobilePartStockInfo.SWarehouseId = item.SWarehouseId;
                    mobilePartStockInfo.MobilePartId = item.MobilePartId;
                    mobilePartStockInfo.DescriptionId = item.DescriptionId;
                    mobilePartStockInfo.CostPrice = item.CostPrice;
                    mobilePartStockInfo.SellPrice = item.SellPrice;
                    mobilePartStockInfo.StockInQty = item.Quantity;
                    mobilePartStockInfo.StockOutQty = 0;
                    mobilePartStockInfo.OrganizationId = orgId;
                    mobilePartStockInfo.BranchId = branchId;
                    mobilePartStockInfo.EUserId = userId;
                    mobilePartStockInfo.EntryDate = DateTime.Now;
                    mobilePartStockInfoRepository.Insert(mobilePartStockInfo);
                }
                mobilePartStockDetails.Add(StockDetail);
            }
            mobilePartStockDetailRepository.InsertAll(mobilePartStockDetails);
            return mobilePartStockDetailRepository.Save();
        }
        //Branch Transfer Stock out
        //public bool SaveMobilePartStockOut(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId)
        //{
        //    List<MobilePartStockDetail> mobilePartStockDetails = new List<MobilePartStockDetail>();
        //    foreach (var item in mobilePartStockDetailDTO)
        //    {
        //        MobilePartStockDetail StockDetail = new MobilePartStockDetail();
        //        StockDetail.MobilePartStockDetailId = item.MobilePartStockDetailId;
        //        StockDetail.MobilePartId = item.MobilePartId;
        //        StockDetail.SWarehouseId = item.SWarehouseId;
        //        StockDetail.CostPrice = item.CostPrice;
        //        StockDetail.SellPrice = item.SellPrice;
        //        StockDetail.Quantity = item.Quantity;
        //        StockDetail.Remarks = item.Remarks;
        //        StockDetail.OrganizationId = orgId;
        //        StockDetail.BranchId = branchId;
        //        StockDetail.EUserId = userId;
        //        StockDetail.EntryDate = DateTime.Now;
        //        StockDetail.StockStatus = StockStatus.StockOut;
        //        StockDetail.ReferrenceNumber = item.ReferrenceNumber;
        //        StockDetail.DescriptionId = item.DescriptionId; //Nishad

        //        var warehouseInfo = _mobilePartStockInfoBusiness.GetMobilePartStockInfoByModelAndMobilePartsAndCostPrice(item.DescriptionId.Value,item.MobilePartId.Value,item.CostPrice,orgId,branchId);  //_mobilePartStockInfoBusiness.GetAllMobilePartStockInfoById(orgId, branchId).Where(o => item.SWarehouseId == item.SWarehouseId && o.MobilePartId == item.MobilePartId && o.CostPrice == item.CostPrice).FirstOrDefault();
        //        warehouseInfo.StockOutQty += item.Quantity;
        //        warehouseInfo.UpUserId = userId;
        //        mobilePartStockInfoRepository.Update(warehouseInfo);
        //        mobilePartStockDetails.Add(StockDetail);
        //    }
        //    mobilePartStockDetailRepository.InsertAll(mobilePartStockDetails);
        //    return mobilePartStockDetailRepository.Save();
        //}
        public bool SaveMobilePartStockOut(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long orgId, long branchId, long userId)
        {
            bool IsSuccess = false;
            var warehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(orgId, branchId).FirstOrDefault();
            List<MobilePartStockDetail> stockDetails = new List<MobilePartStockDetail>();

            foreach (var item in mobilePartStockDetailDTO)
            {
                var reqQty = item.Quantity;
                var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByModelAndBranch(orgId, item.DescriptionId.Value, branchId).Where(i => i.MobilePartId == item.MobilePartId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

                if (partsInStock.Count() > 0)
                {
                    int remainQty = reqQty;
                    foreach (var stock in partsInStock)
                    {

                        var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
                        var stockOutQty = 0;
                        if (totalStockqty <= remainQty)
                        {
                            stock.StockOutQty += totalStockqty;
                            stockOutQty = totalStockqty.Value;
                            remainQty -= totalStockqty.Value;
                        }
                        else
                        {
                            stockOutQty = remainQty;
                            stock.StockOutQty += remainQty;
                            remainQty = 0;
                        }


                        MobilePartStockDetail stockDetail = new MobilePartStockDetail()
                        {
                            DescriptionId = item.DescriptionId,
                            SWarehouseId = warehouse.SWarehouseId,
                            MobilePartId = item.MobilePartId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = stockOutQty,
                            Remarks = item.Remarks,
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockOut,
                            //ReferrenceNumber = reqInfo.RequsitionCode
                        };
                        stockDetails.Add(stockDetail);
                        mobilePartStockInfoRepository.Update(stock);
                        if (remainQty == 0)
                        {
                            break;
                        }
                    }
                }
            }
            mobilePartStockDetailRepository.InsertAll(stockDetails);
            return mobilePartStockDetailRepository.Save();
        }

        public bool SaveMobilePartStockOutByReq(long reqId,string status, long orgId, long branchId, long userId)
        {
            bool IsSuccess = false;
            var reqInfo = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(reqId, orgId);

            var modelId = _jobOrderBusiness.GetJobOrdersByIdWithBranch(reqInfo.JobOrderId.Value, reqInfo.BranchId.Value, orgId).DescriptionId;
            var reqDetails = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(reqId, orgId, reqInfo.BranchId.Value);
            List<MobilePartStockDetail> stockDetails = new List<MobilePartStockDetail>();
            List<TechnicalServicesStockDTO> servicesStockDTOs = new List<TechnicalServicesStockDTO>();

            foreach (var item in reqDetails)
            {
                var reqQty = item.Quantity;
                var reqDetailsF = _requsitionDetailForJobOrderBusiness.GetDetailsByDetailsId(item.RequsitionDetailForJobOrderId, orgId, branchId);
                if (reqDetailsF != null)
                {
                    reqDetailsF.IssueQty = reqQty;
                    reqDetailsF.UpUserId = userId;
                    reqDetailsF.UpdateDate = DateTime.Now;
                    requsitionDetailForJobOrderRepository.Update(reqDetailsF);
                    requsitionDetailForJobOrderRepository.Save();
                }
                var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByModelAndBranch(orgId, modelId, branchId).Where(i => i.MobilePartId == item.PartsId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

                if (partsInStock.Count() > 0)
                {
                    int remainQty = reqQty;
                    foreach (var stock in partsInStock)
                    {

                        var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
                        var stockOutQty = 0;
                        if (totalStockqty <= remainQty)
                        {
                            stock.StockOutQty += totalStockqty;
                            stockOutQty = totalStockqty.Value;
                            remainQty -= totalStockqty.Value;
                        }
                        else
                        {
                            stockOutQty = remainQty;
                            stock.StockOutQty += remainQty;
                            remainQty = 0;
                        }
                        

                        MobilePartStockDetail stockDetail = new MobilePartStockDetail()
                        {
                            DescriptionId = modelId,
                            SWarehouseId = item.SWarehouseId,
                            MobilePartId = item.PartsId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = stockOutQty,
                            Remarks = item.Remarks,
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockOut,
                            ReferrenceNumber = reqInfo.RequsitionCode
                        };
                        TechnicalServicesStockDTO tsStock = new TechnicalServicesStockDTO()
                        {
                            JobOrderId = item.JobOrderId,
                            SWarehouseId = item.SWarehouseId,
                            RequsitionInfoForJobOrderId = item.RequsitionInfoForJobOrderId,
                            PartsId = item.PartsId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = stockOutQty,
                            StateStatus = "Stock-Open",
                            UsedQty = 0,
                            ReturnQty = 0,
                            Remarks = item.Remarks,
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                        };
                        servicesStockDTOs.Add(tsStock);
                        stockDetails.Add(stockDetail);
                        mobilePartStockInfoRepository.Update(stock);
                        if (remainQty == 0)
                        {
                            break;
                        }
                    }
                }
            }
            mobilePartStockDetailRepository.InsertAll(stockDetails);
            if (mobilePartStockDetailRepository.Save())
            {
                IsSuccess = _technicalServicesStockBusiness.SaveTechnicalServicesStockIn(servicesStockDTOs, userId, orgId, branchId, modelId);
                if (IsSuccess == true)
                {
                    return _requsitionInfoForJobOrderBusiness.SaveRequisitionStatus(reqId, status, userId, orgId, reqInfo.BranchId.Value);
                }
            }
            return IsSuccess;
        }

        //public bool StockOutAccessoriesSells(long invoiceId, long orgId, long branchId, long userId)
        //{
        //    bool IsSuccess = false;
        //    var invInfo = _invoiceInfoBusiness.GetAllInvoiceByOrgId(invoiceId, orgId, branchId);
        //    var invDetail = _invoiceDetailBusiness.GetAllDetailByInfoId(invoiceId, orgId, branchId);
        //    List<MobilePartStockDetail> stockDetails = new List<MobilePartStockDetail>();
        //    foreach (var item in invDetail)
        //    {
        //        var reqQty = item.Quantity;
        //        var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(orgId, branchId).Where(i => i.MobilePartId == item.PartsId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

        //        if (partsInStock.Count() > 0)
        //        {
        //            int remainQty = reqQty;
        //            foreach (var stock in partsInStock)
        //            {

        //                var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
        //                var stockOutQty = 0;
        //                if (totalStockqty <= remainQty)
        //                {
        //                    stock.StockOutQty += totalStockqty;
        //                    stockOutQty = totalStockqty.Value;
        //                    remainQty -= totalStockqty.Value;
        //                }
        //                else
        //                {
        //                    stockOutQty = remainQty;
        //                    stock.StockOutQty += remainQty;
        //                    remainQty = 0;
        //                }


        //                MobilePartStockDetail stockDetail = new MobilePartStockDetail()
        //                {
        //                    MobilePartId = item.PartsId,
        //                    CostPrice = stock.CostPrice,
        //                    SellPrice = stock.SellPrice,
        //                    Quantity = stockOutQty,
        //                    Remarks = item.Remarks,
        //                    OrganizationId = orgId,
        //                    BranchId = branchId,
        //                    EUserId = userId,
        //                    EntryDate = DateTime.Now,
        //                    StockStatus = StockStatus.StockOut,
        //                    ReferrenceNumber = invInfo.InvoiceCode
        //                };
        //                stockDetails.Add(stockDetail);
        //                mobilePartStockInfoRepository.Update(stock);
        //                if (remainQty == 0)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    mobilePartStockDetailRepository.InsertAll(stockDetails);
        //    mobilePartStockDetailRepository.Save();
        //    return IsSuccess;
        //}

        public bool SaveReturnPartsStockIn(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO,long returnInfoId,string status, long userId, long orgId, long branchId)
        {
            List<MobilePartStockDetail> mobilePartStockDetails = new List<MobilePartStockDetail>();
            foreach (var item in mobilePartStockDetailDTO)
            {
                var warehouseInfo = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(orgId, branchId).Where(o => o.MobilePartId == item.MobilePartId && o.DescriptionId == item.DescriptionId).FirstOrDefault();
                warehouseInfo.StockInQty += item.Quantity;
                warehouseInfo.UpUserId = userId;
                warehouseInfo.UpdateDate = DateTime.Now;
                mobilePartStockInfoRepository.Update(warehouseInfo);

                MobilePartStockDetail StockDetail = new MobilePartStockDetail();
                StockDetail.MobilePartId = item.MobilePartId;
                StockDetail.SWarehouseId = warehouseInfo.SWarehouseId;
                StockDetail.DescriptionId = warehouseInfo.DescriptionId;
                StockDetail.CostPrice = warehouseInfo.CostPrice;
                StockDetail.SellPrice = warehouseInfo.SellPrice;
                StockDetail.Quantity = item.Quantity;
                StockDetail.Remarks = item.Remarks;
                StockDetail.OrganizationId = orgId;
                StockDetail.BranchId = branchId;
                StockDetail.EUserId = userId;
                StockDetail.EntryDate = DateTime.Now;
                StockDetail.StockStatus = StockStatus.StockIn;
                StockDetail.ReferrenceNumber = item.ReferrenceNumber;
                mobilePartStockDetails.Add(StockDetail);
            }
            mobilePartStockDetailRepository.InsertAll(mobilePartStockDetails);
            if (mobilePartStockDetailRepository.Save() == true)
            {
                return _tsStockReturnInfoBusiness.UpdateReturnInfoStatus(returnInfoId, status, userId,orgId,branchId);
            }
            return false;
        }

        public bool StockInByBranchTransferApproval(long transferId, string status, long userId, long branchId, long orgId)
        {
            bool IsSuccess = false;

            return IsSuccess;

        }

        public bool SaveMobilePartStockInByBranchRequsition(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId)
        {
            List<MobilePartStockDetail> mobilePartStockDetails = new List<MobilePartStockDetail>();
            foreach (var item in mobilePartStockDetailDTO)
            {
                MobilePartStockDetail StockDetail = new MobilePartStockDetail();
                StockDetail.MobilePartStockDetailId = item.MobilePartStockDetailId;
                StockDetail.MobilePartId = item.MobilePartId;
                StockDetail.SWarehouseId = item.SWarehouseId;
                StockDetail.DescriptionId = item.DescriptionId;
                StockDetail.CostPrice = item.CostPrice;
                StockDetail.SellPrice = item.SellPrice;
                StockDetail.Quantity = item.Quantity;
                StockDetail.Remarks = "Stock-In By Branch Requsition";
                StockDetail.OrganizationId = orgId;
                StockDetail.BranchId = branchId;
                StockDetail.EUserId = userId;
                StockDetail.EntryDate = DateTime.Now;
                StockDetail.StockStatus = StockStatus.StockIn;
                StockDetail.BranchFrom = item.BranchFrom;
                StockDetail.ReferrenceNumber = item.ReferrenceNumber;

                var warehouseInfo = _mobilePartStockInfoBusiness.GetMobilePartStockInfoByModelAndMobilePartsAndCostPrice(item.DescriptionId.Value, item.MobilePartId.Value, item.CostPrice, orgId, branchId);

                if (warehouseInfo != null)
                {
                    warehouseInfo.StockInQty += item.Quantity;
                    warehouseInfo.UpUserId = userId;
                    warehouseInfo.UpdateDate = DateTime.Now;
                    mobilePartStockInfoRepository.Update(warehouseInfo);
                }
                else
                {
                    MobilePartStockInfo mobilePartStockInfo = new MobilePartStockInfo();
                    mobilePartStockInfo.SWarehouseId = item.SWarehouseId;
                    mobilePartStockInfo.MobilePartId = item.MobilePartId;
                    mobilePartStockInfo.DescriptionId = item.DescriptionId;
                    mobilePartStockInfo.CostPrice = item.CostPrice;
                    mobilePartStockInfo.SellPrice = item.SellPrice;
                    mobilePartStockInfo.StockInQty = item.Quantity;
                    mobilePartStockInfo.StockOutQty = 0;
                    mobilePartStockInfo.OrganizationId = orgId;
                    mobilePartStockInfo.BranchId = branchId;
                    mobilePartStockInfo.EUserId = userId;
                    mobilePartStockInfo.EntryDate = DateTime.Now;
                    mobilePartStockInfoRepository.Insert(mobilePartStockInfo);
                }
                mobilePartStockDetails.Add(StockDetail);
            }
            mobilePartStockDetailRepository.InsertAll(mobilePartStockDetails);
            return mobilePartStockDetailRepository.Save();
        }

        public IEnumerable<TotalStockDetailsDTO> TotalStockDetailsReport(long orgId, long branchId, long? modelId, long? partsId)
        {
            return _configurationDb.Db.Database.SqlQuery<TotalStockDetailsDTO>(QueryForTotalStockDetails(orgId, branchId,modelId,partsId)).ToList();
        }
        private string QueryForTotalStockDetails(long orgId, long branchId, long? modelId, long? partsId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (orgId > 0)
            {
                param += string.Format(@" and st.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@" and st.BranchId={0}", branchId);
            }

            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and st.DescriptionId ={0}", modelId);
            }
            if (partsId != null && partsId > 0)
            {
                param += string.Format(@" and st.MobilePartId ={0}", partsId);
            }

            query = string.Format(@"Select ModelName,DescriptionId,PartsName,MobilePartId,PartsCode,((GoodStock+FaultyStock+ScrapStock+DustStock+CareTransfer+TransferAModel+EngPending+Sales+StockReturnPending)-ReceiveAModel)'ParsesStock',ReceiveAModel,(GoodStock+FaultyStock+ScrapStock+DustStock+CareTransfer+TransferAModel+EngPending+Sales+StockReturnPending)'Stock',GoodStock,FaultyStock,ScrapStock,DustStock,CareTransfer,TransferAModel,EngPending,Sales,StockReturnPending From (Select DISTINCT m.ModelName,st.DescriptionId,p.MobilePartName'PartsName',st.MobilePartId,p.MobilePartCode'PartsCode',

(Select ISNULL(Sum(StockInQty-StockOutQty),0) From tblMobilePartStockInfo
Where DescriptionId=st.DescriptionId and MobilePartId=st.MobilePartId and BranchId=st.BranchId)'GoodStock',

(Select ISNULL(Sum(StockInQty-StockOutQty),0) From tblFaultyStockInfo
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'FaultyStock',

(Select ISNULL(Sum(ScrapQuantity-ScrapOutQty),0) From tblScrapStockInfo
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'ScrapStock',

(Select ISNULL(SUM(StockInQty),0) From tblDustStockInfo
Where ModelId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'DustStock',

(Select ISNULL(SUM(IssueQty),0) From tblTransferDetails
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchTo=st.BranchId)'CareTransfer',

(Select ISNULL(SUM(Quantity),0) From [FrontDesk].dbo.tblTechnicalServicesStock
Where StateStatus='Stock-Open' and ModelId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'EngPending',

(Select ISNULL(SUM(Quantity),0) From StockTransferDetailModelToModels
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'TransferAModel',

(Select ISNULL(SUM(Quantity),0) From StockTransferDetailModelToModels
Where ToDescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'ReceiveAModel',

(Select ISNULL(SUM(Quantity),0) From [FrontDesk].dbo.InvoiceDetails
Where  ModelId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'Sales',

(Select ISNULL(SUM(Quantity),0) From [FrontDesk].dbo.tblTsStockReturnInfo i
Left Join [FrontDesk].dbo.tblTsStockReturnDetails d on i.ModelId=d.ModelId
Where i.StateStatus='Stock-Return' and i.ReturnInfoId=d.ReturnInfoId and i.BranchId=st.BranchId and d.ModelId=st.DescriptionId and d.PartsId=st.MobilePartId)'StockReturnPending'

From tblMobilePartStockInfo st
Left Join tblModelSS m on st.DescriptionId=m.ModelId
Left Join tblMobileParts p on st.MobilePartId=p.MobilePartId
Where 1=1{0}) tbl1
", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<TotalStockDetailsDTO> AllBranchTotalStock(long? branch, long orgId)
        {
            if (branch == null)
            {
                branch = 22;
            }
            var data= _configurationDb.Db.Database.SqlQuery<TotalStockDetailsDTO>(string.Format(@"Select ModelName,DescriptionId,PartsName,MobilePartId,PartsCode,((GoodStock+FaultyStock+ScrapStock+DustStock+CareTransfer+TransferAModel+EngPending+Sales)-ReceiveAModel)'ParsesStock',ReceiveAModel,(GoodStock+FaultyStock+ScrapStock+DustStock+CareTransfer+TransferAModel+EngPending+Sales)'Stock',GoodStock,FaultyStock,ScrapStock,DustStock,CareTransfer,TransferAModel,EngPending,Sales From (Select DISTINCT m.ModelName,st.DescriptionId,p.MobilePartName'PartsName',st.MobilePartId,p.MobilePartCode'PartsCode',

(Select ISNULL(Sum(StockInQty-StockOutQty),0) From tblMobilePartStockInfo
Where DescriptionId=st.DescriptionId and MobilePartId=st.MobilePartId and BranchId={0})'GoodStock',

(Select ISNULL(Sum(StockInQty-StockOutQty),0) From tblFaultyStockInfo
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId={0})'FaultyStock',

(Select ISNULL(Sum(ScrapQuantity-ScrapOutQty),0) From tblScrapStockInfo
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId={0})'ScrapStock',

(Select ISNULL(SUM(StockInQty),0) From tblDustStockInfo
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId={0})'DustStock',

(Select ISNULL(SUM(IssueQty),0) From tblTransferDetails
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchTo={0})'CareTransfer',

(Select ISNULL(SUM(Quantity),0) From [FrontDesk].dbo.tblTechnicalServicesStock
Where StateStatus='Stock-Open' and ModelId=st.DescriptionId and PartsId=st.MobilePartId and BranchId={0})'EngPending',

(Select ISNULL(SUM(Quantity),0) From StockTransferDetailModelToModels
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId={0})'TransferAModel',

(Select ISNULL(SUM(Quantity),0) From StockTransferDetailModelToModels
Where ToDescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId={0})'ReceiveAModel',

(Select ISNULL(SUM(Quantity),0) From [FrontDesk].dbo.InvoiceDetails
Where  ModelId=st.DescriptionId and PartsId=st.MobilePartId and BranchId={0})'Sales'

From tblMobilePartStockInfo st
Left Join tblModelSS m on st.DescriptionId=m.ModelId
Left Join tblMobileParts p on st.MobilePartId=p.MobilePartId
Where st.BranchId={0} and st.OrganizationId={1}) tbl1", branch,orgId)).ToList();
            return data;
        }
        private string QueryForAllBranchTotalStock(long? branch, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (orgId > 0)
            {
                param += string.Format(@" and st.OrganizationId={0}", orgId);
            }

            //if (branch != null && branch > 0)
            //{
            //    param += string.Format(@" and BranchId ={0}", branch);
            //}
            query = string.Format(@"Select ModelName,DescriptionId,PartsName,MobilePartId,PartsCode,((GoodStock+FaultyStock+ScrapStock+DustStock+CareTransfer+TransferAModel+EngPending)-ReceiveAModel)'ParsesStock',ReceiveAModel,(GoodStock+FaultyStock+ScrapStock+DustStock+CareTransfer+TransferAModel+EngPending)'Stock',GoodStock,FaultyStock,ScrapStock,DustStock,CareTransfer,TransferAModel,EngPending From (Select DISTINCT m.ModelName,st.DescriptionId,p.MobilePartName'PartsName',st.MobilePartId,p.MobilePartCode'PartsCode',

(Select ISNULL(Sum(StockInQty-StockOutQty),0) From tblMobilePartStockInfo
Where DescriptionId=st.DescriptionId and MobilePartId=st.MobilePartId and BranchId=st.BranchId)'GoodStock',

(Select ISNULL(Sum(StockInQty-StockOutQty),0) From tblFaultyStockInfo
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'FaultyStock',

(Select ISNULL(Sum(ScrapQuantity-ScrapOutQty),0) From tblScrapStockInfo
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'ScrapStock',

(Select ISNULL(SUM(StockInQty),0) From tblDustStockInfo
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'DustStock',

(Select ISNULL(SUM(IssueQty),0) From tblTransferDetails
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchTo=st.BranchId)'CareTransfer',

(Select ISNULL(SUM(Quantity),0) From [FrontDesk].dbo.tblTechnicalServicesStock
Where StateStatus='Stock-Open' and ModelId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'EngPending',

(Select ISNULL(SUM(Quantity),0) From StockTransferDetailModelToModels
Where DescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'TransferAModel',

(Select ISNULL(SUM(Quantity),0) From StockTransferDetailModelToModels
Where ToDescriptionId=st.DescriptionId and PartsId=st.MobilePartId and BranchId=st.BranchId)'ReceiveAModel'

From tblMobilePartStockInfo st
Left Join tblModelSS m on st.DescriptionId=m.ModelId
Left Join tblMobileParts p on st.MobilePartId=p.MobilePartId
Where 1=1{0}) tbl1
", Utility.ParamChecker(param));
            return query;
        }
        public bool UpdateReqStatusAndWarehouseStockOutAndTsStockIn(RequsitionInfoForJobOrderDTO dto, long orgId, long branchId, long userId)
        {
            bool IsSuccess = false;
            var warehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(orgId, branchId).FirstOrDefault();
            List<MobilePartStockDetail> stockDetails = new List<MobilePartStockDetail>();
            List<TechnicalServicesStockDTO> servicesStockDTOs = new List<TechnicalServicesStockDTO>();

            foreach (var item in dto.RequsitionDetailForJobOrders)
            {
                var reqQty = item.IssueQty;
                if (reqQty > 0)
                {
                    var reqDetails = _requsitionDetailForJobOrderBusiness.GetDetailsByDetailsId(item.RequsitionDetailForJobOrderId, orgId, branchId);
                    if(reqDetails != null)
                    {
                        reqDetails.IssueQty = reqQty;
                        reqDetails.UpUserId = userId;
                        reqDetails.UpdateDate = DateTime.Now;
                        requsitionDetailForJobOrderRepository.Update(reqDetails);
                        requsitionDetailForJobOrderRepository.Save();
                    }
                    var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByModelAndBranch(orgId, dto.DescriptionId, branchId).Where(i => i.MobilePartId == item.PartsId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

                    if (partsInStock.Count() > 0)
                    {
                        int remainQty = reqQty;
                        foreach (var stock in partsInStock)
                        {

                            var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
                            var stockOutQty = 0;
                            if (totalStockqty <= remainQty)
                            {
                                stock.StockOutQty += totalStockqty;
                                stockOutQty = totalStockqty.Value;
                                remainQty -= totalStockqty.Value;
                            }
                            else
                            {
                                stockOutQty = remainQty;
                                stock.StockOutQty += remainQty;
                                remainQty = 0;
                            }


                            MobilePartStockDetail stockDetail = new MobilePartStockDetail()
                            {
                                DescriptionId = dto.DescriptionId,
                                SWarehouseId = warehouse.SWarehouseId,
                                MobilePartId = item.PartsId,
                                CostPrice = stock.CostPrice,
                                SellPrice = stock.SellPrice,
                                Quantity = stockOutQty,
                                Remarks = item.Remarks,
                                OrganizationId = orgId,
                                BranchId = branchId,
                                EUserId = userId,
                                EntryDate = DateTime.Now,
                                StockStatus = StockStatus.StockOut,
                                ReferrenceNumber = dto.RequsitionCode
                            };
                            TechnicalServicesStockDTO tsStock = new TechnicalServicesStockDTO()
                            {
                                JobOrderId = dto.JobOrderId,
                                SWarehouseId = warehouse.SWarehouseId,
                                RequsitionInfoForJobOrderId = dto.RequsitionInfoForJobOrderId,
                                PartsId = item.PartsId,
                                CostPrice = stock.CostPrice,
                                SellPrice = stock.SellPrice,
                                Quantity = stockOutQty,
                                StateStatus = "Stock-Open",
                                UsedQty = 0,
                                ReturnQty = 0,
                                Remarks = item.Remarks,
                                OrganizationId = orgId,
                                BranchId = branchId,
                                EUserId = userId,
                                EntryDate = DateTime.Now,
                            };
                            servicesStockDTOs.Add(tsStock);
                            stockDetails.Add(stockDetail);
                            mobilePartStockInfoRepository.Update(stock);
                            if (remainQty == 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            mobilePartStockDetailRepository.InsertAll(stockDetails);
            if (mobilePartStockDetailRepository.Save())
            {
                IsSuccess = _technicalServicesStockBusiness.SaveTechnicalServicesStockIn(servicesStockDTOs, userId, orgId, branchId, dto.DescriptionId);
                if (IsSuccess == true)
                {
                    return _requsitionInfoForJobOrderBusiness.SaveRequisitionStatus(dto.RequsitionInfoForJobOrderId,dto.StateStatus, userId, orgId, branchId);
                }
            }
            return IsSuccess;
        }

        public bool SaveMobilePartStockInByRepaired(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId)
        {
            List<MobilePartStockDetail> mobilePartStockDetails = new List<MobilePartStockDetail>();
            foreach (var item in mobilePartStockDetailDTO)
            {
                MobilePartStockDetail StockDetail = new MobilePartStockDetail();
                StockDetail.MobilePartStockDetailId = item.MobilePartStockDetailId;
                StockDetail.MobilePartId = item.MobilePartId;
                StockDetail.SWarehouseId = item.SWarehouseId;
                StockDetail.DescriptionId = item.DescriptionId;
                StockDetail.CostPrice = item.CostPrice;
                StockDetail.SellPrice = item.SellPrice;
                StockDetail.Quantity = item.Quantity;
                StockDetail.Remarks = "Stock-In By Repaired";
                StockDetail.OrganizationId = orgId;
                StockDetail.BranchId = branchId;
                StockDetail.EUserId = userId;
                StockDetail.EntryDate = DateTime.Now;
                StockDetail.StockStatus = StockStatus.StockIn;
                StockDetail.BranchFrom = item.BranchFrom;
                StockDetail.ReferrenceNumber = item.ReferrenceNumber;

                var warehouseInfo = _mobilePartStockInfoBusiness.GetMobilePartStockInfoByModelAndMobilePartsAndCostPrice(item.DescriptionId.Value, item.MobilePartId.Value, item.CostPrice, orgId, branchId);

                if (warehouseInfo != null)
                {
                    warehouseInfo.StockInQty += item.Quantity;
                    warehouseInfo.UpUserId = userId;
                    warehouseInfo.UpdateDate = DateTime.Now;
                    mobilePartStockInfoRepository.Update(warehouseInfo);
                }
                else
                {
                    MobilePartStockInfo mobilePartStockInfo = new MobilePartStockInfo();
                    mobilePartStockInfo.SWarehouseId = item.SWarehouseId;
                    mobilePartStockInfo.MobilePartId = item.MobilePartId;
                    mobilePartStockInfo.DescriptionId = item.DescriptionId;
                    mobilePartStockInfo.CostPrice = item.CostPrice;
                    mobilePartStockInfo.SellPrice = item.SellPrice;
                    mobilePartStockInfo.StockInQty = item.Quantity;
                    mobilePartStockInfo.StockOutQty = 0;
                    mobilePartStockInfo.OrganizationId = orgId;
                    mobilePartStockInfo.BranchId = branchId;
                    mobilePartStockInfo.EUserId = userId;
                    mobilePartStockInfo.EntryDate = DateTime.Now;
                    mobilePartStockInfoRepository.Insert(mobilePartStockInfo);
                }
                mobilePartStockDetails.Add(StockDetail);
            }
            mobilePartStockDetailRepository.InsertAll(mobilePartStockDetails);
            return mobilePartStockDetailRepository.Save();
        }

        public IEnumerable<MobilePartStockDetailDTO> GetAllStockDetails(long? descriptionId, long? mobilePartId, long orgId, long branchId, string stockStatus, string fromDate, string toDate)
        {
            return _configurationDb.Db.Database.SqlQuery<MobilePartStockDetailDTO>(QueryForAllStockDetails(descriptionId,mobilePartId,orgId,branchId,stockStatus,fromDate,toDate)).ToList();
        }
        private string QueryForAllStockDetails(long? descriptionId, long? mobilePartId, long orgId, long branchId, string stockStatus, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (orgId > 0)
            {
                param += string.Format(@" and sd.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@" and sd.BranchId={0}", branchId);
            }
            if (descriptionId != null && descriptionId > 0)
            {
                param += string.Format(@" and sd.DescriptionId ={0}", descriptionId);
            }
            if (mobilePartId != null && mobilePartId > 0)
            {
                param += string.Format(@" and sd.MobilePartId ={0}", mobilePartId);
            }
            if (!string.IsNullOrEmpty(stockStatus))
            {
                param += string.Format(@"and sd.StockStatus Like '%{0}%'", stockStatus);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(sd.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(sd.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(sd.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select m.ModelName,sd.DescriptionId,p.MobilePartName'PartsName',sd.MobilePartId,
p.MobilePartCode'PartsCode',sd.CostPrice,
sd.SellPrice,sd.Quantity,sd.StockStatus,sd.Remarks,sd.EntryDate 
From tblMobilePartStockDetails sd
Left Join tblModelSS m on sd.DescriptionId=m.ModelId
Left join tblMobileParts p on sd.MobilePartId=p.MobilePartId
Where 1=1{0}
Order By sd.EntryDate desc
", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveMobilePartStockOutByGoodToFaulty(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long orgId, long branchId, long userId)
        {
            bool IsSuccess = false;
            var warehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(orgId, branchId).FirstOrDefault();
            List<MobilePartStockDetail> stockDetails = new List<MobilePartStockDetail>();

            foreach (var item in mobilePartStockDetailDTO)
            {
                var reqQty = item.Quantity;
                var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByModelAndBranch(orgId, item.DescriptionId.Value, branchId).Where(i => i.MobilePartId == item.MobilePartId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

                if (partsInStock.Count() > 0)
                {
                    int remainQty = reqQty;
                    foreach (var stock in partsInStock)
                    {

                        var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
                        var stockOutQty = 0;
                        if (totalStockqty <= remainQty)
                        {
                            stock.StockOutQty += totalStockqty;
                            stockOutQty = totalStockqty.Value;
                            remainQty -= totalStockqty.Value;
                        }
                        else
                        {
                            stockOutQty = remainQty;
                            stock.StockOutQty += remainQty;
                            remainQty = 0;
                        }


                        MobilePartStockDetail stockDetail = new MobilePartStockDetail()
                        {
                            DescriptionId = item.DescriptionId,
                            SWarehouseId = warehouse.SWarehouseId,
                            MobilePartId = item.MobilePartId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = stockOutQty,
                            Remarks = "Stock-Out By Faulty Add",
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockOut,
                            //ReferrenceNumber = reqInfo.RequsitionCode
                        };
                        stockDetails.Add(stockDetail);
                        mobilePartStockInfoRepository.Update(stock);
                        if (remainQty == 0)
                        {
                            break;
                        }
                    }
                }
            }
            mobilePartStockDetailRepository.InsertAll(stockDetails);
            return mobilePartStockDetailRepository.Save();
        }
    }
}
