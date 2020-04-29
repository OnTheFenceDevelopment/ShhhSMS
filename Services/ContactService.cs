using Newtonsoft.Json;
using ShhhSMS.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShhhSMS.Services
{
    public class ContactService : IContactService
    {
        private string _backingFilePath => Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "contacts.json");

        public List<Contact> GetContacts()
        {
            if (ContactFileExists() == false)
                return new List<Contact>();

            // Read existing Json File
            var contactJson = File.ReadAllText(_backingFilePath);
            var contactList = JsonConvert.DeserializeObject<List<Contact>>(contactJson);
            return contactList;
        }

        public bool SaveContact(Contact contact)
        {
            try
            {
                // Will assume that checks for existing contact were made elsewhere (Contact Exists?)
                List<Contact> contacts;

                // Read Existing Contact Store
                if (ContactFileExists())
                {
                    // TODO: Read into local collection
                    var foo = File.ReadAllText(_backingFilePath);
                    contacts = JsonConvert.DeserializeObject<List<Contact>>(foo);
                }
                else
                {
                    contacts = new List<Contact>();
                }

                // TODO: Remove Existing Contact?

                // Add New Contact
                contacts.Add(contact);

                // Save to File
                File.WriteAllText(_backingFilePath, JsonConvert.SerializeObject(contacts));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool ContactFileExists()
        {
            return _backingFilePath != null && File.Exists(_backingFilePath);
        }
    }
}