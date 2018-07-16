namespace SuperClient
{
    /// <summary>
    /// Interaction logic for AddServerWindow.xaml
    /// </summary>
    public partial class AddServerWindow
    {
        public string ServerTitleText { get; set; } = "fff";
        public AddServerWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
