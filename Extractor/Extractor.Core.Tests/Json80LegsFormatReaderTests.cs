using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Xtrmstep.Extractor.Core.Tests
{
    public class Json80LegsFormatReaderTests
    {
        [Fact(DisplayName = "80 Legs Json / Regex / Key-Value object")]
        public void GetMatches_should_match_json_one_parameter()
        {
            string text = @"{""key"":""value""}";
            var reader = new Json80LegsFormatReader();

            var matches = reader.GetMatches(text);

            Assert.NotNull(matches);
            Assert.Equal(1, matches.Count);
            Assert.Equal("key", matches[0].Groups["key"].Value);
            Assert.Equal("value", matches[0].Groups["value"].Value);
        }

        [Fact(DisplayName = "80 Legs Json / Regex / Key-Value,Key-Value object")]
        public void GetMatches_should_match_json_two_parameters()
        {
            string text = @"{""key1"":""value1"",""key2"":""value2""}";
            var reader = new Json80LegsFormatReader();

            var matches = reader.GetMatches(text);

            Assert.NotNull(matches);
            Assert.Equal(2, matches.Count);
            Assert.Equal("key1", matches[0].Groups["key"].Value);
            Assert.Equal("value1", matches[0].Groups["value"].Value);
            Assert.Equal("key2", matches[1].Groups["key"].Value);
            Assert.Equal("value2", matches[1].Groups["value"].Value);
        }

        [Fact(DisplayName = "80 Legs Json / Reader / Key-Value,Key-Value object")]
        public void GetMatches_should_read_json_one_parameter()
        {
            string text = @"{""key1"":""value1"",""key2"":""value2""}";
            var reader = new Json80LegsFormatReader();

            foreach (var symbol in text)
            {
                if (reader.Read(symbol)) break; // entry is ready
            }
            var result = reader.GetEntry();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("value1", result["key1"]);
            Assert.Equal("value2", result["key2"]);
        }
    }
}
