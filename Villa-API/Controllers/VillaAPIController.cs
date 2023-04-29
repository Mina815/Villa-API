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
        public VillaAPIController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
		public ActionResult<IEnumerable<VillaDTO>> GetVillaList()
		{
			return Ok(_db.villas.ToList());
		}

		[HttpGet("{id:int}", Name = "GetVilla")]
		// For Document the response types 
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		public ActionResult<VillaDTO> Getvilla(int id)
		{
			if(id == 0) return BadRequest();
			var villa = _db.villas.FirstOrDefault(u => u.Id == id);
			if(villa == null) return NotFound();
			return Ok(villa);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public ActionResult<VillaDTO> CreateVilla([FromBody]VillaCreateDTO villaDTO)
		{
			//if(!ModelState.IsValid) return BadRequest(ModelState);
			if(_db.villas.FirstOrDefault(u=> u.Name.ToLower() == villaDTO.Name)!= null)
			{
				ModelState.AddModelError("CustomError", "Villa name is already exsists");
				return BadRequest(ModelState);
			}
			if(villaDTO == null) return BadRequest(villaDTO);
			//if(villaDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

			Villa model = new()
			{
				Amenity = villaDTO.Amenity,
				Details = villaDTO.Details,
				ImageUrl = villaDTO.ImageUrl,
				Name = villaDTO.Name,
				Occupancy = villaDTO.Occupancy,
				Rate = villaDTO.Rate,
				Sqft = villaDTO.Sqft
			};
			_db.villas.Add(model);
			_db.SaveChanges();
			return CreatedAtRoute("GetVilla",new {id = model.Id}, model);
		}

		[HttpDelete("{id:int}", Name = "DeleteVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeleteVilla(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var villa = _db.villas.FirstOrDefault(u => u.Id == id);
			if (villa == null) return NotFound();
			_db.villas.Remove(villa);
			_db.SaveChanges();
			return NoContent();
		}
		[HttpPut("{id:int}", Name = "UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdateVilla(int id , [FromBody] VillaUpdateDTO villaDTO)
		{
			if(id == 0 || villaDTO == null) return BadRequest();
			//var villa = _db.villas.FirstOrDefault(u=>u.Id == id);
			//if (villa == null) return NotFound();
			////villa.Name = villaDTO.Name;
			//villa.Sqft = villaDTO.Sqft;
			//villa.Occupancy = villaDTO.Occupancy;
			Villa model = new()
			{
				Amenity = villaDTO.Amenity,
				Details = villaDTO.Details,
				//Id = villaDTO.Id,
				ImageUrl = villaDTO.ImageUrl,
				Name = villaDTO.Name,
				Occupancy = villaDTO.Occupancy,
				Rate = villaDTO.Rate,
				Sqft = villaDTO.Sqft
			};
			_db.villas.Update(model);
			_db.SaveChanges();
			return NoContent();
		}
		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> VillaPatch)
		{
			if(id == 0 ||  VillaPatch == null) return BadRequest();
			var villa = _db.villas.AsNoTracking().FirstOrDefault(u => u.Id == id);
			VillaUpdateDTO villaDTO = new()
			{
				Amenity = villa.Amenity,
				Details = villa.Details,
				Id = villa.Id,
				ImageUrl = villa.ImageUrl,
				Name = villa.Name,
				Occupancy = villa.Occupancy,
				Rate = villa.Rate,
				Sqft = villa.Sqft
			};
			if (villa == null) return NotFound();

			VillaPatch.ApplyTo(villaDTO, ModelState);
			if(!ModelState.IsValid)return BadRequest(ModelState);
			Villa model = new Villa()
			{
				Amenity = villaDTO.Amenity,
				Details = villaDTO.Details,
				Id = villaDTO.Id,
				ImageUrl = villaDTO.ImageUrl,
				Name = villaDTO.Name,
				Occupancy = villaDTO.Occupancy,
				Rate = villaDTO.Rate,
				Sqft = villaDTO.Sqft
			};

			_db.villas.Update(model);
			_db.SaveChanges();
			return NoContent();
		}
	}
}
