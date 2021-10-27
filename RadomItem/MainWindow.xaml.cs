using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace RadomItem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary> 
    public partial class MainWindow : Window
    {
        private List<string> list = new List<string>();
        private bool isGo;
        private Thread thread;
        private int now;
        private bool isRun = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isGo)
            {
                isGo = true;
                Button1.Content = "停止";
            }
            else
            {
                isGo = false;
                int a = new Random(Guid.NewGuid().GetHashCode()).Next(0, list.Count - 1);
                Text1.Content = list[a];
                Button1.Content = "开始";
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("list.txt"))
            {
                MessageBox.Show("没有名单文件list.txt，请新建后再启动", "无抽奖名单");
                Application.Current.Shutdown();
                return;
            }
            var data  = File.ReadAllText("list.txt");
            var temp = data.Split('\n');
            list.AddRange(temp);
            thread = new Thread(Go);
            thread.Start();
        }

        private void Go()
        {
            while (isRun)
            {
                if (isGo)
                {
                    now++;
                    if (now >= list.Count)
                    {
                        now = 0;
                    }
                    Dispatcher.Invoke(() =>
                    {
                        Text1.Content = list[now];
                    });
                }
                Thread.Sleep(50);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            isRun = false;
            isGo = false;
            thread.Join();
        }
    }
}
