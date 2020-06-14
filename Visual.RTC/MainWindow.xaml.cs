using System.Windows;
using System.Windows.Controls;

namespace Visual.RTC
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Chapter_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "Chapter1": new PIT01().Show(); break;
                case "Chapter2": new PIT02().Show(); break;
                case "Chapter4": new PIT04().Show(); break;
                case "Chapter5": new PIT05().Show(); break;
                case "Chapter6": new PIT06().Show(); break;
                case "Chapter7": new PIT07().Show(); break;
                case "Chapter9": new PIT09().Show(); break;
                case "Chapter10": new PIT10().Show(); break;
                case "Chapter11": new PIT11().Show(); break;
                case "Chapter12": new PIT12().Show(); break;
                case "Chapter14": new PIT14().Show(); break;
                case "Chapter15": new PIT15().Show(); break;
                case "Chapter17": new PIT17().Show(); break;
                default: return;
            }
        }

        private void Final_Click(object sender, RoutedEventArgs e)
        {
            new Final().Show();
        }
    }
}
