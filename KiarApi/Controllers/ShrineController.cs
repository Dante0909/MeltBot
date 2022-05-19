using PassionLib;
using PassionLib.DAL;
using PassionLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KiarApi.Controllers
{
    [ApiController]
    [Route("api/shrine")]
    public class ShrineController : BaseApiController
    {
        private readonly RunsContext context;

        //public RunsController(IConfiguration configuration, RunsContext context) : this(context)
        //{
        //    this.context = context;
        //}
        public ShrineController(RunsContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPongs()
        {
            return Ok(await context.Cereal.ToListAsync());
        }
    }
}
