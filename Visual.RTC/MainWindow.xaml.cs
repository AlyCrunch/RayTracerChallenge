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
            switch((sender as Button).Name)
            {
                case "Chapter1" : new PIT01().Show(); break;
                case "Chapter2": new PIT02().Show(); break;
                case "Chapter4": new PIT04().Show(); break;
                default: return;
            }
        }
    }
}
