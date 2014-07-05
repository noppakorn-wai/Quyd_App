using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Quyd.Resources;

using Parse;

using Quyd.Model;

namespace Quyd
{
    class TestFunction
    {
        //#User:Register
        async void register()
        {
            var user = new ParseUser()
            {
                Username = "my name",
                Password = "my pass",
                Email = "email@example.com"
            };

            // other fields can be set just like with ParseObject
            user["phone"] = "415-392-0202";

            await user.SignUpAsync();
        }

        //#User:Login
        async void login()
        {
            try
            {
                await ParseUser.LogInAsync("my name", "my pass");
                // Login was successful.
            }
            catch (ParseException ex)
            {
                // The login failed. Check the error to see why.
            }
        }

        //#Store:Creat
        async void create()
        {
            //-if error cover with catch block
            IList<string> phones = new List<string>();
            phones.Add("12345678");
            phones.Add("12345679");
            ParseGeoPoint location = new ParseGeoPoint(0, 0);
            Store new_store = new Store("testingStore", location, phones);
            await new_store.save();
        }
        //#Store:Get
        async void get(ParseUser owner)
        {
            var query = from store_t in ParseObject.GetQuery("Store")
                        where store_t["owner"] == ParseUser.CurrentUser
                        select store_t;

            var stores = await query.FindAsync();
        }
    }
}
