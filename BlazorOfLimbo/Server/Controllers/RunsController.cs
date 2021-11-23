using PassionLib;
using PassionLib.DAL;
using PassionLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlazorOfLimbo.Server.Controllers
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
            return Ok(await context.Runs.Include(r=>r.Quest).Include(r=>r.Dps).Include(r=>r.Party).ToListAsync());
        }
    }
}
