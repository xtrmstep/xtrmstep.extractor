using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json;

namespace Xtrmstep.Extractor.Core
{
    public class FileReader
    {
        public NameValueCollection Read(string fileName)
        {
            NameValueCollection collection = new NameValueCollection();
            string data = File.ReadAllText(fileName);
            object obj = JsonConvert.DeserializeObject(data);
            dynamic[] arrayOfObj = obj as dynamic[];
            if (arrayOfObj != null)
            {
                foreach (dynamic item in arrayOfObj)
                {
                    string url = item.url;
                    string result = item.result;
                    collection.Add(url, result);
                }
            }
            return collection;
        }

        public string[] LookupFiles(string dirName)
        {
            return Directory.GetFiles(dirName);
        }
    }
}