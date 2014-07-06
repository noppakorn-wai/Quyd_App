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
        private static IEnumerable<ParseObject> items;

        public async Task Load()
        {
            var query = from item_t in ParseObject.GetQuery("Item")
                        where true
                        select item_t;
            try
            {
                items = await query.FindAsync();
            }
            catch(ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    items = new List<ParseObject>();
                }
            }
        }

        public int size()
        {
            return items.Count<ParseObject>();
        }

        public string getType(int i)
        {
            return items.ElementAt<ParseObject>(i).Get<string>("type");
        }

        public string getName(int i)
        {
            return items.ElementAt<ParseObject>(i).Get<string>("name");
        }

        public string getDescription(int i)
        {
            return items.ElementAt<ParseObject>(i).Get<string>("description");
        }

        public string getMaterial(int i)
        {
            return items.ElementAt<ParseObject>(i).Get<string>("material");
        }

        public string getMaterialType(int i)
        {
            return items.ElementAt<ParseObject>(i).Get<string>("materialType");
        }

        public string getMaterialType(int i)
        {
            return items.ElementAt<ParseObject>(i).Get<string>("materialType");
        }

        public async Task<Photos> getPhotos(int i)
        {
            var photos = new Photos();
            var itemId = items.ElementAt(i).ObjectId;
            await photos.load(itemId);
            return photos;
        }

        class Photos
        {
            private IEnumerable<ParseObject> photos;

            public Photos()
            {
                photos = new List<ParseObject>();
            }

            public async Task load(string itemId)
            {
                var query = from photos_t in ParseObject.GetQuery("itemPhoto")
                            where photos_t.ObjectId == itemId
                            select photos_t;
                try
                {
                    photos = await query.FindAsync();
                }
                catch (ParseException ex)
                {
                    if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                    {
                        photos = new List<ParseObject>();
                    }
                }
            }

            public int size()
            {
                return photos.Count<ParseObject>();
            }

            public string getDescription(int i)
            {
                return photos.ElementAt<ParseObject>(i).Get<string>("description");
            }

            public string getLink(int i)
            {
                return photos.ElementAt<ParseObject>(i).Get<string>("link");
            }
        }
    }
}
