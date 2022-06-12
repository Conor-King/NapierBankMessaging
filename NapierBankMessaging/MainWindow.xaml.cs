using NapierBankMessaging.Pages;

namespace NapierBankMessaging
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new InputPage(this);
        }
    }
}