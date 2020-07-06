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
                "inner join MailboxUser mu on mu.MailboxId = m.Id inner join User u on mu.UserId = u.Id;";
            return db.LoadData<FullMailboxModel, dynamic>(sql, new { }, _connectionString);
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
    }
}
