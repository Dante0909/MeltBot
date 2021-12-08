﻿/*using PassionLib;
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

namespace KiarApi.Controllers
{
    [ApiController]
    [Route("api/pongs")]
    public class PongsController : BaseApiController
    {
        private readonly RunsContext context;

        //public RunsController(IConfiguration configuration, RunsContext context) : this(context)
        //{
        //    this.context = context;
        //}
        public PongsController(RunsContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPongs()
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
            return Ok(await context.Pongs.ToListAsync());
        }
    }
}