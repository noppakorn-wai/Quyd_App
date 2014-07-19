using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;

namespace Quyd.Models
{
    public class Item
    {
        static public IEnumerable<Item> defaultItemList;

        public ParseObject ItemData { get; protected set; }

        public Item()
        {
        }

        public Item(ParseObject item)
        {
            this.ItemData = item;
        }

        public virtual async Task saveAsync()
        {
            await Task.FromResult(true);
        }

        #region get set method (normalObject)
        public virtual string Type
        {
            get
            {
                //return ItemData.Get<string>("type");
                return getItemBy(ItemId).ItemData.Get<string>("type");
            }
        }

        public virtual string Name
        {
            get
            {
                //return ItemData.Get<string>("name"); 
                return getItemBy(ItemId).ItemData.Get<string>("name");
            }
        }

        public virtual string Description
        {
            get
            {
                //return ItemData.Get<string>("description"); 
                return getItemBy(ItemId).ItemData.Get<string>("description");
            }
        }

        public virtual string Material
        {
            get
            {
                //return ItemData.Get<string>("material");
                return getItemBy(ItemId).ItemData.Get<string>("material");
            }
        }

        public virtual string MaterialType
        {
            get
            {
                //return ItemData.Get<string>("materialType");
                return getItemBy(ItemId).ItemData.Get<string>("materialType");
            }
        }

        public virtual string Icon
        {
            get
            {
                //return ItemData.Get<string>("icon");
                return getItemBy(ItemId).ItemData.Get<string>("icon");
            }
        }

        public virtual string ItemId
        {
            get
            {
                return ItemData.ObjectId;
            }
        }
        
        public static void setDefaultItemList(IEnumerable<ParseObject> _defaultItemList)
        {
            defaultItemList = _defaultItemList.Select(item => new Item(item));
        }
        #endregion

        #region static get set method (defaultObjectList support)
        public static Item getItemBy(string itemId)
        {
            return defaultItemList.Where(item => item.ItemId == itemId).First<Item>();
        }
        #endregion

        # region operator
        public static explicit operator Item(ParseObject parseObject)
        {
            Item result = new Item();

            result.ItemData = parseObject;

            return result;
        }

        public static explicit operator ParseObject(Item item)
        {
            return item.ItemData;
        }

        public static bool operator ==(ParseObject parseObject, Item item)
        {
            return (parseObject.ObjectId == item.ItemId);
        }

        public override int GetHashCode()
        {
            return ItemData.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Item objAsItem = obj as Item;
            if (objAsItem == null) return false;
            return ItemData.ObjectId == objAsItem.ItemData.ObjectId;
        }

        public static bool operator !=(ParseObject parseObject, Item item)
        {
            return (parseObject.ObjectId != item.ItemId);
        }
        #endregion operator
    }

    public class PostItem : Item, Quantifiable
    {
        public ParseObject postItemData { get; private set; }

        public PostItem()
        {
        }

        public PostItem(ParseObject postItem)
        {
            this.postItemData = postItem;
            ItemData = new ParseObject("Item");
            ItemData.ObjectId = postItem.Get<ParseObject>("item").ObjectId;
        }

        public override sealed async Task saveAsync()
        {
            await postItemData.SaveAsync();
        }

        #region get set method (simple)

        public async Task<Post> getPostAsync()
        {
            return new Post(await postItemData.Get<ParseObject>("post").FetchIfNeededAsync<ParseObject>());
        }

        public Post Post
        {
            set
            {
                postItemData["post"] = value;
            }
        }

        #endregion

        #region get set method (Interface)

        public double Quantity
        {
            get
            {
                return postItemData.Get<double>("quantity");
            }

            set
            {
                postItemData["quantity"] = value;
            }
        }

        #endregion
    }

    public class StoreItem : Item, Priceable
    {
        public ParseObject storeItemData { get; private set; }

        public StoreItem()
        {
        }

        public StoreItem(ParseObject storeItem)
        {
            this.storeItemData = storeItem;
        }

        public override sealed async Task saveAsync()
        {
            await storeItemData.SaveAsync();
        }

        public double Price
        {
            get
            {
                return storeItemData.Get<double>("price");
            }

            set
            {
                storeItemData["price"] = value;
            }
        }
    }

    #region interface
    interface Quantifiable
    {
        double Quantity
        {
            get;
            set;
        }
    }

    interface Priceable
    {
        double Price
        {
            get;
            set;
        }
    }
    #endregion
}