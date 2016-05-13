using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Uri;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using WinPhoneCarControler;


namespace WinPhoneCarControler
{
    class General
    {
        ///<summary>
        ///Open bluetooth setting page
        ///</summary>
        public async void bluetoothSettingsOpen()
        {
            string uriToLaunch = "ms-settings-bluetooth:";
            var uri = new System.Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }
        /// <summary>
        /// Control methods
        /// </summary>
        private void Up()
        {
            string command = "8";
            MainPage O = new MainPage();
            O.CommandSend(command);
        }

        private void Down()
        {
            string command = "2";
            MainPage O = new MainPage();
            O.CommandSend(command);
        }

        private void Left()
        {
            string command = "4";
            MainPage O = new MainPage();
            O.CommandSend(command);
        }

        private void Right()
        {
            string command = "6";
            MainPage O = new MainPage();
            O.CommandSend(command);
        }

    }
}
