// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents an ViewModel that contains shortcut, layout and <see cref="ViewModels.Setting"/>.
    /// </summary>
    public partial class SettingViewModel : INotifyPropertyChanged
    {
        
        /// <summary> Sets or Gets whether snaps a element to others's edge. </summary>
        public bool IsSnap
        {
            get => this.isSnap;
            set
            {
                this.isSnap = value;
                this.OnPropertyChanged(nameof(this.IsSnap));//Notify 
            }
        }
        private bool isSnap = true;

        /// <summary> Sets or Gets the on state of the ruler on the canvas. </summary>
        public bool IsRuler
        {
            get => this.isRuler;
            set
            {
                this.isRuler = value;
                this.OnPropertyChanged(nameof(this.IsRuler));//Notify 
            }
        }
        private bool isRuler;

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

        /// <summary> Gets or sets the self control-point's mode </summary>
        public SelfControlPointMode ControlPointMode
        {
            get => this.controlPointMode; 
            set
            {
                if (this.controlPointMode == value) return;
                this.controlPointMode = value;
                this.OnPropertyChanged(nameof(this.ControlPointMode));//Notify 
            }
        }
        private SelfControlPointMode controlPointMode;
        

        //Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName"> Name of the property used to notify listeners. </param>
        protected void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}