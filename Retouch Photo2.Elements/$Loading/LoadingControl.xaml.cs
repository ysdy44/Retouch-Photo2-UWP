// Core:              
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Loading animation controls.
    /// </summary>
    public sealed partial class LoadingControl : UserControl
    {

        // String
        readonly static ResourceLoader resource = ResourceLoader.GetForCurrentView();
        private string StringsConverter(LoadingState value)
        {
            if (value == LoadingState.None) return null;
            if (value == LoadingState.LoadingWithProgress) value = LoadingState.Loading;

            return LoadingControl.resource.GetString($"$Loading_{value}");
        }


        //@Converter
        private Visibility VisibilityConverter(LoadingState value) => value == LoadingState.None ? Visibility.Collapsed : Visibility.Visible;

        private bool ProgressRingBooleanConverter(LoadingState value) => value == LoadingState.Loading || value == LoadingState.Saving;
        private Visibility ProgressRingVisibilityConverter(LoadingState value) => this.ProgressRingBooleanConverter(value) ? Visibility.Visible : Visibility.Collapsed;

        private Visibility ProgressBarVisibilityConverter(LoadingState value) => value == LoadingState.LoadingWithProgress ? Visibility.Visible : Visibility.Collapsed;

        private Visibility CorrectVisibilityConverter(LoadingState value) => value == LoadingState.SaveSuccess ? Visibility.Visible : Visibility.Collapsed;
        private Visibility ErrorVisibilityConverter(LoadingState value)
        {
            switch (value)
            {
                case LoadingState.LoadFailed:
                case LoadingState.FileCorrupt:
                case LoadingState.FileNull:
                case LoadingState.SaveFailed: return Visibility.Visible;
                default: return Visibility.Collapsed;
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets whether the <see cref = "LoadingControl" /> state. </summary>
        public LoadingState State
        {
            get => (LoadingState)base.GetValue(StateProperty);
            set => base.SetValue(StateProperty, value);
        }
        /// <summary> Identifies the <see cref = "LoadingControl.IsActive" /> dependency property. </summary>
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof(State), typeof(LoadingState), typeof(LoadingControl), new PropertyMetadata(LoadingState.None));


        #endregion


        //@Content
        /// <summary>
        /// Occurs when the Storyboard object has finished playing.
        /// </summary>
        public event EventHandler<object> Completed
        {
            add => this.KeyFrames.Completed += value;
            remove => this.KeyFrames.Completed -= value;
        }
        /// <summary>
        /// Occurs when the range value changes.
        /// </summary>
        public event RangeBaseValueChangedEventHandler ValueChanged
        {
            add => this.ProgressBar.ValueChanged += value;
            remove => this.ProgressBar.ValueChanged -= value;
        }
        /// <summary>
        /// Start the set of animations associated with the storyboard.
        /// </summary>
        public void Begin()
        {
            this.ProgressBar.Value = 0.0d;
            this.Storyboard.Begin(); // Storyboard}
        }


        //@Construct
        /// <summary>
        /// Initializes a LoadingControl.
        /// </summary>
        public LoadingControl()
        {
            this.InitializeComponent();
        }
    }
}