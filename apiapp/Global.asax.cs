using System.Web;
using System.Web.Http;

namespace Zoltu.AzureApiApp
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}
