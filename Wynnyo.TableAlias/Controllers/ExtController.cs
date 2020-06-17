using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Wynnyo.TableAlias.Services;
using Microsoft.AspNetCore.Mvc;

namespace Wynnyo.TableAlias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtController : ControllerBase
    {
        private readonly ExtService _extService;

        public ExtController(ExtService dbService)
        {
            _extService = dbService;
        }

        [HttpPost]
        [Route("testQueryExt")]
        public void TestQueryExt()
        {
            try
            {
                _extService.TestQueryExt();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


    }
}