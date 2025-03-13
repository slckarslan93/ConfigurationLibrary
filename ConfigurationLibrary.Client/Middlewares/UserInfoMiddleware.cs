using ConfigurationLibrary.UI.Entities.Identity;
using ConfigurationLibrary.UI.Models.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ConfigurationLibrary.UI.Middlewares
{
    public class UserInfoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UserInfoMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            // Kullanıcı zaten /Auth/SignIn sayfasındayken tekrar yönlendirme yapmaktan kaçının
            if (path.Equals("/Auth/SignIn", StringComparison.OrdinalIgnoreCase) || path.Equals("/sign-in", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (context.User?.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var userEmail = context.User.FindFirstValue(ClaimTypes.Email);

                if (!string.IsNullOrEmpty(userEmail))
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    var appUserInfo = await userManager.FindByEmailAsync(userEmail);

                    if (appUserInfo != null)
                    {
                        context.Items["AppUser"] = new UserInfoDto
                        {
                            Id = appUserInfo.Id,
                            Email = appUserInfo.Email,
                            FullName = appUserInfo.FullName,
                            UserImage = appUserInfo.UserImage,
                        };
                    }
                    else
                    {
                        context.Response.Redirect("/Auth/SignIn");
                        return;
                    }
                }
                else
                {
                    context.Response.Redirect("/Auth/SignIn");
                    return;
                }
            }
            else
            {
                context.Response.Redirect("/Auth/SignIn");
                return;
            }

            await _next(context);
        }
    }
}
