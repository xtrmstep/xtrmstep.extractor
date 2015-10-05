using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Xtrmstep.Extractor.Core.Tests
{
    public class FileReaderTests
    {
        const string testDataFolder = @"c:\TestData\";

        [Fact(DisplayName = "Read JSON Files / One entry")]
        public void Should_read_an_entry()
        {
            var filePath = testDataFolder + "jsonFormat.txt";
            var fileReader = new FileReader();
            var values = fileReader.Read(filePath).ToArray();

            Assert.Equal(1, values.Length);
            Assert.Equal("url_text", values[0].url);
            Assert.Equal("html_text", values[0].result);
        }

        [Fact(DisplayName = "Read JSON Files / Several entries")]
        public void Should_read_sequence()
        {
            var filePath = testDataFolder + "jsonSequence.txt";
            var fileReader = new FileReader();
            var values = fileReader.Read(filePath).ToArray();

            Assert.Equal(2, values.Length);
            Assert.Equal("url_text_1", values[0].url);
            Assert.Equal("html_text_1", values[0].result);
            Assert.Equal("url_text_2", values[1].url);
            Assert.Equal("html_text_2", values[1].result);
        }

        [Fact(DisplayName = "Read JSON Files / Big file")]
        public void Should_read_big_file()
        {
            throw new NotImplementedException();
        }
    }
}
