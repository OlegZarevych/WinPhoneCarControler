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
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Text;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WinPhoneCarControler
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StreamSocket BTSock;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        private IBuffer GetBufferFromByteArray(byte[] package)
        {
            using (DataWriter dw = new DataWriter())
            {
                dw.WriteBytes(package);
                return dw.DetachBuffer();
            }
        }
        /// <summary>
        /// Open bluetooth settings page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BluetoothSettings_Click(object sender, RoutedEventArgs e)
        {
            General O = new General();
            O.bluetoothSettingsOpen();

        }
        /// <summary>
        /// This method connect to bluetooth device and send 'HELLO'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TestConntection(object sender, RoutedEventArgs e)
        {
            string deviceName = "HC-06";
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = ""; // Grab Paired Devices
            var PF = await PeerFinder.FindAllPeersAsync(); // Store Paired Devices
            BTSock = new StreamSocket();

            for (int i = 0; i <= PF.Count; i++)
           {
               if (PF[i].DisplayName == deviceName)
               {
                   await BTSock.ConnectAsync(PF[i].HostName, "1");
                   break;
               }
           }
            var datab = GetBufferFromByteArray(Encoding.UTF8.GetBytes("HELLO")); // Create Buffer/Packet for Sending
            await BTSock.OutputStream.WriteAsync(datab); // Send Arduino Buffer/Packet Message
        }

        public async void CommandSend(string command)
        {
            BTSock = new StreamSocket();

            var datab = GetBufferFromByteArray(Encoding.UTF8.GetBytes(command)); // Create Buffer/Packet for Sending
            await BTSock.OutputStream.WriteAsync(datab); // Send Arduino Buffer/Packet Message
        }
    }
}
