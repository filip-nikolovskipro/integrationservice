using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorker.Authentication
{
    public class AuthToken
    {
        public int id { get; set; }
        public string token { get; set; }
        public Nullable<bool> extendSession { get; set; }
    }

    public class ObjectWrapper<T>
    {
        public T AuthToken { get; set; }
    }
}
