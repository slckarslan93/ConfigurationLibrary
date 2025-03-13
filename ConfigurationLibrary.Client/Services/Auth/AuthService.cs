using ConfigurationLibrary.UI.Entities.Identity;
using ConfigurationLibrary.UI.Models.User;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ConfigurationLibrary.UI.Services.Auth
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<ServiceResponse> SignInPortal(UserSignInModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var portalRoles = _roleManager.Roles.ToList();

                var hasMatchingRole = userRoles.Any(userRole => portalRoles.Any(role => role.Name == userRole));

                if (hasMatchingRole)
                {
                    var isPasswordTrue = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (isPasswordTrue)
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, false, lockoutOnFailure: false);

                        if (signInResult.Succeeded)
                        {
                            return new ServiceResponse<UserSignInModel>
                            {
                                IsSuccess = true,
                                StatusCode = HttpStatusCode.OK,
                                Message = "Giriş yapıldı.",
                                Data = model
                            };
                        }
                    }

                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Kullanıcı adı veya şifre hatalı."
                    };
                }

                return new ServiceResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    Message = "Portala giriş yetkiniz bulunmamaktadır."
                };
            }

            return new ServiceResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = "Kullanıcı bulunamadı.",
            };
        }

        public async Task<ServiceResponse> SignOutPortal()
        {
            await _signInManager.SignOutAsync();
            return new ServiceResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
            };
        }


    }
}
