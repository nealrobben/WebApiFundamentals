using System;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;

namespace TheCodeCamp.Controllers
{
    public class CampsController : ApiController
    {
        private readonly ICampRepository _repository;

        public CampsController(ICampRepository repository)
        {
            _repository = repository;
        }

        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var camps = await _repository.GetAllCampsAsync();

                return Ok(camps);
            }
            catch (Exception e)
            {
                //TODO: add logging
                return InternalServerError();
            }
        }
    }
}