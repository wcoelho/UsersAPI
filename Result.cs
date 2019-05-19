using System.Collections.Generic;

namespace UsersAPI
{
    public class Result
    {
        public string Action { get; set; }

        public bool Success
        {
            get { return _Inconsistencies == null || Inconsistencies.Count == 0; }
        }

        private List<string> _Inconsistencies = new List<string>();
        public List<string> Inconsistencies
        {
            get { return _Inconsistencies; }
        }
    }
}