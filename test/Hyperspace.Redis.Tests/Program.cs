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
            var builder = modelBuilder.As<ForumContext>();
            builder.Entry(f => f.Announcement).ShortName("ann");

            builder.EntrySet(f => f.Discussions, disb =>
            {
                disb.ShortName("dis")
                    .Identifier(d => d.ID)
                    .EntryItem(db =>
                    {
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
                dsb.EntryItem(db =>
                {
                });
            });

        }

    }

    public class ForumDiscussion : RedisHash
    {
        public ForumDiscussion(RedisContext context, RedisKey key) : base(context, key)
        {
        }

        public Guid ID => GetIdentifier<Guid>();

        public RedisText Title => GetSubEntry<RedisText>();
        public RedisText Author => GetSubEntry<RedisText>();
        public RedisGuid AuthorID => GetSubEntry<RedisGuid>();

        public RedisNumber CountViews => GetSubEntry<RedisNumber>();
        public RedisNumber CountFollows => GetSubEntry<RedisNumber>();
        public RedisNumber CountComments => GetSubEntry<RedisNumber>();

        public RedisList<Guid> Comments => GetSubEntry<RedisList<Guid>>();
    }

    public class ForumComment : RedisHash
    {
        public ForumComment(RedisContext context, RedisKey key) : base(context, key)
        {
        }

        public Guid ID => GetIdentifier<Guid>();

        public RedisText Author => GetSubEntry<RedisText>();
        public RedisGuid AuthorID => GetSubEntry<RedisGuid>();

    }
}
