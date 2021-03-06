﻿//-----------------------------------------------------------------------
// <copyright file="RestClient.cs" company="https://github.com/jhueppauff/RestTestClient">
// Copyright 2018 Jhueppauff
// MIT
// For licence details visit https://github.com/jhueppauff/RestTestClient/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace RestTestClient
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using RestSharp;

    /// <summary>
    /// Rest Client Class.
    /// </summary>
    public class RestClient
    {
        /// <summary>
        /// Executes the rest request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="restHeaders">The rest headers.</param>
        /// <param name="method">The Rest method.</param>
        /// <param name="body">The body.</param>
        /// <returns>Returns the <see cref="IRestResponse"/></returns>
        public async Task<IRestResponse> ExecuteRestRequest(string url, List<RestHeader> restHeaders, Method method, string body = null)
        {
            RestSharp.RestClient restClient = new RestSharp.RestClient(url);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            RestRequest restRequest = new RestRequest(method)
            {
                Timeout = Properties.Settings.Default.Timeout,
                ReadWriteTimeout = Properties.Settings.Default.Timeout
            };

            foreach (RestHeader restHeader in restHeaders)
            {
                restRequest.AddHeader(restHeader.KeyName, restHeader.KeyValue);
            }

            if (body != null)
            {
                restRequest.AddParameter("undefined", body, ParameterType.RequestBody);
            }

            IRestResponse restResponse = await restClient.ExecuteTaskAsync(restRequest, cancellationTokenSource.Token).ConfigureAwait(false);

            return restResponse;
        }
    }
}