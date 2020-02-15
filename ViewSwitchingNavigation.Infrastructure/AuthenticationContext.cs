using System.Collections.Generic;

namespace ViewSwitchingNavigation.Infrastructure
{

    public class AuthenticationContext {

        public bool IsAuthorized { get; set; }

        public static int GlobalFinancialYear;
        public Enumerations.AuthorizationLevel AuthLevel { get; set; }

        public IList<string> AllowedModules { get; set; }
        public int FinancialYear { get; set; }

    }

}