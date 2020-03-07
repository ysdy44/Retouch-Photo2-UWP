using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FanKit.Transformers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Menus;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "TextFrameTool"/>.
    /// </summary>
    public sealed partial class TextFramePage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        TextFrameLayer FrameLayer => this.SelectionViewModel.TextFrameLayer;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        //@Converter
        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;


        //@VisualState
        public bool _vsIsFullScreen;
        public VisualState VisualState
        {
            get => this._vsIsFullScreen ? this.FullScreen : this.Normal;
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        public TextFramePage()
        {
            this.InitializeComponent();

            this.TextBox.TextChanged += (s, e) =>
            {
                if (this.FrameLayer == null) return;
                this.FrameLayer.Text = this.TextBox.Text;
                this.ViewModel.Invalidate();//Invalidate
            };
            
            this.CharacterButton.Tapped += (s, e) =>
            {
                this.TipViewModel.SetMenuState(MenuType.Character, MenuState.FlyoutHide, MenuState.FlyoutShow);
            };

            this.FullScreenButton.Tapped += (s, e) =>
            {
                this._vsIsFullScreen = !this._vsIsFullScreen;
                this.VisualState = this.VisualState;//State
            };
            
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}