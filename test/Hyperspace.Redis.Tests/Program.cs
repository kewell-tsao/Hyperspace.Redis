using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hyperspace.Redis.Metadata;
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
        public RedisText Announcement => GetEntry<RedisText>();

        public RedisEntrySet<ForumDiscussion, Guid> Discussions => GetEntry<ForumDiscussion, Guid>();
        public RedisEntrySet<ForumDiscussion, Guid> Comments => GetEntry<ForumDiscussion, Guid>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.As<ForumContext>();
            builder.Entry(f => f.Announcement).MapTo("annt");

            builder.EntrySet(f => f.Discussions, disb =>
            {
                disb.MapTo("dscs")
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
                dsb.MapTo("cmts");
                dsb.EntryItem(db =>
                {
                });
            });

        }

    }

    public class ForumDiscussion : RedisHash
    {
        public ForumDiscussion(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
        }

        public Guid ID => GetIdentifier<Guid>();

        public RedisText Title => GetEntry<RedisText>();
        public RedisText Author => GetEntry<RedisText>();
        public RedisGuid AuthorID => GetEntry<RedisGuid>();

        public RedisNumber CountViews => GetEntry<RedisNumber>();
        public RedisNumber CountFollows => GetEntry<RedisNumber>();
        public RedisNumber CountComments => GetEntry<RedisNumber>();

        public RedisList<Guid> Comments => GetEntry<RedisList<Guid>>();
    }

    public class ForumComment : RedisHash
    {
        public ForumComment(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
        }

        public Guid ID => GetIdentifier<Guid>();

        public RedisText Author => GetEntry<RedisText>();
        public RedisGuid AuthorID => GetEntry<RedisGuid>();
    }
}
