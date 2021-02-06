// Core:              
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      
using System.Collections.Generic;
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
                this.TextBlock.Text = this.Dictionary[value] ?? string.Empty;
                this.state = value;
            }
        }
        private LoadingState state;

        public IDictionary<LoadingState, string> Dictionary = new Dictionary<LoadingState, string>();


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

            this.Dictionary.Add(LoadingState.None, null);

            this.Dictionary.Add(LoadingState.Loading, resource.GetString("$Loading_Loading"));
            this.Dictionary.Add(LoadingState.LoadFailed, resource.GetString("$Loading_LoadFailed"));

            this.Dictionary.Add(LoadingState.FileCorrupt, resource.GetString("$Loading_FileCorrupt"));
            this.Dictionary.Add(LoadingState.FileNull, resource.GetString("$Loading_FileNull"));

            this.Dictionary.Add(LoadingState.Saving, resource.GetString("$Loading_Saving"));
            this.Dictionary.Add(LoadingState.SaveSuccess, resource.GetString("$Loading_SaveSuccess"));
            this.Dictionary.Add(LoadingState.SaveFailed, resource.GetString("$Loading_SaveFailed"));
        }
    }
}