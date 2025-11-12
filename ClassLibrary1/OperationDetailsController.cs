using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apidiesel.Models;

namespace apidiesel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationDetailsController : ControllerBase
    {
        private readonly dieselContext _context;

        public OperationDetailsController(dieselContext context)
        {
            _context = context;
        }

        // GET: api/OperationDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationDetail>>> GetOperationDetails()
        {
            return await _context.OperationDetails.ToListAsync();
        }

        // GET: api/OperationDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationDetail>> GetOperationDetail(int id)
        {
            var operationDetail = await _context.OperationDetails.FindAsync(id);

            if (operationDetail == null)
            {
                return NotFound();
            }

            return operationDetail;
        }

        // PUT: api/OperationDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperationDetail(int id, OperationDetail operationDetail)
        {
            if (id != operationDetail.DetailId)
            {
                return BadRequest();
            }

            _context.Entry(operationDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperationDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OperationDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OperationDetail>> PostOperationDetail(OperationDetail operationDetail)
        {
            _context.OperationDetails.Add(operationDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOperationDetail", new { id = operationDetail.DetailId }, operationDetail);
        }

        // DELETE: api/OperationDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperationDetail(int id)
        {
            var operationDetail = await _context.OperationDetails.FindAsync(id);
            if (operationDetail == null)
            {
                return NotFound();
            }

            _context.OperationDetails.Remove(operationDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OperationDetailExists(int id)
        {
            return _context.OperationDetails.Any(e => e.DetailId == id);
        }
    }
}
