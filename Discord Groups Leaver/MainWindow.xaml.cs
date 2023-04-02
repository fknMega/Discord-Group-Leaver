using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Discord_Groups_Leaver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Label UserNameLabel => UserName;
        public ListBox ListControl => List;

        public MainWindow()
        {
            InitializeComponent();
            Discord_Groups_Leaver.ConsoleManager.Show();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            placeholderText.Visibility = string.IsNullOrEmpty(passwordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var token = passwordBox.Password;
            Console.WriteLine(token);
            Program.Login(token, this);
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            Program.Logout();
        }

        private void scrape_Click(object sender, RoutedEventArgs e)
        {
            Program.ListControl.ScrapeGroups(this);
        }

        private void Leave_Click(object sender, RoutedEventArgs e)
        {
            Program.ListControl.LeaveSelectedGroups(this);
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            List.SelectAll();
        }
    }

}
