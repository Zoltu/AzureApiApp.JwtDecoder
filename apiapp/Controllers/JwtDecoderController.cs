using System;
using System.Diagnostics.Contracts;
using System.Web.Http;

namespace Zoltu.RepoCreator.Controllers
{
	public class JwtDecoderController : ApiController
	{
		[HttpGet]
		[Route("api/JwtDecoder")]
		public String Get()
		{
			Contract.Ensures(Contract.Result<String>() != null);

			return "Hello API App!";
		}
	}
}
