using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;

namespace Quyd.Model
{
    class Items
    {
        private List<Item> items;

        public Items()
        {
            items = new List<Item>();
        }

        public async Task Load()
        {
            var query = from item_t in ParseObject.GetQuery("Item")
                        where true
                        select item_t;
            try
            {
                var items_t = await query.FindAsync();
                foreach (var item in items_t)
                {
                    items.Add(new Item(item));
                }
            }
            catch(ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    items = new List<Item>();
                }
            }
        }

        public int size()
        {
            return items.Count;
        }
    }

    class Item
    {
        ParseObject item;
        Photos photos;
        public Item(ParseObject item_t)
        {
            item = item_t;
            photos = new Photos();
        }

        public string getType(int i)
        {
            return item.Get<string>("type");
        }

        public string getName(int i)
        {
            return item.Get<string>("name");
        }

        public string getDescription(int i)
        {
            return item.Get<string>("description");
        }

        public string getMaterial(int i)
        {
            return item.Get<string>("material");
        }

        public string getMaterialType(int i)
        {
            return item.Get<string>("materialType");
        }

        public string getIcon(int i)
        {
            return item.Get<string>("icon");
        }

        public async Task<Photos> getPhotos()
        {
            if (photos.size() == 0)
            {
                await photos.load(item.ObjectId);
            }
            return photos;
        }

        public ParseObject getObject()
        {
            return item;
        }
    }

    class Photos
    {
        private List<Photo> photos;

        public Photos()
        {
            photos = new List<Photo>();
        }

        public async Task load(string itemId)
        {
            var query = from photos_t in ParseObject.GetQuery("itemPhoto")
                        where photos_t.ObjectId == itemId
                        select photos_t;
            try
            {
                var photos_t = await query.FindAsync();

                foreach (var photo in photos_t)
                {
                    //put group function here
                    photos.Insert(0, new Photo(photo));
                }
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    photos = new List<Photo>();
                }
            }
        }

        public int size()
        {
            return photos.Count;
        }

    }
    class Photo
    {
        ParseObject photo;

        public Photo(ParseObject photo)
        {
            this.photo = photo;
        }

        public string getDescription(int i)
        {
            return photo.Get<string>("description");
        }

        public string getLink(int i)
        {
            return photo.Get<string>("link");
        }
    }
}
