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


    }
}
