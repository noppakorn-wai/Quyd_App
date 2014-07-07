using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;
using Quyd.Resources;

namespace Quyd.Model
{
    class NotificationSet
    {
        //Don't forget pined post
        private List<Notification> notifications;

        public enum type { bid, select, confirm, cancle };

        public NotificationSet()
        {
            notifications = new List<Notification>();
        }

        public async void send(ParseUser toUser, bool isForStore, Post fromPost, type type, bool pined)
        {
            ParseObject notification = new ParseObject("Notification"); ;
            /*if (type == type.bid)
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
            }*/

            notification["toUser"] = toUser;
            notification["forStore"] = isForStore;
            notification["fromPost"] = fromPost;
            notification["type"] = type;
            notification["pined"] = pined;
            notification["read"] = false;

            await notification.SaveAsync();
        }

        public async Task loadUnread()
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

        public async Task loadMore(int limit)
        {
            var user = ParseUser.CurrentUser;

            ParseObject lastNotification;

            if (notifications.Count > 0)
            {
                lastNotification = notifications[notifications.Count - 1].getObject();
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

        public int size()
        {
            return notifications.Count;
        }
    }
    class Notification
    {
        ParseObject notification;

        public Notification(ParseObject notification_t)
        {
            notification = notification_t;
        }

        public NotificationSet.type getType()
        {
            return notification.Get<NotificationSet.type>("type");
        }

        public bool isRead()
        {
            return notification.Get<bool>("read");
        }

        public async Task setRead()
        {
            notification["read"] = true;
            await save();
        }

        public bool getIsForStore()
        {
            return notification.Get<bool>("isForStore");
        }

        public Post getFromPost()
        {
            return notification.Get<Post>("fromPost");
        }

        public ParseObject getObject()
        {
            return notification;
        }

        private async Task save()
        {
            await notification.SaveAsync();
        }
    }
}
