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
        public List<Photo> photos { get; private set; }

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

        public int Size
        {
            get
            {
                return photos.Count;
            }
        }
    }

    class Photo
    {
        ParseObject photo;

        public Photo(ParseObject photo)
        {
            this.photo = photo;
        }

        public string Description
        {
            get
            {
                return photo.Get<string>("description");
            }
        }

        public string Link
        {
            get
            {
                return photo.Get<string>("link");
            }
        }
    }
}
