using ERPBLL.Accounts;
using ERPBLL.Accounts.Interface;
using ERPBLL.Configuration;
using ERPBLL.Configuration.Interface;
using ERPBLL.ControlPanel;
using ERPBLL.ControlPanel.Interface;
using ERPBLL.FrontDesk;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.Inventory;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production;
using ERPBLL.Production.Interface;
using ERPBLL.Report;
using ERPBLL.Report.Interface;
using ERPBLL.ReportSS;
using ERPBLL.ReportSS.Interface;
using ERPBLL.SalesAndDistribution;
using ERPBLL.SalesAndDistribution.Interface;
using ERPDAL.AccountsDAL;
using ERPDAL.ConfigurationDAL;
using ERPDAL.ControlPanelDAL;
using ERPDAL.FrontDeskDAL;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using ERPDAL.SalesAndDistributionDAL;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace ERPWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            // e.g. container.RegisterType<ITestService, TestService>();
            // Inventory Database
            #region Inventory
            container.RegisterType<ISTransferDetailsMToMBusiness, STransferDetailsMToMBusiness>();
            container.RegisterType<ISTransferInfoMToMBusiness, STransferInfoMToMBusiness>();
            container.RegisterType<IIQCStockDetailBusiness, IQCStockDetailBusiness>();
            container.RegisterType<IIQCStockInfoBusiness, IQCStockInfoBusiness>();
            container.RegisterType<IIQCItemReqDetailList, IQCItemReqDetailListBusiness>();
            container.RegisterType<IIQCItemReqInfoList, IQCItemReqInfoListBusiness>();
            container.RegisterType<IIQCBusiness, IQCBusiness>();
            container.RegisterType<IWarehouseStockDetailBusiness, WarehouseStockDetailBusiness>();
            container.RegisterType<IWarehouseStockInfoBusiness, WarehouseStockInfoBusiness>();
            container.RegisterType<IItemBusiness, ItemBusiness>();
            container.RegisterType<IUnitBusiness, UnitBusiness>();
            container.RegisterType<IItemTypeBusiness, ItemTypeBusiness>();
            container.RegisterType<IWarehouseBusiness, WarehouseBusiness>();
            container.RegisterType<IWarehouseFaultyStockInfoBusiness, WarehouseFaultyStockInfoBusiness>();
            container.RegisterType<IWarehouseFaultyStockDetailBusiness, WarehouseFaultyStockDetailBusiness>();
            container.RegisterType<IItemPreparationInfoBusiness, ItemPreparationInfoBusiness>();
            container.RegisterType<IItemPreparationDetailBusiness, ItemPreparationDetailBusiness>();
            container.RegisterType<ERPBLL.Inventory.Interface.ISupplierBusiness, ERPBLL.Inventory.SupplierBusiness>();
            container.RegisterType<IInventoryReportBusiness, InventoryReportBusiness>();
            container.RegisterType<ERPBLL.Inventory.Interface.ICategoryBusiness, ERPBLL.Inventory.CategoryBusiness>();
            container.RegisterType<ERPBLL.Inventory.Interface.ICategoryBusiness, ERPBLL.Inventory.CategoryBusiness>();
            container.RegisterType<ERPBLL.Inventory.Interface.IBrandBusiness, ERPBLL.Inventory.BrandBusiness>();
            container.RegisterType<ERPBLL.Inventory.Interface.IBrandCategoriesBusiness, ERPBLL.Inventory.BrandCategoriesBusiness>();
            container.RegisterType<ERPBLL.Inventory.Interface.IColorBusiness, ERPBLL.Inventory.ColorBusiness>();
            container.RegisterType<ERPBLL.Inventory.Interface.IModelColorBusiness, ERPBLL.Inventory.ModelColorBusiness>();
            container.RegisterType<ERPBLL.Inventory.Interface.IHandSetStockBusiness, ERPBLL.Inventory.HandSetStockBusiness>();
            container.RegisterType<ERPBLL.Inventory.Interface.IBrandCategoriesBusiness, ERPBLL.Inventory.BrandCategoriesBusiness>();
            container.RegisterType<IInventoryUnitOfWork, InventoryUnitOfWork>(); // database 
            #endregion

            // Production Database
            #region Production
            container.RegisterType<IRepairSectionSemiFinishTransferDetailsBusiness, RepairSectionSemiFinishTransferDetailsBusiness>();
            container.RegisterType<IRepairSectionSemiFinishTransferInfoBusiness, RepairSectionSemiFinishTransferInfoBusiness>();
            container.RegisterType<IRepairSectionSemiFinishStockDetailsBusiness, RepairSectionSemiFinishStockDetailsBusiness>();
            container.RegisterType<IRepairSectionSemiFinishStockInfoBusiness, RepairSectionSemiFinishStockInfoBusiness>();
            container.RegisterType<ILotInLogBusiness, LotInLogBusiness>();
            container.RegisterType<IProductionLineBusiness, ProductionLineBusiness>();
            container.RegisterType<IRequsitionDetailBusiness, RequsitionDetailBusiness>();
            container.RegisterType<IRequsitionInfoBusiness, RequsitionInfoBusiness>();
            container.RegisterType<IProductionStockDetailBusiness, ProductionStockDetailBusiness>();
            container.RegisterType<IProductionStockInfoBusiness, ProductionStockInfoBusiness>();
            container.RegisterType<IItemReturnInfoBusiness, ItemReturnInfoBusiness>();
            container.RegisterType<IItemReturnDetailBusiness, ItemReturnDetailBusiness>();
            container.RegisterType<IDescriptionBusiness, DescriptionBusiness>();
            container.RegisterType<IFinishGoodsInfoBusiness, FinishGoodsInfoBusiness>();
            container.RegisterType<IFinishGoodsRowMaterialBusiness, FinishGoodsRowMaterialBusiness>();
            container.RegisterType<IFinishGoodsStockDetailBusiness, FinishGoodsStockDetailBusiness>();
            container.RegisterType<IFinishGoodsStockInfoBusiness, FinishGoodsStockInfoBusiness>();
            container.RegisterType<IFinishGoodsStockDetailBusiness, FinishGoodsStockDetailBusiness>();
            container.RegisterType<IFinishGoodsSendToWarehouseInfoBusiness, FinishGoodsSendToWarehouseInfoBusiness>();
            container.RegisterType<IFinishGoodsSendToWarehouseDetailBusiness, FinishGoodsSendToWarehouseDetailBusiness>();
            container.RegisterType<IAssemblyLineBusiness, AssemblyLineBusiness>();
            container.RegisterType<ITransferStockToAssemblyInfoBusiness, TransferStockToAssemblyInfoBusiness>();
            container.RegisterType<ITransferStockToAssemblyDetailBusiness, TransferStockToAssemblyDetailBusiness>();
            container.RegisterType<IAssemblyLineStockInfoBusiness, AssemblyLineStockInfoBusiness>();
            container.RegisterType<IAssemblyLineStockDetailBusiness, AssemblyLineStockDetailBusiness>();
            container.RegisterType<IQualityControlBusiness, QualityControlBusiness>();
            container.RegisterType<ITransferStockToQCInfoBusiness, TransferStockToQCInfoBusiness>();
            container.RegisterType<ITransferStockToQCDetailBusiness, TransferStockToQCDetailBusiness>();
            container.RegisterType<IQCLineStockInfoBusiness, QCLineStockInfoBusiness>();
            container.RegisterType<IQCLineStockDetailBusiness, QCLineStockDetailBusiness>();
            container.RegisterType<IRepairLineBusiness, RepairLineBusiness>();
            container.RegisterType<IPackagingLineBusiness, PackagingLineBusiness>();
            container.RegisterType<ITransferFromQCInfoBusiness, TransferFromQCInfoBusiness>();
            container.RegisterType<ITransferFromQCDetailBusiness, TransferFromQCDetailBusiness>();
            container.RegisterType<IPackagingLineStockInfoBusiness, PackagingLineStockInfoBusiness>();
            container.RegisterType<IPackagingLineStockDetailBusiness, PackagingLineStockDetailBusiness>();
            container.RegisterType<ITransferStockToPackagingLine2InfoBusiness, TransferStockToPackagingLine2InfoBusiness>();
            container.RegisterType<ITransferStockToPackagingLine2DetailBusiness, TransferStockToPackagingLine2DetailBusiness>();
            container.RegisterType<IRepairLineStockInfoBusiness, RepairLineStockInfoBusiness>();
            container.RegisterType<IRepairLineStockDetailBusiness, RepairLineStockDetailBusiness>();
            container.RegisterType<ITransferRepairItemToQcInfoBusiness, TransferRepairItemToQcInfoBusiness>();
            container.RegisterType<ITransferRepairItemToQcDetailBusiness, TransferRepairItemToQcDetailBusiness>();
            container.RegisterType<IQCItemStockInfoBusiness, QCItemStockInfoBusiness>();
            container.RegisterType<IQCItemStockDetailBusiness, QCItemStockDetailBusiness>();
            container.RegisterType<IRepairItemStockInfoBusiness, RepairItemStockInfoBusiness>();
            container.RegisterType<IRepairItemStockDetailBusiness, RepairItemStockDetailBusiness>();
            container.RegisterType<IPackagingItemStockInfoBusiness, PackagingItemStockInfoBusiness>();
            container.RegisterType<IPackagingItemStockDetailBusiness, PackagingItemStockDetailBusiness>();
            container.RegisterType<IQRCodeTraceBusiness, QRCodeTraceBusiness>();
            container.RegisterType<IFaultyItemStockInfoBusiness, FaultyItemStockInfoBusiness>();
            container.RegisterType<IFaultyItemStockDetailBusiness, FaultyItemStockDetailBusiness>();
            container.RegisterType<IFaultyCaseBusiness, FaultyCaseBusiness>();
            container.RegisterType<IRepairItemBusiness, RepairItemBusiness>();
            container.RegisterType<IRepairSectionFaultyItemTransferInfoBusiness, RepairSectionFaultyItemTransferInfoBusiness>();
            container.RegisterType<IRepairSectionRequisitionInfoBusiness, RepairSectionRequisitionInfoBusiness>();
            container.RegisterType<IRepairSectionRequisitionDetailBusiness, RepairSectionRequisitionDetailBusiness>();
            container.RegisterType<IProductionFaultyStockDetailBusiness, ProductionFaultyStockDetailBusiness>();
            container.RegisterType<IProductionFaultyStockInfoBusiness, ProductionFaultyStockInfoBusiness>();
            container.RegisterType<IRepairSectionFaultyItemTransferDetailBusiness, RepairSectionFaultyItemTransferDetailBusiness>();
            container.RegisterType<IQCPassTransferInformationBusiness, QCPassTransferInformationBusiness>();
            container.RegisterType<IProductionAssembleStockDetailBusiness, ProductionAssembleStockDetailBusiness>();
            container.RegisterType<IProductionAssembleStockInfoBusiness, ProductionAssembleStockInfoBusiness>();
            container.RegisterType<IProductionToPackagingStockTransferInfoBusiness, ProductionToPackagingStockTransferInfoBusiness>();
            container.RegisterType<IProductionToPackagingStockTransferDetailBusiness, ProductionToPackagingStockTransferDetailBusiness>();
            container.RegisterType<IQRCodeTransferToRepairInfoBusiness, QRCodeTransferToRepairInfoBusiness>();
            container.RegisterType<IQRCodeProblemBusiness, QRCodeProblemBusiness>();
            container.RegisterType<IRequisitionItemInfoBusiness, RequisitionItemInfoBusiness>();
            container.RegisterType<IRequisitionItemDetailBusiness, RequisitionItemDetailBusiness>();
            container.RegisterType<ITempQRCodeTraceBusiness, TempQRCodeTraceBusiness>();
            container.RegisterType<IQCPassTransferDetailBusiness, QCPassTransferDetailBusiness>();
            container.RegisterType<IMiniStockTransferInfoBusiness, MiniStockTransferInfoBusiness>();
            container.RegisterType<IMiniStockTransferDetailBusiness, MiniStockTransferDetailBusiness>();
            container.RegisterType<IIMEITransferToRepairInfoBusiness, IMEITransferToRepairInfoBusiness>();
            container.RegisterType<ITransferToPackagingRepairInfoBusiness, TransferToPackagingRepairInfoBusiness>();
            container.RegisterType<ITransferToPackagingRepairDetailBusiness, TransferToPackagingRepairDetailBusiness>();
            container.RegisterType<IPackagingRepairRawStockInfoBusiness, PackagingRepairRawStockInfoBusiness>();
            container.RegisterType<IPackagingRepairRawStockDetailBusiness, PackagingRepairRawStockDetailBusiness>();
            container.RegisterType<IPackagingRepairItemStockInfoBusiness, PackagingRepairItemStockInfoBusiness>();
            container.RegisterType<IPackagingRepairItemStockDetailBusiness, PackagingRepairItemStockDetailBusiness>();
            container.RegisterType<IIMEITransferToRepairDetailBusiness, IMEITransferToRepairDetailBusiness>();
            container.RegisterType<ITransferPackagingRepairItemToQcInfoBusiness, TransferPackagingRepairItemToQcInfoBusiness>();
            container.RegisterType<ITransferPackagingRepairItemToQcDetailBusiness, TransferPackagingRepairItemToQcDetailBusiness>();
            container.RegisterType<IPackagingFaultyStockInfoBusiness, PackagingFaultyStockInfoBusiness>();
            container.RegisterType<IPackagingFaultyStockDetailBusiness, PackagingFaultyStockDetailBusiness>();
            container.RegisterType<IIMEIGenerator, IMEIGenerator>();
            container.RegisterType<IGeneratedIMEIBusiness, GeneratedIMEIBusiness>();
            container.RegisterType<IStockItemReturnInfoBusiness, StockItemReturnInfoBusiness>();
            container.RegisterType<IStockItemReturnDetailBusiness, StockItemReturnDetailBusiness>();
            container.RegisterType<IProductionUnitOfWork, ProductionUnitOfWork>();
            #endregion

            // ControlPanel Database
            #region ControlPanel
            container.RegisterType<ISubMenuBusiness, SubMenuBusiness>();
            container.RegisterType<IManiMenuBusiness, ManiMenuBusiness>();
            container.RegisterType<IAppUserBusiness, AppUserBusiness>();
            container.RegisterType<IRoleBusiness, RoleBusiness>();
            container.RegisterType<IBranchBusiness, BranchBusiness>();
            container.RegisterType<IModuleBusiness, ModuleBusiness>();
            container.RegisterType<IOrganizationBusiness, OrganizationBusiness>();
            container.RegisterType<IModuleBusiness, ModuleBusiness>();
            container.RegisterType<IOrganizationAuthBusiness, OrganizationAuthBusiness>();
            container.RegisterType<IUserAuthorizationBusiness, UserAuthorizationBusiness>();
            container.RegisterType<IRoleAuthorizationBusiness, RoleAuthorizationBusiness>();
            container.RegisterType<IControlPanelUnitOfWork, ControlPanelUnitOfWork>();
            #endregion

            // Configuration Database
            #region Configuration
            container.RegisterType<IModelSSBusiness, ModelSSBusiness>();
            container.RegisterType<IBrandSSBusiness, BrandSSBusiness>();
            container.RegisterType<IColorSSBusiness, ColorSSBusiness>();
            container.RegisterType<IDealerSSBusiness, DealerSSBusiness>();
            container.RegisterType<IScrapStockDetailBusiness, ScrapStockDetailBusiness>();
            container.RegisterType<IScrapStockInfoBusiness, ScrapStockInfoBusiness>();
            container.RegisterType<IStockTransferInfoModelToModelBusiness, StockTransferInfoModelToModelBusiness>();
            container.RegisterType<IStockTransferDetailModelToModelBusiness, StockTransferDetailModelToModelBusiness>();
            container.RegisterType<IMissingStockBusiness, MissingStockBusiness>();
            container.RegisterType<ERPBLL.Configuration.Interface.IHandSetStockBusiness, ERPBLL.Configuration.HandSetStockBusiness>();
            container.RegisterType<IFaultyStockDetailBusiness, FaultyStockDetailBusiness>();
            container.RegisterType<IFaultyStockInfoBusiness, FaultyStockInfoBusiness>();
            container.RegisterType<IWorkShopBusiness, WorkShopBusiness>();
            container.RegisterType<IRepairBusiness, RepairBusiness>();
            container.RegisterType<IServiceBusiness, ServiceBusiness>();
            container.RegisterType<IFaultBusiness, FaultBusiness>();
            container.RegisterType<ITransferDetailBusiness, TransferDetailBusiness>();
            container.RegisterType<ITransferInfoBusiness, TransferInfoBusiness>();
            container.RegisterType<IBranchBusiness2, BranchBusiness2>();
            container.RegisterType<IMobilePartStockDetailBusiness, MobilePartStockDetailBusiness>();
            container.RegisterType<IMobilePartStockInfoBusiness, MobilePartStockInfoBusiness>();
            container.RegisterType<IServicesWarehouseBusiness, ServicesWarehouseBusiness>();
            container.RegisterType<ICustomerServiceBusiness, CustomerServiceBusiness>();
            container.RegisterType<ITechnicalServiceBusiness, TechnicalServiceBusiness>();
            container.RegisterType<ICustomerBusiness, CustomerBusiness>();
            container.RegisterType<IMobilePartBusiness, MobilePartBusiness>();
            container.RegisterType<IClientProblemBusiness, ClientProblemBusiness>();
            container.RegisterType<IAccessoriesBusiness, AccessoriesBusiness>();
            container.RegisterType<IConfigurationUnitOfWork, ConfigurationUnitOfWork>();
            #endregion

            // FrontDesk Database
            #region FrontDesk
            container.RegisterType<IHandsetChangeTraceBusiness, HandsetChangeTraceBusiness>();
            container.RegisterType<IFaultyStockAssignTSBusiness, FaultyStockAssignTSBusiness>();
            container.RegisterType<IJobOrderReturnDetailBusiness, JobOrderReturnDetailBusiness>();
            container.RegisterType<IJobOrderTransferDetailBusiness, JobOrderTransferDetailBusiness>();
            container.RegisterType<IInvoiceDetailBusiness, InvoiceDetailBusiness>();
            container.RegisterType<IInvoiceInfoBusiness, InvoiceInfoBusiness>();
            container.RegisterType<ITsStockReturnDetailsBusiness, TsStockReturnDetailsBusiness>();
            container.RegisterType<ITsStockReturnInfoBusiness, TsStockReturnInfoBusiness>();
            container.RegisterType<IJobOrderRepairBusiness, JobOrderRepairBusiness>();
            container.RegisterType<IJobOrderServiceBusiness, JobOrderServiceBusiness>();
            container.RegisterType<IJobOrderFaultBusiness, JobOrderFaultBusiness>();
            container.RegisterType<IJobOrderProblemBusiness, JobOrderProblemBusiness>();
            container.RegisterType<IJobOrderAccessoriesBusiness, JobOrderAccessoriesBusiness>();
            container.RegisterType<IJobOrderTSBusiness, JobOrderTSBusiness>();
            container.RegisterType<ITechnicalServicesStockBusiness, TechnicalServicesStockBusiness>();
            container.RegisterType<IRequsitionDetailForJobOrderBusiness, RequsitionDetailForJobOrderBusiness>();
            container.RegisterType<IRequsitionInfoForJobOrderBusiness, RequsitionInfoForJobOrderBusiness>();
            container.RegisterType<IJobOrderBusiness, JobOrderBusiness>();
            container.RegisterType<IFrontDeskUnitOfWork, FrontDeskUnitOfWork>();
            #endregion

            // Report Purpose
            #region Report
            container.RegisterType<IJobOrderReportBusiness, JobOrderReportBusiness>();
            container.RegisterType<IProductionReportBusiness, ProductionReportBusiness>();

            #endregion

            #region Sales & Distribution
            // Sales & Distribution 
            container.RegisterType<ISalesAndDistributionUnitOfWork, SalesAndDistributionUnitOfWork>();
            container.RegisterType<IDealerBusiness, DealerBusiness>();
            container.RegisterType<IBTRCApprovedIMEIBusiness, BTRCApprovedIMEIBusiness>();
            container.RegisterType<IItemStockBusiness, ItemStockBusiness>();
            container.RegisterType<ERPBLL.SalesAndDistribution.Interface.ICategoryBusiness, ERPBLL.SalesAndDistribution.CategoryBusiness>();
            container.RegisterType<ERPBLL.SalesAndDistribution.Interface.IBrandBusiness, ERPBLL.SalesAndDistribution.BrandBusiness>();
            container.RegisterType<ERPBLL.SalesAndDistribution.Interface.IBrandCategoriesBusiness, ERPBLL.SalesAndDistribution.BrandCategoriesBusiness>();
            container.RegisterType<IModelBusiness, ModelBusiness>();
            container.RegisterType<ERPBLL.SalesAndDistribution.Interface.IColorBusiness, ERPBLL.SalesAndDistribution.ColorBusiness>();
            container.RegisterType<ERPBLL.SalesAndDistribution.Interface.IModelColorBusiness, ERPBLL.SalesAndDistribution.ModelColorBusiness>();
            container.RegisterType<IDivisionBusiness, DivisionBusiness>();
            container.RegisterType<IDistrictBusiness, DistrictBusiness>();
            container.RegisterType<IZoneBusiness, ZoneBusiness>();
            container.RegisterType<IRSMBusiness, RSMBusiness>();
            container.RegisterType<IASMBusiness, ASMBusiness>();
            container.RegisterType<ITSEBusiness, TSEBusiness>();
            container.RegisterType<ISalesRepresentativeBusiness, SalesRepresentativeBusiness>();
            container.RegisterType<IDealerRequisitionInfoBusiness, DealerRequisitionInfoBusiness>();
            container.RegisterType<IDealerRequisitionDetailBusiness, DealerRequisitionDetailBusiness>();            
            #endregion

            #region Accounts
            container.RegisterType<IAccountsUnitOfWork, AccountsUnitOfWork>();
            container.RegisterType<IAccountsHeadBusiness, AccountsHeadBusiness>();
            container.RegisterType<IJournalBusiness, JournalBusiness>();
            container.RegisterType<IFinanceYearBusiness, FinanceYearBusiness>();
            container.RegisterType<IChequeBookBusiness, ChequeBookBusiness>();
            container.RegisterType<ICustomersBusiness,CustomersBusiness>();
            container.RegisterType<ERPBLL.Accounts.Interface.ISupplierBusiness, ERPBLL.Accounts.SupplierBusiness>();
            #endregion

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}