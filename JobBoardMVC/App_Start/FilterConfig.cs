using System.Web;
using System.Web.Mvc;

namespace JobBoardMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

			//in case we want to add global Authorize attribute (would need to add [AllowAnonymous] for controller actions where necessary):
			//filters.Add(new AuthorizeAttribute());

			//adding for google sign-in:
			filters.Add(new RequireHttpsAttribute());
        }
    }
}
