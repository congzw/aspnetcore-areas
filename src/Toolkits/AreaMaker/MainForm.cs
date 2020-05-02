using System;
using System.Windows.Forms;
using AreaMaker.Common;
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

        private TemplateConfig _templateConfig = null;
        private string templateDir;
        private void MainForm_Load(object sender, EventArgs e)
        {
            _templateConfig = _areaService.GetTemplateConfig();
            var areaConfig = _areaService.GetAreaConfig();
            this.txtAreaName.Text = areaConfig.Area;
            this.txtProjectPrefix.Text = areaConfig.ProjectPrefix;

            templateDir = MyIOHelper.MakeSubFolderPath(AppDomain.CurrentDomain.BaseDirectory, _templateConfig.AreaPlaceHolder);
            var vr = _areaService.ValidateTemplateFolder(templateDir);
            if (!vr.Success)
            {
                MessageBox.Show(vr.Message);
                this.Close();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string areaName = this.txtAreaName.Text.Trim();
            string projectPrefix = this.txtProjectPrefix.Text.Trim();

            var result = _areaService.ValidateAreaName(areaName);
            if (!result.Success)
            {
                MessageBox.Show(result.Message);
                return;
            }

            var createAreaArgs = new CreateAreaArgs()
            {
                ProjectPrefix = projectPrefix,
                Area = areaName,
                TemplateFolderPath = templateDir
            };
            var messageResult = _areaService.CreateArea(_templateConfig, createAreaArgs);

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
