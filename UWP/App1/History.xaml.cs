using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class History : Page
    {


        string history;

        public History()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            history = e.Parameter as string;
            
            ObservableCollection<MidiFile> collection = new ObservableCollection<MidiFile>();


            string[] result = history.Split('\n');

            foreach(string s in result)
            {
                collection.Add(new MidiFile() { name = s });
            }

            collection = new ObservableCollection<MidiFile>(collection.Reverse());

            melodyHistory.ItemsSource = collection;


        }


    }
}
