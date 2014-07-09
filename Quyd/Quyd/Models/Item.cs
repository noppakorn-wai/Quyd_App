using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;

namespace Quyd.Models
{
    public class ItemList : IEnumerable
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

        public async Task loadPostItemsAsync(Post post)
        {
            itemList.Clear();
            imutable = true;

            if (post.PostBy.Equals(ParseUser.CurrentUser))
            {
                imutable = false;
            }

            var query = from postItem in ParseObject.GetQuery("PostItem").Include("item")
                        where postItem.Get<ParseObject>("post") == post.Object
                        select postItem;
            try
            {
                var postItems = await query.FindAsync();
                foreach (var postItem in postItems)
                {
                    itemList.Add(new PostItem(postItem));
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
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public ItemListEnum GetEnumerator()
        {
            return new ItemListEnum(itemList);
        }
    }

    public class Item
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

    public class PostItem : Item, Quantifiable
    {
        public ParseObject postItem { get; private set; }

        public PostItem(ParseObject postItem)
        {
            this.postItem = postItem;
            item = postItem.Get<ParseObject>("item");
        }

        public double Quantity
        {
            get
            {
                return postItem.Get<double>("quantity");
            }

            set
            {
                postItem["quantity"] = value;
            }
        }
        
        public override sealed async Task saveAsync()
        {
            await postItem.SaveAsync();
        }
    }

    public class StoreItem : Item, Priceable
    {
        public ParseObject storeItem { get; private set; }

        public StoreItem(ParseObject storeItem)
        {
            this.storeItem = storeItem;
            item = storeItem.Get<ParseObject>("item");
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

    public class ItemListEnum : IEnumerator
    {
        public IList<Item> items { get; private set; }

        // Enumerators are positioned before the first element 
        // until the first MoveNext() call. 
        int position = -1;

        public ItemListEnum(IList<Item> list)
        {
            items = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < items.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Item Current
        {
            get
            {
                try
                {
                    return items.ElementAt(position);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

}
