using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Villa_API.Data;
using Villa_API.Logging;
using Villa_API.Models;
using Villa_API.Models.Dto;

namespace Villa_API.Controllers
{
	[Route("Api/VillaAPI")]
	// If you remove apiController annotation you have to exciplicitly validate the model by using model state
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;
        public VillaAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
			_mapper = mapper;
        }
        [HttpGet]
		public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillaList()
		{
			IEnumerable<Villa> villa = await _db.villas.ToListAsync();
			return  Ok(_mapper.Map<List<VillaDTO>>(villa));
		}

		[HttpGet("{id:int}", Name = "GetVilla")]
		// For Document the response types 
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		public async Task<ActionResult<VillaDTO>> Getvilla(int id)
		{
			if(id == 0) return BadRequest();
			var villa = await _db.villas.FirstOrDefaultAsync(u => u.Id == id);
			if(villa == null) return NotFound();
			return Ok(_mapper.Map<VillaDTO>(villa));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody]VillaCreateDTO villaDTO)
		{
			//if(!ModelState.IsValid) return BadRequest(ModelState);
			if(await _db.villas.FirstOrDefaultAsync(u=> u.Name.ToLower() == villaDTO.Name)!= null)
			{
				ModelState.AddModelError("CustomError", "Villa name is already exsists");
				return BadRequest(ModelState);
			}
			if(villaDTO == null) return BadRequest(villaDTO);
			//if(villaDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

			Villa model = _mapper.Map<Villa>(villaDTO);
			await _db.villas.AddAsync(model);
			await _db.SaveChangesAsync();
			return CreatedAtRoute("GetVilla",new {id = model.Id}, model);
		}

		[HttpDelete("{id:int}", Name = "DeleteVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteVilla(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var villa = await _db.villas.FirstOrDefaultAsync(u => u.Id == id);
			if (villa == null) return NotFound();
			_db.villas.Remove(villa);
			await _db.SaveChangesAsync();
			return NoContent();
		}
		[HttpPut("{id:int}", Name = "UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateVilla(int id , [FromBody] VillaUpdateDTO villaDTO)
		{
			if(id != villaDTO.Id || villaDTO == null) return BadRequest();
			Villa model = _mapper.Map<Villa>(villaDTO);
		
			_db.villas.Update(model);
			await _db.SaveChangesAsync();
			return NoContent();
		}
		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> VillaPatch)
		{
			if(id == 0 ||  VillaPatch == null) return BadRequest();
			var villa = await _db.villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

			VillaUpdateDTO villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villa);
			if (villa == null) return NotFound();

			VillaPatch.ApplyTo(villaUpdateDTO, ModelState);
			if(!ModelState.IsValid)return BadRequest(ModelState);
			Villa model = _mapper.Map<Villa>(villaUpdateDTO);

			_db.villas.Update(model);
			await _db.SaveChangesAsync();
			return NoContent();
		}
	}
}
