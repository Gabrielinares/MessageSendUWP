using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MessageSendApp.ViewModels;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Windows.Media.Protection.PlayReady;
using MessageSendApp.Models;
using Windows.UI.Popups;

namespace MessageSendApp
{
    public sealed partial class MainPage : Page
    {
        private readonly Uri apiURL = new Uri("https://localhost:7184/");

        public MainPage()
        {
            this.InitializeComponent();

            LoadMessagesAsync();
        }

        #region Load data from API
        private async void LoadMessagesAsync()
        {
            var messages = await GetMessagesAsync();
            if (messages != null)
            {
                MessagesListView.ItemsSource = messages; // Set data to ListView
            }
        }

        private async Task<List<MessageViewModel>> GetMessagesAsync()
        {
            using (var handler = new HttpClientHandler())
            {
                // Certificates are not validated
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = apiURL;
                    try
                    {
                        // Get request
                        var response = await client.GetAsync("Messages");

                        if (response.IsSuccessStatusCode)
                        {
                            // Read response
                            var json = await response.Content.ReadAsStringAsync();
                            // Deserialize JSON to List<MessageViewModel>
                            return JsonConvert.DeserializeObject<List<MessageViewModel>>(json);
                        }
                        else
                        {
                            // Error handling
                            System.Diagnostics.Debug.WriteLine($"Error: {response.StatusCode}");
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        // General exception handling
                        System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                        return null;
                    }
                }
            }
        }
        #endregion

        #region Send Message to API
        // Click event for Send button
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Separate phone numbers by comma
            var phoneNumbers = ToTextBox.Text.Split(',');
            List<string> wrongNumbers = new List<string>();

            // Eliminate white spaces
            phoneNumbers = phoneNumbers.Select(x => x.Trim()).ToArray();

            // Validate input message
            if (string.IsNullOrEmpty(MessageTextBox.Text))
            {
                var messageDialog = new MessageDialog("Message is required", "Alert");
                messageDialog.Commands.Add(new UICommand("OK"));
                await messageDialog.ShowAsync();
                return;
            }

            foreach (var phoneNumber in phoneNumbers)
            {
                // Validate phone number (to)
                if (!phoneNumber.StartsWith('+'))
                {
                    wrongNumbers.Add(phoneNumber);
                }
            }

            if(wrongNumbers.Count > 0)
            {
                var messageDialog = new MessageDialog("Invalid phone numbers: " + string.Join(", ", wrongNumbers), "Alert");
                messageDialog.Commands.Add(new UICommand("OK"));
                await messageDialog.ShowAsync();
                return;
            }

            using (var handler = new HttpClientHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = apiURL;

                    foreach (var phoneNumber in phoneNumbers)
                    {
                        // Create new MessageViewModel object
                        var message = new Message
                        {
                            Id = 0,
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            To = phoneNumber,
                            MessageBody = MessageTextBox.Text
                        };

                        // Serialize object to JSON
                        var json = JsonConvert.SerializeObject(message);

                        // Create HttpContent object
                        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                        try
                        {
                            // Post request
                            var response = await client.PostAsync("Messages", content);

                            if (response.IsSuccessStatusCode)
                            {
                                // Reload messages
                                LoadMessagesAsync();

                                // Clean input fields
                                ToTextBox.Text = "";
                                MessageTextBox.Text = "";
                            }
                            else
                            {
                                // Error handling
                                System.Diagnostics.Debug.WriteLine($"Error: {response.StatusCode}");
                            }
                        }
                        catch (Exception ex)
                        {
                            // General exception handling
                            System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                        }
                    }
                }
            }
        }
        #endregion

        #region Orderable table
        private async void orderTo_Click(object sender, RoutedEventArgs e)
        {
            var messages = await GetMessagesAsync();
            if (messages != null)
            {
                MessagesListView.ItemsSource = messages.OrderBy(x => x.To); // Set data to ListView
            }
        }
        private async void orderMessage_Click(object sender, RoutedEventArgs e)
        {
            var messages = await GetMessagesAsync();
            if (messages != null)
            {
                MessagesListView.ItemsSource = messages.OrderBy(x => x.Message); // Set data to ListView
            }
        }

        private async void orderDate_Click(object sender, RoutedEventArgs e)
        {
            var messages = await GetMessagesAsync();
            if (messages != null)
            {
                MessagesListView.ItemsSource = messages.OrderBy(x => x.DateSent); // Set data to ListView
            }
        }

        private async void orderStatus_Click(object sender, RoutedEventArgs e)
        {
            var messages = await GetMessagesAsync();
            if (messages != null)
            {
                MessagesListView.ItemsSource = messages.OrderBy(x => x.Status); // Set data to ListView
            }
        }
        #endregion
    }
}
