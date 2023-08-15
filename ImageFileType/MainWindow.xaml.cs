using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Windows;

namespace ImageFileType
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = dialog.FileName;
                if (string.IsNullOrEmpty(path))
                {
                    System.Windows.MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                PathTextBox.Text = path;

                //if(File.Exists(path))
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var format = ImageFormatChecker.GetImageFormat(fs);
                Debug.WriteLine($"check finish, image file type is {format}");
            }
        }

        private void CheckFormat(string dir, string suffix, ImageFormat format)
        {
            var files = Directory.GetFiles(dir, suffix, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                if (ImageFormatChecker.GetImageFormat(fs) != format)
                {
                    Debug.WriteLine($"check error, file path = {file}");
                }
            }
        }
    }
}
