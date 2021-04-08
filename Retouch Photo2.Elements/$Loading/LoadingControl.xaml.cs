// Core:              
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Loading animation controls.
    /// </summary>
    public sealed partial class LoadingControl : UserControl
    {

        //String
        readonly static ResourceLoader resource = ResourceLoader.GetForCurrentView();
        private string StringsConverter(LoadingState value)
        {
            if (value == LoadingState.None) return null;

            return LoadingControl.resource.GetString($"$Loading_{value}");
        }

        //@Converter
        private Visibility VisibilityConverter(LoadingState value) => value == LoadingState.None ? Visibility.Collapsed : Visibility.Visible;
        private bool ProgressBooleanConverter(LoadingState value) => value == LoadingState.Loading || value == LoadingState.Saving;
        private Visibility ProgressVisibilityConverter(LoadingState value) => this.ProgressBooleanConverter(value) ? Visibility.Visible : Visibility.Collapsed;
        private Visibility CorrectVisibilityConverter(LoadingState value) => value == LoadingState.SaveSuccess ? Visibility.Visible : Visibility.Collapsed;
        private Visibility ErrorVisibilityConverter(LoadingState value)
        {
            switch (value)
            {
                case LoadingState.None: return Visibility.Collapsed;
                case LoadingState.Loading: return Visibility.Collapsed;
                case LoadingState.LoadFailed: return Visibility.Visible;
                case LoadingState.FileCorrupt: return Visibility.Visible;
                case LoadingState.FileNull: return Visibility.Visible;
                case LoadingState.Saving: return Visibility.Collapsed;
                case LoadingState.SaveSuccess: return Visibility.Collapsed;
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