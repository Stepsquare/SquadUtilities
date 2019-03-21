using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Utilities.Webservice;

namespace Utilities.Webservice
{
    /// <summary>
    /// T is a class of the wanted return type. Models/Webservice/Outputs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestManager<T> : IRequestManager<T>
    {
        #region PRIVATE VARIABLES
        private RestRequest Request = new RestRequest();
        private string Username = "";
        private string Password = "";
        private AuthenticationTypes AuthType;
        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Define credentials for the service.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void SetCredentials(string username, string password, AuthenticationTypes type)
        {
            Username = username;
            Password = password;
            AuthType = type;
        }

        /// <summary>
        /// AddHeader do the request.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddHeader(string name, string value)
        {
            Request.AddHeader(name, value);
        }

        /// <summary>
        /// AddJsonBody to the request.
        /// </summary>
        /// <param name="obj"></param>
        public void AddJsonBody(object obj)
        {
            Request.AddJsonBody(obj);
        }

        /// <summary>
        /// Makes a call.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="methodType"></param>
        /// <returns></returns>
        public T Call(string endpoint, string resource, RestSharp.Method methodType)
        {
            var client = new RestClient(endpoint);

            switch(AuthType)
            {
                case AuthenticationTypes.BASIC:
                    client.Authenticator = new HttpBasicAuthenticator(Username, Password);
                    break;
                case AuthenticationTypes.NTLM:
                    client.Authenticator = new NtlmAuthenticator(Username, Password);
                    break;
            }
                
            
            Request.Method = methodType;
            Request.Resource = resource;

            //RestResponse<T> response = (RestResponse<T>)client.Execute<T>(Request);
            var response = client.Execute(Request);

            T result = JsonConvert.DeserializeObject<T>(response.Content);

            return result;
        }
        #endregion

    }
}
