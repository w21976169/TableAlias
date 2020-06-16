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
    public class DbController : ControllerBase
    {
        private readonly DbService _dbService;

        public DbController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        [Route("init")]
        public void Init()
        {
            try
            {
                _dbService.Init();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Route("testMapping")]
        public void TestMapping()
        {
            try
            {
                _dbService.TestMapping();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Route("testAs")]
        public void TestAs()
        {
            try
            {
                _dbService.TestAs();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Route("testPriority")]
        public void TestPriority()
        {
            try
            {
                _dbService.TestPriority();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
}