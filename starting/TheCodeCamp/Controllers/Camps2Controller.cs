using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [ApiVersion("2.0")]
    [RoutePrefix("api/camps")]
    public class Camps2Controller : ApiController
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public Camps2Controller(ICampRepository repository, IMapper mapper)
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

                return Ok(new
                {
                    Count = camps.Length,
                    Results = camps
                });
            }
            catch (Exception e)
            {
                //TODO: add logging
                return InternalServerError();
            }
        }

        [MapToApiVersion("1.0")]
        [Route("{moniker}", Name = "GetCamp20")]
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
        public async Task<IHttpActionResult> Post(CampModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(await _repository.GetCampAsync(model.Moniker) != null)
                        ModelState.AddModelError("","Moniker in use");

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

            return BadRequest(ModelState);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Put(string moniker, CampModel model)
        {
            try
            {
                var camp = await _repository.GetCampAsync(moniker);

                if (camp == null)
                    return NotFound();

                _mapper.Map(model, camp);

                if (await _repository.SaveChangesAsync())
                {
                    var newModel = _mapper.Map<CampModel>(camp);

                    return Ok(newModel);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception e)
            {
                return InternalServerError();
            }

            return BadRequest(ModelState);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Delete(string moniker)
        {
            try
            {
                var camp = await _repository.GetCampAsync(moniker);

                if (camp == null)
                    return NotFound();

                _repository.DeleteCamp(camp);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }
    }
}