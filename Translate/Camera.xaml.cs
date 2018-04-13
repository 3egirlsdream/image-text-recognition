using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFMediaKit.DirectShow.Controls;

namespace Translate
{
    /// <summary>
    /// Camera.xaml 的交互逻辑
    /// </summary>
    public partial class Camera : Window
    {
        public bool isHasCamera = true;
        public Camera()
        {
            InitializeComponent();
        }

       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(MultimediaUtil.VideoInputNames.Length > 0)
            {
                video.VideoCaptureSource = MultimediaUtil.VideoInputNames[0];
            }
            else
            {
                Warning warn = new Warning("没有检测到任何摄像头");
                btn.IsEnabled = false;
                isHasCamera = false;
                warn.ShowDialog();
            }
        }

        public void SaveImage(string fileName)
        {
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)video.ActualWidth, (int)video.ActualHeight, 0, 0, PixelFormats.Default);
            bitmap.Render(video);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(fs);
            fs.Close();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveImage(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\photo.jpg");
            }
            catch
            {
                Title = "识别失败";
            }
        }
    }
}
