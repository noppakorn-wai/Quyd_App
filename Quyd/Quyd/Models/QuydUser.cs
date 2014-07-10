using Facebook;
using Facebook.Client;
using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Quyd.Models
{
    internal class QuydUser : ParseUser
    {
        public static new ParseUser CurrentUser
        {
            get
            {
                return ParseUser.CurrentUser;
            }
        }
    }
}