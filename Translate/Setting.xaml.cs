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
using System.Windows.Shapes;

namespace Translate
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        public Setting(string str)
        {
            InitializeComponent();
            bd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["Skin.Color.Default"]));
            CheckBox.IsChecked = ConfigurationManager.AppSettings["AutoStart"] == "true" ? true : false;

            //comboBox绑定
            
            List<Topic> topics = new List<Topic>();
            topics.Add(new Topic { ID = 0, Name = "White", Values = "#FFFFFF" });
            topics.Add(new Topic { ID = 1, Name = "Black", Values = "#2D2D30" });
            topics.Add(new Topic { ID = 2, Name = "Blue" , Values = "#3A5DD9" });
            topics.Add(new Topic { ID = 3, Name = "Red"  , Values = "#C62F2F" });  
            comboBox.ItemsSource = topics;
            comboBox.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["Default"]);

            tb.Text = "版本编译时间：" + str;
        }

        MainWindow parentWindow;
        public MainWindow ParentWindow
        {
            get { return parentWindow; }
            set { parentWindow = value; }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox.IsChecked == true)
                ChangeDefaultStart("true");
            else if (CheckBox.IsChecked == false)
            {
                ChangeDefaultStart("false");
            }
        }

        private void ChangeDefaultStart(string str)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["AutoStart"].Value = str;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void CLOSE_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ParentWindow.border1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["Skin.Color.Default"]));
            bd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["Skin.Color.Default"]));
            this.Close();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            int str = (int)comboBox.SelectedValue;
            switch (str)
            {
                case 0: config.AppSettings.Settings["Skin.Color.Default"].Value = "#FFFFFF"; config.AppSettings.Settings["Default"].Value = "0"; break;
                case 1: config.AppSettings.Settings["Skin.Color.Default"].Value = "#2D2D30"; config.AppSettings.Settings["Default"].Value = "1"; break;
                case 2: config.AppSettings.Settings["Skin.Color.Default"].Value = "#3A5DD9"; config.AppSettings.Settings["Default"].Value = "2"; break;
                case 3: config.AppSettings.Settings["Skin.Color.Default"].Value = "#C62F2F"; config.AppSettings.Settings["Default"].Value = "3"; break;      
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
