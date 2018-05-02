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
    using Newtonsoft.Json;
    using Microsoft.Win32;
    using System.IO;
    using System.Text;

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

        /// <summary>
        /// Handles the Click event of the MenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            TbxEndpointUrl.Focus();
            TbxBody.Text = string.Empty;
            CbxMethod.SelectedItem = null;
            restHeaders.Clear();

            DtgHeader.ItemsSource = restHeaders;
            DtgHeader.Items.Refresh();
            TbxEndpointUrl.Text = string.Empty;
        }

        /// <summary>
        /// Handles the exit event of the MenuItem_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the 2 event of the MenuItem_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            if (this.CbxMethod.SelectedItem == null)
            {
                MessageBox.Show("Please select a RestMethod", "Missing Method", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Json file (*.json)|*.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            RestRequestConfiguration requestConfiguration = new RestRequestConfiguration()
            {
                Body = TbxBody.Text,
                EndpointUrl = TbxEndpointUrl.Text,
                Method = (Method)CbxMethod.SelectedItem,
                RestHeaders = restHeaders
            };


            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(requestConfiguration));
            }    
        }

        /// <summary>
        /// Handles the Click event of the BtnOpen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Json file (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string json = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);

                RestRequestConfiguration requestConfiguration = JsonConvert.DeserializeObject<RestRequestConfiguration>(json);

                this.TbxBody.Text = requestConfiguration.Body;
                this.TbxEndpointUrl.Text = requestConfiguration.EndpointUrl;
                this.CbxMethod.SelectedItem = requestConfiguration.Method;
                this.restHeaders = requestConfiguration.RestHeaders;

                this.TbxEndpointUrl.Focus();
                this.DtgHeader.ItemsSource = this.restHeaders;
                this.DtgHeader.Items.Refresh();
            }
        }
    }
}
