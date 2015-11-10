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
            //pfx:Discussions:123:CountViews
            //pfx:Discussions:123:CountComments

            var context = new ForumContext();
            //context.Discussions["123"].CountViews.Increment();
            //var id = context.Discussions["123"].AuthorID.Get();
        }
    }



    public class ForumContext : RedisContext
    {
        public RedisText Announcement => GetSubEntry<RedisText>();

        public RedisEntrySet<ForumDiscussion, Guid> Discussions => GetSubEntrySet<ForumDiscussion, Guid>();
        public RedisEntrySet<ForumDiscussion, Guid> Comments => GetSubEntrySet<ForumDiscussion, Guid>();

        protected internal virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builder = (ModelBuilder<ForumContext>)modelBuilder;
            builder.Entry(f => f.Announcement).ShortName("ann");

            builder.EntrySet(f => f.Discussions, dsb =>
            {
                dsb.ShortName("dis");
                dsb.EntrySetItem(db =>
                {
                    db.Identifier(d => d.ID);

                    db.SubEntry(d => d.Title);
                    db.SubEntry(d => d.Author);
                    db.SubEntry(d => d.AuthorID);
                    db.SubEntry(d => d.CountViews);
                    db.SubEntry(d => d.CountFollows);
                    db.SubEntry(d => d.CountComments);
                });
            });


            builder.EntrySet(f => f.Comments, dsb =>
            {
                dsb.ShortName("dis");
                dsb.EntrySetItem(db =>
                {
                });
            });
        }

    }

    public class ForumDiscussion : RedisSortedSet<Guid>
    {
        public ForumDiscussion(RedisContext context, RedisKey key) : base(context, key)
        {
        }

        public Guid ID { get; set; }

        public RedisText Title => GetSubEntry<RedisText>(this);
        public RedisText Author => GetSubEntry<RedisText>(this);
        public RedisGuid AuthorID => GetSubEntry<RedisGuid>(this);

        public RedisNumber CountViews => GetSubEntry<RedisNumber>(this);
        public RedisNumber CountFollows => GetSubEntry<RedisNumber>(this);
        public RedisNumber CountComments => GetSubEntry<RedisNumber>(this);

    }

    public class ForumComment : RedisHash
    {
        public ForumComment(RedisContext context, RedisKey key) : base(context, key)
        {
        }
    }
}
