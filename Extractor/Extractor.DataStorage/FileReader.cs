using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json;
using Xtrmstep.Extractor.Core.Model;

namespace Xtrmstep.Extractor.Core
{
    public class FileReader
    {
        const int MAX_BLOCK_LENGTH = 1024;

        public class JsonFormat
        {
            public string url { get; set; }
            public string result { get; set; }
        }

        public IEnumerable<JsonFormat> Read(string fileName)
        {
            var reader = new Json80LegsFormatReader();

            Func<Json80LegsFormatReader, JsonFormat> f = t =>
           {
               var entry = reader.GetEntry();
               return new JsonFormat
               {
                   url = entry["url"],
                   result = entry["result"]
               };
           };

            using (StreamReader sr = new StreamReader(fileName))
            {
                char[] buffer = new char[MAX_BLOCK_LENGTH];
                while (!sr.EndOfStream)
                {
                    var readCount = sr.ReadBlock(buffer, 0, MAX_BLOCK_LENGTH);
                    for (int i = 0; i < readCount; i++)
                    {
                        char symbol = buffer[i];
                        if (reader.Read(symbol))
                        {
                            yield return f(reader);
                        }
                    }
                }
            }
            yield return f(reader);
        }

        public string[] LookupFiles(string dirName)
        {
            return Directory.GetFiles(dirName);
        }
    }
}