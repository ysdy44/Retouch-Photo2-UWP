using FanKit.Transformers;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SettingViewModel" />. 
    /// </summary>
    public partial class SettingViewModel : INotifyPropertyChanged
    {
               
        /// <summary> Scaling around the center. </summary>
        public bool IsCenter
        {
            get => this.isCenter;
            set
            {
                this.isCenter = value;
                this.OnPropertyChanged(nameof(this.IsCenter));//Notify 
            }
        }
        private bool isCenter;

        /// <summary> Maintain a ratio when scaling. </summary>
        public bool IsRatio
        {
            get => this.isRatio;
            set
            {
                this.isRatio = value;
                this.OnPropertyChanged(nameof(this.IsRatio));//Notify 
            }
        }
        private bool isRatio;

        /// <summary> Equal width and height. </summary>
        public bool IsSquare
        {
            get => this.isSquare;
            set
            {
                this.isSquare = value;
                this.OnPropertyChanged(nameof(this.IsSquare));//Notify 
            }
        }
        private bool isSquare;

        /// <summary> Step Frequency when spinning. </summary>
        public bool IsStepFrequency
        {
            get => this.isStepFrequency;
            set
            {
                this.isStepFrequency = value;
                this.OnPropertyChanged(nameof(this.IsStepFrequency));//Notify 
            }
        }
        private bool isStepFrequency;

        /// <summary> Mode of composite between layers. </summary>
        public MarqueeCompositeMode CompositeMode
        {
            get => this.compositeMode;
            set
            {
                if (this.compositeMode == value) return;
                this.compositeMode = value;              
                this.OnPropertyChanged(nameof(this.CompositeMode));//Notify 
            }
        }
        private MarqueeCompositeMode compositeMode;


        /// <summary> Sets or Gets the page layout is full screen. </summary>
        public bool IsFullScreen
        {
            get => this.isFullScreen;
            set
            {
                this.isFullScreen = value;
                this.OnPropertyChanged(nameof(this.IsFullScreen));//Notify 
            }
        }
        private bool isFullScreen;


    }
}