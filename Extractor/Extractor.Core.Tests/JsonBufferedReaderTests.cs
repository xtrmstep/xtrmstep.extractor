using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Xunit;

namespace Xtrmstep.Extractor.Core.Tests
{
    public class JsonBufferedReaderTests
    {
        [Fact(DisplayName = "Json Reader / Reader / Key-Value,Key-Value object")]
        public void Should_read_two_pairs_in_json()
        {
            string text = @"{""key1"":""value1"",""key2"":""value2""},";
            JsonBufferedReader reader = new JsonBufferedReader();
            NameValueCollection result = null;

            foreach (char symbol in text)
            {
                if (reader.Read(symbol))
                {
                    result = reader.PopEntry();
                }
            }            

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("value1", result["key1"]);
            Assert.Equal("value2", result["key2"]);
        }

        [Fact(DisplayName = "Json Reader / Reader / Several objects")]
        public void Should_read_several_json_objects()
        {
            string text = @"{""key1"":""value1"",""key2"":""value2""},{""key3"":""value3"",""key4"":""value4""},";
            JsonBufferedReader reader = new JsonBufferedReader();

            var entries = new List<NameValueCollection>();
            foreach (char symbol in text)
            {
                if (reader.Read(symbol))
                {
                    var entry = reader.PopEntry();
                    entries.Add(entry);
                }
            }

            Assert.Equal(2, entries.Count);
            Assert.Equal(2, entries[0].Count);
            Assert.Equal(2, entries[1].Count);
            Assert.Equal("value1", entries[0]["key1"]);
            Assert.Equal("value2", entries[0]["key2"]);
            Assert.Equal("value3", entries[1]["key3"]);
            Assert.Equal("value4", entries[1]["key4"]);
        }

        [Fact(DisplayName = "Json Reader / Reader / Values with curly brackets")]
        public void Should_read_json_values_with_curly_brackets()
        {
            string text = @"{""key1"":""{value1}"",""key2"":""}value2{""},{""key3"":""{v},{alue3}"",""key4"":""}}{value4}""},";
            JsonBufferedReader reader = new JsonBufferedReader();

            var entries = new List<NameValueCollection>();
            foreach (char symbol in text)
            {
                if (reader.Read(symbol))
                {
                    var entry = reader.PopEntry();
                    entries.Add(entry);
                }
            }

            Assert.Equal(2, entries.Count);
            Assert.Equal(2, entries[0].Count);
            Assert.Equal(2, entries[1].Count);
            Assert.Equal("{value1}", entries[0]["key1"]);
            Assert.Equal("}value2{", entries[0]["key2"]);
            Assert.Equal("{v},{alue3}", entries[1]["key3"]);
            Assert.Equal("}}{value4}", entries[1]["key4"]);
        }

        [Fact(DisplayName = "Json Reader / Reader / Values with double quotation")]
        public void Should_read_json_values_with_double_quotation()
        {
            string text = "{\"key1\":\"\\\"value1\\\"\",\"\\\"key2\\\"\":\"\\\"value2\\\"\"},";
            JsonBufferedReader reader = new JsonBufferedReader();

            var entries = new List<NameValueCollection>();
            foreach (char symbol in text)
            {
                if (reader.Read(symbol))
                {
                    var entry = reader.PopEntry();
                    entries.Add(entry);
                }
            }

            Assert.Equal(1, entries.Count);
            Assert.Equal(2, entries[0].Count);
            Assert.Equal("\\\"value1\\\"", entries[0]["key1"]);
            Assert.Equal("\\\"value2\\\"", entries[0]["\\\"key2\\\""]);
        }
    }
}