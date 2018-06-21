using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

namespace Translate
{
    /// <summary>
    /// Skin.xaml 的交互逻辑
    /// </summary>
    public partial class Skin : Page
    {
        public Skin()
        {
            InitializeComponent();
        }
        //设置page的父窗口，在父窗口绑定
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

        private SolidColorBrush GetColor(string str)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(str)); ;
        }
        //修改配置文件，使得默认主题为当前选中的主题
        private void ChangeDefaultColor(string str)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["Skin.Color.Default"].Value = str;
            switch (str)
            {
                case "#FFFFFF": config.AppSettings.Settings["Default"].Value = "0"; break;
                case "#2D2D30": config.AppSettings.Settings["Default"].Value = "1"; break;
                case "#3A5DD9": config.AppSettings.Settings["Default"].Value = "2"; break;
                case "#C62F2F": config.AppSettings.Settings["Default"].Value = "3"; break;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        //加载配置文件，并修改默认默认启动主题
        private void RgbBlack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.parentWindow.border1.Background = GetColor(ConfigurationManager.AppSettings["Skin.Color.Black"]);
            ChangeDefaultColor(ConfigurationManager.AppSettings["Skin.Color.Black"]);
        }

        private void RgbRed_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.parentWindow.border1.Background = GetColor(ConfigurationManager.AppSettings["Skin.Color.Red"]);
            ChangeDefaultColor(ConfigurationManager.AppSettings["Skin.Color.Red"]);
        }

        private void RgbWhite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.parentWindow.border1.Background = GetColor(ConfigurationManager.AppSettings["Skin.Color.White"]);
            ChangeDefaultColor(ConfigurationManager.AppSettings["Skin.Color.White"]);
        }

        private void RgbBlue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.parentWindow.border1.Background = GetColor(ConfigurationManager.AppSettings["Skin.Color.Blue"]);
            ChangeDefaultColor(ConfigurationManager.AppSettings["Skin.Color.Blue"]);
        }

        private void Page_MouseLeave(object sender, MouseEventArgs e)
        {
            this.parentWindow.cc.Visibility = Visibility.Collapsed;
        }
    }
}
