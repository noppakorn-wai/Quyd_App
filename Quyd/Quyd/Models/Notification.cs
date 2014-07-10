using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;
using Quyd.Resources;

namespace Quyd.Models
{
    public enum notificationType {bid, select, confirm, cancle };

     public class NotificationList
    {
        //Don't forget pined post
        private List<Notification> notifications;

        public NotificationList()
        {
            notifications = new List<Notification>();
        }

        public async Task loadUnreadAsync()
        {
            var user = ParseUser.CurrentUser;

            var query = from notification in ParseObject.GetQuery("Notification")
                        where notification.Get<ParseUser>("receiver").Equals(user)
                        where notification.Get<bool>("read") == false
                        where notification.Get<bool>("pined") == false
                        orderby notification.UpdatedAt ascending
                        select notification;

            var notifications_t = await query.FindAsync();

            foreach (var notification in notifications_t)
            {
                //put group function here
                notifications.Insert(0, new Notification(notification));
            }
        }

        public async Task loadMoreAsync(int limit)
        {
            var user = ParseUser.CurrentUser;

            ParseObject lastNotification;

            if (notifications.Count > 0)
            {
                lastNotification = notifications[notifications.Count - 1].Object;
                var query = from notification in ParseObject.GetQuery("Notification").Limit(limit)
                            where notification.Get<ParseUser>("receiver").Equals(user)
                            where notification.UpdatedAt > lastNotification.UpdatedAt
                            where notification.Get<bool>("pined") == false
                            orderby notification.UpdatedAt descending
                            select notification;
                try
                {
                    var notifications_t = await query.FindAsync();

                    //put group function here
                    foreach (var notification in notifications_t)
                    {
                        notifications.Add(new Notification(notification));
                    }
                }
                catch(ParseException ex)
                {
                    //not handler ""no more space exception
                    if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                    {
                        //no older notification found
                    }
                }
            }
        }

        public Notification get(int i)
        {
            return notifications[i];
        }

        public int Size
        {
            get
            {
                return notifications.Count;
            }
        }
    }

    public class Notification
    {
        ParseObject notification;
        Post post;

        public Notification()
        {
            notification = null;
            post = null;
        }

        public Notification(ParseObject notification_t)
        {
            notification = notification_t;
            post = null;
        }

        public async Task setReadAsync()
        {
            notification["read"] = true;
            await saveAsync();
        }

        public async Task<Post> getPostAsync()
        {
            if (post == null)
            {
                await post.loadAsync(notification.Get<string>("fromPost"));
            }
            return post;
        }

        private async Task saveAsync()
        {
            await notification.SaveAsync();
        }

        public async Task sendAsync(ParseUser toUser, bool isForStore, Post fromPost, notificationType type, bool pined)
        {
            ParseObject notification = new ParseObject("Notification"); ;

            notification["toUser"] = toUser;
            notification["forStore"] = isForStore;
            notification["fromPost"] = fromPost;
            notification["type"] = type;
            notification["pined"] = pined;
            notification["read"] = false;

            await notification.SaveAsync();
        }

        public notificationType Type
        {
            get
            {
                return notification.Get<notificationType>("type");
            }
        }

        public bool isRead()
        {
            return notification.Get<bool>("read");
        }

        public bool IsForStore
        {
            get
            {
                return notification.Get<bool>("isForStore");
            }
        }

        public ParseObject Object
        {
            get
            {
                return notification;
            }
        }

    }
}
