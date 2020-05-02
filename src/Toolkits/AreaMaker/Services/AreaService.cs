using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AreaMaker.Common;

namespace AreaMaker.Services
{
    public interface IAreaService
    {
        TemplateConfig GetTemplateConfig();
        AreaConfig GetAreaConfig();
        MessageResult ValidateTemplateFolder(string templateFolderPath);
        MessageResult ValidateAreaName(string area);
        MessageResult CreateArea(TemplateConfig config, CreateAreaArgs args);
    }

    public class CreateAreaArgs
    {
        public string ProjectPrefix { get; set; }
        public string Area { get; set; }
        public string TemplateFolderPath { get; set; }
    }

    public class AreaService : IAreaService
    {
        public TemplateConfig GetTemplateConfig()
        {
            //todo read by config
            return new TemplateConfig();
        }

        public AreaConfig GetAreaConfig()
        {
            //todo read by config
            return new AreaConfig();
        }

        public MessageResult ValidateTemplateFolder(string templateFolderPath)
        {
            var vr = new MessageResult();
            if (!Directory.Exists(templateFolderPath))
            {
                vr.Message = "模板不存在: " + templateFolderPath;
                return vr;
            }

            vr.Message = "OK";
            vr.Success = true;
            return vr;
        }

        public MessageResult ValidateAreaName(string areaName)
        {
            var result = new MessageResult();
            if (string.IsNullOrWhiteSpace(areaName))
            {
                result.Message = "模块名称不能为空";
                return result;
            }

            if (!IsAllEnglish(areaName))
            {
                result.Message = "模块名称必须全部是英文字母";
                return result;
            }

            result.Message = "OK";
            result.Success = true;
            return result;
        }

        public MessageResult CreateArea(TemplateConfig config, CreateAreaArgs args)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var result = CreateNewArea(config.ProjectPrefixPlaceHolder,
                config.AreaPlaceHolder,
                config.AutoCreateFolders,
                args.ProjectPrefix,
                args.Area,
                args.TemplateFolderPath);
            return result;
        }

        private MessageResult CreateNewArea(string projectPrefixPlaceHolder, string areaPlaceHolder, IList<string> autoCreateFolders, string projectPrefix, string areaName, string templateFolderPath)
        {
            if (string.IsNullOrWhiteSpace(projectPrefix))
            {
                throw new ArgumentNullException(nameof(projectPrefix));
            }

            if (string.IsNullOrWhiteSpace(areaName))
            {
                throw new ArgumentNullException(nameof(areaName));
            }

            if (string.IsNullOrWhiteSpace(templateFolderPath))
            {
                throw new ArgumentNullException(nameof(templateFolderPath));
            }
            //生成思路：
            //1 将{{Area}}文件夹Copy一份，重命名为新的Area名称
            //2 检测并保证有几个空目录
            //    Content/css
            //    Content/scripts
            //3 修改如下：
            //    替换文本:
            //        替换{{ProjectPrefix}}
            //        替换{{Area}}
            //    替换文件名
            //        替换{{ProjectPrefix}}
            //        替换{{Area}}

            var result = new MessageResult();
            string areaDir = templateFolderPath.Replace(areaPlaceHolder, areaName);
            if (Directory.Exists(areaDir))
            {
                result.Message = string.Format("要创建的路径{0}\r\n处已经有一个同名模块，请确认！", areaDir);
                return result;
            }

            try
            {
                string message = "";

                MyIOHelper.CopyFolder(templateFolderPath, areaDir);

                foreach (var subFolder in autoCreateFolders)
                {
                    MyIOHelper.PrepareSubFolder(areaDir, subFolder);
                }

                var files = MyIOHelper.GetFiles(areaDir, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    string fileContent = MyIOHelper.ReadAllText(file);
                    var saveContent = fileContent
                        .Replace(projectPrefixPlaceHolder, projectPrefix)
                        .Replace(areaPlaceHolder, areaName);

                    if (!MyIOHelper.TrySaveFile(file, saveContent, null, out message))
                    {
                        result.Message = message;
                        return result;
                    }

                    var newFile = file
                        .Replace(projectPrefixPlaceHolder, projectPrefix)
                        .Replace(areaPlaceHolder, areaName);

                    if (!MyIOHelper.TryChangeFileName(file, newFile))
                    {
                        result.Message = string.Format("change file name failed: {0} -> {1}", file, newFile);
                        return result;
                    }
                }

                result.Success = true;
                result.Message = "创建完毕";
                result.Data = areaDir;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        private bool IsAllEnglish(string input)
        {
            string pattern = @"^[A-Za-z]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
    }
}
