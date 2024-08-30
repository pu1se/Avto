using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestic.RethinkUI._Core
{
    public class Settings
    {
        public string BaseUrl
        {
            get
            {
#if DEBUG
                return "https://albion-test7.rethinkportal.com";
#endif
#pragma warning disable CS0162
                return "https://albion-test7.rethinkportal.com";
#pragma warning restore CS0162
            }
        }

        public string Auth0Url => "https://rethink-dev.eu.auth0.com";
        public string Auth0ClientId => "k60FvtApWNrCszAWPG0uu2mc0eeCUaVA";
        public string Auth0ClientSecret => "_-aikSxWiUuIfys1I169gvlgesd_gKaB9YJ8ytUlWFwqLahhH7SCGDzJUTmpBoeP";
        public string Auth0Db => "RethinkUserDb";
        public string Auth0Audience => "https://p3-test-01-Rethink";

        public string SellerAdminAuthLogin => "grin2@qa.qa";
        public string SellerAdminAuthPassword => "Grin2@qa.qa";

        public string CustomerAdminAuthLogin => "customer.admin@auto.testic.com";
        public string CustomerAdminAuthPassword => "!123Qqwe";

        public string SuperAdminAuthLogin => "pasha.parrrker@gmail.com";
        public string SuperAdminAuthPassword => "!123Qqwe";

        public string RethinkBackApiUrl = "https://appxite-rethink-back-api-p3-test-01.azurewebsites.net/api/";
        public int RethinkBackApiRequestTimeoutInSec = 60;

        public Guid CustomerOrganizationId => new Guid("B950BB7C-0A8F-46E4-8B2E-FEDF7B74E503");
        public string SellerPlatformUrl => "https://albion-test7.rethinkportal.com";
    }
}
