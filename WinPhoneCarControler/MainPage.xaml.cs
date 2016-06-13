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
using Windows.UI.Popups;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
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
        public async void TestConntection(object sender, RoutedEventArgs e)
        {
            string deviceName = "HC-06";
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = ""; // Grab Paired Devices
            var PF = await PeerFinder.FindAllPeersAsync(); // Store Paired Devices
            StreamSocket BTSock = new StreamSocket();

          
           var devicesInfoCollection = await DeviceInformation.FindAllAsync();

            var device = devicesInfoCollection.FirstOrDefault(d => d.Name.ToUpper().Contains(deviceName));
            string idDevice = device.Id;
            var BTService = await RfcommDeviceService.FromIdAsync(idDevice);
 //          string tmp = @"\?\BTHENUM#{00001101-0000-1000-8000-00805f9b34fb}_LOCALMFG&001d#6&268da77&0&201512225156_C00000000#{b142fc3e-fa4e-460b-8abc-072b628b3c70}";

            for (int i = 0; i <= PF.Count; i++)
           {
               if (PF[i].DisplayName == deviceName)
               {
                   try
                   {
                       await BTSock.ConnectAsync(PF[i].HostName, BTService.ConnectionServiceName);
//                       break;
                   }
                   catch (Exception ex)
                   {
                       MessageDialog msgbox = new MessageDialog("Exception on sending : " + ex);
                   }
               }
           }
           
            statusField.Text = "HC-06 is connected";
            

            try
            {
            var datab = GetBufferFromByteArray(Encoding.UTF8.GetBytes("3")); // Create Buffer/Packet for Sending
            var w = await BTSock.OutputStream.WriteAsync(datab); // Send Arduino Buffer/Packet Message
             }

            catch (Exception ex)
            {
   //             MessageBox.Show("Exception on sending : {0}", "Error", MessageBoxButton.OK, ex);
                MessageDialog msgbox = new MessageDialog ("Exception on sending : " +ex);
            }
        }

        public async void CommandSend(string command)
        {
            StreamSocket BTSock = new StreamSocket();

            var datab = GetBufferFromByteArray(Encoding.UTF8.GetBytes(command)); // Create Buffer/Packet for Sending
            try 
            { 
            var w = await BTSock.OutputStream.WriteAsync(datab); // Send Arduino Buffer/Packet Message
                }
            catch (Exception ex)
            {
                //             MessageBox.Show("Exception on sending : {0}", "Error", MessageBoxButton.OK, ex);
                MessageDialog msgbox = new MessageDialog("Exception on sending : " + ex);
            }
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            string command = "8";
            MainPage O = new MainPage();
            O.CommandSend(command);
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            string command = "2";
            MainPage O = new MainPage();
            O.CommandSend(command);
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            string command = "4";
            MainPage O = new MainPage();
            O.CommandSend(command);
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            string command = "6";
            MainPage O = new MainPage();
            O.CommandSend(command);
        }


    }
}
