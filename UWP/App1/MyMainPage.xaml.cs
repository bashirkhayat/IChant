using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyMainPage : Page
    {
        CloudTable table;
        //PlayerEntity player;

        public MyMainPage()
        {
            this.InitializeComponent();

            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=usernames;AccountKey=2+tATd/c7KeiVQ69GkTfYXCHCx4wn5ZXvI+oyS49alCZNcSVn+qTB7W/zElHmI/RjGseiefCbfLcHq/QHUbMmQ==;EndpointSuffix=core.windows.net");

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            table = tableClient.GetTableReference("usernames");
            // Create the table if it doesn't exist.
            table.CreateIfNotExistsAsync();

        }

        async private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (signInName.Text == "")
            {
                var dialog = new MessageDialog("Enter a valid username");
                await dialog.ShowAsync();
            }
            if (signInPassword.Password == "")
            {
                var dialog = new MessageDialog("Enter a valid password");
                await dialog.ShowAsync();
            }
            else
            {
               /* TableOperation retrieveOperation = TableOperation.Retrieve<PlayerEntity>(signInName.Text, signInName.Text);
                TableResult result = await table.ExecuteAsync(retrieveOperation);
                if (result.Result == null)
                {
                    var dialog = new MessageDialog("Wrong username");
                    await dialog.ShowAsync();
                }*/
                TableQuery<PlayerEntity> query = new TableQuery<PlayerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, signInName.Text));
                TableQuerySegment<PlayerEntity> queryResult = await table.ExecuteQuerySegmentedAsync(query, null);
               
                foreach (PlayerEntity entity in queryResult)
                {
                    if (entity.playerPassword == signInPassword.Password)
                    {
                        this.Frame.Navigate(typeof(MainPage), entity);
                        return;
                    }
                    else
                    {
                        var dialog = new MessageDialog("Wrong password");
                        await dialog.ShowAsync();
                        return;
                    }
                }
                var dialog1 = new MessageDialog("Wrong username");
                await dialog1.ShowAsync();
            }

        }
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignUp));
        }
    }
}
