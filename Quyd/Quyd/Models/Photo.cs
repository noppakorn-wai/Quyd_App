using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;

namespace Quyd.Models
{
    class PhotoList
    {
        private List<Photo> photos;

        public PhotoList()
        {
            photos = new List<Photo>();
        }

        public async Task loadAsync(string itemId)
        {
            var query = from photos_t in ParseObject.GetQuery("itemPhoto")
                        where photos_t.ObjectId == itemId
                        select photos_t;
            try
            {
                var photos_t = await query.FindAsync();

                foreach (var photo in photos_t)
                {
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
