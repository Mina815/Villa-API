using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
		
        [HttpGet]
		public ActionResult<IEnumerable<VillaDTO>> GetVillaList()
		{
			return Ok(VillaStore.VillaList);
		}

		[HttpGet("{id:int}", Name = "GetVilla")]
		// For Document the response types 
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		public ActionResult<VillaDTO> Getvilla(int id)
		{
			if(id == 0) return BadRequest();
			var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
			if(villa == null) return NotFound();
			return villa;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO Villa)
		{
			//if(!ModelState.IsValid) return BadRequest(ModelState);
			if(VillaStore.VillaList.FirstOrDefault(u=> u.Name.ToLower() == Villa.Name)!= null)
			{
				ModelState.AddModelError("CustomError", "Villa name is already exsists");
				return BadRequest(ModelState);
			}
			if(Villa == null) return BadRequest(Villa);
			if(Villa.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

			Villa.Id = VillaStore.VillaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
			VillaStore.VillaList.Add(Villa);
			return CreatedAtRoute("GetVilla",new {id = Villa.Id},Villa);
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
			var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
			if (villa == null) return NotFound();
			VillaStore.VillaList.Remove(villa);
			return NoContent();
		}
		[HttpPut("{id:int}", Name = "UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdateVilla(int id , [FromBody] VillaDTO villaDTO)
		{
			if(id == 0 || villaDTO == null) return BadRequest();
			var villa = VillaStore.VillaList.FirstOrDefault(u=>u.Id == id);
			if (villa == null) return NotFound();
			villa.Name = villaDTO.Name;
			villa.Sqft = villaDTO.Sqft;
			villa.occupency = villaDTO.occupency;
			return NoContent();
		}
		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> VillaPatch)
		{
			if(id == 0 ||  VillaPatch == null) return BadRequest();
			var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
			if (villa == null) return NotFound();
			VillaPatch.ApplyTo(villa,ModelState);
			if(!ModelState.IsValid)return BadRequest(ModelState);
			return NoContent();
		}
	}
}
