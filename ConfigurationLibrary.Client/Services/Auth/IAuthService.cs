using ConfigurationLibrary.UI.Models.User;

namespace ConfigurationLibrary.UI.Services.Auth
{
    public interface IAuthService
    {
        Task<ServiceResponse> SignInPortal(UserSignInModel model);

        Task<ServiceResponse> SignOutPortal();
    }
}
