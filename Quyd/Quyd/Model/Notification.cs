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

        public Notification()
        {
        }

        public async void send(ParseUser toUser, bool isForStore, Post fromPost, string type)
        {
            var notification = new ParseObject("Notification");

            notification["toUser"] = toUser;
            notification["forStore"] = isForStore;
            notification["fromPost"] = fromPost;
            notification["type"] = type;

            await notification.SaveAsync();
        }

        public async void receive()
        {
            var user = ParseUser.CurrentUser;

            var query = from notification in ParseObject.GetQuery("Notification")
                        where notification["receiver"] == user
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
