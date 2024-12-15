using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Net.WebSockets;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	
	public class RegionsController : ControllerBase
	{
		
		private readonly NZWalksDbContext _nZWalksDbContext;
		private readonly IRegionRepository _regionRepository;
		private readonly IMapper _mapper;

		public RegionsController(NZWalksDbContext nZWalksDbContext, IRegionRepository regionRepository, IMapper mapper)
        {
			_nZWalksDbContext = nZWalksDbContext;
			_regionRepository = regionRepository;
			_mapper = mapper;
		}

		// GET ALL REGIONS
		// GET Action: https://localhost:7253/api/Regions
		[HttpGet]
		[Authorize(Roles ="Reader")]
		public async Task<IActionResult> GetAllRegions() 
		{
			// Get Data from Database.
			var regionsDomain = await _regionRepository.GetAllAsync();


			// Map Domain Models to DTOs.
			var regionsDto = _mapper.Map<List<Region>>(regionsDomain);

			// Return DTOs.

			return Ok(regionsDto);
		}

		// GET SINGLE REGION
		// GET Action By Id: https://localhost:7253/api/Regions/{id}

		[HttpGet]
		[Route("{id:int}")]
		[Authorize(Roles = "Reader")]
		public async Task<IActionResult> GetRegionById([FromRoute]int id) 
		{
			// Get region Domain Model from Database
		    var regionDomain = await _regionRepository.GetByIdAsync(id);

			if (regionDomain == null) 
			{ 
			    return NotFound("It doesn't exist! Please try again.");
			}

			// Map Domain Models to DTOs.

			var regionDto = _mapper.Map<RegionDto>(regionDomain);

			// Return DTOs.
			return Ok(regionDto);
		}


		// Post Method to Create new Region.

		[HttpPost]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
		{
			if (ModelState.IsValid)
			{
				// Map or Convert Dto to Domain Model.

				var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

				// Use Domain Model to Create Region.

				regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);


				// Map or Convert Domain Model to Dto.

				var regionDtoModel = _mapper.Map<RegionDto>(regionDomainModel);

				return CreatedAtAction(nameof(GetRegionById), new { id = regionDtoModel.Id }, regionDtoModel);

			}
			else 
			{
				return BadRequest(ModelState);
			}			
		}

		// Update the Region.
		[HttpPut]
		[Route("{id:int}")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
		{
			if (ModelState.IsValid)
			{
				// Map DTO to Domain Model.
				var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);

				// check if Region exists.

				regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

				if (regionDomainModel == null)
				{
					return NotFound();
				}

				// Map or Convert from Domain Model to DTO.

				var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

				return Ok(regionDto);
			}
			else 
			{ 
			   return BadRequest(ModelState);
			}
			
		}

		[HttpDelete]
		[Route("{id:int}")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var regionDomainModel = await _regionRepository.DeleteAsync(id);

			if(regionDomainModel == null)
			{
				return NotFound();
			}

			// Map Domain Model to DTO.
			var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
			
			return Ok(regionDto);
		}

	}
}
