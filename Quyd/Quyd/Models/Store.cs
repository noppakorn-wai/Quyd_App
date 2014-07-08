using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;
using Quyd.Resources;

namespace Quyd.Models
{
    class Store
    {
        public ParseObject store { get; private set; }

        public ItemList items { get; private set; }

        public Store()
        {
            store = null;
        }
        public Store(ParseObject store)
        {
            this.store = store;
        }

        public Store(string name, ParseGeoPoint location, IList<string> phones, ItemList items)
        {
            store = new ParseObject("Store");
            store["name"] = name;
            var owner = ParseUser.CurrentUser;
            store["owner"] = owner;
            store["location"] = location;
            store["phones"] = phones;
        }

        public async Task saveAsync()
        {
            try
            {
                await this.validAsync();
                await store.SaveAsync();


                //edit to save items
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task loadAsync(ParseUser owner)
        {
            var query = from store_t in ParseObject.GetQuery("Store")
                        where store_t.Get<ParseUser>("owner").Equals(owner)
                        select store_t;
            try
            {
                store = await query.FirstAsync();
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    throw new QuydException(QuydException.ErrorCode.store_notFound, "Store not found");
                }
                else
                {
                    throw ex;
                }
            }

            items = new ItemList(new Store(store));
        }

        private async Task<bool> isNameExistAsync(string storeName)
        {
            try
            {
                var findByNameQuery = from store_t in ParseObject.GetQuery("Store")
                                      where store_t.Get<string>("name").Equals(storeName)
                                      select store_t;
                ParseObject storeObject = await findByNameQuery.FirstAsync();
                return true;
            }
            catch(ParseException ex)
            {
                if(ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    return false;
                }
                else
                {
                    throw ex;
                }
            }
        }

        private async Task<bool> validAsync()
        {
            //put validation here
            if(getName().Length==0)
            {
                throw new QuydException(QuydException.ErrorCode.store_nameTooShort, "Store name is too short.");
            }
            else if(getLocation().Equals(new ParseGeoPoint(0,0)))
            {
                throw new QuydException(QuydException.ErrorCode.store_locationInvalid, "Location is invalid.");
            }
            else if(getPhones().Count == 0)
            {
                throw new QuydException(QuydException.ErrorCode.store_phoneInvalid, "Phone number is invalid.");
            }
            else
            {
                bool nameExist = await isNameExistAsync(store.Get<string>("name"));
                if(nameExist)
                {
                    throw new QuydException(QuydException.ErrorCode.store_nameExist, "Store name is already exist.");
                }
                else 
                {
                    return true;
                }
            }
        }

        #region Store Getter Setter

        public void setName(string name)
        {
            store["name"] = name;
        }

        public string getName()
        {
            return store.Get<string>("name");
        }

        public void setLocation(ParseGeoPoint location)
        {
            store["location"] = location;
        }

        public ParseGeoPoint getLocation()
        {
            return store.Get<ParseGeoPoint>("location");
        }

        public void setPhones(IList<string> phones)
        {
            store["phones"] = phones;
        }

        public IList<string> getPhones()
        {
            return store.Get<IList<string>>("phones");
        }

        public string getOwnerId()
        {
            return store.Get<string>("owner");
        }

        #endregion

    }
}
