using System;
using System.Collections.Generic;
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
        public string color;
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

        private SolidColorBrush GetRgb(byte r, byte g, byte b)
        {
            return new SolidColorBrush(System.Windows.Media.Color.FromRgb(r, g, b));
        }

        private void RgbBlack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //string tootip = this.parentWindow.border1.ToolTip.ToString();
            this.parentWindow.border1.Background = GetRgb(45, 45, 48);
            color = "#2D2D30";
        }

        private void RgbRed_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //string tootip = this.parentWindow.border1.ToolTip.ToString();
            this.parentWindow.border1.Background = GetRgb(198, 47, 47);
            color = "#C62F2F";
        }

        private void RgbWhite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.parentWindow.border1.Background = GetRgb(255, 255, 255);
        }

        private void RgbBlue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.parentWindow.border1.Background = GetRgb(58, 93, 217);
        }
    }
}
