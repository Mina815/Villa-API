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
		public IEnumerable<VillaDTO> VillaList()
		{
			return VillaStore.VillaList;
		}

		[HttpGet("{id:int}")]
		public VillaDTO Getvilla(int id)
		{
			return VillaStore.VillaList.FirstOrDefault(u=>u.Id== id);
		}
	}
}
