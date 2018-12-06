using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;

namespace Retouch_Photo.Library
{
    class Debug
    {
        /// <summary>
        /// 把一个类转化成json并打开的方法
        /// </summary>
        /// <param name="obj">类</param>
        /// <param name="fileName">文件名</param>
        public static async void SerializeObject(object obj, string fileName = "demo.json")
        {
            string jsonCollection = JsonConvert.SerializeObject(obj);

            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, jsonCollection);
            }
            catch (Exception) { }

            await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
        }


        /// <summary>
        /// 消息框弹出类的内容
        /// </summary>
        /// <param name="obj">类</param>
        public static async void Dialog(object obj)
        {
            Type type = obj.GetType();

            //标题
            string title = type.ToString();

            //内容
            string Content = string.Empty;
            foreach (PropertyInfo p in type.GetProperties()) Content += p.Name + "：" + p.GetValue(obj) + "\n";

            //窗口
            MessageDialog msgDialog = new MessageDialog(Content) { Title = title };
            msgDialog.Commands.Add(new UICommand("Back"));
            await msgDialog.ShowAsync();
        }



    }
}
