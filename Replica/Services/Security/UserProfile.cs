using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace R.Security
{
    /// <summary>
    ///  User profile
    /// </summary>
    public class UserProfile
    {
        public long ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> Groups { get; set; } = new List<string>();
        //public Dictionary<string, object> UserProperties { get; set; } = new Dictionary<string, object>();
        public UserProfile()
        {
        }
        public UserProfile(object item)
        {
            if (item != null)
            {
                //this.ID = item.ID;
                //this.Login = item["login"].ToString();
                //this.Roles = new List<string>(item["roles"].ToString().Split(','));
                //this.Groups = new List<string>(item["groups"].ToString().Split(','));
            }
        }
    }

}
