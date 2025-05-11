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

namespace ChatRoomClientWPF
{
    /// <summary>
    /// Interaction logic for AdminUser.xaml
    /// </summary>
    public partial class AdminUser : UserControl
    {
        public AdminUser()
        {
            InitializeComponent();
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.SendMessage(this.MessageBox.Text);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Logout();
        }

        private void ChatListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UsersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CreateNewUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSelectedUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSelectedMessage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
