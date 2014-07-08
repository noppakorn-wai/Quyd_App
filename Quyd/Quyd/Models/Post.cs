using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;

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

        public int size()
        {
            return posts.Count;
        }
    }

    class Post
    {
        ParseObject post;

        public Post()
        {
            post = null;
        }

        public async Task loadPostAsync(string postId)
        {
            var query = from post in ParseObject.GetQuery("Post")
                        where post.ObjectId == postId
                        select post;
            try
            {
                post = await query.FirstAsync();
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
    }
}
