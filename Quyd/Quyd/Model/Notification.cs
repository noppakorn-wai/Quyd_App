using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;

namespace Quyd.Model
{
    class Notification
    {
        private IEnumerable<ParseObject> notifications;

        enum type { bid, select, confirm, cancle };

        public Notification()
        {
        }

        public async void send(ParseUser toUser, bool isForStore, Post fromPost, type type)
        {
            ParseObject notification = new ParseObject("Notification");;
            if (type == type.bid)
            {
                var query = from notification_t in ParseObject.GetQuery("Notification")
                            where notification_t.Get<ParseUser>("receiver").Equals(toUser)
                            where notification_t.Get<bool>("read") == false
                            where notification_t.Get<int>("type") == (int)type.bid
                            orderby notification_t.UpdatedAt descending
                            select notification_t;
                try
                {
                    notification = await query.FirstAsync();
                }
                catch (ParseException ex)
                {
                    if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                    {
                        //if not found bid notification for this user don't update
                    }
                }
            }

            notification["toUser"] = toUser;
            notification["forStore"] = isForStore;
            notification["fromPost"] = fromPost;
            notification["type"] = type;
            notification["read"] = false;

            await notification.SaveAsync();
        }

        public async void receive()
        {
            var user = ParseUser.CurrentUser;

            var query = from notification in ParseObject.GetQuery("Notification")
                        where notification.Get<ParseUser>("receiver").Equals(user)
                        where notification.Get<bool>("read") == false
                        select notification;

            notifications = await query.FindAsync();
        }

        public int size()
        {
            return notifications.Count<ParseObject>();
        }

        public string getType(int i)
        {
            return notifications.ElementAt<ParseObject>(i).Get<string>("type");
        }

        public bool getIsForStore(int i)
        {
            return notifications.ElementAt<ParseObject>(i).Get<bool>("isForStore");
        }

        public Post getFromPost(int i)
        {
            return notifications.ElementAt<ParseObject>(i).Get<Post>("fromPost");
        }
    }
}
