using System.Web.Http;

namespace TheCodeCamp.Controllers
{
    public class CampsController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(new { Name = "Shawn", Occupation = "Teacher" });
        }
    }
}