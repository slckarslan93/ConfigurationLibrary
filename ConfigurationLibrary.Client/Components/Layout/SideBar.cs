using Microsoft.AspNetCore.Mvc;

namespace ConfigurationLibrary.Client.Components.Layout
{
    public class SideBar : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Components/Layout/Sidebar.cs.cshtml");
        }
    }
}