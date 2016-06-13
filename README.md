# WinPhoneCarControler
WP app for control robocar
This app developed for Windows Phone 8.1.
Main functionality is - send data using Bluetooth to HC-06 bluetooth adapter, which are connected to stm32f3.

For now(13.06.2016) this app isn't finish, because I founded problem with method RfcommDeviceService.FromIdAsync. This method return exception and I can't continue. I found only this

https://social.msdn.microsoft.com/Forums/en-US/c2ed8563-0a3a-4479-b529-82a4ab2db8f2/why-does-rfcommdeviceservicefromidasync-return-null?forum=winappswithcsharp

but, I can't resolve my problem.
