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

        public void AddChild()
        {
            var tabItem = new TabItem() { Header = new ContentControl() };
            TabItems.Add(tabItem);
            var panel = _childStarter.CreatePanel(tabItem);
            Panels.Add(panel);
            LastUsed++;
            SelectedItemIndex = LastUsed;
            var process = _childStarter.StartChild(LastUsed, Servers[LastUsed].Item1, Servers[LastUsed].Item2);
            Processes.Add(process);
            _childStarter.PutChildOnPanel(process, panel);
        }

        public void AddServer()
        {
            var vm = new AddServerViewModel();
            var wm = new WindowManager();
            wm.ShowDialog(vm);
        }
    }
}