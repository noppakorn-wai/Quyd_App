using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;

namespace Quyd.Models
{
    class ItemList
    {
        public IList<Item> itemList { get; private set; }
        public bool imutable { get; private set; }

        public ItemList()
        {
            itemList = new List<Item>();
            imutable = false;
        }

        public ItemList(Store store)
        {
            itemList = new List<Item>();
            imutable = false;

            if(store.getOwnerId().Equals(ParseUser.CurrentUser))
            {
                imutable = true;
            }
        }

        public ItemList(Post post)
        {
            itemList = new List<Item>();
            imutable = false;
        }
    }

    class Item
    {
        public ParseObject item { get; protected set; }

        public Item()
        {
            item = null;
        }

        public Item(ParseObject item)
        {
            this.item = item;
        }

        public string getType()
        {
            return item.Get<string>("type");
        }

        public string getName()
        {
            return item.Get<string>("name");
        }

        public string getDescription()
        {
            return item.Get<string>("description");
        }

        public string getMaterial()
        {
            return item.Get<string>("material");
        }

        public string getMaterialType()
        {
            return item.Get<string>("materialType");
        }

        public string getIcon()
        {
            return item.Get<string>("icon");
        }
    }

    class UserItem : Item, Quantifiable
    {
        public ParseObject userItem { get; private set; }

        public UserItem()
        {

        }

        public double getQuantity()
        {
            return userItem.Get<double>("quantity");
        }

    }

    class StoreItem : Item, Priceable
    {
        public ParseObject storeItem { get; private set; }

        public StoreItem()
        {

        }

        public double getPrice()
        {
            return storeItem.Get<double>("price");
        }
    }

#region interface
    interface Quantifiable
    {
        double getQuantity();
    }

    interface Priceable
    {
        double getPrice();
    }
#endregion
}
