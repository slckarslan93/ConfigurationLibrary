using ConfigurationLibrary.Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationLibrary.Client.Components.Layout
{
    public class Header :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (HttpContext.Items["AppUser"] is UserInfoDto userInfo)
                return View("~/Components/Layout/Header.cs.cshtml", userInfo);

            return View("~/Components/Layout/Header.cs.cshtml", new UserInfoDto());
        }
    }
}
