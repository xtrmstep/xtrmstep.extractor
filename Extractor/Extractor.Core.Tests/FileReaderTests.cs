using System;
using System.Linq;
using Xtrmstep.Extractor.Core.JsonFormats;
using Xunit;

namespace Xtrmstep.Extractor.Core.Tests
{
    public class FileReaderTests
    {
        private const string testDataFolder = @"c:\TestData\";

        [Fact(DisplayName = "Read JSON Files / One entry")]
        public void Should_read_an_entry()
        {
            string filePath = testDataFolder + "jsonFormat.txt";
            JsonFileReader fileReader = new JsonFileReader();
            Json80LegsFormat[] values = fileReader.Read(filePath, Json80LegsFormat.Converter).ToArray();

            Assert.Equal(1, values.Length);
            Assert.Equal("url_text", values[0].url);
            Assert.Equal("html_text", values[0].result);
        }

        [Fact(DisplayName = "Read JSON Files / Several entries")]
        public void Should_read_sequence()
        {
            string filePath = testDataFolder + "jsonSequence.txt";
            JsonFileReader fileReader = new JsonFileReader();
            Json80LegsFormat[] values = fileReader.Read(filePath, Json80LegsFormat.Converter).ToArray();

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