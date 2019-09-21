using Newtonsoft.Json;
using Retouch_Photo2.Elements;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SettingViewModel" />. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SettingViewModel
    {
        public const int DefaultLayoutPhoneMaxWidth = 600;
        public const int DefaultLayoutPadMaxWidth = 900;


        //Theme
        [JsonProperty]
        public ElementTheme ElementTheme { get; set; } = ElementTheme.Default;


        //Layout
        [JsonProperty]
        public DeviceLayoutType LayoutDeviceType { get; set; } = DeviceLayoutType.Adaptive;

        [JsonProperty]
        public int LayoutPhoneMaxWidth { get; set; } = 600;

        [JsonProperty]
        public int LayoutPadMaxWidth { get; set; } = 900;


        public static async Task<SettingViewModel> CreateFromLocalFile()
        {
            string json = await ApplicationLocalTextFileUtility.ReadFromLocalFolder("Setting.json");
            
            if (json != null)
            {
                SettingViewModel setting = JsonConvert.DeserializeObject<SettingViewModel>(json);
                return setting;
            }

            return null;
        }

        public async void WriteToLocalFolder()
        {
            string json = JsonConvert.SerializeObject(this);
            await ApplicationLocalTextFileUtility.WriteToLocalFolder(json, "Setting.json");
        }

    }
}