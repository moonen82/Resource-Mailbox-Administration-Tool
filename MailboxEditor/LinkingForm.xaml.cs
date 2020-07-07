using RSMailboxLibrary;
using RSMailboxLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MailboxEditor
{
    /// <summary>
    /// Interaction logic for LinkingForm.xaml
    /// </summary>
    public partial class LinkingForm : Window
    {
        public LinkingForm()
        {
            InitializeComponent();
        }

        private void MainFormBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LinkMailboxUserBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteCrud sql = new SqliteCrud(MainWindow.GetConnectionString());
            MailboxModel mailbox = new MailboxModel();
            UserModel user = new UserModel();
            ResourceUser resourceUser = new ResourceUser();

            mailbox.MailboxName = mailboxUpdateText.Text.ToLower();
            user.MailAddress = userUpdateText.Text.ToLower();

            if (string.IsNullOrEmpty(mailbox.MailboxName) || string.IsNullOrEmpty(user.MailAddress))
            {
                MessageBox.Show("Both fields are empty; both need to have a value", "Field(s) empty", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                bool checkMailboxId = sql.CheckMailboxId(mailbox);
                bool checkUserId = sql.CheckUserMailAddressId(user);
                if (checkMailboxId || checkUserId)
                {
                    MessageBox.Show("Mailbox Name and/or User Mail Address does not exists; please provide existing mailboxes and user mail adresses", "Missing parameters", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    resourceUser.MailboxId = sql.GetMailboxId(mailbox);
                    resourceUser.UserId = sql.GetUserId(user);

                    sql.CreateLinking(resourceUser);
                    MessageBox.Show("Link created succesfully", "Link created!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void UnlinkMailboxUserBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteCrud sql = new SqliteCrud(MainWindow.GetConnectionString());
            MailboxModel mailbox = new MailboxModel();
            UserModel user = new UserModel();
            ResourceUser resourceUser = new ResourceUser();

            mailbox.MailboxName = mailboxUpdateText.Text.ToLower();
            user.MailAddress = userUpdateText.Text.ToLower();

            if (string.IsNullOrEmpty(mailbox.MailboxName) || string.IsNullOrEmpty(user.MailAddress))
            {
                MessageBox.Show("Both fields are empty; both need to have a value", "Field(s) empty", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                bool checkMailboxId = sql.CheckMailboxId(mailbox);
                bool checkUserId = sql.CheckUserMailAddressId(user);
                if (checkMailboxId || checkUserId)
                {
                    MessageBox.Show("Mailbox Name and/or User Mail Address does not exists; please provide existing mailboxes and user mail adresses", "Missing parameters", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    resourceUser.MailboxId = sql.GetMailboxId(mailbox);
                    resourceUser.UserId = sql.GetUserId(user);

                    sql.DeleteLinking(resourceUser);
                }
            }
        }
    
    }
}
