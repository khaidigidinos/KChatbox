using System;
using System.Collections.Generic;

namespace SignalRApi.Exceptions
{
    public class ValidationException : Exception
    {
        private readonly Dictionary<string, List<string>> _errorDict;
        public Dictionary<string, List<string>> ErrorDict => _errorDict ?? new Dictionary<string, List<string>>();

        public ValidationException(Dictionary<string, List<string>> errorDict)
        {
            _errorDict = errorDict;
        }
    }
}
