using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Report.Interface;
using ERPBO.Production.ReportModels;
using ERPWeb.Filters;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPWeb.Infrastructure;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Imaging;
using ERPBLL.Production.Interface;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class ReportController : BaseController
    {
        private readonly IProductionReportBusiness _productionReportBusiness; // Production
        private readonly IWarehouseStockDetailBusiness _warehouseStockDetailBusiness;
        private readonly IInventoryReportBusiness _inventoryReportBusiness;
        private readonly IDescriptionBusiness _descriptionBusiness;
        private readonly IWarehouseBusiness _warehouseBusiness;
        private readonly ISupplierBusiness _supplierBusiness;
        private readonly IQRCodeProblemBusiness _qRCodeProblemBusiness;
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness;
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;

        public ReportController(IProductionReportBusiness productionReportBusiness, IWarehouseStockDetailBusiness warehouseStockDetailBusiness, IInventoryReportBusiness inventoryReportBusiness, IDescriptionBusiness descriptionBusiness, IWarehouseBusiness warehouseBusiness, ISupplierBusiness supplierBusiness, IQRCodeProblemBusiness qRCodeProblemBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IRequsitionInfoBusiness requsitionInfoBusiness)
        {
            this._productionReportBusiness = productionReportBusiness;
            this._qRCodeProblemBusiness = qRCodeProblemBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._requsitionInfoBusiness = requsitionInfoBusiness;


            #region Inventory
            this._warehouseStockDetailBusiness = warehouseStockDetailBusiness;
            this._inventoryReportBusiness = inventoryReportBusiness;
            this._descriptionBusiness = descriptionBusiness;
            this._warehouseBusiness = warehouseBusiness;
            this._supplierBusiness = supplierBusiness;
            #endregion

        }

        #region Generate 128
        public ActionResult GenerateCode128(string flag, string barcode)
        {
            if (string.IsNullOrEmpty(flag) && string.IsNullOrEmpty(barcode))
            {
                return View();
            }
            else
            {
                var byt = GenerateCode128(barcode);
                ViewBag.BarcodeImage = string.Format(@"data:image/png;base64,{0}", Convert.ToBase64String(byt));
                return View();
            }
        }
        [HttpPost]
        private byte[] GenerateCode128(string barcode)
        {
            Zen.Barcode.BarcodeDraw barcode1 = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            var ms = new MemoryStream();
            byte[] imei;
            Image image = barcode1.Draw(barcode, 12);
            image.Save(ms, ImageFormat.Png);
            imei = ms.ToArray();
            return imei;
        }
        #endregion

        #region Production Report
        //[HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult GetProductionRequisitionReport(long reqInfoId)
        {
            bool IsSuccess = false;
            //string 
            IEnumerable<ProductionRequisitionReport> reportData = _productionReportBusiness.GetProductionRequisitionReport(reqInfoId);
            reportData.FirstOrDefault().OrganizationName = User.OrgName;
            reportData.FirstOrDefault().ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ERPRpt/Production/rptProductionRequisition.rdlc");
            string id = string.Empty;
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource = new ReportDataSource("ProductionRequisition", reportData);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource);
                localReport.Refresh();
                localReport.DisplayName = reportData.FirstOrDefault().ReqInfoCode;

                //string deviceInfo =
                //"<DeviceInfo>" +
                //"<OutputFormat>Excel</OutputFormat>" +
                //"</DeviceInfo>";
                string mimeType;
                string encoding;
                string fileNameExtension = ".pdf";
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    "Pdf",
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                //MemoryStream stream = new MemoryStream(renderedBytes);
                //id= Guid.NewGuid().ToString();
                //TempData[id] = stream.ToArray();

                var base64 = Convert.ToBase64String(renderedBytes);
                var fs = String.Format("data:application/pdf;base64,{0}", base64);
                IsSuccess = true;
                return Json(new { IsSuccess = IsSuccess, File = fs, FileName = "ProductionRequisition_" + localReport.DisplayName });

                //System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ProductionRequisition.xlsx");
                //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //return File(renderedBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductionRequisition.xlsx");
            }
            return Json(new { IsSuccess = IsSuccess, Id = id });
        }

        public ActionResult GetQrCodeFileByItemColorAndRefNum(long? itemId, long referenceId, string rptType)
        {
            IEnumerable<QRCodesByRef> reportData = _productionReportBusiness.GetQRCodesByRefId(itemId, referenceId, User.OrgId);
            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ERPRpt/Production/rptQRCodesByRef.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
            }
            ReportDataSource dataSource1 = new ReportDataSource("QRCodesByRef", reportData);
            localReport.DataSources.Add(dataSource1);
            string reportType = rptType;
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;

            var renderedBytes = localReport.Render(
                reportType,
                "",
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
                );
            return File(renderedBytes, mimeType);
        }

        public ActionResult GetYourReport(string id, string type = "", string reportName = "")
        {
            if (TempData[id] != null)
            {
                //MemoryStream stream = (MemoryStream)TempData[id];
                byte[] byteInfo = TempData[id] as byte[];
                //stream.Write(byteInfo, 0, byteInfo.Length);
                //stream.Position = 0;
                //Response.AddHeader("content-disposition", "attachment; filename=ProductionRequisition.xlsx");
                //return File(byteInfo, "ProductionRequisition.xlsx");

                //return new FileStreamResult()
            }
            return Content("");
        }

        public ActionResult GetLotInBarcodePDFFileByItemColorAndRefNum(long? itemId, long referenceId, string rptType)
        {
            IEnumerable<QRCodesByRef> reportData = _productionReportBusiness.GetQRCodesByRefId(itemId, referenceId, User.OrgId);
            List<QRCodesByRef> LotInBarcodeList = new List<QRCodesByRef>();
            foreach (var item in reportData)
            {
                QRCodesByRef qR = new QRCodesByRef();
                qR.ItemName = item.ItemName;
                qR.CodeNo = item.CodeNo;
                qR.BarcodeLotInNumber = GenerateCode128(item.CodeNo);
                LotInBarcodeList.Add(qR);
            }
            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ERPRpt/Production/rptLotInBarcodeSticker.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
            }
            ReportDataSource dataSource1 = new ReportDataSource("dsLotInBarcode", LotInBarcodeList);
            localReport.DataSources.Add(dataSource1);
            string reportType = rptType;
            string mimeType;
            string encoding;
            string fileNameExtension = ".pdf";
            Warning[] warnings;
            string[] streams;
            string deviceInfo =
                    "<DeviceInfo>" +
                    "<OutputFormat>PDF</OutputFormat>" +
                    "<PageWidth>1.18in</PageWidth>" +
                    "<PageHeight>0.5in</PageHeight>" +
                    "<MarginTop>0in</MarginTop>" +
                    "<MarginLeft>0.03in</MarginLeft>" +
                    "<MarginRight>0in</MarginRight>" +
                    "<MarginBottom>0in</MarginBottom>" +
                    "</DeviceInfo>";
            var renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
                );
            //var base64 = Convert.ToBase64String(renderedBytes);
            //var file = String.Format("{0}", base64);
            //return Json(new { File = file });
            return File(renderedBytes, mimeType);
        }

        public ActionResult GetRequsitionDetailsReport(long infoId)
        {
            var detailsdata = _requsitionDetailBusiness.GetRequsitionDetailsForReport(infoId, User.OrgId);

            // var detailsdata = _invoiceDetailBusiness.InvoiceDetailsReport(infoId,User.OrgId, User.BranchId);
            var infodata = _requsitionInfoBusiness.GetReqInfoDataForReport(infoId, User.OrgId);

            //ServicesReportHead reportHead = _jobOrderReportBusiness.GetBranchInformation(User.OrgId, User.BranchId);
            //reportHead.ReportImage = Utility.GetImageBytes(User.LogoPaths[0]);
            //List<ServicesReportHead> servicesReportHeads = new List<ServicesReportHead>();
            //servicesReportHeads.Add(reportHead);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ERPRpt/Production/rptRequsitionDetails.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
            }
            ReportDataSource dataSource1 = new ReportDataSource("ReqDetails", detailsdata);
            ReportDataSource dataSource2 = new ReportDataSource("ReqInfo", infodata);
            //ReportDataSource dataSource3 = new ReportDataSource("ReportHead", servicesReportHeads);
            localReport.DataSources.Add(dataSource1);
            localReport.DataSources.Add(dataSource2);
            //localReport.DataSources.Add(dataSource3);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;

            var renderedBytes = localReport.Render(
                reportType,
                "",
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
                );
            return File(renderedBytes, mimeType);
        }
        #endregion

        #region Inventory Reports
        public ActionResult GetWarehouseStockDetailsReport(string refNum, long? ddlWarehouse, long? ddlModelName, long? ddlItemType, long? ddlItem, string ddlStockStatus, long? ddlSupplier, string fromDate, string toDate, string rptType)
        {
            var reportData = _warehouseStockDetailBusiness.GetWarehouseStockDetailInfoLists(ddlWarehouse, ddlModelName, ddlItemType, ddlItem, ddlStockStatus, fromDate, toDate, refNum, ddlSupplier, User.OrgId);

            var reportHead = _productionReportBusiness.GetReportHead(User.BranchId, User.OrgId);

            reportHead.FirstOrDefault().OrgLogo = Utility.GetImageBytes(User.LogoPaths[0]);
            reportHead.FirstOrDefault().ReportLogo = Utility.GetImageBytes(User.LogoPaths[0]);
            rptType = "PDF";

            byte[] fileBytes = null;
            string fileMimeType = null;
            string path = string.Format(@"~/Reports/ERPRpt/Inventory/rptWarehouseStockDetail.rdlc");

            // Report Generator //
            GetReportFileByTwoDataSource(path, reportData, "WarehouseStockDetail", reportHead, "ReportHead", rptType, out fileBytes, out fileMimeType);

            return File(fileBytes, fileMimeType);
        }
        #region InventoryReportPanel

        public ActionResult InventoryReportPanel()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem
            {
                Text = s.text,
                Value = s.value
            }).ToList();
            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();
            ViewBag.ddlSupplier = _supplierBusiness.GetSuppliers(User.OrgId).Select(sup => new SelectListItem { Text = sup.SupplierName, Value = sup.SupplierId.ToString() }).ToList();
            return View();
        }

        public ActionResult GetWarehouseStockShortageOrExcess(string fromDate, string toDate, long model, string format)
        {
            var data = _warehouseStockDetailBusiness.StockShortageOrExcessQty(User.OrgId, fromDate, toDate, model);
            string path = string.Format(@"~/Reports/ERPRpt/Inventory/rptStockShortageOrExcess.rdlc");
            byte[] fileBytes = null;
            string fileMimeType = null;
            string rptType = format;
            GetReportFileByOneDataSource(path, data, "ShortageOrExcess", rptType, out fileBytes, out fileMimeType);

            var fileName = "StockShortageOrExcessReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + (format == "PDF" ? ".pdf" : (format == "EXCEL" ? ".xls" : ".doc"));
            if (format == "PDF")
            {
                return File(fileBytes, fileMimeType);
            }
            else
            {
                return File(fileBytes, fileMimeType, fileName);
            }
            //LocalReport report = new LocalReport();
            //report.ReportPath = Server.MapPath("~/Reports/ERPRpt/Inventory/rptStockShortageOrExcess.rdlc");
            //ReportDataSource dataSource1 = new ReportDataSource("ShortageOrExcess", data);
            //report.DataSources.Add(dataSource1);
            //report.PrintToPrinter();
            //return Content("Printed");

            //PrinterSettings settings = new PrinterSettings();
            //return Content(settings.PrinterName);

        } 

        public ActionResult GetWarehouseModelWiseTodayStockReport(long model, string format)
        {
            var data = _inventoryReportBusiness.GetModelWiseDailyItemStocks(User.OrgId, model);
            string path = string.Format(@"~/Reports/ERPRpt/Inventory/rptModelWiseDailyItemStock.rdlc");
            byte[] fileBytes = null;
            string fileMimeType = null;
            string rptType = format;
            GetReportFileByOneDataSource(path, data, "ModelWiseDailyItemStock", rptType, out fileBytes, out fileMimeType);

            var fileName = "ModelWiseDailyItemStockReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + (format == "PDF" ? ".pdf" : (format == "EXCEL" ? ".xls" : ".doc"));
            if (format == "PDF")
            {
                return File(fileBytes, fileMimeType);
            }
            else
            {
                return File(fileBytes, fileMimeType, fileName);
            }
        }
        #endregion

        #endregion

        #region QRCode Problem List
        public ActionResult GetQRCodeProblemList(long? ddlQCLineNo,long? ddlQCName,long? ddlModelName, string qrCode,string prbName,string rptType)
        {
            bool IsSuccess = false;

            var data = _qRCodeProblemBusiness.GetQRCodeProblemList(ddlQCLineNo, qrCode, ddlModelName, prbName, ddlQCName, User.OrgId);

            List<ReportHead> reportHead = _productionReportBusiness.GetReportHead(User.BranchId, User.OrgId).ToList();

            reportHead.FirstOrDefault().OrgLogo = Utility.GetImageBytes(User.LogoPaths[0]);
            reportHead.FirstOrDefault().ReportLogo = Utility.GetImageBytes(User.LogoPaths[0]);

            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath("~/Reports/ERPRpt/Production/rptQRCodeProblemList.rdlc");
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource("QRCodeProblem", data);
                ReportDataSource dataSource2 = new ReportDataSource("ReportHead", reportHead);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(dataSource1);
                localReport.DataSources.Add(dataSource2);
                localReport.Refresh();
                localReport.DisplayName = "QRCode";

                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = localReport.Render(
                    rptType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
            }
            return new EmptyResult();
        }
        #endregion

        #region otherFile
        private void GetReportFileByOneDataSource(string path, object reportData, string dataSourceName, string rptType, out byte[] fileBytes, out string fileMimeType)
        {
            fileBytes = null;
            fileMimeType = null;
            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath(path);
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource(dataSourceName, reportData);
                localReport.DataSources.Add(dataSource1);
                string reportType = rptType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;

                var renderedBytes = localReport.Render(
                    reportType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                fileBytes = renderedBytes;
                fileMimeType = mimeType;
            }
        }

        private void GetReportFileByTwoDataSource(string path, object reportData1, string dataSourceName1, object reportData2, string dataSourceName2, string rptType, out byte[] fileBytes, out string fileMimeType)
        {
            fileBytes = null;
            fileMimeType = null;
            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath(path);
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;

                ReportDataSource dataSource1 = new ReportDataSource(dataSourceName1, reportData1);
                localReport.DataSources.Add(dataSource1);

                ReportDataSource dataSource2 = new ReportDataSource(dataSourceName2, reportData2);
                localReport.DataSources.Add(dataSource2);

                string reportType = rptType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;

                var renderedBytes = localReport.Render(
                    reportType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                fileBytes = renderedBytes;
                fileMimeType = mimeType;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion
    }
}