using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Helpers;
using PruebaTecnica.Models;
using PruebaTecnica.Services;

namespace PruebaTecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<Card>>> Create([FromBody] Card card)

        {
            var response = await _cardService.Create(card);
            if (response.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult<Response<PaginatedList<Card>>>> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string customerName = null)
        {
            var response = await _cardService.GetAll(pageIndex, pageSize, customerName);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Card>>> GetById(int id)
        {
            var response = await _cardService.GetById(id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }
    }
}