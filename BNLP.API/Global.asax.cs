using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace BNLP.API
{
	public class Global : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			NLPToken.GlobalVariable.ResourceLocation = Server.MapPath("~/Data/");
			Common.Dummy();
		}
	}
}
