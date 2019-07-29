using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camps")]
    public class CampsController : ApiController
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var camps = await _repository.GetAllCampsAsync();

                return Ok(_mapper.Map<IEnumerable<CampModel>>(camps));
            }
            catch (Exception e)
            {
                //TODO: add logging
                return InternalServerError();
            }
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Get(string moniker)
        {
            try
            {
                var camps = await _repository.GetCampAsync(moniker);

                return Ok(_mapper.Map<CampModel>(camps));
            }
            catch (Exception e)
            {
                //TODO: add logging
                return InternalServerError();
            }
        }
    }
}