using System;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Web.Apis
{
    [Route("Api/Test")]
    public class TestApiController : ControllerBase
    {
        [HttpGet("GetDate")]
        public DateTime GetDate()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 获取Api描述
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetApiDesc")]
        public ActionResult<ApiDesc> GetApiDesc()
        {
            return ApiDesc.Create(GetApiDescName(), "1.0.0");
        }

        /// <summary>
        /// 默认的Api描述名称
        /// </summary>
        /// <returns></returns>
        protected virtual string GetApiDescName()
        {
            return this.GetType().FullName;
        }
    }

    /// <summary>
    /// Api描述
    /// </summary>
    public class ApiDesc
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 服务器时间
        /// </summary>
        public DateTime ServerDateTime { get; set; }

        public static ApiDesc Create(string name, string version)
        {
            return new ApiDesc() { Name = name, ServerDateTime = DateTime.Now, Version = version};
        }
    }
}
