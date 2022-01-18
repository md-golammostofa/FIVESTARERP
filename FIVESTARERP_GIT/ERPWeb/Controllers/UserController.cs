using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.FrontDesk.DTOModels;
using ERPBO.FrontDesk.ViewModels;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ReportModels;
using ERPBO.Production.ViewModels;
using ERPWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class UserController : BaseController
    {
        // GET: User
        private readonly IProductionLineBusiness _productionLineBusiness;
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IFinishGoodsStockInfoBusiness _finishGoodsStockInfoBusiness;
        private readonly IFinishGoodsStockDetailBusiness _finishGoodsStockDetailBusiness;
        private readonly IItemReturnInfoBusiness _itemReturnInfoBusiness;
        private readonly IJobOrderBusiness _jobOrderBusiness;
        private readonly IJobOrderTSBusiness _jobOrderTSBusiness;
        private readonly IRequsitionInfoForJobOrderBusiness _requsitionInfoForJobOrderBusiness;
        private readonly ITsStockReturnInfoBusiness _tsStockReturnInfoBusiness;
        private readonly ITsStockReturnDetailsBusiness _tsStockReturnDetailsBusiness;
        private readonly IMobilePartBusiness _mobilePartBusiness;
        private readonly IDescriptionBusiness _descriptionBusiness;
        private readonly IPackagingLineBusiness _packagingLineBusiness;
        private readonly IModelSSBusiness _modelSSBusiness;
        private readonly IAssemblyLineBusiness _assemblyLineBusiness;
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;
        private readonly ILotInLogBusiness _lotInLogBusiness;
        private readonly IQCPassTransferDetailBusiness _qCPassTransferDetailBusiness;
        private readonly IRepairOutBusiness _repairOutBusiness;
        private readonly IRepairInBusiness _repairInBusiness;
        private readonly IQC1DetailBusiness _qC1DetailBusiness;
        private readonly IQC2DetailBusiness _qC2DetailBusiness;
        private readonly IQC3DetailBusiness _qC3DetailBusiness;

        public UserController (IRequsitionInfoBusiness requsitionInfoBusiness, IFinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness, IProductionLineBusiness productionLineBusiness, IFinishGoodsStockDetailBusiness finishGoodsStockDetailBusiness,IItemReturnInfoBusiness itemReturnInfoBusiness, IJobOrderBusiness jobOrderBusiness, IJobOrderTSBusiness jobOrderTSBusiness, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, ITsStockReturnInfoBusiness tsStockReturnInfoBusiness, ITsStockReturnDetailsBusiness tsStockReturnDetailsBusiness, IMobilePartBusiness mobilePartBusiness, IDescriptionBusiness descriptionBusiness, IPackagingLineBusiness packagingLineBusiness, IModelSSBusiness modelSSBusiness, IAssemblyLineBusiness assemblyLineBusiness, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, ILotInLogBusiness lotInLogBusiness, IQCPassTransferDetailBusiness qCPassTransferDetailBusiness, IRepairOutBusiness repairOutBusiness, IRepairInBusiness repairInBusiness, IQC1DetailBusiness qC1DetailBusiness, IQC2DetailBusiness qC2DetailBusiness, IQC3DetailBusiness qC3DetailBusiness)
        {
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._finishGoodsStockInfoBusiness = finishGoodsStockInfoBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._finishGoodsStockDetailBusiness = finishGoodsStockDetailBusiness;
            this._itemReturnInfoBusiness = itemReturnInfoBusiness;
            this._jobOrderBusiness = jobOrderBusiness;
            this._jobOrderTSBusiness = jobOrderTSBusiness;
            this._requsitionInfoForJobOrderBusiness=requsitionInfoForJobOrderBusiness;
            this._tsStockReturnInfoBusiness = tsStockReturnInfoBusiness;
            this._tsStockReturnDetailsBusiness = tsStockReturnDetailsBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._descriptionBusiness = descriptionBusiness;
            this._packagingLineBusiness = packagingLineBusiness;
            this._modelSSBusiness = modelSSBusiness;
            this._assemblyLineBusiness = assemblyLineBusiness;
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            this._lotInLogBusiness = lotInLogBusiness;
            this._qCPassTransferDetailBusiness = qCPassTransferDetailBusiness;
            this._repairInBusiness = repairInBusiness;
            this._repairOutBusiness = repairOutBusiness;
            this._qC1DetailBusiness = qC1DetailBusiness;
            this._qC2DetailBusiness = qC2DetailBusiness;
            this._qC3DetailBusiness = qC3DetailBusiness;
        }
        public ActionResult Index(string flag)
        {
            if(User.AppType == ApplicationType.ERP)
            {
                if (string.IsNullOrEmpty(flag))
                {
                    ViewBag.ddlAssemblyLine = _assemblyLineBusiness.GetAssemblyLines(User.OrgId).Select(s => new SelectListItem { Text = s.AssemblyLineName, Value = s.AssemblyLineId.ToString() }).ToList();
                }
                // Requisition Summery
                IEnumerable<DashboardRequisitionSummeryDTO> dto = _requsitionInfoBusiness.DashboardRequisitionSummery(User.OrgId);
                IEnumerable<DashboardRequisitionSummeryViewModel> viewModel = new List<DashboardRequisitionSummeryViewModel>();
                AutoMapper.Mapper.Map(dto, viewModel);
                ViewBag.RequisitionSummery = viewModel;
                // Requisition Status // YSN

                var reqAccepted = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Accepted);
                ViewBag.RequisitionAccepted = (reqAccepted == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Accepted", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqAccepted.StateStatus, TotalCount = reqAccepted.TotalCount };

                var reqApproved = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Approved);
                ViewBag.RequisitionApproved = (reqApproved == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Approved", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqApproved.StateStatus, TotalCount = reqApproved.TotalCount };

                var reqPending = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Pending);
                ViewBag.RequisitionPending = (reqPending == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Pending", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqPending.StateStatus, TotalCount = reqPending.TotalCount };

                var reqRecheck = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Rechecked);
                ViewBag.RequisitionRecheck = (reqRecheck == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Rechecked", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqRecheck.StateStatus, TotalCount = reqRecheck.TotalCount };

                var reqCancel = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Canceled);
                ViewBag.RequisitionCancel = (reqCancel == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Canceled", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqCancel.StateStatus, TotalCount = reqCancel.TotalCount };

                var reqReject = dto.FirstOrDefault(req => req.StateStatus == RequisitionStatus.Rejected);
                ViewBag.RequisitionReject = (reqReject == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = "Rejected", TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = reqReject.StateStatus, TotalCount = reqReject.TotalCount };

                //--------------------//
                // Line wise daily Production
                IEnumerable<DashboardLineWiseProductionDTO> dashboardLineWises = _finishGoodsStockDetailBusiness.DashboardLineWiseDailyProduction(User.OrgId);
                IEnumerable<DashboardLineWiseProductionViewModel> dashboardLines = new List<DashboardLineWiseProductionViewModel>();
                AutoMapper.Mapper.Map(dashboardLineWises, dashboardLines);
                ViewBag.DashboardLineWiseProductionViewModel = dashboardLines;

                // Line wise OverAll Production
                IEnumerable<DashboardLineWiseProductionDTO> overalldto = _finishGoodsStockDetailBusiness.DashboardLineWiseOverAllProduction(User.OrgId);
                IEnumerable<DashboardLineWiseProductionViewModel> overallViews = new List<DashboardLineWiseProductionViewModel>();
                AutoMapper.Mapper.Map(overalldto, overallViews);
                ViewBag.DashboardLineWiseOverallProductionViewModel = overallViews;

                // Faculty daily wise Production
                IEnumerable<DashboardFacultyWiseProductionDTO> dailyFacultyDTO = _itemReturnInfoBusiness.DashboardFacultyDayWiseProduction(User.OrgId);
                IEnumerable<DashboardFacultyWiseProductionViewModel> dailyFacultyViews = new List<DashboardFacultyWiseProductionViewModel>();
                AutoMapper.Mapper.Map(dailyFacultyDTO, dailyFacultyViews);
                ViewBag.DashboardFacultyWiseProductionViewModel = dailyFacultyViews;

                // Faculty wise OveAll Production
                IEnumerable<DashboardFacultyWiseProductionDTO> OverAllFacultyDTO = _itemReturnInfoBusiness.DashboardFacultyOverAllWiseProduction(User.OrgId);
                IEnumerable<DashboardFacultyWiseProductionViewModel> OverAllFacultyViews = new List<DashboardFacultyWiseProductionViewModel>();
                AutoMapper.Mapper.Map(OverAllFacultyDTO, OverAllFacultyViews);
                ViewBag.DashboardFacultyWiseOverAllProductionViewModel = OverAllFacultyViews;

                #region Assembly Line Progress

                var assemblyProgressDto =_requsitionInfoBusiness.GetDashBoardAssemblyProgresses(null, null, User.OrgId);

                List<DashBoardAssemblyProgressViewModel> assemblyProgressViewModel = new List<DashBoardAssemblyProgressViewModel>();

                AutoMapper.Mapper.Map(assemblyProgressDto, assemblyProgressViewModel);
                ViewBag.AssemblyProgress = assemblyProgressViewModel;

                #endregion
                //Packaging Line Process
                #region Packaging Line Process
                var packagingLineProcess = _packagingLineBusiness.GetPackagingLineDashboard(User.OrgId);
                List<PackagingLineDashboardViewModel> packagingLineDashboardView = new List<PackagingLineDashboardViewModel>();
                AutoMapper.Mapper.Map(packagingLineProcess, packagingLineDashboardView);
                ViewBag.PackagingLineProcess = packagingLineDashboardView;
                #endregion

                return View();
            }
            else if(User.AppType == ApplicationType.Service)
            {
                IEnumerable<DashboardRequisitionSummeryDTO> dto = _jobOrderBusiness.DashboardJobOrderSummery(User.OrgId,User.BranchId);
                IEnumerable<DashboardRequisitionSummeryViewModel> viewModel = new List<DashboardRequisitionSummeryViewModel>();
                AutoMapper.Mapper.Map(dto, viewModel);
                ViewBag.JobOrderSummery = viewModel;

                var repairDone = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.RepairDone);
                ViewBag.RepairDone = (repairDone == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.RepairDone, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = repairDone.StateStatus, TotalCount = repairDone.TotalCount };

                var jobOrderApproved = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.JobInitiated);
                ViewBag.JobOrderApproved = (jobOrderApproved == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.JobInitiated, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = jobOrderApproved.StateStatus, TotalCount = jobOrderApproved.TotalCount };

                var deliveryDone = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.DeliveryDone);
                ViewBag.DeliveryDone = (deliveryDone == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.DeliveryDone, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = deliveryDone.StateStatus, TotalCount = deliveryDone.TotalCount };

                var jobOrdeAssignToTS = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.AssignToTS);
                ViewBag.JobOrdeAssignToTS = (jobOrdeAssignToTS == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.AssignToTS, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = jobOrdeAssignToTS.StateStatus, TotalCount = jobOrdeAssignToTS.TotalCount };

                var jobOrdeAssignToQC = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.QCAssigned);
                ViewBag.JobOrdeAssignToQC = (jobOrdeAssignToQC == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.QCAssigned, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = jobOrdeAssignToQC.StateStatus, TotalCount = jobOrdeAssignToQC.TotalCount };

                var jobOrdeHandsetChange = dto.FirstOrDefault(req => req.StateStatus == JobOrderStatus.HandSetChange);
                ViewBag.JobOrdeHandsetChange = (jobOrdeHandsetChange == null) ? new DashboardRequisitionSummeryViewModel { StateStatus = JobOrderStatus.HandSetChange, TotalCount = 0 } : new DashboardRequisitionSummeryViewModel { StateStatus = jobOrdeHandsetChange.StateStatus, TotalCount = jobOrdeHandsetChange.TotalCount };

                // Daily Job Orders
                IEnumerable<DashboardDailyReceiveJobOrderDTO> dashboardJobOrders = _jobOrderBusiness.DashboardDailyJobOrder(User.OrgId,User.BranchId);
                IEnumerable<DashboardDailyReceiveJobOrderViewModel> dashboardJob = new List<DashboardDailyReceiveJobOrderViewModel>();
                AutoMapper.Mapper.Map(dashboardJobOrders, dashboardJob);
                ViewBag.DashboardDailyJobOrderViewModel = dashboardJob;

                // Daily Sells//25-10-2020
                IEnumerable<DashboardSellsDTO> dailysellDTO = _jobOrderBusiness.DashboardDailySells(User.OrgId, User.BranchId);
                IEnumerable<DashboardSellsViewModel> dailyView = new List<DashboardSellsViewModel>();
                AutoMapper.Mapper.Map(dailysellDTO, dailyView);
                ViewBag.DashboardDailySellsViewModel = dailyView;

                // Not Assign Job//29-12-2020
                IEnumerable<DashboardDailyReceiveJobOrderDTO> notAssignDTO = _jobOrderBusiness.DashboardNotAssignJob(User.OrgId, User.BranchId);
                IEnumerable<DashboardDailyReceiveJobOrderViewModel> notAssignView = new List<DashboardDailyReceiveJobOrderViewModel>();
                AutoMapper.Mapper.Map(notAssignDTO, notAssignView);
                ViewBag.DashboardNotAssignJobViewModel = notAssignView;

                // Total Sells//25-10-2020
                IEnumerable<DashboardSellsDTO> totalDTO = _jobOrderBusiness.DashboardTotalSells(User.OrgId, User.BranchId);
                IEnumerable<DashboardSellsViewModel> totalView = new List<DashboardSellsViewModel>();
                AutoMapper.Mapper.Map(totalDTO, totalView);
                ViewBag.DashboardTotalSellsViewModel = totalView;

                // Daily Job Transfer//25-10-2020
                IEnumerable<DashboardDailyReceiveJobOrderDTO> dailyTransferDTO = _jobOrderBusiness.DashboardDailyTransferJob(User.OrgId, User.BranchId);
                IEnumerable<DashboardDailyReceiveJobOrderViewModel> dailyTransferView = new List<DashboardDailyReceiveJobOrderViewModel>();
                AutoMapper.Mapper.Map(dailyTransferDTO, dailyTransferView);
                ViewBag.DashboardTotalTransferViewModel = dailyTransferView;

                // Daily Job Receive//25-10-2020
                IEnumerable<DashboardDailyReceiveJobOrderDTO> receiveJobDTO = _jobOrderBusiness.DashboardDailyReceiveJob(User.OrgId, User.BranchId);
                IEnumerable<DashboardDailyReceiveJobOrderViewModel> receiveJOBView = new List<DashboardDailyReceiveJobOrderViewModel>();
                AutoMapper.Mapper.Map(receiveJobDTO, receiveJOBView);
                ViewBag.DashboardTotalReceiveViewModel = receiveJOBView;

                // Daily Warranty and Billing Job
                IEnumerable<DashboardDailyBillingAndWarrantyJobDTO> andWarrantyJobDTO = _jobOrderBusiness.DashboardDailyBillingAndWarrantyJob(User.OrgId, User.BranchId);
                IEnumerable<DashboardDailyBillingAndWarrantyJobViewModel> andWarrantyJobViewModels = new List<DashboardDailyBillingAndWarrantyJobViewModel>();
                AutoMapper.Mapper.Map(andWarrantyJobDTO, andWarrantyJobViewModels);
                ViewBag.DashboardDailyWarrantyandBillingViewModel = andWarrantyJobViewModels;

                // Daily singIn Out
                IEnumerable<DashboardDailySingInAndOutDTO> singInAndOutDTO = _jobOrderTSBusiness.DashboardDailySingInAndOuts(User.OrgId, User.BranchId);
                IEnumerable<DashboardDailySingInAndOutViewModel> inAndOutViewModels = new List<DashboardDailySingInAndOutViewModel>();
                AutoMapper.Mapper.Map(singInAndOutDTO, inAndOutViewModels);
                ViewBag.DashboardDailySingInOutViewModel = inAndOutViewModels;

                // Daily singIn Out/DashboardDailySingInAndOutByEng
                IEnumerable<DashboardDailySingInAndOutDTO> singInAndOutengDTO = _jobOrderTSBusiness.DashboardDailySingInAndOutByEng(User.OrgId, User.BranchId,User.UserId);
                IEnumerable<DashboardDailySingInAndOutViewModel> inAndOutengViewModels = new List<DashboardDailySingInAndOutViewModel>();
                AutoMapper.Mapper.Map(singInAndOutengDTO, inAndOutengViewModels);
                ViewBag.DashboardDailySingInOutengViewModel = inAndOutengViewModels;


                // Daily Spare Parts Request
                IEnumerable<DashboardRequestSparePartsDTO> sparePartsDTO = _requsitionInfoForJobOrderBusiness.DashboardRequestSpareParts(User.OrgId, User.BranchId);
                IEnumerable<DashboardRequestSparePartsViewModel> sparePartsViewModels = new List<DashboardRequestSparePartsViewModel>();
                AutoMapper.Mapper.Map(sparePartsDTO, sparePartsViewModels);
                ViewBag.DashboardDailySparePartsViewModel = sparePartsViewModels;

                // Requsition Pending
                IEnumerable<DashboardApprovedRequsitionDTO> pendingRequsitionDTO = _jobOrderBusiness.DashboardPendingRequsition(User.OrgId, User.BranchId);
                IEnumerable<DashboardApprovedRequsitionViewModel> pendingRequsitionViewModels = new List<DashboardApprovedRequsitionViewModel>();
                AutoMapper.Mapper.Map(pendingRequsitionDTO, pendingRequsitionViewModels);
                ViewBag.DashboardPendingViewModel = pendingRequsitionViewModels;

                // Requsition Current
                IEnumerable<DashboardApprovedRequsitionDTO> currentRequsitionDTO = _jobOrderBusiness.DashboardCurrentRequsition(User.OrgId, User.BranchId);
                IEnumerable<DashboardApprovedRequsitionViewModel> currentRequsitionViewModels = new List<DashboardApprovedRequsitionViewModel>();
                AutoMapper.Mapper.Map(currentRequsitionDTO, currentRequsitionViewModels);
                ViewBag.DashboardCurrentViewModel = currentRequsitionViewModels;

                // Requsition Another Branch
                IEnumerable<DashboardApprovedRequsitionDTO> anotherBranchReqDTO = _jobOrderBusiness.DashboardAnotherBranchRequsition(User.OrgId, User.BranchId);
                IEnumerable<DashboardApprovedRequsitionViewModel> anotherBranchReqViewModels = new List<DashboardApprovedRequsitionViewModel>();
                AutoMapper.Mapper.Map(anotherBranchReqDTO, anotherBranchReqViewModels);
                ViewBag.AnotherBranchRequsition = anotherBranchReqViewModels;

                // Return Spareparts
                IEnumerable<DashbordTsPartsReturnDTO> returnDTO = _tsStockReturnInfoBusiness.DashboardReturnParts(User.OrgId, User.BranchId);
                IEnumerable<DashbordTsPartsReturnViewModel> returnViewModels = new List<DashbordTsPartsReturnViewModel>();
                AutoMapper.Mapper.Map(returnDTO, returnViewModels);
                ViewBag.ReturnPartsViewModel = returnViewModels;

                // Call Center Approval
                IEnumerable<JobOrderDTO> callDto = _jobOrderBusiness.DashboardCallCenterApproval(User.OrgId,User.BranchId, User.UserId);
                IEnumerable<JobOrderViewModel> callViewModels = new List<JobOrderViewModel>();
                AutoMapper.Mapper.Map(callDto, callViewModels);
                ViewBag.CallCenterViewModel = callViewModels;
                //QC Center Approval
                IEnumerable<JobOrderDTO> qcdto = _jobOrderBusiness.DashboardQCStatus(User.OrgId, User.BranchId, User.UserId);
                IEnumerable<JobOrderViewModel> qcviewModel = new List<JobOrderViewModel>();
                AutoMapper.Mapper.Map(qcdto, qcviewModel);
                ViewBag.QCStatusViewModel = qcviewModel;
                //TotalPending Job
                IEnumerable<JobOrderDTO> pendingDelivery = _jobOrderBusiness.GetDashBoardPendingDeliveryJob(User.OrgId, User.BranchId);
                IEnumerable<JobOrderViewModel> pdViewModel = new List<JobOrderViewModel>();
                AutoMapper.Mapper.Map(pendingDelivery, pdViewModel);
                ViewBag.PendingDelivery = pdViewModel;
                //TodayQCPassFail
                IEnumerable<JobOrderDTO> todayQc = _jobOrderBusiness.TodayQCPassFail(User.OrgId, User.BranchId);
                IEnumerable<JobOrderViewModel> qcPFViewModels = new List<JobOrderViewModel>();
                AutoMapper.Mapper.Map(todayQc, qcPFViewModels);
                ViewBag.TodayQCPassFail = qcPFViewModels;

                return View("Index2");
            }
            else
            {
                return View("Index3");
            }
        }

        public ActionResult GetAssemblyLineWiseDashboardData(long assemblyId)
        {
            var tempData = _tempQRCodeTraceBusiness.GetAssemblyLineWiseDataForDashBoard(assemblyId, User.OrgId).FirstOrDefault();
            var result = _tempQRCodeTraceBusiness.GetAssemblyDashBoard(assemblyId, User.OrgId);
            return Json(new { BrandName = tempData != null ? tempData.BrandName : "", ItemName = tempData != null ? tempData.ItemName : "", ModelName = tempData != null ? tempData.ModelName : "", ColorName = tempData != null ? tempData.ColorName : "", Time = DateTime.Now.ToString("hh:mm tt"), Date = DateTime.Now.Date.ToString("dd-MMM-yyyy"), LotIn = result.TotalLotIn, QCPass = result.TotalQCPass, Pending = (result.TotalLotIn) - result.TotalQCPass/*(result.TotalQCPass + result.TotalQCFail - result.TotalRepairDone)*/, QCFail = result.TotalQCFail, TotalQC1 = result.TotalQC1, TotalQC2 = result.TotalQC2, TotalQC3 = result.TotalQC3, RepairDone = result.TotalRepairDone, RepairPending = (result.TotalQC1 + result.TotalQC2 + result.TotalQC3) - result.TotalRepairDone, TotalItem = result.TotalHandset, TotalMiniWarehouseReceived = result.MiniStockReceivedQty });
        }

        public ActionResult GetINPUT_OUTPUTBarchart(long assemblyId)
        {
            var qcPass = _qCPassTransferDetailBusiness.GetAllQCPassLogDataByAssemblyIdWithTimeWise(assemblyId, DateTime.Today, User.OrgId);
            var queryForQCPass = qcPass.AsEnumerable()
                .GroupBy(row => row.EntryDate.Value.ToString("HH:00"))
                .Select(grp => new
                {
                    Hour = grp.Key,
                    Count = grp.Count()
                }).ToList().OrderBy(s => s.Hour).ToList();

            var lotIn = _lotInLogBusiness.GetAllLotInLogDataByAssemblyIdWithTimeWise(assemblyId, DateTime.Today, User.OrgId);
            var queryForLotIn = lotIn.AsEnumerable()
                .GroupBy(row => row.EntryDate.Value.ToString("HH:00"))
                .Select(grp => new
                {
                    Hour = grp.Key,
                    Count = grp.Count()
                }).ToList().OrderBy(s => s.Hour).ToList();

            List<BarChatTimeWiseData> barChatTimeWiseDatas = new List<BarChatTimeWiseData>(); List<BarChatTimeWiseData> listBar = new List<BarChatTimeWiseData>();
            List<string> hour = new List<string>();
            List<int> qcPassCount = new List<int>();
            List<int> lotInCount = new List<int>();
            List<QCPassAndLotInHourSameData> sameTimesForLotIn = new List<QCPassAndLotInHourSameData>();
            List<QCPassAndLotInHourSameData> sameTimesForQCPass = new List<QCPassAndLotInHourSameData>();

            if (queryForQCPass.Count() > 0)
            {
                foreach (var item in queryForQCPass)
                {
                    foreach (var item2 in queryForLotIn)
                    {
                        if (item.Hour == item2.Hour)
                        {
                            BarChatTimeWiseData barChatTimeWiseData = new BarChatTimeWiseData()
                            {
                                Hour = item.Hour,
                                QCPassCount = item.Count,
                                LotInCount = item2.Count,
                            };
                            barChatTimeWiseDatas.Add(barChatTimeWiseData);
                            //queryForLotIn.RemoveAll(s => s.Hour == item2.Hour);
                            QCPassAndLotInHourSameData sameTimeForLotIn = new QCPassAndLotInHourSameData()
                            {
                                Hour = item2.Hour,
                                Count = item2.Count,
                            };
                            sameTimesForLotIn.Add(sameTimeForLotIn);
                            QCPassAndLotInHourSameData sameTimeForQCPass = new QCPassAndLotInHourSameData()
                            {
                                Hour = item.Hour,
                                Count = item.Count,
                            };
                            sameTimesForQCPass.Add(sameTimeForQCPass);
                        }
                    }
                }

                foreach (var item in sameTimesForLotIn)
                {
                    queryForLotIn.RemoveAll(s => s.Hour == item.Hour);
                }

                foreach (var item in queryForLotIn)
                {
                    BarChatTimeWiseData barChatTimeWiseData = new BarChatTimeWiseData()
                    {
                        Hour = item.Hour,
                        //QCPassCount = item2.Count,
                        LotInCount = item.Count
                    };
                    barChatTimeWiseDatas.Add(barChatTimeWiseData);
                    //listBar.Add(barChatTimeWiseData);
                }

                foreach (var item in sameTimesForQCPass)
                {
                    queryForQCPass.RemoveAll(s => s.Hour == item.Hour);
                }

                foreach (var item in queryForQCPass)
                {
                    BarChatTimeWiseData barChatTimeWiseData = new BarChatTimeWiseData()
                    {
                        Hour = item.Hour,
                        QCPassCount = item.Count,
                        //LotInCount = item.Count
                    };
                    barChatTimeWiseDatas.Add(barChatTimeWiseData);
                    //listBar.Add(barChatTimeWiseData);
                }
            }
            else
            {
                foreach (var item in queryForLotIn)
                {
                    BarChatTimeWiseData barChatTimeWiseData = new BarChatTimeWiseData()
                    {
                        Hour = item.Hour,
                        //QCPassCount = 0,
                        LotInCount = item.Count
                    };
                    barChatTimeWiseDatas.Add(barChatTimeWiseData);
                    //listBar.Add(barChatTimeWiseData);
                }
            }

            var list = barChatTimeWiseDatas.ToList().OrderBy(s => s.Hour);
            foreach (var item in list)
            {
                hour.Add(Convert.ToDateTime(item.Hour).ToString("hh:00"));
                qcPassCount.Add(item.QCPassCount);
                lotInCount.Add(item.LotInCount);
            }
            ViewBag.Hour = hour.ToList();
            ViewBag.QCPassCount = qcPassCount.ToList();
            ViewBag.LotInCount = lotInCount.ToList();

            return Json(new { Hour = ViewBag.Hour, QCPassCount = ViewBag.QCPassCount, LotInCount = ViewBag.LotInCount });
        }

        public ActionResult GetREPAIRIN_OUTBarchart(long assemblyId)
        {
            var repairIn = _repairInBusiness.GetAllRepairInDataByAssemblyIdWithTimeWise(assemblyId, DateTime.Today, User.OrgId);
            var queryForRepairIn = repairIn.AsEnumerable()
                .GroupBy(row => row.EntryDate.Value.ToString("HH:00"))
                .Select(grp => new
                {
                    Hour = grp.Key,
                    Count = grp.Count()
                }).ToList().OrderBy(s => s.Hour).ToList();

            var repairOut = _repairOutBusiness.GetAllRepairOutDataByAssemblyIdWithTimeWise(assemblyId, DateTime.Today, User.OrgId);
            var queryForRepairOut = repairOut.AsEnumerable()
                .GroupBy(row => row.EntryDate.Value.ToString("HH:00"))
                .Select(grp => new
                {
                    Hour = grp.Key,
                    Count = grp.Count()
                }).ToList().OrderBy(s => s.Hour).ToList();

            List<BarChatTimeWiseData> barChatTimeWiseDatas = new List<BarChatTimeWiseData>(); List<BarChatTimeWiseData> listBar = new List<BarChatTimeWiseData>();
            List<string> hour = new List<string>();
            List<int> repairInCount = new List<int>();
            List<int> repairOutCount = new List<int>();
            List<RepairInAndRepairOutHourSameData> sameTimesForRepairIn = new List<RepairInAndRepairOutHourSameData>();
            List<RepairInAndRepairOutHourSameData> sameTimesForRepairOut = new List<RepairInAndRepairOutHourSameData>();

            if (queryForRepairIn.Count() > 0)
            {
                foreach (var item in queryForRepairIn)
                {
                    foreach (var item2 in queryForRepairOut)
                    {
                        if (item.Hour == item2.Hour)
                        {
                            BarChatTimeWiseData barChatTimeWiseData = new BarChatTimeWiseData()
                            {
                                Hour = item.Hour,
                                RepairInCount = item.Count,
                                RepairOutCount = item2.Count,
                            };
                            barChatTimeWiseDatas.Add(barChatTimeWiseData);
                            //queryForLotIn.RemoveAll(s => s.Hour == item2.Hour);
                            RepairInAndRepairOutHourSameData sameTimeForRepairOut = new RepairInAndRepairOutHourSameData()
                            {
                                Hour = item2.Hour,
                                Count = item2.Count,
                            };
                            sameTimesForRepairOut.Add(sameTimeForRepairOut);
                            RepairInAndRepairOutHourSameData sameTimeForRepairIn = new RepairInAndRepairOutHourSameData()
                            {
                                Hour = item.Hour,
                                Count = item.Count,
                            };
                            sameTimesForRepairIn.Add(sameTimeForRepairIn);
                        }
                    }
                }

                foreach (var item in sameTimesForRepairOut)
                {
                    queryForRepairOut.RemoveAll(s => s.Hour == item.Hour);
                }

                foreach (var item in queryForRepairOut)
                {
                    BarChatTimeWiseData barChatTimeWiseData = new BarChatTimeWiseData()
                    {
                        Hour = item.Hour,
                        //QCPassCount = item2.Count,
                        RepairOutCount = item.Count
                    };
                    barChatTimeWiseDatas.Add(barChatTimeWiseData);
                    //listBar.Add(barChatTimeWiseData);
                }

                foreach (var item in sameTimesForRepairIn)
                {
                    queryForRepairIn.RemoveAll(s => s.Hour == item.Hour);
                }

                foreach (var item in queryForRepairIn)
                {
                    BarChatTimeWiseData barChatTimeWiseData = new BarChatTimeWiseData()
                    {
                        Hour = item.Hour,
                        RepairInCount = item.Count,
                        //LotInCount = item.Count
                    };
                    barChatTimeWiseDatas.Add(barChatTimeWiseData);
                    //listBar.Add(barChatTimeWiseData);
                }
            }
            else
            {
                foreach (var item in queryForRepairOut)
                {
                    BarChatTimeWiseData barChatTimeWiseData = new BarChatTimeWiseData()
                    {
                        Hour = item.Hour,
                        //QCPassCount = item2.Count,
                        RepairOutCount = item.Count,
                        //RepairInCount = 0,
                    };
                    barChatTimeWiseDatas.Add(barChatTimeWiseData);
                    //listBar.Add(barChatTimeWiseData);
                }
            }

            var list = barChatTimeWiseDatas.ToList().OrderBy(s => s.Hour);
            foreach (var item in list)
            {
                hour.Add(Convert.ToDateTime(item.Hour).ToString("hh:00"));
                repairInCount.Add(item.RepairInCount);
                repairOutCount.Add(item.RepairOutCount);
            }
            ViewBag.Hour = hour.ToList();
            ViewBag.RepairInCount = repairInCount.ToList();
            ViewBag.RepairOutCount = repairOutCount.ToList();

            return Json(new { Hour = ViewBag.Hour, RepairInCount = ViewBag.RepairInCount, RepairOutCount = ViewBag.RepairOutCount });
        }
        public ActionResult QC_1_Problem_HorizontalBarChart(long assemblyId)
        {
            var qrCodeProblem = _qC1DetailBusiness.GetAllQC1ProblemByAssemblyId(assemblyId, DateTime.Today, User.OrgId);
            List<string> repetedProblemName = new List<string>();
            List<int> repetedProblem = new List<int>();
            List<QRCodeProblemCountWithProblem> countWithProblem = new List<QRCodeProblemCountWithProblem>();
            var problems = qrCodeProblem.Select(s => s.ProblemName).Distinct();
            foreach (var item in problems)
            {
                QRCodeProblemCountWithProblem pro = new QRCodeProblemCountWithProblem()
                {
                    Value = qrCodeProblem.Count(s => s.ProblemName == item),
                    Text = qrCodeProblem.Where(s => s.ProblemName == item).FirstOrDefault().ProblemName,
                };
                countWithProblem.Add(pro);
            }
            countWithProblem = countWithProblem.OrderByDescending(x => x.Value).ThenBy(s => s.Text).ToList();
            var takeTopTen = countWithProblem.ToList().Take(10);
            foreach (var item in takeTopTen)
            {
                repetedProblemName.Add(item.Text);
                repetedProblem.Add(item.Value);
            }
            ViewBag.ProblemNames = repetedProblemName.ToList();
            ViewBag.RepetedProblems = repetedProblem.ToList();
            return Json(new { ProblemNames = ViewBag.ProblemNames, RepetedProblems = ViewBag.RepetedProblems });
        }
        public ActionResult QC_2_Problem_HorizontalBarChart(long assemblyId)
        {
            var qrCodeProblem = _qC2DetailBusiness.GetAllQC2ProblemByAssemblyId(assemblyId, DateTime.Today, User.OrgId);
            List<string> repetedProblemName = new List<string>();
            List<int> repetedProblem = new List<int>();
            List<QRCodeProblemCountWithProblem> countWithProblem = new List<QRCodeProblemCountWithProblem>();
            var problems = qrCodeProblem.Select(s => s.ProblemName).Distinct();
            foreach (var item in problems)
            {
                QRCodeProblemCountWithProblem pro = new QRCodeProblemCountWithProblem()
                {
                    Value = qrCodeProblem.Count(s => s.ProblemName == item),
                    Text = qrCodeProblem.Where(s => s.ProblemName == item).FirstOrDefault().ProblemName,
                };
                countWithProblem.Add(pro);
            }
            countWithProblem = countWithProblem.OrderByDescending(x => x.Value).ThenBy(s => s.Text).ToList();
            var takeTopTen = countWithProblem.ToList().Take(10);
            foreach (var item in takeTopTen)
            {
                repetedProblemName.Add(item.Text);
                repetedProblem.Add(item.Value);
            }
            ViewBag.ProblemNames = repetedProblemName.ToList();
            ViewBag.RepetedProblems = repetedProblem.ToList();
            return Json(new { ProblemNames = ViewBag.ProblemNames, RepetedProblems = ViewBag.RepetedProblems });
        }
        public ActionResult QC_3_Problem_HorizontalBarChart(long assemblyId)
        {
            var qrCodeProblem = _qC3DetailBusiness.GetAllQC3ProblemByAssemblyId(assemblyId, DateTime.Today, User.OrgId);
            List<string> repetedProblemName = new List<string>();
            List<int> repetedProblem = new List<int>();
            List<QRCodeProblemCountWithProblem> countWithProblem = new List<QRCodeProblemCountWithProblem>();
            var problems = qrCodeProblem.Select(s => s.ProblemName).Distinct();
            foreach (var item in problems)
            {
                QRCodeProblemCountWithProblem pro = new QRCodeProblemCountWithProblem()
                {
                    Value = qrCodeProblem.Count(s => s.ProblemName == item),
                    Text = qrCodeProblem.Where(s => s.ProblemName == item).FirstOrDefault().ProblemName,
                };
                countWithProblem.Add(pro);
            }
            countWithProblem = countWithProblem.OrderByDescending(x => x.Value).ThenBy(s => s.Text).ToList();
            var takeTopTen = countWithProblem.ToList().Take(10);
            foreach (var item in takeTopTen)
            {
                repetedProblemName.Add(item.Text);
                repetedProblem.Add(item.Value);
            }
            ViewBag.ProblemNames = repetedProblemName.ToList();
            ViewBag.RepetedProblems = repetedProblem.ToList();
            return Json(new { ProblemNames = ViewBag.ProblemNames, RepetedProblems = ViewBag.RepetedProblems });
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult GetChartData()
        {
            var data =_finishGoodsStockDetailBusiness.GetDayAndLineProductionChartsData(User.OrgId);
            var lineNumber = (from ln in data
                         select ln.LineNumber).Distinct().ToArray();

            var months = (from ln in data
                          select ln.Date).Distinct().ToArray();
            List<List<int>> charts = new List<List<int>>();

            foreach (var item in lineNumber)
            {
                var qtys = data.Where(c => c.LineNumber == item).Select(c => c.Quantity).ToList();
                charts.Add(qtys);
            }

            // Chart-2 
            var data2 = _finishGoodsStockDetailBusiness.GetDayAndModelWiseProductionChart(User.OrgId);
            var modelNames = (from ln in data2
                              select ln.ModelName).Distinct().ToArray();

            var months2 = (from ln in data
                          select ln.Date).Distinct().ToArray();
            List<List<int>> charts2 = new List<List<int>>();

            foreach (var item in modelNames)
            {
                var qtys = data2.Where(c => c.ModelName == item).Select(c => c.Quantity).ToList();
                charts2.Add(qtys);
            }
            
            return Json(new {lines= lineNumber, months= months, charts= charts, models= modelNames, charts2=charts2,months2= months2 });

            
        }
       
        public ActionResult ReturnStockDetails(long id)
        {
            var info = _tsStockReturnInfoBusiness.GetAllReturnId(id, User.OrgId,User.BranchId);
            var jobOrderInfo = _jobOrderBusiness.GetJobOrderById(info.JobOrderId, User.OrgId);
            var jobOrder = new RequsitionInfoForJobOrderViewModel();
            if(jobOrderInfo != null)
            {
                jobOrder = new RequsitionInfoForJobOrderViewModel
                {
                    RequsitionCode = info.RequsitionCode,
                    JobOrderCode = (jobOrderInfo.JobOrderCode),
                    Type = jobOrderInfo.Type,
                    ModelName = (_modelSSBusiness.GetModelById(jobOrderInfo.DescriptionId, User.OrgId).ModelName),
                    ModelColor= jobOrderInfo.ModelColor,
                    Requestby = UserForEachRecord(info.EUserId.Value).UserName,
                    EntryDate = info.EntryDate,
                };
            }
            ViewBag.ReqInfoJobOrder = jobOrder;

            IEnumerable<TsStockReturnDetailDTO> tsStock = _tsStockReturnDetailsBusiness.GetAllTsStockReturn(User.OrgId, User.BranchId).Where(s=> s.ReturnInfoId == id).Select(detail => new TsStockReturnDetailDTO
            {
                ReqInfoId = detail.ReturnInfoId,
                ReturnDetailId = detail.ReturnDetailId,
                PartsId = detail.PartsId,
                PartsName = (_mobilePartBusiness.GetMobilePartOneByOrgId(detail.PartsId, User.OrgId).MobilePartName),
                PartsCode = (_mobilePartBusiness.GetMobilePartOneByOrgId(detail.PartsId, User.OrgId).MobilePartCode),
                Quantity = detail.Quantity,
                ModelId=detail.ModelId.Value,
                
            }).ToList();
            List<TsStockReturnDetailViewModel> detailViewModels = new List<TsStockReturnDetailViewModel>();
            AutoMapper.Mapper.Map(tsStock, detailViewModels);
            return PartialView("ReturnStockDetails", detailViewModels);
        }
        public ActionResult GetDailySellsChart()
        {
            var data = _jobOrderBusiness.DailySellsChart(string.Empty, string.Empty, User.OrgId, User.BranchId);


            var days = data.Select(s => s.EntryDate).ToArray();
            var charts = data.Select(s => s.Sells).ToArray();
            return Json(new { days = days, charts = charts });

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}