using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUp : Page
    {

        CloudTable table;

        public SignUp()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested +=
    App_BackRequested;
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=usernames;AccountKey=2+tATd/c7KeiVQ69GkTfYXCHCx4wn5ZXvI+oyS49alCZNcSVn+qTB7W/zElHmI/RjGseiefCbfLcHq/QHUbMmQ==;EndpointSuffix=core.windows.net");

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            table = tableClient.GetTableReference("usernames");
            // Create the table if it doesn't exist.
            table.CreateIfNotExistsAsync();
            
        }

        private void App_BackRequested(object sender,
Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {

            if (signUpName.Text == "")
            {
                var dialog = new MessageDialog("Enter a valid username");
                await dialog.ShowAsync();
            }
            if (signUpPassword.Password == "")
            {
                var dialog = new MessageDialog("Enter a valid password");
                await dialog.ShowAsync();
            }
            if (signUpConfirmPassword.Password == "")
            {
                var dialog = new MessageDialog("confirm your password!");
                await dialog.ShowAsync();
            }
            if (signUpPassword.Password != signUpConfirmPassword.Password)
            {
                var dialog = new MessageDialog("The passwords you gave don't match, try again!");
                await dialog.ShowAsync();
            }

            TableOperation retrieveOperation = TableOperation.Retrieve<PlayerEntity>(signUpName.Text, signUpName.Text);

            TableResult result = await table.ExecuteAsync(retrieveOperation);
            if (result.Result != null)
            {
                var dialog = new MessageDialog("Username is already taken");
                await dialog.ShowAsync();
                return;
            }

            // Create a new player entity.
            PlayerEntity player = new PlayerEntity(signUpName.Text, signUpPassword.Password);
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(player);
            // Execute the insert operation.
            await table.ExecuteAsync(insertOperation);
            this.Frame.Navigate(typeof(MyMainPage));
        }

    }
}
