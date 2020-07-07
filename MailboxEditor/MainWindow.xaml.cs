using Microsoft.Extensions.Configuration;
using RSMailboxLibrary;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace MailboxEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = "";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteCrud sql = new SqliteCrud(GetConnectionString());

            if (string.IsNullOrWhiteSpace(searchText.Text))
            {
                var mailboxesAll = sql.ConvertToBindingList();
                mailboxList.ItemsSource = mailboxesAll;
            } 
            else
            {
                var searchMailboxResult = sql.ConvertSearchToBindingList(searchText.Text);
                mailboxList.ItemsSource = searchMailboxResult;
            }
        }

        private void crudMailboxBtn_Click(object sender, RoutedEventArgs e)
        {
            MailboxesCUD mailboxesCUD = new MailboxesCUD();

            mailboxesCUD.Show();
        }
    }
}
