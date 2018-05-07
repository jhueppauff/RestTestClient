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
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using Microsoft.Win32;
    using Newtonsoft.Json;
    using RestSharp;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The rest headers
        /// </summary>
        private List<RestHeader> restHeaders = new List<RestHeader>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.LoadMethods();

            this.DtgHeader.ItemsSource = this.restHeaders;
        }

        /// <summary>
        /// Loads the methods into the combo box.
        /// </summary>
        private void LoadMethods()
        {
            this.CbxMethod.ItemsSource = Enum.GetValues(typeof(Method)).Cast<Method>();
        }

        /// <summary>
        /// Handles the Click event of the Button Submit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (this.CbxMethod.SelectedItem == null)
            {
                MessageBox.Show("Please select a RestMethod", "Missing Method", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IRestResponse response = null;

            try
            {
                RestClient restClient = new RestClient();
                
                response = restClient.ExecuteRestRequest(this.TbxEndpointUrl.Text, this.restHeaders, (Method)this.CbxMethod.SelectedItem, this.TbxBody.Text).ConfigureAwait(false).GetAwaiter().GetResult();

                ResponseViewer responseViewer = new ResponseViewer(response);
                responseViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                this.log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the Click event of the MenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            this.TbxEndpointUrl.Focus();
            this.TbxBody.Text = string.Empty;
            this.CbxMethod.SelectedItem = null;
            this.restHeaders.Clear();

            this.DtgHeader.ItemsSource = this.restHeaders;
            this.DtgHeader.Items.Refresh();
            this.TbxEndpointUrl.Text = string.Empty;
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
                RestHeaders = this.restHeaders
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(requestConfiguration));
            }    
        }

        /// <summary>
        /// Handles the Click event of the Button Open control.
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
