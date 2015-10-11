using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xtrmstep.Extractor.Core.JsonFormats;
using Xunit;
using Xunit.Abstractions;

namespace Xtrmstep.Extractor.Core.Tests
{
    public class FileReaderTests
    {
        private ITestOutputHelper output;

        public FileReaderTests(ITestOutputHelper output)
        {
            this.output = output;
        }

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
            string filePath = testDataFolder + "jsonBigFileAndPages.txt"; // 38Mb
            JsonFileReader fileReader = new JsonFileReader();
            var values = fileReader.Read(filePath, Json80LegsFormat.Converter);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var count = 0;
            foreach (var value in values)
            {
                Assert.NotNull(value.url); // duplicates are not skipped
                Assert.NotNull(value.result);
                count++;
            }
            sw.Stop();
            output.WriteLine("Read {0} entries in {1} ms ({2} ticks)", count, sw.ElapsedMilliseconds, sw.ElapsedTicks);
        }

        [Fact(DisplayName = "Read JSON Files / Html content in file")]
        public void Should_read_html_content()
        {
            string filePathJson = testDataFolder + "jsonHtmlContent.txt";
            string filePathHtml = testDataFolder + "htmlContent.txt";
            
            JsonFileReader fileReader = new JsonFileReader();
            var values = fileReader.Read(filePathJson, Json80LegsFormat.Converter).ToArray();

            var expected = File.ReadAllText(filePathHtml);

            Assert.Equal(1, values.Length);

            Assert.Equal("url/value", values[0].url);
            Assert.Equal(expected, values[0].result);
        }

    }
}