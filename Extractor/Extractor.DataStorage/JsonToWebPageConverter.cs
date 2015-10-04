using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xtrmstep.Extractor.Core.Model;

namespace Xtrmstep.Extractor.Core
{
    public class Json80LegsFormatReader
    {
        StringBuilder _buffer = new StringBuilder();

        int _counterArray = 0;
        int _counterObject = 0;
        int _counterScoupe = 0;
        int lastArray = 0;
        int lastObject = 0;
        int lastScoupe = 0;

        public bool Read(char symbol)
        {
            switch (symbol)
            {
                case '[':
                    lastArray = _counterArray++;
                    break;
                case ']':
                    if (lastArray == _counterArray)
                        lastArray = --_counterArray;
                    else
                        --_counterArray;
                    break;
                case '{':
                    lastObject = _counterObject++;
                    break;
                case '}':
                    if (lastObject == _counterObject)
                        lastObject = --_counterObject;
                    else
                        --_counterObject;
                    break;
                case '(':
                    lastScoupe = _counterScoupe++;
                    break;
                case ')':
                    if (lastScoupe == _counterScoupe)
                        lastScoupe = --_counterScoupe;
                    else
                        --_counterScoupe;
                    break;
                case ',':
                    if (lastObject == _counterObject)
                        return true;// end of object and value is ready
                    break;
            }
            
            // skip some symbols from buffer
            if (new[] { '[', ']' }.Contains(symbol) == false)
                _buffer.Append(symbol);

            return false;
        }

        public MatchCollection GetMatches(string text)
        {
            string pattern = "(?<entry>[\"](?<key>\\w+)[\"]:[\"](?<value>\\w+)[\"])";
            return Regex.Matches(text, pattern);
        }

        public NameValueCollection GetEntry()
        {
            var matches = GetMatches(_buffer.ToString());
            var result = new NameValueCollection();
            foreach (Match match in matches)
            {
                var key = match.Groups["key"].Value;
                var value = match.Groups["value"].Value;
                result.Add(key, value);
            }

            _buffer.Length = 0;

            _counterArray = 0;
            _counterObject = 0;
            _counterScoupe = 0;
            lastArray = 0;
            lastObject = 0;
            lastScoupe = 0;

            return result;
        }
    }
}
