using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;
using Quyd.Resources;

namespace Quyd.Models
{
    public class Store
    {
        public ParseObject data { get; private set; }

        public Store()
        {

        }

        public Store(Store store=null)
        {
            this.data = store.data;
        }

        public string Name
        {
            get
            {
                return data.Get<string>("name");
            }
            set
            {
                data["name"] = value;
            }
        }

        public ParseGeoPoint Location
        {
            get
            {
                return data.Get<ParseGeoPoint>("location");
            }
            set
            {
                data["location"] = value;
            }
        }

        public List<string> Phones
        {
            get
            {
                return data.Get<List<string>>("phones");
            }
            set
            {
                data["phones"] = value;
            }
        }

        public User Owner
        {
            get
            {
                return data.Get<User>("owner");
            }
            private set
            {
                data["owner"] = value;
            }
        }

        public async Task<IEnumerable<Item>> StoreItemsAsync()
        {
            IEnumerable<Item> items = null;
            try
            {
                ParseQuery<ParseObject> query = data.GetRelation<ParseObject>("storeItem").Query;
                items = (await query.FindAsync()).Cast<Item>();
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    items =  new List<Item>();
                }
            }
            return items;
        }

        #region async method
        public async Task<Store> LoadAsync(User owner)
        {
            ParseQuery<ParseObject> query = from store_q in ParseObject.GetQuery("Store")
                                            where store_q.Get<ParseUser>("owner") == (ParseUser)owner
                                            select store_q;

            Store store = (Store)await query.FirstAsync();

            return this;
        }
        #endregion async method

        # region operator
        public static explicit operator Store(ParseObject parseObject)
        {
            Store result = new Store();

            result.data = parseObject;

            return result;
        }

        public static implicit operator ParseObject(Store store)
        {
            return store.data;
        }

        public static bool operator ==(ParseObject parseObject, Store store)
        {
            ParseObject _data = null;
            try
            {
                _data = store.data;
            }
            catch(NullReferenceException)
            {
                if(parseObject==null)
                {
                    return true;
                }
            }

            return (parseObject == _data);
        }

        public override int GetHashCode()
        {
            return data.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator !=(ParseObject parseObject, Store store)
        {
            return (parseObject != store.data);
        }
        #endregion operator
    }
}
