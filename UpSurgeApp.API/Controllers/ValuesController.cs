using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpSurgeApp.API.Dtos;
using UpSurgeApp.API.Errors;

namespace UpSurgeApp.API.Controllers
{

    public class ValuesController : BaseApiController
    {
        // To get the Country with codes from the DB

        private readonly IMapper _mapper;
        private readonly IGenericRepository<Country> _countRepo;

        public ValuesController(IMapper mapper, IGenericRepository<Country> countRepo)
        {
            _mapper = mapper;
            _countRepo = countRepo;
        }

        [HttpGet("GetCountryList")]
        public async Task<ActionResult<List<CountryDto>>> GetCountryList()
        {
            try
            {
                var count = new Country();
                var countryList = await _countRepo.ListAllAsync();
                if (countryList.Count == 0)
                {
                    return NotFound(new ApiResponse(404));
                }

                var mappedData = _mapper.Map<List<Country>, List<CountryDto>>(countryList);

                return Ok(mappedData);
            }

            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                    { ex.Message }
                });
            }


        }




    }
}
