using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SuperClient
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
        private const int SwShowmaximized = 3;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(HandleRef hWnd, HandleRef hWndParent);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);

        public Panel[] Panels;
        public int LastUsed = -1;
        public MainWindow()
        {
            InitializeComponent();
            Panels = new[] {Panel0, Panel1, Panel2};

        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
//            TabControl.SelectedIndex = 0;
//            IntPtr childHandle0 = StartChild(0);
//            // FirstWindowsFormsHost.Child = WrapChildByPanel(childHandle0);
//            PutChildOnPanel(childHandle0, Panel0);
//
//            TabControl.SelectedIndex = 1;
//            IntPtr childHandle1 = StartChild(1);
//            //   SecondWindowsFormsHost.Child = WrapChildByPanel(childHandle1);
//            PutChildOnPanel(childHandle1, Panel1);
//
//            TabControl.SelectedIndex = 2;
//            IntPtr childHandle2 = StartChild(2);
//            //   SecondWindowsFormsHost.Child = WrapChildByPanel(childHandle1);
//            PutChildOnPanel(childHandle2, Panel2);
        }

//        private Panel WrapChildByPanel(IntPtr childHandle)
//        {
//            Panel panel = new Panel();
//            int oldStyle = GetWindowLong(childHandle, GwlStyle);
//            SetWindowLong(childHandle, GwlStyle, (oldStyle | WsChild) & ~WsBorder);
//            var parentHandle = new HandleRef(null, panel.Handle);
//            SetParent(new HandleRef(null, childHandle), parentHandle);
//            var childHandleRef = new HandleRef(null, childHandle);
//            ShowWindowAsync(childHandleRef, SwShowmaximized);
//            return panel;
//        }

        private void PutChildOnPanel(IntPtr childHandle, Panel panel)
        {
            int oldStyle = GetWindowLong(childHandle, GwlStyle);
            SetWindowLong(childHandle, GwlStyle, (oldStyle | WsChild) & ~WsBorder);
            var parentHandle = new HandleRef(null, panel.Handle);
            SetParent(new HandleRef(null, childHandle), parentHandle);
            var childHandleRef = new HandleRef(null, childHandle);
            ShowWindowAsync(childHandleRef, SwShowmaximized);
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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LastUsed++;
            TabControl.SelectedIndex = LastUsed;
            IntPtr childHandle = StartChild(LastUsed);
            PutChildOnPanel(childHandle, Panels[LastUsed]);
        }
    }
}
