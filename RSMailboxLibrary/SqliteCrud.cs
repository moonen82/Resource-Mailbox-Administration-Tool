using DataAccessLibrary;
using RSMailboxLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;

namespace RSMailboxLibrary
{
    public class SqliteCrud
    {
        private readonly string _connectionString;
        private SqliteDataAccess db = new SqliteDataAccess();

        public SqliteCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<FullMailboxModel> GetAllMailboxes()
        {
            string sql = "select m.Id, m.MailboxName, m.MailAlias, m.Password, mu.UserId, u.FirstName, u.LastName, u.MailAddress from Mailbox m " +
                "inner join MailboxUser mu on mu.MailboxId = m.Id inner join User u on u.Id = mu.UserId;";
            return db.LoadData<FullMailboxModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<MailboxModel> GetOnlyMailboxName()
        {
            string sql = "select MailboxName from Mailbox;";
            return db.LoadData<MailboxModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<UserModel> GetOnlyUserMailAddress()
        {
            string sql = "select MailAddress from User;";
            return db.LoadData<UserModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<FullMailboxModel> SearchAllMailboxes(string searchKey)
        {
            var fullList = GetAllMailboxes();
            var resultMailboxName = fullList.Where(m => m.MailboxName.Contains(searchKey) ||
                                                    m.MailAlias.Contains(searchKey) ||
                                                    m.FirstName.Contains(searchKey) ||
                                                    m.LastName.Contains(searchKey) ||
                                                    m.MailAddress.Contains(searchKey));
            return resultMailboxName.ToList();            
        }

        public BindingList<FullMailboxModel> ConvertToBindingList()
        {
            var mailboxList = GetAllMailboxes();
            var mailboxBindingList = new BindingList<FullMailboxModel>(mailboxList);
            return mailboxBindingList;
        }

        public BindingList<FullMailboxModel> ConvertSearchToBindingList(string searchKey)
        {
            var oldList = SearchAllMailboxes(searchKey);
            var newBindingList = new BindingList<FullMailboxModel>(oldList);
            return newBindingList;
        }
       
        public void CreateMailbox(MailboxModel mailbox)
        {
            string sql = "insert into Mailbox (MailboxName, MailAlias, Password) values (@MailboxName, @MailAlias, @Password);";
            db.SaveData(sql, new { mailbox.MailboxName, mailbox.MailAlias, mailbox.Password }, _connectionString);           
        }

        public bool CheckMailboxId(MailboxModel mailbox)
        {            
            string sql = "select Id from Mailbox where MailboxName = @MailboxName;";

            bool mailboxStatus = (db.LoadData<IdCheckModel, dynamic>(sql, new { mailbox.MailboxName }, _connectionString).FirstOrDefault() == null);
            
            return mailboxStatus;
        }

        public int GetMailboxId(MailboxModel mailbox)
        {
            string sql = "select Id from Mailbox where MailboxName = @MailboxName;";
            int mailboxId = 0;

            if (!CheckMailboxId(mailbox))
            {
                mailboxId = db.LoadData<IdLookupModel, dynamic>(sql, new { mailbox.MailboxName }, _connectionString).First().Id;
            }

            return mailboxId;
            
        }

        public void CreateUser(UserModel user)
        {
            string sql = "insert into User (FirstName, LastName, MailAddress) values (@FirstName, @LastName, @MailAddress);";
            db.SaveData(sql, new { user.FirstName, user.LastName, user.MailAddress }, _connectionString);
        }

        public bool CheckUserId(UserModel user)
        {
            string sql = "select Id from User where FirstName = @FirstName and LastName = @LastName and MailAddress = @MailAddress;";

            bool userStatus = (db.LoadData<IdCheckModel, dynamic>(sql, new { user.FirstName, user.LastName, user.MailAddress }, _connectionString).FirstOrDefault() == null);

            return userStatus;
        }

        public int GetUserId(UserModel user)
        {
            string sql = "select Id from User where FirstName = @FirstName and LastName = @LastName and MailAddress = @MailAddress;";
            int userId = 0;

            if (!CheckUserId(user))
            {
                userId = db.LoadData<IdLookupModel, dynamic>(sql, new { user.FirstName, user.LastName, user.MailAddress }, _connectionString).First().Id;
            }
            return userId;
        }

        public bool CheckUserMailAddressId(UserModel user)
        {
            string sql = "select Id from User where MailAddress = @MailAddress;";

            bool userStatus = (db.LoadData<IdCheckModel, dynamic>(sql, new { user.MailAddress }, _connectionString).FirstOrDefault() == null);

            return userStatus;
        }

        public void CreateLinking(ResourceUser resourceUser)
        {
            string sql = "insert into MailboxUser (MailboxId, UserId) values (@MailboxId, @UserId);";
            db.SaveData(sql, new { resourceUser.MailboxId, resourceUser.UserId }, _connectionString);
        }
        
        public void DeleteLinking(ResourceUser resourceUser)
        {
            string sql = "delete from MailboxUser where MailboxId = @MailboxId and UserId = @UserId;";
            db.SaveData(sql, new { resourceUser.MailboxId, resourceUser.UserId }, _connectionString);
        }

        public void DeleteMailbox(ResourceUser mailbox)
        {
            string sql = "delete from MailboxUser where MailboxId = @MailboxId;";
            db.SaveData(sql, new { mailbox.MailboxId }, _connectionString);

            sql = "delete from Mailbox where Id = @MailboxId;";
            db.SaveData(sql, new { mailbox.MailboxId }, _connectionString);
        }

        public void DeleteUser(ResourceUser user)
        {
            string sql = "delete from MailboxUser where UserId = @UserId;";
            db.SaveData(sql, new { user.UserId }, _connectionString);

            sql = "delete from User where Id = @UserId;";
            db.SaveData(sql, new { user.UserId }, _connectionString);
        }

        public void UpdateMailbox(MailboxModel mailbox)
        {
            string sql = "update Mailbox set MailboxName = @MailboxName, MailAlias = @MailAlias, Password = @Password where Id = @Id;";
            db.SaveData(sql, mailbox, _connectionString);
        }

        public void UpdateUser(UserModel user)
        {
            string sql = "update User set FirstName = @Firstname, LastName = @LastName, MailAddress = @MailAddress where Id = @Id;";
            db.SaveData(sql, user, _connectionString);
        }        

        public int GetUserIdExtra(UserModel user)
        {
            string sql = "select Id from User where MailAddress = @MailAddress;";
            int userId = 0;

            if (!CheckUserMailAddressId(user))
            {
                userId = db.LoadData<IdLookupModel, dynamic>(sql, new { user.FirstName, user.LastName, user.MailAddress }, _connectionString).First().Id;
            }
            return userId;
        }
    }
}
