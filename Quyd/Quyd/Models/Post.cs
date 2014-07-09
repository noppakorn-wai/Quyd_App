using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;
using Quyd.Resources;

namespace Quyd.Models
{
    enum postStatus { inProgress, close };

    class PostList
    {

        public List<Post> posts { get; private set; }

        public PostList()
        {
            posts = new List<Post>();
        }

        public async Task loadAsync(ParseUser user)
        {
            var query = from post in ParseObject.GetQuery("Post")
                        where post.Get<ParseUser>("postBy") == user
                        select post;
            try
            {
                IEnumerable<ParseObject> posts_t = await query.FindAsync();
                foreach (ParseObject post in posts_t)
                {
                    posts.Add(new Post(post));
                }
            }
            catch(ParseException ex)
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

    class Post
    {
        public ParseObject post { get; private set; }

        public ItemList userItems { get; private set; }

        public Post()
        {
            post = null;
        }

        public Post(ParseObject post)
        {
            this.post = post;
        }

        public ParseObject Object
        {
            get
            {
                return post;
            }
        }

        public async Task loadPostAsync(string postId)
        {
            var query = from post in ParseObject.GetQuery("Post").Include("selectedStore")
                        where post.ObjectId == postId
                        select post;
            try
            {
                post = await query.FirstAsync();
                userItems = new ItemList();
                await userItems.loadUserItemsAsync(new Post(post));
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
                return post.Get<string>("description");
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
                return post.Get<ParseUser>("user");
            }
            set
            {
                post["user"] = value;
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
