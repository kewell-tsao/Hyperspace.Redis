using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hyperspace.Redis.Metadata;
using Hyperspace.Redis.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Hyperspace.Redis.Tests
{
    public class Program
    {
        public static async Task DoTest()
        {
            var connection = ConnectionMultiplexer.Connect("localhost");
            var database = connection.GetDatabase(0);

            var b = database.CreateBatch();
            var t1 = b.ListLeftPushAsync("AA", "AAV");
            var t2 = b.ListLeftPushAsync("AA", "AAV");
            var t3 = b.ListLeftPushAsync("AA", "AAV");
            b.Execute();

            var r1 = t1.Result;
            var r2 = t2.Result;
            var r3 = t3.Result;

            //var transaction = database.CreateTransaction();
            //var messageEnqueueTask = transaction.ListLeftPushAsync("AA", "AAV");
            //var messagePublishTask = transaction.PublishAsync(new RedisChannel("AA", RedisChannel.PatternMode.Literal), "AAM");
            //var committed = await transaction.ExecuteAsync();

            //if (committed)
            //{
            //    var queueLength = await messageEnqueueTask;
            //    var subscriberCount = await messagePublishTask;
            //}
        }

        public static void Main(string[] args)
        {
            DoTest().Wait();
            //return;

            var services = new ServiceCollection();
            services.AddRedis()
                    .AddRedisContext<ForumContext>(options => options.UseConnection("localhost")
                                                                     .UseDatabase(0));
            var provider = services.BuildServiceProvider();

            //pfx:Discussions:123:CountViews
            //pfx:Discussions:123:CountComments

            using (var context = provider.GetRequiredService<ForumContext>())
            {
                context.BeginTransaction()
                    .When(c => c.HashExists("", ""))
                    .Done(c =>
                    {
                        var currentID = c.Announcement.SetAsync("");
                        return new[] { currentID }.Select(t => t.Result);
                    })
                    .Execute(r =>
                   {
                       var aa = r;
                   });

                context.BeginTransaction()
                    .When(c => c.HashExists("", ""))
                    .Done(c =>
                    {
                        c.Announcement.SetAsync("");
                        c.Announcement.SetAsync("");
                    })
                    .Execute();
            }

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
