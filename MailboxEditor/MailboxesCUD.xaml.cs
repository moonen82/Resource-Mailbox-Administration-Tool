﻿using RSMailboxLibrary;
using RSMailboxLibrary.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MailboxesCUD.xaml
    /// </summary>
    public partial class MailboxesCUD : Window
    {
        public MailboxesCUD()
        {
            InitializeComponent();
        }

        private void returnMainFormBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LinkBtn_Click(object sender, RoutedEventArgs e)
        {
            LinkingForm linking = new LinkingForm();

            linking.Show();
        }

        private void createMailboxBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteCrud sql = new SqliteCrud(MainWindow.GetConnectionString());
            MailboxModel mailbox = new MailboxModel();
           
            mailbox.MailboxName = mailboxNameText.Text.ToLower();
            mailbox.MailAlias = mailboxAliasText.Text.ToLower();
            mailbox.Password = mailboxPasswordText.Text;           

            if (!string.IsNullOrEmpty(mailbox.MailboxName))
            {
                bool mailboxStatus = sql.CheckMailboxId(mailbox);
                if (mailboxStatus)
                {
                    sql.CreateMailbox(mailbox);
                    MessageBox.Show("Mailbox created succesfully", "Mailbox created!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Mailbox already exists, exiting", "Already exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Mailbox Name field is empty; needs a value", "Error, no value detected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void createUserBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteCrud sql = new SqliteCrud(MainWindow.GetConnectionString());
            UserModel user = new UserModel();

            user.FirstName = userFirstNameText.Text.ToLower();
            user.LastName = userLastNameText.Text.ToLower();
            user.MailAddress = userMailAddressText.Text.ToLower();

            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) || string.IsNullOrEmpty(user.MailAddress))
            {
                MessageBox.Show("User fields are empty; all need a value", "Error; no value detected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                bool userStatus = sql.CheckUserId(user);
                if (userStatus)
                {
                    sql.CreateUser(user);
                    MessageBox.Show("User created succesfully", "User created!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("User already exists, exiting", "Already exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void UpdateDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            LinkingForm linking = new LinkingForm();

            linking.Show();
        }        
    }
}
