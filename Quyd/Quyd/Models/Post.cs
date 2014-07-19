using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;
using Quyd.Resources;

namespace Quyd.Models
{
    public enum postStatus { inProgress, close };

    /*public class PostList
    {

        public List<Post> posts { get; private set; }

        public PostList()
        {
            posts = new List<Post>();
        }

        public async Task loadUserPostAsync(ParseUser user)
        {
            var query = from post in ParseObject.GetQuery("Post").Include("postBy")
                        where post.Get<ParseUser>("postBy") == user
                        orderby post.CreatedAt ascending
                        select post;
            try
            {
                IEnumerable<ParseObject> posts_t = await query.FindAsync();
                foreach (ParseObject post in posts_t)
                {
                    posts.Add(new Post(post));
                }
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no post found
                }
                else
                {
                    throw ex;
                }
            }
        }

        public async Task loadStorePostAsync(Store store)
        {
            var query = from bid in ParseObject.GetQuery("Bid").Include("post").Include("post.postBy")
                        where bid.Get<ParseObject>("bidStore") == store.Object
                        orderby bid.CreatedAt ascending
                        select bid;
            try
            {
                IEnumerable<ParseObject> bids_t = await query.FindAsync();
                foreach (ParseObject bid in bids_t)
                {
                    ParseObject post = bid.Get<ParseObject>("post");
                    posts.Add(new Post(post));
                }
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no post found
                }
                else
                {
                    throw ex;
                }
            }
        }

        public async Task loadFeedAsync(ParseUser user)
        {
            var query = from post in ParseObject.GetQuery("Post").Include("postBy")
                        where post.Get<ParseUser>("postBy") != user
                        orderby post.CreatedAt ascending
                        select post;
            try
            {
                IEnumerable<ParseObject> posts_t = await query.FindAsync();
                foreach (ParseObject post_t in posts_t)
                {
                    Post post = new Post(post_t);
                    if(await post.isBidable(user) == true)
                    {
                        posts.Add(post);
                    }
                }
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no post found
                }
                else
                {
                    throw ex;
                }
            }
        }

        public int Size
        {
            get
            {
                return posts.Count;
            }
        }
    }*/

    public class Post
    {
        public ParseObject data { get; private set; }

        public Post()
        {
        }
       
        public Post(ParseObject post)
        {
            this.data = post;
        }

        public Post(ParseGeoPoint location, ParseUser user)
        {
            data = new ParseObject("Post");
            data["location"] = location;
            data["postBy"] = user;
        }

        public async Task saveAsync()
        {
            await data.SaveAsync();
        }

        #region get set
        
        public postStatus Status
        {
            get
            {
                return data.Get<postStatus>("status");
            }
            set
            {
                data["status"] = value;
            }
        }

        public string Description
        {
            get
            {
                return data.Get<string>("description") == null ? "" : data.Get<string>("description");
            }
            set
            {
                data["description"] = value;
            }
        }

        public string Location
        {
            get
            {
                return "Bangkok, Thailand";//data.Get<ParseGeoPoint>("location");
            }
            set
            {
                data["location"] = value;
            }
        }

        public User PostBy
        {
            get
            {
                return (User) data.Get<ParseUser>("postBy");
            }
            set
            {
                data["postBy"] = value.data;
            }
        }

        public Store SelectedStore
        {
            get
            {
                return (Store) data.Get<ParseObject>("selectedStore");
            }
            set
            {
                data["selectedStore"] = value;
            }
        }

        public DateTime? CreatedAt
        {
            get
            {
                return data.CreatedAt;
            }
        }

        public async Task<IEnumerable<PostItem>> getItemsAsync()
        {
            try
            {
                 ParseQuery<ParseObject>  query =  ParseObject.GetQuery("PostItem").WhereEqualTo("post", data);
                 return (await query.FindAsync()).Select(item => new PostItem(item));
                
            }
            catch (ParseException ex)
            {
                //not handler ""no more space exception
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no item found
                }
            }

            return new List<PostItem>();
        }

        public async Task<bool> isBidableAsync()
        {
            if(this.PostBy == User.CurrentUser)
            {
                return false;
            }
            else
            {
                Store userStore = null;

                try
                {
                    userStore = await User.CurrentUser.Store();
                }
                catch (QuydException ex)
                {
                    if (ex.Code == QuydException.ErrorCode.store_notFound)
                    {
                        return false;
                    }
                }

                ParseQuery<ParseObject> query = ParseObject.GetQuery("Bid").WhereEqualTo("post", data).WhereEqualTo("bidStore", userStore);

                try
                {
                    await query.FirstAsync();
                    return false;
                }
                catch(ParseException ex)
                {
                    if(ex.Code == ParseException.ErrorCode.ObjectNotFound)
                    {
                    }
                }
            }

            return true;
        }
        /*
        public async Task<BidList> getBidList()
        {
            try
            {
                if (bidList == null)
                {
                    bidList = new BidList();
                    await bidList.getBidListAsync(new Post(post));
                }
            }
            catch (ParseException ex)
            {
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no post found
                }
                else
                {
                    throw ex;
                }
            }

            return bidList;
        }

        public async Task<bool> isBidable(ParseUser user)
        {
            if (PostBy.ObjectId == user.ObjectId)
            {
                return false;
            }
            else
            {
                await getBidList();
                // if no bidder
                if (bidList.Size != 0)
                {
                    if (await bidList.contain(user))
                    {
                        return false;
                    }
                }
            }

            return true;

        }*/

        #endregion

        # region operator
        public static explicit operator Post(ParseObject parseObject)
        {
            Post result = new Post();

            result.data = parseObject;

            return result;
        }

        public static implicit operator ParseObject(Post post)
        {
            return post.data;
        }

        public static bool operator ==(ParseObject parseObject, Post post)
        {
            return (parseObject == post.data);
        }

        public override int GetHashCode()
        {
            return data.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator !=(ParseObject parseObject, Post post)
        {
            return (parseObject != post.data);
        }
        #endregion operator

    }
}
