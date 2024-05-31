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

        private void btnOpenReport_Click(object sender, EventArgs e)
        {
            string basePath = Path.GetDirectoryName(Application.ExecutablePath);  // �ҵ��Ǹ�exe�ļ����ڵ�λ��
            string mainHtmlPath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";
            Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", mainHtmlPath);
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            string basePath = Path.GetDirectoryName(Application.ExecutablePath);
            string mainHtmlPath = basePath + Path.DirectorySeparatorChar + "html" + Path.DirectorySeparatorChar + "MainPage.html";
            string html = "<!DOCTYPE html>\r\n" +//ҳ������
                "<html lang=\"en\">\r\n" +
                "<head>\r\n" +
                "<meta charset=\"UTF-8\">\r\n" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n" +
                "<title>�����л�ҳ��</title>\r\n" +

                "<style>\r\n" +//css�ļ��༭����ʽ����
                "body, html {\r\n" +
                "margin: 0;\r\n" +
                "padding: 0;\r\n" +
                "overflow: hidden;\r\n" +
                "height: 100%;\r\n" +
                "font-family: Arial, sans-serif;\r\n" +
                "}\r\n" +

                ".page {\r\n" +
                "position: absolute;\r\n" +
                "width: 100%;\r\n" +
                "height: 100%;\r\n" +
                "display: flex;\r\n" +
                "justify-content: center;\r\n" +
                "align-items: center;\r\n" +
                "transition: transform 0.5s ease-in-out;\r\n" +
                "}\r\n" +

                ".s-fcRed{\r\n" +
                "color: red; /* ����������ɫΪ��ɫ */\r\n" +
                "font-style: italic; /* ��������Ϊб�� */\r\n" +
                "}\r\n" +
                ".text-wrapper {\r\n" +
                "position: absolute;\r\n" +
                "top: 50%; /* �����ְ�����������ҳ��Ĵ�ֱ�м� */\r\n" +
                "transform: translateY(-50%);\r\n" +
                "width: 100%;\r\n" +
                "text-align: center; /* ���־��� */\r\n" +
                "}\r\n" +

                "#page1 {\r\n" +
                "background-color: #f1c40f;\r\n" +
                "transform: translateY(0);\r\n" +
                "}\r\n" +

                "#page2 {\r\n" +
                "background-color: #e74c3c;\r\n" +
                "transform: translateY(100%);\r\n" +
                "}\r\n" +

                "#page3 {\r\n" +
                "background-color: #3498db;\r\n" +
                "transform: translateY(200%);\r\n" +
                "}\r\n" +

                "#page4 {\r\n" +
                "background-color: #3498db;\r\n" +
                "transform: translateY(300%);\r\n" +
                "}\r\n" +

                "#page5 {\r\n" +
                "background-color: #3498db;\r\n" +
                "transform: translateY(400%);\r\n" +
                "}\r\n" +

                "#page6 {\r\n" +
                "background-color: #3498db;\r\n" +
                "transform: translateY(500%);\r\n" +
                "}\r\n" +
                "</style>\r\n" +
                "</head>\r\n" +

                "<body>\r\n" +//ҳ������༭

                "<div class=\"container\" id=\"container\">\r\n" +

                "<div class=\"page\" id=\"page1\">\r\n" +//��һ��ҳ��
                "<div class=\"text-wrapper\">\r\n" +
                "<h1 style=\"letter-spacing:.2em;font-size:29px;\">�ҵ������ռ�</h1>\r\n" +
                "<p style=\"letter-spacing:.3em;font-size:14px;\">����һ�����΢��ʹ�ñ���</p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "<div class=\"page\" id=\"page2\">\r\n" +//�ڶ���ҳ��
                "<div class=\"text-wrapper\">\r\n" +
                "<h1>��һ����</h1>\r\n" +
                "<p>��һ��ȥ��<em class=\"s-fcRed\">13</em>������</p>\r\n" +
                "<p>������õ���<em class=\"s-fcRed\">�Ͼ�</em></p>\r\n" +
                "<p>&nbsp;</p>\r\n" +
                "<p>�����еķ�����</p>\r\n" +
                "<p>ϲ����<em class=\"s-fcRed\">����</em>����΢��</p>\r\n" +
                "<p>&nbsp;</p>\r\n" +
                "<p>����ϲ��˵�Ĵ�����<em class=\"s-fcRed\">���Ȱ�</em></p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "<div class=\"page\" id=\"page3\">\r\n" +//������ҳ��
                "<div class=\"text-wrapper\">\r\n" +
                "<p><em class=\"s-fcRed\">8��19��</em></p>\r\n" +
                "<p>��һ����˯�ú���</p>\r\n" +
                "<p><em class=\"s-fcRed\">4��11</em>������΢�����</p>\r\n" +
                "<p>&nbsp;</p>\r\n" +
                "<p>��һ���㷢�˸�΢��</p>\r\n" +
                "<p><em class=\"s-fcRed\">������賿�ĵ��������</em></p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "<div class=\"page\" id=\"page4\">\r\n" +//������ҳ��
                "<div class=\"text-wrapper\">\r\n" +
                "<p>���ǵ���ҳ</p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "<div class=\"page\" id=\"page5\">\r\n" +//������ҳ��
                "<div class=\"text-wrapper\">\r\n" +
                "<p>���ǵ���ҳ</p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "<div class=\"page\" id=\"page6\">\r\n" +//������ҳ��
                "<div class=\"text-wrapper\">\r\n" +
                "<p>���ǵ���ҳ</p>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +

                "</div>\r\n" +

                "<script>\r\n" +//Javascript�༭����������
                "document.addEventListener('DOMContentLoaded', () => {\r\n" +
                "let currentPage = 0;\r\n" +
                "const pages = document.querySelectorAll('.page');\r\n" +
                "const switchPage = (direction) => {\r\n" +
                "if (direction === 'up' && currentPage > 0) {\r\n" +
                "currentPage--;\r\n" +
                "} else if (direction === 'down' && currentPage < pages.length - 1) {\r\n" +
                "currentPage++;\r\n" +
                "}\r\n" +
                "pages.forEach((page, index) => {\r\n" +
                "page.style.transform = `translateY(${(index - currentPage) * 100}%)`;\r\n" +
                "});\r\n" +
                "};\r\n" +
                "function handleScroll(event) {\r\n" +
                "if (event.deltaY > 0) {\r\n" +
                "switchPage('down');\r\n" +//ҳ���»�
                "} else {\r\n" +
                "switchPage('up');\r\n" +//ҳ���ϻ�
                "}\r\n" +
                "showPage(currentPage);\r\n" +
                "}\r\n" +
                "window.addEventListener('wheel', handleScroll);\r\n" +
                "});\r\n" +
                "</script>\r\n" +
                "</body>\r\n" +
                "</html>";
            File.WriteAllText(mainHtmlPath, html);
        }
    }
}
