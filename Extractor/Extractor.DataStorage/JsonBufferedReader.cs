using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xtrmstep.Extractor.Core
{
    public class JsonBufferedReader
    {
        private readonly char[] _skippedSymbols =
        {
            '[', ']'
        };

        private StringBuilder _buffer = new StringBuilder();
        private int _counterArray;
        private int _counterObject;
        private int _counterScoupe;
        private int _lastArray;
        private int _lastObject;
        private int _lastScoupe;

        private void Reset()
        {
            _buffer = _buffer ?? new StringBuilder();
            _buffer.Length = 0;

            _counterArray = 0;
            _counterObject = 0;
            _counterScoupe = 0;
            _lastArray = 0;
            _lastObject = 0;
            _lastScoupe = 0;
        }

        public bool Read(char symbol)
        {
            // todo: get rid of useless cases
            switch (symbol)
            {
                case '[':
                    _lastArray = _counterArray++;
                    break;
                case ']':
                    if (_lastArray == _counterArray)
                    {
                        _lastArray = --_counterArray;
                    }
                    else
                    {
                        --_counterArray;
                    }
                    break;
                case '{':
                    _lastObject = _counterObject++;
                    break;
                case '}':
                    if (_lastObject == _counterObject)
                    {
                        _lastObject = --_counterObject;
                    }
                    else
                    {
                        --_counterObject;
                    }
                    break;
                case '(':
                    _lastScoupe = _counterScoupe++;
                    break;
                case ')':
                    if (_lastScoupe == _counterScoupe)
                    {
                        _lastScoupe = --_counterScoupe;
                    }
                    else
                    {
                        --_counterScoupe;
                    }
                    break;
                case ',':
                    if (_lastObject == _counterObject)
                    {
                        return true; // end of object and value is ready
                    }
                    break;
            }

            // skip some symbols from buffer
            if (_skippedSymbols.Contains(symbol) == false)
            {
                _buffer.Append(symbol);
            }

            return false;
        }

        public MatchCollection GetJsonKeyValues(string text)
        {
            const string pattern = "(?<entry>[\"](?<key>\\w+)[\"]:[\"](?<value>\\w+)[\"])";
            return Regex.Matches(text, pattern);
        }

        public NameValueCollection GetEntry()
        {
            MatchCollection matches = GetJsonKeyValues(_buffer.ToString());
            NameValueCollection result = new NameValueCollection();
            foreach (Match match in matches)
            {
                string key = match.Groups["key"].Value;
                string value = match.Groups["value"].Value;
                result.Add(key, value);
            }

            Reset();

            return result;
        }
    }
}