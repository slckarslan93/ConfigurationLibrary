using ConfigurationLibrary.UI.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationLibrary.UI.Components.Layout
{
    public class Header : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (HttpContext.Items["AppUser"] is UserInfoDto userInfo)
                return View("~/Components/Layout/Header.cs.cshtml", userInfo);

            return View("~/Components/Layout/Header.cs.cshtml", new UserInfoDto());
        }
    }
}