/*using PassionLib;
using PassionLib.DAL;
using PassionLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KiarApi.Controllers
{
    public class RunsController : BaseApiController
    {
        private readonly RunsContext context;

        //public RunsController(IConfiguration configuration, RunsContext context) : this(context)
        //{
        //    this.context = context;
        //}
        public RunsController(RunsContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Run>>> GetRuns()
        {
            return await context.Runs.Take(10).ToListAsync();
        }
    }
}
*/
using PassionLib;
using PassionLib.DAL;
using PassionLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KiarApi.Controllers
{
    [ApiController]
    [Route("api/alias")]
    public class CeController : BaseApiController
    {
        private readonly RunsContext context;

        //public RunsController(IConfiguration configuration, RunsContext context) : this(context)
        //{
        //    this.context = context;
        //}
        public CeController(RunsContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetServants()
        {

            return Ok(await context.CraftEssences.ToListAsync());
        }
    }
}
