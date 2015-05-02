using System;
using System.Linq;
using System.Runtime.InteropServices;
using Kontur.Courses.Testing.Implementations;
using NUnit.Framework;

namespace Kontur.Courses.Testing
{
	public class WordsStatistics_Tests
	{
		public Func<IWordsStatistics> createStat = () => new WordsStatistics_CorrectImplementation(); // меняется на разные реализации при запуске exe
		public IWordsStatistics stat;
        public IWordsStatistics stat2;

		[SetUp]
		public void SetUp()
		{
			stat = createStat();
            stat2 = createStat();
		}

		[Test]
		public void no_stats_if_no_words()
		{
			CollectionAssert.IsEmpty(stat.GetStatistics());
		}

		[Test]
		public void same_word_twice()
		{
			stat.AddWord("xxx");
			stat.AddWord("xxx");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "xxx") }, stat.GetStatistics());
		}

		[Test]
		public void single_word()
		{
			stat.AddWord("hello");
			CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello") }, stat.GetStatistics());
		}

        [Test]
        public void another_register()
        {
            stat.AddWord("Hello");
            stat.AddWord("hello");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "hello") }, stat.GetStatistics());
        }

        [Test]
        public void empty_string()
        {
            stat.AddWord("");
            CollectionAssert.IsEmpty(stat.GetStatistics());
        }

        [Test]
        public void null_string()
        {
            stat.AddWord(null);
            CollectionAssert.IsEmpty(stat.GetStatistics());
        }

		[Test]
		public void two_same_words_one_other()
		{
			stat.AddWord("hello");
			stat.AddWord("world");
			stat.AddWord("world");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "world"), Tuple.Create(1, "hello") }, stat.GetStatistics());
		}

        [Test]
        public void two_letters_spaces_numbers()
        {
            stat.AddWord("a");
            stat.AddWord("1");
            stat.AddWord(" ");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, " "), Tuple.Create(1, "1"), Tuple.Create(1, "a") }, stat.GetStatistics());
        }

        [Test]
        public void only_numbers()
        {
            stat.AddWord("3");
            stat.AddWord("1");
            stat.AddWord("2");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "1"), Tuple.Create(1, "2"), Tuple.Create(1, "3") }, stat.GetStatistics());
        }

        [Test]
        public void two_same_words_one_other_different_position()
        {
            stat.AddWord("world");
            stat.AddWord("hello");
            stat.AddWord("world");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "world"), Tuple.Create(1, "hello") }, stat.GetStatistics());
        }

        [Test, Timeout(1000)]
        public void big_number()
        {
            for (int i = 0; i < 10000; i++)
            {
                stat.AddWord(i.ToString());    
            }
            Assert.That(stat.GetStatistics(), Is.Ordered);
        }

        [Test, Timeout(1000)]
        public void double_using()
        {
            for (int i = 0; i < 10000; i++)
            {
                stat.AddWord(i.ToString());
            }
            Assert.That(stat2.GetStatistics(), Is.Empty);
        }

        [Test, Timeout(1000)]
        public void length_more_then_10()
        {
            stat.AddWord("world012345");
            stat.AddWord("world012346");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "world01234") }, stat.GetStatistics());
        }

        [Test]
        public void length_more_then_()
        {
            var r = new Random();
            for (int i = 0; i < 10; i++)
            {
                String new_item = "world" + r.Next(100000000, 1000000000).ToString() + i.ToString();
                stat.AddWord(new_item);
                stat.AddWord(new_item);
                stat.AddWord(new_item);
                stat.AddWord(new_item);
                stat.AddWord(new_item);
            }
            stat.AddWord("world" + r.Next(100000000, 1000000000).ToString());
            stat.AddWord("world" + r.Next(100000000, 1000000000).ToString());
            stat.AddWord("world" + r.Next(100000000, 1000000000).ToString());
//            CollectionAssert.AreEqual(new[] { Tuple.Create( "world01234") }, stat.GetStatistics());
            var s = stat.GetStatistics().ToList();
            for (int i = 0; i < s.Count(); i++)
            {
                Console.WriteLine(s[i]);
            }
            CollectionAssert.AllItemsAreUnique(stat.GetStatistics());
        }
	}
}