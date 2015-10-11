using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace Xtrmstep.Extractor.Core
{
    public class JsonFileReader
    {
        private const int MAX_BLOCK_LENGTH = 1024;

        public IEnumerable<TJsonFormat> Read<TJsonFormat>(string fileName, Func<NameValueCollection, TJsonFormat> converter)
        {
            Func<JsonBufferedReader, TJsonFormat> materializeBuffer = reader =>
            {
                NameValueCollection entry = reader.PopEntry();
                return converter(entry);
            };

            JsonBufferedReader jsonBufferedReader = new JsonBufferedReader();
            using (StreamReader sr = new StreamReader(fileName))
            {
                char[] buffer = new char[MAX_BLOCK_LENGTH];
                while (!sr.EndOfStream)
                {
                    int readCount = sr.ReadBlock(buffer, 0, MAX_BLOCK_LENGTH);
                    for (int i = 0; i < readCount; i++)
                    {
                        char symbol = buffer[i];
                        if (jsonBufferedReader.Read(symbol))
                        {
                            yield return materializeBuffer(jsonBufferedReader);
                        }
                    }
                }
            }
        }

        public string[] LookupFiles(string dirName)
        {
            return Directory.GetFiles(dirName);
        }
    }
}