using System;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Web.Areas.Demo.Apis
{
    [Route("Api/Demo/Test")]
    public class TestApiController : ControllerBase
    {
        [HttpGet("GetDate")]
        public DateTime GetDate()
        {
            return DateTime.Now;
        }
    }
}
