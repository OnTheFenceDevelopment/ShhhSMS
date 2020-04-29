using ShhhSMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShhhSMS.Services
{
    public interface IContactService
    {
        List<Contact> GetContacts();
        bool SaveContact(Contact contact);
    }
}