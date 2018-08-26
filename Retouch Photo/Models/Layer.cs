using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models
{
    public class Layer: INotifyPropertyChanged
    {

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private double opacity = 100;
        public double Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }

        private bool isVisual = true;
        public bool IsVisual
        {
            get => isVisual;
            set
            {
                isVisual = value;
                OnPropertyChanged(nameof(IsVisual));
            }
        }

        private int blendIndex;
        public int BlendIndex
        {
            get => blendIndex;
            set
            {
                blendIndex = value;
                OnPropertyChanged(nameof(BlendIndex));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
