using Api_REST_CRUD.Models;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using ConectarDatos;

namespace Api_REST_CRUD.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            using (var ctx = new AdventureWorksLT2017Entities())
            {
                bool isCredentialValid = (from s in ctx.Personas
                                          where s.nombre == login.Username
                                          && s.NIF == login.Password
                                          select s).Any();

                if (isCredentialValid)
                {
                    var token = TokenGenerator.GenerateTokenJwt(login.Username);
                    return Ok(token);
                }
                else
                {
                    return Unauthorized();
                }
            }
        }
    }
}
