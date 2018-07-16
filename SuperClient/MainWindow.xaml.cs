using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using Panel = System.Windows.Forms.Panel;
using System.Windows.Forms.Integration;

namespace SuperClient
{
    public class SomeClass
    {
        public string Text { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        const string ClientFilename = @"c:\VsGitProjects\SuperClientE\LittleClient\bin\Debug\LittleClient";
        //        const string ClientFilename = @"c:\VsGitProjects\Fibertest\Client\WpfClient\bin\Debug\Iit.Fibertest.Client.exe";

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

        public List<Panel> Panels;
        public List<Tuple<string, string>> Servers;
        public int LastUsed = -1;
        public List<Process> Processes;

        public MainWindow()
        {
            InitializeComponent();
            Panels = new List<Panel>();
            Servers = new List<Tuple<string, string>>();
            Servers.Add(new Tuple<string, string>("192.168.96.21", "11840"));
            Servers.Add(new Tuple<string, string>("172.16.4.105", "11840"));
            Servers.Add(new Tuple<string, string>("172.16.4.115", "11840"));
            Servers.Add(new Tuple<string, string>("172.16.4.100", "11840"));
            Processes = new List<Process>();
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }


        private void PutChildOnPanel(IntPtr childHandle, Panel panel)
        {
            int oldStyle = GetWindowLong(childHandle, GwlStyle);
            SetWindowLong(childHandle, GwlStyle, (oldStyle | WsChild) & ~WsBorder);
            var parentHandle = new HandleRef(null, panel.Handle);
            SetParent(new HandleRef(null, childHandle), parentHandle);
            var childHandleRef = new HandleRef(null, childHandle);
            ShowWindowAsync(childHandleRef, SwShowmaximized);
        }

        private IntPtr StartChild(int number, string serverIp, string serverPort)
        {
            var process = new Process
            {
                StartInfo = {
                    FileName = ClientFilename,
                    Arguments = $"{number} superclient superclient {serverIp} {serverPort}"
                }
            };
            process.Start();
            //            var pause = number == 3 ? 45 : 10;
            var pause = 2;
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(pause));
            Processes.Add(process);
            return process.MainWindowHandle;
        }

        private void CreatePanel()
        {
            var tabItem = new TabItem() { Header = new ContentControl() };
            TabControl.Items.Add(tabItem);

            var windowsFormsHost = new WindowsFormsHost();
            tabItem.Content = windowsFormsHost;

            Panel panel = new Panel();
            Panels.Add(panel);
            windowsFormsHost.Child = panel;
        }
        private void AddChild(object sender, System.Windows.RoutedEventArgs e)
        {
            CreatePanel();
            LastUsed++;
            TabControl.SelectedIndex = LastUsed;
            IntPtr childHandle = StartChild(LastUsed, Servers[LastUsed].Item1, Servers[LastUsed].Item2);
            PutChildOnPanel(childHandle, Panels[LastUsed]);
        }

        private void AddServer(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new AddServerWindow();
            window.Show();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            foreach (var process in Processes)
            {
                process.CloseMainWindow();

            }
        }
    }
}
