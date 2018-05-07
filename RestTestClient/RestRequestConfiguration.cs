//-----------------------------------------------------------------------
// <copyright file="RestRequestConfiguration.cs" company="https://github.com/jhueppauff/RestTestClient">
// Copyright 2018 Jhueppauff
// MIT
// For licence details visit https://github.com/jhueppauff/RestTestClient/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace RestTestClient
{
    using System.Collections.Generic;

    /// <summary>
    /// Rest Request Config for FileSave
    /// </summary>
    public class RestRequestConfiguration
    {
        /// <summary>
        /// Gets or sets the rest headers.
        /// </summary>
        public List<RestHeader> RestHeaders { get; set; }

        /// <summary>
        /// Gets or sets the endpoint URL.
        /// </summary>
        public string EndpointUrl { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        public RestSharp.Method Method { get; set; }
    }
}