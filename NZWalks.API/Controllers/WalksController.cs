using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WalksController : ControllerBase
	{
		private readonly IWalkRepository _walkRepository;
		private readonly IMapper _mapper;

		public WalksController(IWalkRepository walkRepository, IMapper mapper)
		{
			_walkRepository = walkRepository;
			_mapper = mapper;
		}

		// POST Method: Create Walk

		[HttpPost]

		public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
		{
            if (ModelState.IsValid)
            {
				// Map DTO to Domain Model.
				var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

				// Use Domain Model to Create Walk.

				await _walkRepository.CreateAsync(walkDomainModel);

				// Map or Convert Domain Model to Dto.
				var walkDtoModel = _mapper.Map<WalkDto>(walkDomainModel);

				return Ok(walkDtoModel);

			}
            return BadRequest(ModelState);
		}

		// GET: localhost:port/api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumer=1&pageSize=10
		[HttpGet]

		public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
			[FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, 
			[FromQuery] int pageSize = 1000)
		{
			// Get data from Database.

			var walksDomain = await _walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending,pageNumber,pageSize);

			// Map Domain Models to DTOs.

			var walksDto = _mapper.Map<List<WalkDto>>(walksDomain);


			return Ok(walksDto);
		}


		[HttpGet]
		[Route("{id:int}")]

		public async Task<IActionResult> GetWalkById([FromRoute] int id)
		{
			// Get Walk Domain Model from Database.

			var walkDomain = await _walkRepository.GetByIdAsync(id);

			if (walkDomain == null)
			{
				return NotFound("It doesn't exist! Please try again.");
			}

			// Map from Domain Models to Dto.

			var walkDto = _mapper.Map<WalkDto>(walkDomain);

			return Ok(walkDto);
		}

		// Update the Walk.

		[HttpPut]
		[Route("{id:int}")]

		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
		{
			if (ModelState.IsValid)
			{
				// Map DTO to Domain Model.
				var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

				// check if Walk exists.

				walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

				if (walkDomainModel == null)
				{
					return NotFound();
				}

				// Map or Convert from Domain Model to DTO.

				var walkDto = _mapper.Map<WalkDto>(walkDomainModel);

				return Ok(walkDto);
			}
			return BadRequest(ModelState);
		}


		[HttpDelete]
		[Route("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var walkDomainModel = await _walkRepository.DeleteAsync(id);

			if (walkDomainModel == null)
			{
				return NotFound();
			}

			// Map Domain Model to DTO.
			var walkDto = _mapper.Map<WalkDto>(walkDomainModel);

			return Ok(walkDto);
		}
	}
}
