using ConfigurationLibrary.UI.Models.User;
using ConfigurationLibrary.UI.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationLibrary.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("Auth/SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("Auth/SignIn")]
        public async Task<IActionResult> SignIn(UserSignInModel model)
        {
            var result = await _authService.SignInPortal(model);

            if (result.IsSuccess)
            {
                return Redirect("/Home/Index");
            }

            TempData["ToastrState"] = "error";
            TempData["ToastrMessage"] = result.Message;

            return View();
        }

        [Authorize]
        [HttpGet("Auth/SignOut")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.SignOutPortal();

            if (result.IsSuccess)
            {
                TempData["ToastrState"] = "success";
                TempData["ToastrMessage"] = "Çıkış yapıldı";

                return Redirect("/Auth/SignIn");
            }

            TempData["ToastrState"] = "error";
            TempData["ToastrMessage"] = "Bir hata oluştu.";

            return Redirect("/Home/Index");
        }
    }
}
