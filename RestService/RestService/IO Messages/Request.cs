using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Web_Service;
using RestService.Entities;

namespace RestService.IO_Messages
{
    public class Request
    {
        /// <summary>
        /// The uri of the call
        /// </summary>
        public LinkedList<string> uri {get; set;}

        /// <summary>
        /// The method of the call
        /// </summary>
        public RestMethods method { get; set; }

        /// <summary>
        /// The data from the call
        /// </summary>
        public Dictionary<string, string> data;

        /// <summary>
        /// The authorization string
        /// </summary>
        public string authorization;

        /// <summary>
        /// The user who made the call
        /// </summary>
        public User user {get; set;}

        //Contructor
        public Request(LinkedList<string> uri, RestMethods method, Dictionary<string, string> data, User user, string authorization)
        {
            this.uri = uri;
            this.method = method;
            this.data = data;
            this.user = user;
            this.authorization = authorization;
        }
    }
}