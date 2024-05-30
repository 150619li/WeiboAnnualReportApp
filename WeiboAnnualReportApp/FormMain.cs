using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;

namespace WeiboAnnualReportApp
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            string basePath = Path.GetDirectoryName(Application.ExecutablePath);  // �ҵ��Ǹ�exe�ļ����ڵ�λ��
            string mainHtmlPath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";
            Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", mainHtmlPath);
        }
    }
}
