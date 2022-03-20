using ERPBLL.Configuration.Interface;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class TransferFaultyToDustDetailsBusiness: ITransferFaultyToDustDetailsBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly TransferFaultyToDustDetailsRepository transferFaultyToDustDetailsRepository; // repo
        public TransferFaultyToDustDetailsBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            transferFaultyToDustDetailsRepository = new TransferFaultyToDustDetailsRepository(this._configurationDb);
        }
    }
}
