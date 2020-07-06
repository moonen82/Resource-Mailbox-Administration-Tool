using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace RSMailboxLibrary.Models
{
    public class FullMailboxModel
    {
        public int Id { get; set; }
        public string MailboxName { get; set; }
        public string MailAlias { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MailAddress { get; set; }

        //public MailboxModel MailboxesInfo { get; set; }
        //public UserModel UsersInfo { get; set; }
    }
}
