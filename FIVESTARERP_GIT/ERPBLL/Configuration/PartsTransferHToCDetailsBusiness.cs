using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class PartsTransferHToCDetailsBusiness: IPartsTransferHToCDetailsBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly PartsTransferHToCDetailsRepository partsTransferHToCDetailsRepository; // repo
        public PartsTransferHToCDetailsBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            partsTransferHToCDetailsRepository = new PartsTransferHToCDetailsRepository(this._configurationDb);
        }

        public IEnumerable<PartsTransferHToCDetails> GetAllDetailsByInfoId(long infoId, long orgId)
        {
            return this.partsTransferHToCDetailsRepository.GetAll(i => i.InfoId == infoId && i.OrganizationId == orgId).ToList();
        }
    }
}
