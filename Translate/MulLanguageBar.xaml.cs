using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tesseract;

namespace Translate
{
    /// <summary>
    /// MulLanguageBar.xaml 的交互逻辑
    /// </summary>
    public partial class MulLanguageBar : System.Windows.Controls.Page
    {
        public string language;
        string pathname = null;

        public MulLanguageBar()
        {
            InitializeComponent();

            btn2.IsEnabled = false;
            open.IsEnabled = false;
            language = "";
        }
        MainWindow parentWindow;
        public MainWindow ParentWindow
        {
            get
            {
                return parentWindow;
            }
            set
            {
                parentWindow = value;
            }
        }

        private string GetJson(string source, string language)
        {
            string str = this.parentWindow.tb1.Text;
            string appKey = "2b92f227a3de2456";
            string from = source;
            string to = language;
            string salt = DateTime.Now.Millisecond.ToString();
            string appSecret = "KdBgDXh1dzBKkREknjwVcTxE0vxiNxuP";
            MD5 md5 = new MD5CryptoServiceProvider();
            string md5Str = appKey + str + salt + appSecret;
            byte[] output = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(md5Str));
            string sign = BitConverter.ToString(output).Replace("-", "");

            string url = string.Format("http://openapi.youdao.com/api?appKey={0}&q={1}&from={2}&to={3}&sign={4}&salt={5}", appKey, System.Web.HttpUtility.UrlDecode(str, System.Text.Encoding.GetEncoding("UTF-8")), from, to, sign, salt);
            WebRequest translationWebRequest = WebRequest.Create(url);

            WebResponse response = null;

            response = translationWebRequest.GetResponse();
            Stream stream = response.GetResponseStream();

            Encoding encode = Encoding.GetEncoding("utf-8");

            StreamReader reader = new StreamReader(stream, encode);
            return reader.ReadToEnd();
        }

        private void jp_Click(object sender, RoutedEventArgs e)
        {
            language = "jpn";
            open.IsEnabled = true;
            DeleteButton("jp");
            DeleteButton("en");
            DeleteButton("rs");
        }

        private void en_Click(object sender, RoutedEventArgs e)
        {
            language = "eng";
            open.IsEnabled = true;
            DeleteButton("jp");
            DeleteButton("en");
            DeleteButton("rs");
        }

        private void rs_Click(object sender, RoutedEventArgs e)
        {
            language = "rs";
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            StartOrc(language);
            this.parentWindow.japan.Header = "中文";
            btn2.IsEnabled = true;
        }

        
        private void StartOrc(string SourceLanguage = "chi_sim")
        {
            string imageSource = null;
            TesseractEngine engine = new TesseractEngine(@"C:\Program Files (x86)\Tesseract-OCR\tessdata\", SourceLanguage, EngineMode.Default);
            engine.SetVariable("chop_enable", "F");
            engine.SetVariable("enable_new_segsearch", 0);
            engine.SetVariable("use_new_state_cost", "F");
            engine.SetVariable("segment_segcost_rating", "F");
            engine.SetVariable("language_model_ngram_on", 0);
            engine.SetVariable("textord_force_make_prop_words", "F");
            engine.SetVariable("edges_max_children_per_outline", 50);
            if (imageSource == null)
            {
                var file = new Microsoft.Win32.OpenFileDialog();
                file.Filter = "所有文件(*.*)|*.*";
                var image = file.ShowDialog();

                if (file.FileName != string.Empty)
                {
                    try
                    {
                        pathname = file.FileName;   //获得文件的绝对路径
                                                    //MessageBox.Show(pathname);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                //获取当前电脑桌面路径
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                pathname = dir + @"\photo.jpg";
                //MessageBox.Show(pathname);
            }

            if (pathname != null)//文件的绝对路径不为空时
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(pathname);
                Bitmap p = (Bitmap)img;
                var page = engine.Process(p, pageSegMode: engine.DefaultPageSegMode);
                var testText = "";
                testText = page.GetText();
                var c = page.GetMeanConfidence();
                this.parentWindow.tb1.Text = testText;
                if (!testText.Equals(""))
                {
                    btn2.IsEnabled = true;
                }
            }

        }

        private void DeleteButton(string name)//删除控件
        {
            Button btn = grid.FindName(name) as Button;
            if(btn != null)
            {
                grid.Children.Remove(btn);
                grid.UnregisterName(name);
            }
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            GetJapanese();
            this.Visibility = Visibility.Hidden;
            this.parentWindow.btn1.IsEnabled = true;
        }

        private void GetJapanese()
        {
            string result = GetJson("ja", "zh-CHS");
            Root rt = JsonConvert.DeserializeObject<Root>(result);//解析JSON数据
            this.parentWindow.TbJapanese.Document.Blocks.Clear();//清空文本框
            for (int i = 0; i < rt.translation.Count(); i++)
            {
                this.parentWindow.TbJapanese.AppendText(rt.translation[i] + "\n");
            }
        }
    }

    
}
