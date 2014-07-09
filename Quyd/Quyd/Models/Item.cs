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
        }

        public async Task loadItemsAsync()
        {
            itemList.Clear();
            imutable = true;

            try
            {
                IEnumerable<ParseObject> items_t = await ParseObject.GetQuery("Item").FindAsync();

                foreach (var item in items_t)
                {
                    itemList.Add(new Item(item));
                }
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no data found
                }
            }
        }

        public async Task loadUserItemsAsync(Post post)
        {
            itemList.Clear();
            imutable = true;

            if (post.PostBy.Equals(ParseUser.CurrentUser))
            {
                imutable = false;
            }

            var query = from userItem in ParseObject.GetQuery("UserItem").Include("item")
                        where userItem.ObjectId == post.Object.ObjectId
                        select userItem;
            try
            {
                var userItems = await query.FindAsync();
                foreach (var userItem in userItems)
                {
                    itemList.Add(new UserItem(userItem));
                }
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no data found
                }
            }
        }

        public async Task loadStoreItemsAsync(Store store, DateTime atDateTime = new DateTime())
        {
            itemList.Clear();
            imutable = true;

            if (store.OwnerId.Equals(ParseUser.CurrentUser))
            {
                imutable = false;
            }

            try
            {
                IEnumerable<ParseObject> items = await ParseObject.GetQuery("Item").FindAsync();


                foreach (var item in items)
                {
                    ParseObject storeItem_result = null;
                    try
                    {
                        var query = from storeItem in ParseObject.GetQuery("StoreItem")
                                    where storeItem.ObjectId == store.Object.ObjectId
                                    where storeItem["item"] == item
                                    where storeItem.CreatedAt <= atDateTime
                                    where storeItem.Get<DateTime>("validTo") >= atDateTime
                                    select storeItem;

                        storeItem_result = await query.FirstAsync();
                    }
                    catch (ParseException ex)
                    {
                        if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                        {
                            storeItem_result = new ParseObject("StoreItem");
                            storeItem_result["store"] = store.Object;
                            storeItem_result["item"] = item;
                            storeItem_result["price"] = 0;
                        }
                    }

                    itemList.Add(new StoreItem(storeItem_result));
                }
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no data found
                }
            }
        }

        public async Task createStoreItemsAsync(Store store)
        {
            itemList.Clear();
            imutable = true;

            try
            {
                IEnumerable<ParseObject> items = await ParseObject.GetQuery("Item").FindAsync();


                foreach (var item in items)
                {
                    ParseObject storeItem_result = new ParseObject("StoreItem");
                    storeItem_result["store"] = store.Object;
                    storeItem_result["item"] = item;
                    storeItem_result["price"] = 0;
                    itemList.Add(new StoreItem(storeItem_result));
                }
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no data found
                }
            }
        }

        public async Task saveAsync()
        {
            if (!imutable)
            {
                foreach (var item in itemList)
                {
                    await item.saveAsync();
                }
            }
        }

        public int Size
        {
            get
            {
                return itemList.Count;
            }
        }
    }

    class Item
    {
        public ParseObject item { get; private set; }

        public Item()
        {
            item = null;
        }

        public Item(ParseObject item)
        {
            this.item = item;
        }
        public virtual async Task saveAsync()
        {
             await Task.FromResult(true);
        }

        #region get set method
        public string Type
        {
            get
            {
                return item.Get<string>("type");
            }
        }

        public string Name
        {
            get
            {
                return item.Get<string>("name");
            }
        }

        public string Description
        {
            get
            {
                return item.Get<string>("description");
            }
        }

        public string Material
        {
            get
            {
                return item.Get<string>("material");
            }
        }

        public string MaterialType
        {
            get
            {
                return item.Get<string>("materialType");
            }
        }

        public string Icon
        {
            get
            {
                return item.Get<string>("icon");
            }
        }
        #endregion

    }

    class UserItem : Item, Quantifiable
    {
        public ParseObject userItem { get; private set; }

        public UserItem(ParseObject userItem)
        {
            this.userItem = userItem;
        }

        public double Quantity
        {
            get
            {
                return userItem.Get<double>("quantity");
            }

            set
            {
                userItem["quantity"] = value;
            }
        }
        
        public override sealed async Task saveAsync()
        {
            await userItem.SaveAsync();
        }
    }

    class StoreItem : Item, Priceable
    {
        public ParseObject storeItem { get; private set; }

        public StoreItem(ParseObject storeItem)
        {
            this.storeItem = storeItem;
        }

        public double Price
        {
            get
            {
                return storeItem.Get<double>("price");
            }

            set
            {
                storeItem["price"] = value;
            }
        }

        public override sealed async Task saveAsync()
        {
            await storeItem.SaveAsync();
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
