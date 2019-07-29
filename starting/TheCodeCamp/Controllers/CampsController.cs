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
        public async Task<IHttpActionResult> Get(bool includeTalks = false)
        {
            try
            {
                var camps = await _repository.GetAllCampsAsync(includeTalks);

                return Ok(_mapper.Map<IEnumerable<CampModel>>(camps));
            }
            catch (Exception e)
            {
                //TODO: add logging
                return InternalServerError();
            }
        }

        [Route("{moniker}", Name = "GetCamp")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var camp = await _repository.GetCampAsync(moniker, includeTalks);

                if (camp == null)
                    return NotFound();

                return Ok(_mapper.Map<CampModel>(camp));
            }
            catch (Exception e)
            {
                //TODO: add logging
                return InternalServerError();
            }
        }

        [Route("searchByDate/{eventDate:datetime}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByEventDate(DateTime eventDate, bool includeTalks = false)
        {
            try
            {
                var camps = await _repository.GetAllCampsByEventDate(eventDate, includeTalks);

                return Ok(_mapper.Map<IEnumerable<CampModel>>(camps));
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [Route()]
        public async Task<IHttpActionResult> Post([FromBody] CampModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var camp = _mapper.Map<Camp>(model);

                    _repository.AddCamp(camp);

                    if (await _repository.SaveChangesAsync())
                    {
                        var newModel = _mapper.Map<CampModel>(camp);

                        return CreatedAtRoute("GetCamp", new {moniker = newModel.Moniker}, newModel);
                    }
                }
            }
            catch (Exception e)
            {
                return InternalServerError();
            }

            return BadRequest();
        }
    }
}