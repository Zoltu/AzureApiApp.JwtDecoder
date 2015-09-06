using System;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;
using System.Web.Http;

namespace Zoltu.RepoCreator.Controllers
{
	public class JwtDecoderController : ApiController
	{
		[HttpGet]
		[Route("api/Decode")]
		public String Decode(String jwtTokenOrAuthorizationHeader, String base64EncodedSecret, String validAudience, String validIssuer)
		{
			Contract.Requires(jwtTokenOrAuthorizationHeader != null);
			Contract.Requires(base64EncodedSecret != null);
			Contract.Ensures(Contract.Result<String>() != null);

			var jwtEncodedString = GetJwtTokenFromPossiblyHeader(jwtTokenOrAuthorizationHeader);
			ValidateJwtWithHs256(jwtEncodedString, base64EncodedSecret, validAudience, validIssuer);

			var payload = new JwtSecurityToken(jwtEncodedString).Payload;
			Contract.Assume(payload != null);
			var payloadJson = payload.SerializeToJson();
			Contract.Assume(payloadJson != null);
			return payloadJson;
		}

		private static String GetJwtTokenFromPossiblyHeader(String jwtTokenOrAuthorizationHeader)
		{
			Contract.Requires(jwtTokenOrAuthorizationHeader != null);
			Contract.Ensures(Contract.Result<String>() != null);

			if (!jwtTokenOrAuthorizationHeader.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
				return jwtTokenOrAuthorizationHeader;

			return jwtTokenOrAuthorizationHeader.Substring("Bearer ".Length);
		}

		private static void ValidateJwtWithHs256(String encodedJwt, String base64EncodedSecret, String validAudience, String validIssuer)
		{
			var secret = Base64UrlEncoder.DecodeBytes(base64EncodedSecret);
			ValidateJwtWithHs256(encodedJwt, secret, validAudience, validIssuer);
		}

		private static void ValidateJwtWithHs256(String encodedJwt, Byte[] secret, String validAudience, String validIssuer)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidAudience = validAudience,
				ValidIssuer = validIssuer,
				IssuerSigningToken = new BinarySecretSecurityToken(secret),
				ValidateAudience = validAudience != null,
				ValidateIssuer = validIssuer != null,
			};
			SecurityToken securityToken;
			new JwtSecurityTokenHandler().ValidateToken(encodedJwt, tokenValidationParameters, out securityToken);
		}

	}
}
