//-----------------------------------------------------------------------
// <copyright file="ResponseViewer.xaml.cs" company="https://github.com/jhueppauff/RestTestClient">
// Copyright 2018 Jhueppauff
// MIT
// For licence details visit https://github.com/jhueppauff/RestTestClient/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace RestTestClient
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ResponseViewer
    /// </summary>
    public partial class ResponseViewer : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseViewer"/> class.
        /// </summary>
        /// <param name="response">The response text.</param>
        /// <param name="exception">Thrown Exception</param>
        public ResponseViewer(RestSharp.IRestResponse response, Exception exception = null)
        {
            this.InitializeComponent();
            this.TbxResponse.Text += $"Statuscode: {response.StatusDescription}\n";
            this.TbxResponse.Text += response.Content;

            if (exception != null)
            {
                this.TbxResponse.Text += "\n\nStackTrace:\n" + exception.StackTrace;
            }
        }

        /// <summary>
        /// Handles the Click event of the Button Close control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
