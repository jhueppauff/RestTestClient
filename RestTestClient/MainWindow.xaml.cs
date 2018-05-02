//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="https://github.com/jhueppauff/RestTestClient">
// Copyright 2018 Jhueppauff
// MIT
// For licence details visit https://github.com/jhueppauff/RestTestClient/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace RestTestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using RestSharp;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The rest headers
        /// </summary>
        private List<RestHeader> restHeaders = new List<RestHeader>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LoadMethods();

            DtgHeader.ItemsSource = restHeaders;
        }

        /// <summary>
        /// Loads the methods into the combobox.
        /// </summary>
        private void LoadMethods()
        {
            CbxMethod.ItemsSource = Enum.GetValues(typeof(Method)).Cast<Method>();
        }

        /// <summary>
        /// Handles the Click event of the BtnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (CbxMethod.SelectedItem == null)
            {
                MessageBox.Show("Please select a RestMethod", "Missing Method", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IRestResponse response = null;

            try
            {
                RestClient restClient = new RestClient();
                
                response = restClient.ExecuteRestRequest(TbxEndpointUrl.Text, restHeaders, (Method)CbxMethod.SelectedItem, TbxBody.Text).ConfigureAwait(false).GetAwaiter().GetResult();

                ResponseViewer responseViewer = new ResponseViewer(response.Content);
                responseViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                ResponseViewer responseViewer = new ResponseViewer(ex.Message, ex);
                responseViewer.ShowDialog();
            }
        }
    }
}
