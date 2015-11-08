using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.DependencyInjection;
using StackExchange.Redis;

namespace Hyperspace.Redis.Tests
{
    public class Program
    {
        public void Main(string[] args)
        {
            var services = (IServiceCollection)new object();

            services.AddRedis()
                    .AddRedisContext<ForumContext>(options => options.UseConnection("localhost")
                                                                     .UseDatabase(0));
            var context = new ForumContext();
            context.Discussions["123"].CountViews.Increment();
            context.Discussions["123"].CountViews++;
            var id = context.Discussions["123"].AuthorID.Get();
        }
    }

    public class ForumContext : RedisContext
    {
        public RedisEntrySet<ForumDiscussion> Discussions { get; set; }

    }

    public class ForumDiscussion : RedisString
    {
        public ForumDiscussion(RedisContext context, RedisKey key) : base(context, key)
        {
        }

        public RedisText Title { get; set; }
        public RedisText Author { get; set; }
        public RedisGuid AuthorID { get; set; }

        public RedisCounter CountViews { get; set; }
        public RedisCounter CountFollows { get; set; }
        public RedisCounter CountComments { get; set; }

    }

}
