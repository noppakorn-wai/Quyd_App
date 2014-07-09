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

    public class PostList
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
                        select post;
            try
            {
                IEnumerable<ParseObject> posts_t = await query.FindAsync();
                foreach (ParseObject post in posts_t)
                {
                    ItemList postItem = new ItemList();
                    await postItem.loadPostItemsAsync(new Post(post));
                    posts.Add(new Post(post, postItem));
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
                        select post;
            try
            {
                IEnumerable<ParseObject> posts_t = await query.FindAsync();
                foreach (ParseObject post in posts_t)
                {
                    ItemList postItem = new ItemList();
                    await postItem.loadPostItemsAsync(new Post(post));
                    posts.Add(new Post(post, postItem));
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
    }

    public class Post
    {
        public ParseObject post { get; private set; }

        public ItemList postItems;

        public async Task<ItemList> UserItem()
        {
            try
            {
                if (postItems == null)
                {
                    postItems = new ItemList();
                    await postItems.loadPostItemsAsync(new Post(post));
                }
            }
            catch (ParseException ex)
            {
                //not handler ""no more space exception
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no older notification found
                    post = null;
                }
            }

            return postItems;
        }

        public Post()
        {
            post = null;
        }

        public Post(ParseObject post)
        {
            this.post = post;
            this.postItems = null;
        }

        public Post(ParseObject post, ItemList postItems)
        {
            this.post = post;
            this.postItems = postItems;
        }

        public ParseObject Object
        {
            get
            {
                return post;
            }
        }

        public async Task loadAsync(string postId)
        {
            var query = from post in ParseObject.GetQuery("Post").Include("selectedStore")
                        where post.ObjectId == postId
                        select post;
            try
            {
                post = await query.FirstAsync();
                await postItems.loadPostItemsAsync(new Post(post));
            }
            catch (ParseException ex)
            {
                //not handler ""no more space exception
                if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    //no older notification found
                    post = null;
                }
            }
        }

        #region get set

        public postStatus Status
        {
            get
            {
                return post.Get<postStatus>("status");
            }
            set
            {
                post["status"] = value;
            }
        }

        public string Description
        {
            get
            {
                return post.Get<string>("description") == null ? "" : post.Get<string>("description");
            }
            set
            {
                post["description"] = value;
            }
        }

        public ParseGeoPoint Location
        {
            get
            {
                return post.Get<ParseGeoPoint>("location");
            }
            set
            {
                post["location"] = value;
            }
        }

        public ParseUser PostBy
        {
            get
            {
                return post.Get<ParseUser>("postBy");
            }
            set
            {
                post["postBy"] = value;
            }
        }

        public Store SelectedStore
        {
            get
            {
                return new Store(post.Get<ParseObject>("selectedStore"));
            }
            set
            {
                post["selectedStore"] = value;
            }
        }

        #endregion

    }
}
