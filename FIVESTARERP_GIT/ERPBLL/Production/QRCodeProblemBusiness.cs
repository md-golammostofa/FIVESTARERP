using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class QRCodeProblemBusiness : IQRCodeProblemBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QRCodeProblemRepository _qRCodeProblemRepository;
        public QRCodeProblemBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._qRCodeProblemRepository = new QRCodeProblemRepository(this._productionDb);
        }
        public IEnumerable<QRCodeProblemDTO> GetQRCodeProblemDTOByQuery(long transferId, string qrCode, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<QRCodeProblemDTO>(QRCodeProblemDTOByQuery(transferId, qrCode, orgId)).ToList();
        }

        public IEnumerable<QRCodeProblemDTO> GetQRCodeProblemList(long? qcLine, string qrCode, long? modelId, string prbName, long? qcId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<QRCodeProblemDTO>(QueryForQRCodeProblemList(qcLine, qrCode, modelId,prbName,qcId,orgId)).ToList();
        }

        private string QRCodeProblemDTOByQuery(long transferId, string qrCode, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" and qp.OrganizationId={0}",orgId);
            if(transferId > 0)
            {
                param += string.Format(@" and qp.TransferId={0}", transferId);
            }
            if(!string.IsNullOrEmpty(qrCode) && qrCode.Trim() != "")
            {
                param += string.Format(@" and qp.QRCode='{0}'", qrCode.Trim());
            }
            query = string.Format(@"Select * From [Production].dbo.tblQRCodeProblem qp
Where 1=1 {0} order by QRProbId desc", param);
            return query;
        }
        private string QueryForQRCodeProblemList(long? qcLine, string qrCode, long? modelId, string prbName, long? qcId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" and prb.OrganizationId={0}", orgId);
            if (qcLine !=null && qcLine > 0)
            {
                param += string.Format(@" and prb.QCLineId={0}", qcLine);
            }
            if (modelId !=null && modelId > 0)
            {
                param += string.Format(@" and prb.DescriptionId={0}", modelId);
            }
            if (qcId !=null && qcId > 0)
            {
                param += string.Format(@" and prb.SubQCId={0}", qcId);
            }
            if (!string.IsNullOrEmpty(qrCode))
            {
                param += string.Format(@"and prb.QRCode Like '%{0}%'", qrCode);
            }
            if (!string.IsNullOrEmpty(prbName))
            {
                param += string.Format(@"and prb.ProblemName Like '%{0}%'", prbName);
            }
            query = string.Format(@"Select prb.QRProbId,prb.QCLineId,ql.QCName'QCLineNo',prb.QRCode,prb.DescriptionId,model.DescriptionName'ModelName'
,prb.ProblemId,prb.ProblemName,prb.SubQCId,subqc.SubQCName,prb.EntryDate From [Production].dbo.tblQRCodeProblem prb
Left Join [Production].dbo.tblQualityControl ql on prb.QCLineId=ql.QCId
Left Join [Inventory].dbo.tblDescriptions model on prb.DescriptionId=model.DescriptionId
Left Join [Production].dbo.tblSubQC subqc on prb.SubQCId=subqc.SubQCId
Where 1=1{0} order By prb.EntryDate desc", param);
            return query;
        }
    }
}
