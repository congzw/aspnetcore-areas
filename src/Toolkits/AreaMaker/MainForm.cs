using System;
using System.Windows.Forms;
using AreaMaker.Services;

namespace AreaMaker
{
    public partial class MainForm : Form
    {
        private readonly IAreaService _areaService;

        public MainForm(IAreaService areaService)
        {
            _areaService = areaService;
            InitializeComponent();
        }
        
        private string templateName = "_template";
        private string dirPath;
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.txtProjectPrefix.Text = _areaService.ProjectPrefix;
            var rootPath = _areaService.GetRootPath();
            templateName = _areaService.TemplateName;
            dirPath = _areaService.GetTemplateFolderPath(rootPath, null);
            var vr = _areaService.ValidateTemplateFolder(dirPath);
            if (!vr.Success)
            {
                MessageBox.Show(vr.Message);
                this.Close();
            }

            var autoFixTemplate = _areaService.AutoFixTemplate(dirPath);
            if (!autoFixTemplate.Success)
            {
                MessageBox.Show(autoFixTemplate.Message);
                this.Close();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string areaName = this.txtAreaName.Text.Trim();

            var result = _areaService.ValidateAreaName(areaName);
            if (!result.Success)
            {
                MessageBox.Show(result.Message);
                return;
            }

            var messageResult = _areaService.CreateArea(new CreateAreaModel()
                {AreaName = areaName, TemplateFolderPath = dirPath, TemplateName = templateName});

            if (!messageResult.Success)
            {
                MessageBox.Show(messageResult.Message);
                return;
            }

            var outPutDir = messageResult.Data.ToString();
            if (checkOpenDir.Checked)
            {
                try
                {
                    System.Diagnostics.Process.Start(outPutDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
