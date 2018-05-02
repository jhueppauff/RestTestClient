﻿//-----------------------------------------------------------------------
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
    /// Interaction logic for ResponseViewer.xaml
    /// </summary>
    public partial class ResponseViewer : Window
    {
        /// <summary>
        /// The response text
        /// </summary>
        private string response;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseViewer"/> class.
        /// </summary>
        /// <param name="response">The response text.</param>
        public ResponseViewer(string response, Exception exception = null)
        {
            InitializeComponent();
            this.response = response;
            TbxResponse.Text = response;

            if (exception != null)
            {
                TbxResponse.Text += "\n\nStackTrace:\n" + exception.StackTrace;
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}