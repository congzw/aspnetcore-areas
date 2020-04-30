using System;
using System.IO;
using System.Text.RegularExpressions;
using AreaMaker.Common;

namespace AreaMaker.Services
{
    public interface IAreaService
    {
        string TemplateName { get; set; }
        string GetRootPath();
        string GetTemplateFolderPath(string rootPath, string template);
        MessageResult ValidateTemplateFolder(string templateFolderPath);
        MessageResult AutoFixTemplate(string templateFolderPath);
        MessageResult ValidateAreaName(string area);
        MessageResult CreateArea(CreateAreaModel model);
    }

    public class CreateAreaModel
    {
        public string AreaName { get; set; }
        public string TemplateFolderPath { get; set; }
        public string TemplateName { get; set; }
    }

    public class AreaService : IAreaService
    {
        public AreaService()
        {
            TemplateName = "_template";
        }

        public string TemplateName { get; set; }

        public string GetRootPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public string GetTemplateFolderPath(string rootPath, string template)
        {
            if (string.IsNullOrWhiteSpace(rootPath))
            {
                throw new ArgumentNullException(nameof(rootPath));
            }
            

            if (string.IsNullOrWhiteSpace(template))
            {
                template = TemplateName;
            }

            return Path.Combine(GetRootPath().TrimEnd('\\'), template);
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

        public MessageResult AutoFixTemplate(string templateFolderPath)
        {
            UtilsLogger.LogMessage("AutoFixTemplate: " + templateFolderPath);
            var result = new MessageResult();
            if (!Directory.Exists(templateFolderPath))
            {
                result.Message = "模板不存在: " + templateFolderPath;
                return result;
            }
            
            result.Message = "OK";
            result.Success = true;
            return result;
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

        public MessageResult CreateArea(CreateAreaModel model)
        {
            var result = CreateNewArea(model.AreaName, model.TemplateFolderPath, model.TemplateName);
            return result;
        }
        
        private MessageResult CreateNewArea(string areaName, string dirPath, string templateName)
        {
            var result = new MessageResult();
            string outPutDir = dirPath.Replace(templateName, areaName);
            if (Directory.Exists(outPutDir))
            {
                result.Message = string.Format("要创建的路径{0}\r\n处已经有一个同名模块，请确认！", outPutDir);
                return result;
            }

            try
            {
                //生成思路：
                //1 将Xxx文件夹Copy一份，重命名为新的Area名称，以FTC为例子
                //2 检测并保证有几个空目录
                //    Content
                //    Controllers
                //    ViewModels
                //3 修改如下：
                //    替换Properties\AssemblyInfo.cs
                //        替换Web.Areas.Xxx为新的Web.Areas.FTC
                //        替换{新的GUID}为新的guid
                //    替换XxxAreaRegistration.cs
                //        替换Xxx为FTC
                //    替换ZQNB.Web.Areas.Xxx.csproj
                //        替换{新的GUID}为NewGUID
                //        替换Xxx为FTC
                //    改名
                //        修改文件名XxxAreaRegistration.cs为TTCAreaRegistration.cs
                //        修改文件名ZQNB.Web.Areas.Xxx.csproj为ZQNB.Web.Areas.FTC.csproj	

                const string guidPlaceHolder = "新的GUID";
                string message = "";

                MyIOHelper.CopyFolder(dirPath, outPutDir);

                string contentFolder = string.Format("{0}\\{1}", outPutDir, "Content");
                MyIOHelper.TryCreateFolder(contentFolder);
                string controllersFolder = string.Format("{0}\\{1}", outPutDir, "Controllers");
                MyIOHelper.TryCreateFolder(controllersFolder);
                string viewModelsFolder = string.Format("{0}\\{1}", outPutDir, "ViewModels");
                MyIOHelper.TryCreateFolder(viewModelsFolder);

                //Properties\AssemblyInfo.cs
                string assemblyInfoFilePath = string.Format("{0}\\Properties\\AssemblyInfo.cs", outPutDir);
                string assemblyInfoFileContent = MyIOHelper.ReadAllText(assemblyInfoFilePath);
                string newAssemblyInfoFileContent = assemblyInfoFileContent
                    .Replace(templateName, areaName)
                    .Replace(guidPlaceHolder, Guid.NewGuid().ToString("D").ToUpper());

                if (!MyIOHelper.TrySaveFileWithAddAccessRule(assemblyInfoFilePath, newAssemblyInfoFileContent, null, out message))
                {
                    result.Message = message;
                    return result;
                }
                //XxxAreaRegistration.cs
                string xxxAreaRegistrationFilePath = string.Format("{0}\\XxxAreaRegistration.cs", outPutDir);
                string xxxAreaRegistrationFileContent = MyIOHelper.ReadAllText(xxxAreaRegistrationFilePath);
                string newXxxAreaRegistrationFileContent = xxxAreaRegistrationFileContent
                    .Replace(templateName, areaName);
                if (!MyIOHelper.TrySaveFileWithAddAccessRule(xxxAreaRegistrationFilePath, newXxxAreaRegistrationFileContent, null, out message))
                {
                    result.Message = message;
                    return result;
                }
                //ZQNB.Web.Areas.Xxx.csproj
                string csprojFilePath = string.Format("{0}\\ZQNB.Web.Areas.Xxx.csproj", outPutDir);
                string csprojFilePathContent = MyIOHelper.ReadAllText(csprojFilePath);
                string newCsprojFilePathContent = csprojFilePathContent
                    .Replace(templateName, areaName)
                    .Replace(guidPlaceHolder, Guid.NewGuid().ToString("D").ToUpper());
                if (!MyIOHelper.TrySaveFileWithAddAccessRule(csprojFilePath, newCsprojFilePathContent, null, out message))
                {
                    result.Message = message;
                    return result;
                }
                //改名
                string newXxxAreaRegistrationFilePath = xxxAreaRegistrationFilePath.Replace(templateName, areaName);
                if (!MyIOHelper.TryChangeFileName(xxxAreaRegistrationFilePath, newXxxAreaRegistrationFilePath))
                {
                    result.Message = string.Format("change file name failed: {0} -> {1}", xxxAreaRegistrationFilePath, newXxxAreaRegistrationFilePath);
                    return result;
                }
                string newCsprojFilePath = csprojFilePath.Replace(templateName, areaName);
                if (!MyIOHelper.TryChangeFileName(csprojFilePath, newCsprojFilePath))
                {
                    result.Message = string.Format("change file name failed: {0} -> {1}", csprojFilePath, newCsprojFilePath);
                    return result;
                }

                result.Success = true;
                result.Message = "创建完毕";
                result.Data = outPutDir;
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
