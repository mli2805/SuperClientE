using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using Panel = System.Windows.Forms.Panel;
using System;
using System.Runtime.InteropServices;

namespace SuperClientCa 
{
    public class ShellViewModel : Screen, IShell
    {
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
        
        public ObservableCollection<TabItem> TabItems { get; set; } = new ObservableCollection<TabItem>();
        private int _selectedItemIndex;

        public int SelectedItemIndex
        {
            get => _selectedItemIndex;
          
            set   
            {
                if (_selectedItemIndex == value) return;
                _selectedItemIndex = value;
                NotifyOfPropertyChange();
            } 
        }
        public List<Panel> Panels = new List<Panel>();
        public List<Tuple<string, string>> Servers;
        public int LastUsed = -1;
        public List<Process> Processes = new List<Process>();

        private ChildStarter _childStarter;

        public ShellViewModel()
        {
            Servers = new List<Tuple<string, string>>();
            Servers.Add(new Tuple<string, string>("192.168.96.21", "11840"));
            Servers.Add(new Tuple<string, string>("172.16.4.105", "11840"));
            Servers.Add(new Tuple<string, string>("172.16.4.115", "11840"));
            Servers.Add(new Tuple<string, string>("172.16.4.100", "11840"));
            
            _childStarter = new ChildStarter();
        }


        private Panel CreatePanel()
        {
            var tabItem = new TabItem() { Header = new ContentControl() };
            TabItems.Add(tabItem);

            var windowsFormsHost = new WindowsFormsHost();
            tabItem.Content = windowsFormsHost;

            Panel panel = new Panel();
            Panels.Add(panel);
            windowsFormsHost.Child = panel;

            return panel;
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

        public void AddChild()
        {
            var panel = CreatePanel();
            LastUsed++;
            SelectedItemIndex = LastUsed;
            var process = _childStarter.StartChild(LastUsed, Servers[LastUsed].Item1, Servers[LastUsed].Item2);
            Processes.Add(process);
            IntPtr childHandle = process.MainWindowHandle;
            PutChildOnPanel(childHandle, panel);
        }

        public void AddServer()
        {
            var vm = new AddServerViewModel();
            var wm = new WindowManager();
            wm.ShowDialog(vm);
        }
    }
}