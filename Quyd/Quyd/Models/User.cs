using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Parse;

using Quyd.Resources;

namespace Quyd.Models
{
    public class User
    {
        public ParseUser data { get; private set; }

        public User()
        {

        }

        public static User CurrentUser
        {
            get
            {
                return (User)ParseUser.CurrentUser;
            }
        }

        public string Name
        {
            get
            {
                return data.Get<string>("name");
            }
            set
            {
                data["name"] = value;
            }
        }

        public string Username
        {
            get
            {
                return data.Username;
            }
        }

        public BitmapImage ProfilePicture
        {
            get
            {
                return new BitmapImage(new Uri("http://graph.facebook.com/" + data.Get<string>("facebookId") + "/picture", UriKind.Absolute));
            }
        }

        public string Email
        {
            get
            {
                return data.Email;
            }
        }

        public string FacebookId
        {
            get
            {
                return data.Get<string>("facebookId");
            }
        }

        public async Task<Store> Store()
        {
            Store result = null;

            try
            {
                result = (Store)(await (data.Get<ParseObject>("store")).FetchIfNeededAsync());
            }
            catch(ParseException ex)
            {
                if(ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    result = null;
                }
            }
            catch(System.Collections.Generic.KeyNotFoundException)
            {
                result = null;
            }

            return result;
        }

        #region async method
        #endregion async method

        # region operator
        public static explicit operator User(ParseUser user)
        {
            User result = new User();

            result.data = user;

            return result;
        }

        public static explicit operator ParseUser(User user)
        {
            return user.data;
        }

        public static bool operator == (User userA, User userB)
        {
            return (userA.data.ObjectId == userB.data.ObjectId);
        }

        public static bool operator ==(ParseUser parseUser, User user)
        {
            return (parseUser.ObjectId == user.data.ObjectId);
        }

        public static bool operator ==(ParseObject parseObject, User user)
        {
            return (parseObject.ObjectId == user.data.ObjectId);
        }

        public override int GetHashCode()
        {
            return data.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator !=(User userA, User userB)
        {
            return (userA.data.ObjectId != userB.data.ObjectId);
        }

        public static bool operator !=(ParseUser parseUser, User user)
        {
            return (parseUser.ObjectId != user.data.ObjectId);
        }

        public static bool operator !=(ParseObject parseObject, User user)
        {
            return (parseObject.ObjectId != user.data.ObjectId);
        }
        # endregion operator
    }
}
