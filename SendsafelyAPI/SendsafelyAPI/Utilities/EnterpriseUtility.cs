using System;
using System.Collections.Generic;
using System.Text;
using SendSafely.Objects;

namespace SendSafely.Utilities
{
    class EnterpriseUtility
    {
        private Connection connection;

        #region Constructors

        public EnterpriseUtility(Connection connection)
        {
            this.connection = connection;
        }
        
        #endregion

        #region Public Functions

        public EnterpriseInformation GetInformation()
        {
            Endpoint p = ConnectionStrings.Endpoints["enterpriseInfo"].Clone();

            EnterpriseInformationResponse response = connection.Send<EnterpriseInformationResponse>(p);

            EnterpriseInformation info = new EnterpriseInformation();
            info.Host = response.Host;
            info.SystemName = response.SystemName;

            return info;
        }

        #endregion
    }
}
