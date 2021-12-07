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
using System.Linq;
using System.Collections.Generic;

namespace KiarApi.Controllers
{
    [ApiController]
    [Route("api/runs")]
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
        public async Task<IActionResult> GetRuns()
        {
            ////Run r = context.Runs.Take(1).First();
            //User u = new User()
            //{
            //    Id = 1
            //};

            //Run r = new Run(new Quest(94042801, "【高難易度】護法少女スペシャルヒーローショー"), "link", new Servant(603800, "シャルロット・コルデー"), u);

            //string str = JsonConvert.SerializeObject(r, Formatting.Indented);

            ////Run s = JsonConvert.DeserializeObject<Run>(str);
            ////Console.WriteLine(s.Quest.Id);

            //return str;
            
            var l = await context.Runs.Include(r=>r.MysticCode).Include(r => r.Quest).Include(r => r.Party).ThenInclude(p => p.Servant).Include(r => r.Party).ThenInclude(p => p.CraftEssence).ToListAsync();
            l = l.OrderBy(x => x.Quest.CreatedDate).ToList();
            return Ok(l);
        }
    }
}
