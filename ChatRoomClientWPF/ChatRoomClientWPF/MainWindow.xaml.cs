using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatRoomClientWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    Login login = new Login();
    AdminUser adminUser = new AdminUser();
    RegularUser regularUser = new RegularUser();

    public MainWindow()
    {
        InitializeComponent();
        this.contentControl.Content = login;

    }

    public void Login(string username, string password)
    {

        this.contentControl.Content = adminUser;
    }

    public void Logout()
    {
        this.contentControl.Content = login;
    }

    public void SendMessage(string message)
    {

    }
}