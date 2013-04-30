using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Web_Service;

namespace RestService.IO_Messages
{
    public class Request
    {
        /// <summary>
        /// The uri of the call
        /// </summary>
        public string uri {get; set;}

        /// <summary>
        /// The method of the call
        /// </summary>
        public RestMethods method { get; set; }

        /// <summary>
        /// The data from the call
        /// </summary>
        public Dictionary<string, string> data;

        /// <summary>
        /// The user who made the call
        /// </summary>
        public User user {get; set;}

        //Contructor
        public Request(string uri, RestMethods method, Dictionary<string, string> data, User user)
        {
            this.uri = uri;
            this.method = method;
            this.data = data;
            this.user = user;
        }
    }
}