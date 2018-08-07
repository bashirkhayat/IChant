using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Practice : Page
    {

        CloudStorageAccount storageAccount;
        CloudBlobClient blobClient;
        MidiFile midiFile;
        MidiSpeed midiSpeed;
        public PlayerEntity player;

        private DisplayRequest g_DisplayRequest = null;

        public Practice()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested +=App_BackRequested;

            storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=usernames;AccountKey=2+tATd/c7KeiVQ69GkTfYXCHCx4wn5ZXvI+oyS49alCZNcSVn+qTB7W/zElHmI/RjGseiefCbfLcHq/QHUbMmQ==;EndpointSuffix=core.windows.net");
            blobClient = storageAccount.CreateCloudBlobClient();



        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            midiSpeed = e.Parameter as MidiSpeed;
            player = midiSpeed.playerEntity;
            update();

        }


        async void update()
        {

            loading.Visibility = Visibility.Visible;


            CloudBlobContainer container = blobClient.GetContainerReference("practice");

            await container.CreateIfNotExistsAsync();

            ObservableCollection<MidiFile> collection = new ObservableCollection<MidiFile>();


            BlobResultSegment result = null;

            result = await container.ListBlobsSegmentedAsync(null);


            foreach (var item in result.Results)
            {

                CloudBlockBlob blob = (CloudBlockBlob)item;
                string[] fileName = (blob.Name).Split('.');
                collection.Add(new MidiFile() { name = fileName[0] });

            }
            loading.Visibility = Visibility.Collapsed;
            practice.ItemsSource = collection;


        }

        async private void item_Click(object sender, ItemClickEventArgs e)
        {
            
            midiFile = e.ClickedItem as MidiFile;


            CloudBlobContainer container = blobClient.GetContainerReference("practice");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(midiFile.name + ".mid");

            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(midiFile.name, CreationCollisionOption.ReplaceExisting);

            await blockBlob.DownloadToFileAsync(file);


            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=usernames;AccountKey=2+tATd/c7KeiVQ69GkTfYXCHCx4wn5ZXvI+oyS49alCZNcSVn+qTB7W/zElHmI/RjGseiefCbfLcHq/QHUbMmQ==;EndpointSuffix=core.windows.net");

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("usernames");
            // Create the table if it doesn't exist.

            TableOperation retrieveOperation = TableOperation.Retrieve<PlayerEntity>(player.playerName, player.playerName);

            // Execute the operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            PlayerEntity updateEntity = (PlayerEntity)retrievedResult.Result;

            if (updateEntity != null)
            {
                updateEntity.history = updateEntity.history + midiFile.name+"\n";

                // Create the Replace TableOperation.
                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                // Execute the operation.
                await table.ExecuteAsync(updateOperation);
            }


            g_DisplayRequest = new DisplayRequest();
            g_DisplayRequest.RequestActive();


            midiSpeed.storageFile = file;
            this.Frame.Navigate(typeof(Playing), midiSpeed);
        }

        private async Task<uint> sendNote(Byte note)
        {

            tbError.Text = string.Empty;

            try
            {
                App.bluetoothConnect.dataWriter.WriteByte(note);

                var store = await App.bluetoothConnect.dataWriter.StoreAsync().AsTask();

                return store;
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;

                return 0;
            }
        }

        private async void getSignal()
        {
            try
            {
                IAsyncOperation<uint> taskLoad = App.bluetoothConnect.dataReader.LoadAsync(1);
                await taskLoad.AsTask();
                if (taskLoad.GetResults() != 1)
                {
                    tbError.Text = "Connection lost";
                    //tbConnectStatus.Text = "Disconnected";
                    return; // the socket was closed before reading.
                }
                byte b = App.bluetoothConnect.dataReader.ReadByte();
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;

                return;
            }
        }





        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
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
    }
}
