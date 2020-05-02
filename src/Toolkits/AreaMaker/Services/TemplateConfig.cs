using System.Collections.Generic;

namespace AreaMaker.Services
{
    public class TemplateConfig
    {
        public TemplateConfig()
        {
            AreaPlaceHolder = "{{Area}}";
            ProjectPrefixPlaceHolder = "{{ProjectPrefix}}";
            AutoCreateFolders = new List<string>();
            AutoCreateFolders.Add("Content/css");
            AutoCreateFolders.Add("Content/scripts");
        }

        public string AreaPlaceHolder { get; set; }
        public string ProjectPrefixPlaceHolder { get; set; }
        public IList<string> AutoCreateFolders { get; set; }
    }

    public class AreaConfig
    {
        public AreaConfig()
        {
            ProjectPrefix = "MyApp.Areas.Web.";
            Area = "";
        }
        public string ProjectPrefix { get; set; }
        public string Area { get; set; }
    }
}
