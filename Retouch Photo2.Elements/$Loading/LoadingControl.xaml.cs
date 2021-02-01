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
        //@Content
        /// <summary> TextBlock's Text. </summary>
        public LoadingState State
        {
            get => this.state;
            set
            {
                this.Visibility = value == LoadingState.None ? Visibility.Collapsed : Visibility.Visible;
                this.TextBlock.Text = this.GetText(value);
                this.state = value;
            }
        }
        private LoadingState state;


        string _Loading;
        string _LoadFailed;

        string _FileCorrupt;
        string _FileNull;

        string _Saving;
        string _SaveSuccess;
        string _SaveFailed;

        private string GetText(LoadingState value)
        {
            switch (value)
            {
                case LoadingState.None: return string.Empty;

                case LoadingState.Loading: return this._Loading;
                case LoadingState.LoadFailed: return this._LoadFailed;

                case LoadingState.FileCorrupt: return this._FileCorrupt;
                case LoadingState.FileNull: return this._FileNull;

                case LoadingState.Saving: return this._Saving;
                case LoadingState.SaveSuccess: return this._SaveSuccess;
                case LoadingState.SaveFailed: return this._SaveFailed;

                default: return string.Empty;
            }
        }

        #region DependencyProperty

        /// <summary> Gets or sets whether the <see cref = "LoadingControl" /> Visibility. </summary>
        public bool IsActive
        {
            get => (bool)base.GetValue(IsActiveProperty);
            set => base.SetValue(IsActiveProperty, value);
        }       
        /// <summary> Identifies the <see cref = "LoadingControl.IsActive" /> dependency property. </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(LoadingControl), new PropertyMetadata(false));

        #endregion

        //@Construct
        public LoadingControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._Loading = resource.GetString("/$Loading/Loading");
            this._LoadFailed = resource.GetString("/$Loading/LoadFailed");

            this._FileCorrupt = resource.GetString("/$Loading/FileCorrupt");
            this._FileNull = resource.GetString("/$Loading/FileNull");

            this._Saving = resource.GetString("/$Loading/Saving");
            this._SaveSuccess = resource.GetString("/$Loading/SaveSuccess");
            this._SaveFailed = resource.GetString("/$Loading/SaveFailed");
        }
    }
}