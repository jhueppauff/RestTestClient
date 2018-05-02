namespace RestTestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
