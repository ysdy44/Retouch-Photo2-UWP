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
                this.TextBlock.Text = this.ConstructStringsCore(value);
                this.state = value;
            }
        }
        private LoadingState state;


        //String
        readonly static ResourceLoader resource = ResourceLoader.GetForCurrentView();
        private string ConstructStringsCore(LoadingState value)
        {
            return LoadingControl.resource.GetString($"$Loading_{value}");
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
        /// <summary>
        /// Initializes a LoadingControl.
        /// </summary>
        public LoadingControl()
        {
            this.InitializeComponent();
        }
    }
}