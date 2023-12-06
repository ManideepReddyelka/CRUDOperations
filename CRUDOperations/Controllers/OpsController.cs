using CRUDOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CRUDOperations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpsController : ControllerBase
    {
        private readonly OpsContext _Dbcontext;

        public OpsController(OpsContext dbcontext)
        {
            _Dbcontext = dbcontext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ops>>> GetOps()
        {
            if (_Dbcontext == null)
            {
                return NotFound();
            }
            return await _Dbcontext.Ops.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Ops>> GetOps(int id)
        {
            if (_Dbcontext == null)
            {
                return NotFound();
            }
            var ops = await _Dbcontext.Ops.FindAsync(id);
            
                if (ops == null)
                {
                return  NotFound(); 
                   
                }
                 Log.Information("Getting the data =>{@ops}", ops);
           return ops;
            
        }

        [HttpPost]
        public async Task<ActionResult<Ops>> PostOps(Ops ops)
        {
            _Dbcontext.Ops.Add(ops);
            await _Dbcontext.SaveChangesAsync();    
            return CreatedAtAction(nameof(GetOps),new { id = ops.Id}, ops);
        }
        [HttpPut]
        public async Task<IActionResult> PutOps(int id , Ops ops)
        {
            if(id != ops.Id)
            {
                return BadRequest();
            }
            _Dbcontext.Entry(ops).State = EntityState.Modified;
            try
            {
                await _Dbcontext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!OpsAvailble(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
            
        }
        private bool OpsAvailble(int id)
        {
            return (_Dbcontext.Ops?.Any(x => x.Id == id)).GetValueOrDefault();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOps(int id)
        {
            if(_Dbcontext.Ops != null)
            {
                return NotFound();
            }
            var ops = await _Dbcontext.Ops.FindAsync(id);
            if(ops != null)
            {
                return NotFound();
            }
            _Dbcontext.Ops.Remove(ops);

            await _Dbcontext.SaveChangesAsync();
            return Ok();
        }
    }
}
