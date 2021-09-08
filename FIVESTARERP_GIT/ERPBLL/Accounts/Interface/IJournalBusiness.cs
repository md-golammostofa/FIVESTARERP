﻿using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPBO.Common;
using ERPBO.FrontDesk.ReportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBO.Accounts.DTOModels;
using ERPBO.Accounts.DomainModels;

namespace ERPBLL.Accounts.Interface
{
   public interface IJournalBusiness
    {
        bool SaveDebitVouchar(List<JournalDTO> journalDTO, long userId, long orgId);
        bool SaveCreditVouchar(List<JournalDTO> journalDTO, long userId, long orgId);
        bool SaveJournalVouchar(List<JournalDTO> journalDTO, long userId, long orgId);
        IEnumerable<JournalDTO> GetJournalList(string voucherNo, long orgId, string fromDate, string toDate);
        IEnumerable<JournalDTO> CashVoucherList(long orgId, string fromDate, string toDate);
        IEnumerable<JournalDTO> LedgerList(long? accountId,long orgId, string fromDate, string toDate);
        ExecutionStateWithText SaveDebitVoucharAndPrint(List<JournalDTO> journalDTO, long userId, long orgId);
        IEnumerable<JournalDTO> GetDebitVoucherReport(string voucherNo, long orgId);
        ServicesReportHead GetBranchInformation(long orgId, long branchId);
        IEnumerable<JournalDTO> GetCreditVoucherReport(string voucherNo, long orgId);
        ExecutionStateWithText SaveCreditVoucharAndPrint(List<JournalDTO> journalDTO, long userId, long orgId);
        ExecutionStateWithText SaveJournalVoucharAndPrint(List<JournalDTO> journalDTO, long userId, long orgId);
        IEnumerable<JournalDTO> GetJournalVoucherReport(string voucherNo, long orgId);
        bool DeleteJournalVoucher(string voucherNo, long orgId);
        IEnumerable<Journal> GetAllLegder(long accountId,long orgId);
        IEnumerable<Journal> GetDebitDueAmount(long accountId, long orgId);
        IEnumerable<JournalDTO> DebitVoucherList(string voucherNo, long orgId, string fromDate, string toDate);
        IEnumerable<JournalDTO> CreditVoucherList(string voucherNo, long orgId, string fromDate, string toDate);
    }
}
