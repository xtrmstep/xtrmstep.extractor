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
        const string testDataFolder = @"f:\TestData\";

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
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "Read JSON Files / Big file")]
        public void Should_read_big_file()
        {
            throw new NotImplementedException();
        }
    }
}
