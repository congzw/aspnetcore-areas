using System;
using Microsoft.AspNetCore.Mvc;

namespace {{ProjectPrefix}}{{Area}}.Apis
{
    [ApiController]
    [Route("Api/{{Area}}/Test")]
    public class _TestApi : ControllerBase
    {
        [HttpGet("GetDate")]
        public DateTime GetDate()
        {
            return DateTime.Now;
        }
    }
}
