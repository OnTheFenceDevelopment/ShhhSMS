using ShhhSMS.Models;
using System.Collections.Generic;

namespace ShhhSMS.Services
{
    public interface IContactService
    {
        List<Contact> GetContacts();
        bool SaveContact(Contact contact);
    }
}