using System;
using System.Linq;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    public sealed partial class MainPage : Page
    {
        public double _speed = 1;
        CloudTable table;
        public PlayerEntity player;

        public MainPage()
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            player = e.Parameter as PlayerEntity;

            update();
            UserBlock.Text = "Welcome back " + player.playerName + " !";
            UserScore.Text = "Your score is " + player.score.ToString() + " !";
            if (App.bluetoothConnect.connected)
            {
                tbConnectStatus.Text = "Connected";
            }

        }

        async void update()
        {

            /*****************************************************************************************************************/
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
                updateEntity.score = player.score;

                // Create the Replace TableOperation.
                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                // Execute the operation.
                await table.ExecuteAsync(updateOperation);
            }

            /*****************************************************************************************************************/

        }


        private void practice_Click(object sender,
                                 RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Practice), new MidiSpeed { speed = _speed, playerEntity = player });

        }

        private void logOut_Click(object sender,
                                 RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MyMainPage));
        }

        private void chooseFile_Click(object sender,
                                         RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ChooseFile), new MidiSpeed { speed = _speed, playerEntity = player });
        }

        private void history_Click(object sender,
                         RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(History), player.history);
        }

        private async void btnConnect_Click(object sender,
                                    RoutedEventArgs e)
        {
            tbError.Text = string.Empty;
            App.bluetoothConnect.connected = true;

            try
            {
                var devices =
                      await DeviceInformation.FindAllAsync(
                        RfcommDeviceService.GetDeviceSelector(
                          RfcommServiceId.SerialPort));

                String deviceName = "smartChanter";

                var device = devices.Single(x => x.Name == deviceName);

                App.bluetoothConnect.service = await RfcommDeviceService.FromIdAsync(
                                                        device.Id);

                App.bluetoothConnect.socket = new StreamSocket();

                await App.bluetoothConnect.socket.ConnectAsync(
                      App.bluetoothConnect.service.ConnectionHostName,
                      App.bluetoothConnect.service.ConnectionServiceName,
                      SocketProtectionLevel.
                      BluetoothEncryptionAllowNullAuthentication);
                App.bluetoothConnect.dataReader = new DataReader(App.bluetoothConnect.socket.InputStream);
                App.bluetoothConnect.dataWriter = new DataWriter(App.bluetoothConnect.socket.OutputStream);
                tbConnectStatus.Text = "Connected";
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            switch (selectedItem.Name)
            {
                case "cbNormal":
                    _speed = 1;
                    break;
                case "cb75":
                    _speed = (3 / 2);
                    break;
                case "cb5":
                    _speed = 2;
                    break;
                case "cb25":
                    _speed = (5 / 2);
                    break;
                case "cbClick":
                    _speed = -1;
                    break;
            }
        }
    }
}
