using System;
using System.Collections.Generic;
using RestSharp.Authenticators;
using System.Text;

namespace Utilities.Webservice
{
    interface IRequestManager<T>
    {
        void SetCredentials(string username, string password, AuthenticationTypes type);
        void AddHeader(string name, string value);
        void AddJsonBody(object obj);
        T Call(string endpoint, string resource, RestSharp.Method methodType);
    }
}
