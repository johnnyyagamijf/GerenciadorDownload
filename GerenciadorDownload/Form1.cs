using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerenciadorDownload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var text = textBox1.Text;

            //using (var http = new HttpClient())
            //{
            //    var html = http.GetAsync(text);
            //}
            var result = this.GetContent(text);

        }

        private  async Task GetContent(string url)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var http = new HttpClient())
            {
                http.Timeout = new TimeSpan(0, 15, 0);
                var result = await http.GetAsync(url);

                var t = result.Content.ReadAsStringAsync();
                var tet = result.StatusCode;
                var httpClient = new WebClient();
            

                var html = new WebClient().DownloadString(url);

                var link = Regex.Match(html, @"(?<=https://drive.google.com/file.d.).*?(?=.view)").Value;

                var linkFinal = @"https://drive.google.com/uc?id=" + link + "&export=download";


                 html = new WebClient().DownloadString(linkFinal);


                 var link4 = Regex.Match(html, @"uc.export=download.*?(?=id)").Value.Replace("&amp;", string.Empty).Replace("downloadconfirm", "download&confirm");

                var linkDownload = string.Format(@"https://drive.google.com/{0}&id={1}",
                    link4,
                    link);


                var result3 = await http.GetAsync(linkDownload);

                 var ttt = result3.Content.ReadAsByteArrayAsync();

                File.WriteAllBytes(@"D:\got.rar", ttt.Result);

            }
   
        }
    }
}
