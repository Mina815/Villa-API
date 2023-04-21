using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Villa_API.Data;
using Villa_API.Models;
using Villa_API.Models.Dto;

namespace Villa_API.Controllers
{
	[Route("Api/VillaAPI")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		[HttpGet]
		public ActionResult<IEnumerable<VillaDTO>> VillaList()
		{
			return Ok(VillaStore.VillaList);
		}

		[HttpGet("{id:int}")]
		public ActionResult<VillaDTO> Getvilla(int id)
		{
			if(id == 0) return BadRequest();
			var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
			if(villa == null) return NotFound();
			return villa;
		}
	}
}
