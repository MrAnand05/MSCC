namespace ViewSwitchingNavigation.Infrastructure
{

    public interface IAuthenticationService {
        AuthenticationContext Authenticate(string userName, string password,int financialYear);
    }

}
