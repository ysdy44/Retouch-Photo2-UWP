using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITool"/>'s TextTool .
    /// </summary>
    public sealed partial class TextTool : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@VisualState
        public bool _vsIsFullScreen;
        public VisualState VisualState
        {
            get => this._vsIsFullScreen ? this.FullScreen : this.Normal;
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        //@Construct
        public TextTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.TextBox.TextChanged += (s, e) =>
            {
                if (this.TextBox.FocusState == FocusState.Unfocused) return;
                string fontText = this.TextBox.Text;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer.Type == LayerType.TextArtistic || layer.Type == LayerType.TextFrame)
                    {
                        ITextLayer textLayer = (ITextLayer)layer;
                        textLayer.FontText = fontText;
                    }
                });
                this.SelectionViewModel.FontText = fontText;

                this.ViewModel.Invalidate();//Invalidate
            };

            this.CharacterButton.Tapped += (s, e) =>
            {
                this.TipViewModel.ShowMenuLayoutAt(MenuType.Character, this.CharacterButton);
            };
            this.FullScreenButton.Tapped += (s, e) =>
            {
                this._vsIsFullScreen = !this._vsIsFullScreen;
                this.VisualState = this.VisualState;//State
            };
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TextBox.PlaceholderText = resource.GetString("/Tools/Text_PlaceholderText");
        }

    }
}
