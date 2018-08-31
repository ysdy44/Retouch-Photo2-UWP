using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Models
{
  public  class PhotoFile: INotifyPropertyChanged
    {

        /// <summary>文本 </summary>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        private string text;




        /// <summary>图片变暗 </summary>
        public Visibility DarkenVisibility
        {
            get => darkenVisibility;
            set
            {
                darkenVisibility = value;
                OnPropertyChanged(nameof(DarkenVisibility));
            }
        }
        private Visibility darkenVisibility = Visibility.Collapsed;

        /// <summary>指针事件 </summary>
        public void Entered(object sender, PointerRoutedEventArgs e) => DarkenVisibility = Visibility.Visible;
        public void Exited(object sender, PointerRoutedEventArgs e) => DarkenVisibility = Visibility.Collapsed;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
