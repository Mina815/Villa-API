using Microsoft.AspNetCore.Mvc;
using Villa_API.Models;

namespace Villa_API.Controllers
{
	[Route("Api/VillaAPI")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		[HttpGet]
		public IEnumerable<Villa> VillaList()
		{
			return new List<Villa>() {
				new Villa { Id = 1, Name = "Villa1" },
				new Villa { Id = 2, Name = "Villa2" },
			};
		}
	}
}
