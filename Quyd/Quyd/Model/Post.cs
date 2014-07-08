using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;

namespace Quyd.Model
{
    enum postStatus { inProgress, close };

    class PostList
    {
        
        List<Post> posts;

        public PostList()
        {
            posts = new List<Post>();
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
