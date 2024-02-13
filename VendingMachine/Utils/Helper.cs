using System.Security.Claims;

namespace VendingMachineAPI.Utils
{
    public class Helper
    {
        private static IHttpContextAccessor _contextAccessor;
        private Helper() { }
        private static HttpContext CurrentContext
        {
            get
            {
                if (_contextAccessor == null)
                {
                    var HttpContext = StaticServiceProvider.Provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                    return HttpContext;
                }
                return _contextAccessor.HttpContext;
            }
            set { }
        }
        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public static string CurrentUserId()
        {
            var currentUser = CurrentContext.User;
            if (currentUser?.Identity != null && currentUser.Identity.IsAuthenticated)
            {
                return currentUser.Claims.FirstOrDefault(claim => claim.Type == "uid").Value;
            }
            return string.Empty;
        }
        public static string CurrentUserRole()
        {
            var currentUser = CurrentContext.User;
            if (currentUser?.Identity != null && currentUser.Identity.IsAuthenticated)
            {
                return currentUser.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;
            }
            return string.Empty;
        }
    }
}
