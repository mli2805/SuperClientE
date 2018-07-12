using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace SuperClient1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        const string ClientFilename = @"c:\VsGitProjects\SuperClientE\LittleClient\bin\Debug\LittleClient";

        private const int GwlStyle = -16;
        private const int WsBorder = 0x00800000;
        private const int WsChild = 0x40000000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(HandleRef hWnd, HandleRef hWndParent);

     
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            IntPtr childHandle = StartChild(1);

             FirstWay(childHandle);
        }

        private void FirstWay(IntPtr childHandle)
        {
            int oldStyle = GetWindowLong(childHandle, GwlStyle);
            SetWindowLong(childHandle, GwlStyle, (oldStyle | WsChild) & ~WsBorder);
            var parentHandle = new HandleRef(null, new WindowInteropHelper(this).Handle); // the whole window
            SetParent(new HandleRef(null, childHandle), parentHandle);
        }

        private static IntPtr StartChild(int number)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = ClientFilename,
                    Arguments = $"child_#{number}"
                }
            };
            process.Start();
            System.Threading.Thread.Sleep(1000);
            return process.MainWindowHandle;
        }

    }
}
