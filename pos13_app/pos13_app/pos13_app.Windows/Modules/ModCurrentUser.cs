using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using pos13_app.Models;
using pos13_app.pos13_app_services;

namespace pos13_app.Modules
{
    class ModCurrentUser
    {
        
        private SysCurrentUser CurrentUser;

        public Pos13_app_serviceClient service;

        public string LoggedUser;
        public async void ReadCurrentUser()
        {
            CurrentUser = new SysCurrentUser();

            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("SysCurrent");

            if (file == null) return;
            IRandomAccessStream inStream = await file.OpenReadAsync();
            // Deserialize the Session State.
            DataContractSerializer serializer = new DataContractSerializer(typeof(SysCurrentUser));
            var CurrentUserContent = (SysCurrentUser)serializer.ReadObject(inStream.AsStreamForRead());
            inStream.Dispose();

            LoggedUser =  "You are logged in as " + CurrentUserContent.UserName + " - " + CurrentUserContent.UserId;
        }

        public async void SaveCurrentUser(string UserName,string Password)
        {
            CurrentUser = new SysCurrentUser();
            service = new Pos13_app_serviceClient();


            var userId =  await service.getUserIdAsync(UserName, Password);
            var userName =  await service.getUserNameAsync(userId);

            CurrentUser.UserId = int.Parse(userId.ToString());
            CurrentUser.UserName = userName;


            StorageFolder localFolderSysCurrent = ApplicationData.Current.LocalFolder;

            StorageFile sysCurrentFile = await localFolderSysCurrent.CreateFileAsync("SysCurrent", CreationCollisionOption.ReplaceExisting);
            IRandomAccessStream sysCurrentFileRaStream = await sysCurrentFile.OpenAsync(FileAccessMode.ReadWrite);

            using (IOutputStream outStream = sysCurrentFileRaStream.GetOutputStreamAt(0))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(SysCurrentUser));
                serializer.WriteObject(outStream.AsStreamForWrite(), CurrentUser);
                await outStream.FlushAsync();
            }

        }
    }
}
