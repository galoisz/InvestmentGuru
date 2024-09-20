using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data.Entities;
using WebApi.Data.Repositories;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockDataService _service;

        public StocksController(IStockDataService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStockEntries()
        {
            var stockEntries = await _service.GetAllAsync();
            return Ok(stockEntries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetStock(int id)
        {
            var Stock = await _service.GetByIdAsync(id);
            if (Stock == null)
            {
                return NotFound();
            }

            return Ok(Stock);
        }

        [HttpPost]
        public async Task<ActionResult<Stock>> PostStock(Stock Stock)
        {
            var createdStock = await _service.AddAsync(Stock);
            return CreatedAtAction("GetStock", new { id = createdStock.Id }, createdStock);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStock(int id, Stock Stock)
        {
            var updatedStock = await _service.UpdateAsync(id, Stock);
            if (updatedStock == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var deletedStock = await _service.DeleteAsync(id);
            if (deletedStock == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
