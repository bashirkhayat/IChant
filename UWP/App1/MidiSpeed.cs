using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;

namespace App1
{
    internal class MidiSpeed
    {
        public double speed { get; set; }
        public PlayerEntity playerEntity { get; set; }
        public StorageFile storageFile { get; set; }
    }
}