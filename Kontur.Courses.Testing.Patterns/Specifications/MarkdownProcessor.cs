using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Patterns.Specifications
{
	public class MarkdownProcessor
	{
		public string Render(string input)
		{
            var emReplacer = new Regex(@"([^\w\\]|^)_(.*?[^\\])_(\W|$)");
            var strongReplacer = new Regex(@"([^\w\\]|^)__(.*?[^\\])__(\W|$)");
            input = strongReplacer.Replace(input,
                    match => match.Groups[1].Value +
                            "<strong>" + match.Groups[2].Value + "</strong>" +
                            match.Groups[3].Value);
            input = emReplacer.Replace(input,
                    match => match.Groups[1].Value +
                            "<em>" + match.Groups[2].Value + "</em>" +
                            match.Groups[3].Value);
            input = input.Replace(@"\_", "_");
            return input;
		}
	}

	[TestFixture]
	public class MarkdownProcessor_should
	{
//		private readonly MarkdownProcessor mdR = new MarkdownProcessor();
//        string text = System.IO.File.ReadAllText(@"Markdown.txt");

        [Test]
        public void notChangeInput_ifNoMarkup()
        {
            MarkdownProcessor m = new MarkdownProcessor();
            var result = m.Render("Hello world!");
            Assert.That(result, Is.EquivalentTo("Hello world!"));
        }

	    [Test]
        public void Output_is_HTML()
	    {
	        
	    }

        [Test]
        public void mark_single_underline_as_em()
        {
            MarkdownProcessor m = new MarkdownProcessor();
            var result = m.Render("_reboot_");
            Assert.That(result, Is.EquivalentTo("<em>reboot</em>"));
        }

        [Test]
        public void mark_double_underline_as_strong()
        {
            MarkdownProcessor m = new MarkdownProcessor();
            var result = m.Render("__reboot__");
            Assert.That(result, Is.EquivalentTo("<strong>reboot</strong>"));
        }

        [Test]
        public void mark_strong_in_em()
        {
            MarkdownProcessor m = new MarkdownProcessor();
            var result = m.Render("_go __reboot__ now_");
            Assert.That(result, Is.EquivalentTo("<em>go <strong>reboot</strong> now</em>"));
        }

        [Test]
        [TestCase("re_boot", Result = "re_boot")]
        [TestCase("re__boot", Result = "re__boot")]
        [TestCase("1_12", Result = "1_12")]
        [TestCase("1__12", Result = "1__12")]
        public string ignore_inner_underline(string input)
        {
            MarkdownProcessor m = new MarkdownProcessor();
            return m.Render(input);
        }


        [Test]
        [TestCase("_boot", Result = "_boot")]
        [TestCase("__boot", Result = "__boot")]
        [TestCase("_12", Result = "_12")]
        [TestCase("__12", Result = "__12")]
        public string ignore_oneside_underlines(string input)
        {
            MarkdownProcessor m = new MarkdownProcessor();
            return m.Render(input);
        }

        [Test]
        [TestCase(@"\_boot", Result = @"_boot")]
        [TestCase(@"\__boot", Result = @"__boot")]
        [TestCase(@"\_12", Result = @"_12")]
        [TestCase(@"\__12", Result = @"__12")]

        [TestCase(@"\_boot\_", Result = @"_boot_")]
        [TestCase(@"\__boot\__", Result = @"__boot__")]
        [TestCase(@"\_12\_", Result = @"_12_")]
        [TestCase(@"\__12\__", Result = @"__12__")]

        [TestCase(@"\_boot\_", Result = @"_boot_")]
        [TestCase(@"\_\_boot\_\_", Result = @"__boot__")]
        [TestCase(@"\_12\_", Result = @"_12_")]
        [TestCase(@"\_\_12\_\_", Result = @"__12__")]

        [TestCase(@"\_bo _ot\_ privet_", Result = @"_bo <em>ot_ privet</em>")]
        [TestCase(@"\_\_bo __ot\_\_ privet__", Result = @"__bo <strong>ot__ privet</strong>")]
        public string ignore_underline_if_escaping(string input)
        {
            MarkdownProcessor m = new MarkdownProcessor();
            return m.Render(input);
        }
	}
}
