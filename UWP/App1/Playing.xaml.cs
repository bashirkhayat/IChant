using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using MidiSharp;
using MidiSharp.Events;
using MidiSharp.Events.Meta;
using MidiSharp.Events.Voice.Note;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>


    public sealed partial class Playing : Page
    {
        StorageFile file;
        MidiSpeed midiSpeed;
        bool stop_flag = false;


        public Playing()
        {
            this.InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            stop_flag = false;
            midiSpeed = e.Parameter as MidiSpeed;
            file = midiSpeed.storageFile;

            if (file != null)
            {
                tbError.Text = string.Empty;
                using (Stream inputStream = await file.OpenStreamForReadAsync())
                {
                    MidiSequence seq = MidiSequence.Open(inputStream);
                    if (midiSpeed.speed < 0)
                    {
                        await sendNote(1);
                        foreach (MidiTrack track in seq)
                        {
                            if (stop_flag)
                                break;
                            foreach (MidiEvent ev in track.Events)
                            {
                                if (stop_flag)
                                    break;
                                if (ev.GetType().Equals(typeof(OnNoteVoiceMidiEvent)))
                                {
                                    OnNoteVoiceMidiEvent onEvent = ev as OnNoteVoiceMidiEvent;
                                    byte note = onEvent.Note;
                                    await sendNote(note);
                                    int b = await getSignal();
                                    if (b < 0){
                                        tbError.Text = "signal failed";
                                    }
                                    if (b > 0)
                                    {
                                        midiSpeed.playerEntity.score += b - 48;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        await sendNote(2);
                        int division = seq.Division;
                        long tempo = 500000;
                        foreach (MidiTrack track in seq)
                        {
                            if (stop_flag)
                                break;
                            foreach (MidiEvent ev in track.Events)
                            {
                                if (stop_flag)
                                    break;
                                if (ev.GetType().Equals(typeof(OnNoteVoiceMidiEvent)))
                                {
                                    OnNoteVoiceMidiEvent onEvent = ev as OnNoteVoiceMidiEvent;

                                    double bpm = 60000000 / tempo;
                                    double delay = ((60 * ev.DeltaTime) / (bpm * division));
                                    delay *= midiSpeed.speed;
                                    await Task.Delay(TimeSpan.FromSeconds(delay));
                                    byte note = onEvent.Note;
                                    await sendNote(note);
                                }
                                else if (ev.GetType().Equals(typeof(TempoMetaMidiEvent)))
                                {
                                    TempoMetaMidiEvent tempoEvent = ev as TempoMetaMidiEvent;
                                    tempo = tempoEvent.Value;
                                }
                            }
                        }
                    }
                    // we've finished sending notes.
                }
            }
            else
            {
                tbError.Text = "File was not chosen";
                return;
            }
            await file.DeleteAsync();
            await sendNote(0);
            this.Frame.Navigate(typeof(MainPage), midiSpeed.playerEntity);
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

        private async Task<int> getSignal()
        {
            try
            {
                IAsyncOperation<uint> taskLoad = App.bluetoothConnect.dataReader.LoadAsync(1);
                await taskLoad.AsTask();
                if (taskLoad.GetResults() != 1)
                {
                    tbError.Text = "Connection lost";
                    return -1; // the socket was closed before reading.
                }
                byte b = App.bluetoothConnect.dataReader.ReadByte();
                return b;
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;

                return -1;
            }
        }



        private void StopPlaying_Click(object Sender, RoutedEventArgs e)
        {
            //update();
            //this.Frame.Navigate(typeof(MainPage),midiSpeed.playerEntity);
            stop_flag = true;
        }

        private async void update()
        {
            await App.bluetoothConnect.dataWriter.FlushAsync();
             await sendNote(0);
        }

    }
}
