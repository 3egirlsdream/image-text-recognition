using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Tesseract;
using Xu.Common;

namespace Translate
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Skin skin = new Skin();//全局申明，方便SKIN_MouseDown()每次点击加载配置文件
        MulLanguageBar bar = new MulLanguageBar();
        
        public string DebugTime;
        public MainWindow()
        {
            InitializeComponent();
            //主题从默认配置文件加载
            border1.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["Skin.Color.Default"]));

            cc.Content = new Frame() { Content = skin };
            skin.ParentWindow = this;//绑定Page的父窗口
            bar.ParentWindow = this;
            

            cc.Visibility = Visibility.Collapsed;
            if (tb1.Text.Equals(""))//文本框内容为空时，翻译按钮设为不可用
            {
                btn2.IsEnabled = false;
            }

            DebugTime = DateTime.Now.ToString();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CLOSE_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        string pathname = null;
        private void StartOrc(string imageSource = null)
        {
            TesseractEngine engine = new TesseractEngine(@"C:\Program Files (x86)\Tesseract-OCR\tessdata\", "chi_sim", EngineMode.Default);
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
                    tb1.Text = testText;
                    if (!testText.Equals(""))
                    {
                        btn2.IsEnabled = true;
                    }
                }
           
        }

        private string GetJson(string language)
        {
            string str = tb1.Text;
            string appKey = "2b92f227a3de2456";
            string from = "zh-CHS";
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
        //override
      

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            StartOrc();
            japan.Header = "日语";
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {

            GetEnglish();
            GetFrench();
            GetJapanese();
            GetKorean();
            //string str = "";
            //bool flag = false;
            //for (int i = 0; i < result.Count(); i++)
            //{
            //    if (result[i] == '[')
            //    {
            //        i += 1;
            //        while (result[i] != ']')
            //        {
            //            str += result[i];
            //            i++;
            //            //Console.WriteLine(str);
            //            if (result[i] == ']' && result[i - 1] == '"')
            //            {
            //                flag = true;
            //                break;
            //            }
            //        }

            //    }
            //    if (flag) break;
            //}
            //tb2.AppendText(result);
        }

        private void MINN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            Camera camera = new Camera();
            camera.ShowDialog();
            this.Visibility = Visibility.Visible;
            if(camera.isHasCamera) StartOrc("1");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*试用功能*/

            DateTime CurrentDate = new DateTime();
            CurrentDate = DateTime.Now;
            int year = CurrentDate.Year;
            int month = CurrentDate.Month;
            int day = CurrentDate.Day;
            //MessageBox.Show(day.ToString());
            if (year > 2018 || month > 5 || day > 5)
            {
                Warning warning = new Warning("试用结束！");
                warning.ShowDialog();
                this.Close();
            }
            

        }

        public void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /*
            string tootip = border1.ToolTip.ToString();
            if (tootip == "White")
            {
                border1.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(45, 45, 48));
                border1.ToolTip = "Black";
            }
                
            else if(tootip == "Black")
            {
                border1.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                border1.ToolTip = "White";
            }
            */
        }

        private void SKIN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            skin.border.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["Skin.Color.Default"]));
            cc.Visibility = cc.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        //多语言翻译
        private void GetEnglish()
        {
            string result = GetJson("EN");
            Root rt = JsonConvert.DeserializeObject<Root>(result);//解析JSON数据
            this.tb2.Document.Blocks.Clear();//清空文本框
            for (int i = 0; i < rt.translation.Count(); i++)
            {
                tb2.AppendText(rt.translation[i] + "\n");
            }
        }
        private void GetFrench()
        {
            string result = GetJson("fr");
            Root rt = JsonConvert.DeserializeObject<Root>(result);//解析JSON数据
            this.TbFrench.Document.Blocks.Clear();
            for (int i = 0; i < rt.translation.Count(); i++)
            {
                TbFrench.AppendText(rt.translation[i] + "\n");
            }
        }
        private void GetJapanese()
        {
            string result = GetJson("ja");
            Root rt = JsonConvert.DeserializeObject<Root>(result);//解析JSON数据
            this.TbJapanese.Document.Blocks.Clear();//清空文本框
            for (int i = 0; i < rt.translation.Count(); i++)
            {
                TbJapanese.AppendText(rt.translation[i] + "\n");
            }
        }
        private void GetKorean()
        {
            string result = GetJson("ko");
            Root rt = JsonConvert.DeserializeObject<Root>(result);//解析JSON数据
            this.TbKorean.Document.Blocks.Clear();//清空文本框
            for (int i = 0; i < rt.translation.Count(); i++)
            {
                TbKorean.AppendText(rt.translation[i] + "\n");
            }
        }

        private void setting_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Setting setting = new Setting(DebugTime);
            setting.ParentWindow = this;
            setting.ShowDialog();
        }

        private void MulLanguageBtnClick(object sender, MouseButtonEventArgs e)
        {
            btn1.IsEnabled = false;
            MulLanguageModule.Content = new Frame() { Content = bar };
        }
    }
}
