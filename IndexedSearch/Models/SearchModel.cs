using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndexedSearch.Models
{
    public class PhoneBookEntry
    {
        private int contact_id;
        private string contact_name;
        private string phone;

        public int ContactID { 
            get { return (!contact_id.Equals(-1)) ? contact_id : -1; } 
            set { contact_id = (!value.Equals(-1)) ? value : -1; } }
        public string ContactName { 
            get { return (!contact_name.Equals("")) ? contact_name : ""; } 
            set { contact_name = (!value.Equals("")) ? value : ""; } }
        public string Phone
        {
            get { return (!phone.Equals("")) ? phone : ""; }
            set { phone = (!value.Equals("")) ? value : ""; }
        }
    }

    public class SearchModel
    {
        private List<PhoneBookEntry> phone_list = null;
        public SearchModel()
        {
            if (phone_list == null)
                phone_list = new List<PhoneBookEntry>();
        }

        public List<PhoneBookEntry> PhoneList 
            { get { return (phone_list != null) ? phone_list : null; } }
    }
}