using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xtrmstep.Extractor.Core
{
    public class JsonBufferedReader
    {
        enum ReaderState
        {
            EntryStart,
            EntryEnd,
            KeyStart,
            KeyRead,
            KeyEnd,
            ValueStart,
            ValueRead,
            ValueEnd,
            KeySpecialChar,
            ValueSpecialChar
        }

        private StringBuilder _buffer = new StringBuilder();
        private ReaderState _readerState = ReaderState.EntryStart;
        private Stack<NameValueCollection> _entries = new Stack<NameValueCollection>();
        private StringBuilder _currentKey = new StringBuilder();
        private StringBuilder _currentValue = new StringBuilder();
        private NameValueCollection _currentEntry = new NameValueCollection();

        public bool Read(char symbol)
        {
            var currentState = _readerState;
            switch (_readerState)
            {
                #region starting
                case ReaderState.EntryStart:
                    if (symbol == '{')
                        _readerState = ReaderState.KeyStart;
                    break;
                case ReaderState.KeyStart:
                    if (symbol == '"')
                        _readerState = ReaderState.KeyRead;
                    break;
                case ReaderState.ValueStart:
                    if (symbol == '"')
                        _readerState = ReaderState.ValueRead;
                    break; 
                #endregion
                #region reading
                case ReaderState.KeyRead:
                    switch(symbol)
                    {
                        case '"': _readerState = ReaderState.KeyEnd; break;
                        case '\\': _readerState = ReaderState.KeySpecialChar; _buffer.Append(symbol); break;
                        default: _buffer.Append(symbol); break;
                    }
                    break;
                case ReaderState.ValueRead:
                    switch (symbol)
                    {
                        case '"': _readerState = ReaderState.ValueEnd; break;
                        case '\\': _readerState = ReaderState.ValueSpecialChar; _buffer.Append(symbol); break;
                        default: _buffer.Append(symbol); break;
                    }
                    break;
                case ReaderState.KeySpecialChar:
                    _readerState = ReaderState.KeyRead;
                    _buffer.Append(symbol);
                    break;
                case ReaderState.ValueSpecialChar:
                    _readerState = ReaderState.ValueRead;
                    _buffer.Append(symbol);
                    break;
                #endregion
                #region ending
                case ReaderState.EntryEnd:
                    if (symbol == ',')
                        _readerState = ReaderState.EntryStart;
                    break;
                case ReaderState.KeyEnd:
                    if (symbol == ':')
                        _readerState = ReaderState.ValueStart;
                    break;
                case ReaderState.ValueEnd:
                    if (symbol == ',')
                        _readerState = ReaderState.KeyStart;
                    if (symbol == '}')
                        _readerState = ReaderState.EntryEnd;
                    break; 
                    #endregion
            }            

            if (_readerState == ReaderState.KeyEnd)
            {
                _currentKey.Append(_buffer.ToString());
                _buffer.Length = 0;
            }
            if (_readerState == ReaderState.ValueEnd)
            {
                _currentValue.Append(_buffer.ToString());
                _buffer.Length = 0;

                _currentEntry.Add(_currentKey.ToString(), _currentValue.ToString());
                _currentKey.Length = 0;
                _currentValue.Length = 0;
            }
            if (_readerState == ReaderState.EntryEnd && currentState != _readerState) // only if changed
            {
                _entries.Push(_currentEntry);
                _currentEntry = new NameValueCollection();
                return true;
            }
            return false;
        }

        public NameValueCollection PopEntry()
        {
            return _entries.Pop();
        }
    }
}