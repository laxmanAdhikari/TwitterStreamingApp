using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitter.Model.Entities;

namespace Twitter.StreammimgApi.Tests
{
    public class TestDataFactory
    {

        public static IEnumerable<Tweet> GetMockTweets()
        {
        return new List<Tweet>
            {
                new Tweet()
                {
                     AuthorId= 123,
                     TweeterTweetId= "testTweetId",
                     Content ="test content",
                     TwitterPublished =DateTimeOffset.Now
                },
                new Tweet()
                {
                     AuthorId= 1234444,
                     TweeterTweetId= "testTweetId22222",
                     Content ="test content22222",
                     TwitterPublished =DateTimeOffset.Now
                },
            };
        }
    }

    public class TestTwitterStramDbContext
    {
        public TestTwitterStramDbContext()
        {
            this.ReceivedTweets = (DbSet<Tweet>)TestDataFactory.GetMockTweets();
        }

        public DbSet<Tweet> ReceivedTweets { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Tweet item) { }
        public void Dispose() { }
    }

    public class TestDbSet<T> : DbSet<T>, IQueryable, IEnumerable<T>
        where T : class
    {
        ObservableCollection<T> _data;
        IQueryable _query;

        public TestDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }

        public override T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public override T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        public override T Attach(T item)
        {
            _data.Add(item);
            return item;
        }

        public override T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public override ObservableCollection<T> Local
        {
            get { return new ObservableCollection<T>(_data); }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
