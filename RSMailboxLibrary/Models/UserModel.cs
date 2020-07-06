﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RSMailboxLibrary.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MailAddress { get; set; }
    }
}
