using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hyperspace.Redis.Metadata.Builders;
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
        public RedisText Announcement { get; set; }

        public RedisEntrySet<ForumDiscussion> Discussions { get; set; }
        public RedisEntrySet<ForumDiscussion, Guid> Comments { get; set; }

        protected internal virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builder = (ModelBuilder<ForumContext>)modelBuilder;
            builder.Entry(f => f.Announcement).HasKey();

            builder.EntrySet(f => f.Discussions, dsb =>
            {
                dsb.ItemEntry(db =>
                {
                    db.SubEntry(d => d.Title);
                    db.SubEntry(d => d.Author);
                    db.SubEntry(d => d.AuthorID);
                    db.SubEntry(d => d.CountViews);
                    db.SubEntry(d => d.CountFollows);
                    db.SubEntry(d => d.CountComments);
                });
            });
        }

    }

    public class ForumDiscussion : RedisSortedSet<Guid>
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

    public class ForumComment : RedisHash
    {
        public ForumComment(RedisContext context, RedisKey key) : base(context, key)
        {
        }
    }
}
