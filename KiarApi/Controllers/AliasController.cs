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
    public class AliasController : BaseApiController
    {
        private readonly RunsContext context;

        //public RunsController(IConfiguration configuration, RunsContext context) : this(context)
        //{
        //    this.context = context;
        //}
        public AliasController(RunsContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlias()
        {
            var a = new AliasContainer()
            {
                servantAliases = await context.ServantAliases.Include(x => x.Servant).ToListAsync(),
                craftEssenceAliases = await context.CraftEssenceAliases.Include(x => x.CraftEssence).ToListAsync(),
                questAliases = await context.QuestAliases.Include(x => x.Quest).ToListAsync(),
                mysticCodeAliases = await context.MysticCodeAliases.Include(x => x.MysticCode).ToListAsync()
            };

            return Ok(JsonConvert.SerializeObject(a, Formatting.Indented));
        }
        private class AliasContainer
        {
            public List<ServantAlias> servantAliases;
            public List<CraftEssenceAlias> craftEssenceAliases;
            public List<QuestAlias> questAliases;
            public List<MysticCodeAlias> mysticCodeAliases;
        }
    }
}
