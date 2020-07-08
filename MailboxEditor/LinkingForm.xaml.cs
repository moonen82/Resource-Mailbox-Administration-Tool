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

            SqliteCrud sql = new SqliteCrud(MainWindow.GetConnectionString());
            var mailboxNameList = sql.GetOnlyMailboxName();
            var userMailList = sql.GetOnlyUserMailAddress();

            mailboxCombo.DisplayMemberPath = "MailboxName";
            mailboxCombo.ItemsSource = mailboxNameList;

            userMailCombo.DisplayMemberPath = "MailAddress";
            userMailCombo.ItemsSource = userMailList;
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

            var textComboMailbox = mailboxCombo.Text;
            var textComboMailAddress = userMailCombo.Text;
            mailbox.MailboxName = textComboMailbox;
            user.MailAddress = textComboMailAddress;
            
            bool checkMailboxId = sql.CheckMailboxId(mailbox);
            bool checkUserId = sql.CheckUserMailAddressId(user);
            if (checkMailboxId || checkUserId)
            {
                MessageBox.Show("Mailbox Name and/or User Mail Address does not exists; please provide existing mailboxes and user mail adresses", "Missing parameters", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                resourceUser.MailboxId = sql.GetMailboxId(mailbox);
                resourceUser.UserId = sql.GetUserIdExtra(user);

                sql.CreateLinking(resourceUser);
                MessageBox.Show("Link created succesfully", "Link created!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        private void UnlinkMailboxUserBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteCrud sql = new SqliteCrud(MainWindow.GetConnectionString());
            MailboxModel mailbox = new MailboxModel();
            UserModel user = new UserModel();
            ResourceUser resourceUser = new ResourceUser();

            var textComboMailbox = mailboxCombo.Text;
            var textComboMailAddress = userMailCombo.Text;

            mailbox.MailboxName = textComboMailbox;
            user.MailAddress = textComboMailAddress;

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
                    resourceUser.UserId = sql.GetUserIdExtra(user);

                    sql.DeleteLinking(resourceUser);
                    MessageBox.Show("Link removed succesfully", "Link removed!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void DeleteMailboxBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteCrud sql = new SqliteCrud(MainWindow.GetConnectionString());
            MailboxModel mailbox = new MailboxModel();
            ResourceUser resourceUser = new ResourceUser();

            if (mailboxCombo.Text == "")
            {
                MessageBox.Show("Combobox is empty, please select a value", "Missing value", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                mailbox.MailboxName = mailboxCombo.Text;
            }
                

            bool checkMailboxId = sql.CheckMailboxId(mailbox);

            if (checkMailboxId)
            {
                MessageBox.Show("Something went wrong - variable not found", "Variable not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                resourceUser.MailboxId = sql.GetMailboxId(mailbox);

                sql.DeleteMailbox(resourceUser);
                MessageBox.Show("Mailbox deleted succesfully", "Deletion completed!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteCrud sql = new SqliteCrud(MainWindow.GetConnectionString());
            UserModel user = new UserModel();
            ResourceUser resourceUser = new ResourceUser();

            if (userMailCombo.Text == "")
            {
                MessageBox.Show("Combobox is empty, please select a value", "Missing value", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                user.MailAddress = userMailCombo.Text;
            }
                

            bool checkUserId = sql.CheckUserMailAddressId(user);

            if (checkUserId)
            {
                MessageBox.Show("Something went wrong - variable not found", "Variable not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                resourceUser.UserId = sql.GetUserIdExtra(user);

                sql.DeleteUser(resourceUser);
                MessageBox.Show("User deleted succesfully", "Deletion completed!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdateMailboxBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteCrud sql = new SqliteCrud(MainWindow.GetConnectionString());
            MailboxModel mailbox = new MailboxModel();

            if (mailboxCombo.Text == "")
            {
                MessageBox.Show("Combobox is empty, please select a value", "Missing value", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                mailbox.MailboxName = mailboxCombo.Text;
            }
                
            bool checkMailboxId = sql.CheckMailboxId(mailbox);

            if (checkMailboxId || string.IsNullOrEmpty(mailboxNameTxt.Text) || string.IsNullOrEmpty(mailboxAliasTxt.Text))
            {
                MessageBox.Show("Something went wrong - not all fields were supplied", "Variable not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                mailbox.Id = sql.GetMailboxId(mailbox);
                mailbox.MailboxName = mailboxNameTxt.Text.ToLower();
                mailbox.MailAlias = mailboxAliasTxt.Text.ToLower();
                mailbox.Password = mailboxPasswordTxt.Text.ToLower();

                sql.UpdateMailbox(mailbox);
                MessageBox.Show("Mailbox updated succesfully", "Updating completed!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdateUserBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteCrud sql = new SqliteCrud(MainWindow.GetConnectionString());
            UserModel user = new UserModel();

            if (userMailCombo.Text == "")
            {
                MessageBox.Show("Combobox is empty, please select a value", "Missing value", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                user.MailAddress = userMailCombo.Text;
            }
            

            bool checkUserId = sql.CheckUserMailAddressId(user);

            if (checkUserId || string.IsNullOrEmpty(userFirstNameTxt.Text) || string.IsNullOrEmpty(userLastNameTxt.Text) || string.IsNullOrEmpty(userMailAddressTxt.Text))
            {
                MessageBox.Show("Something went wrong - not all fields were supplied", "Variable not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                user.Id = sql.GetUserIdExtra(user);
                user.FirstName = userFirstNameTxt.Text.ToLower();
                user.LastName = userLastNameTxt.Text.ToLower();
                user.MailAddress = userMailAddressTxt.Text.ToLower();

                sql.UpdateUser(user);
                MessageBox.Show("User updated succesfully", "Updating completed!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
