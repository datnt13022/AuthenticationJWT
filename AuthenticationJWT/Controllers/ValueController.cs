using AuthenticationJWT.Filters;
using System.Web.Http;

namespace AuthenticationJWT.Controllers
{
    [Authorize]
    public class ValueController : ApiController
    {
        [JwtAuthentication]
        public string Get()
        {
            return "value";
        }
    }
}
