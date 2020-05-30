using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
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
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

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

            //Key
            this.TextBox.GettingFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = false;
            this.TextBox.LosingFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = true;

            this.TextBox.TextChanged += (s, e) =>
            {
                if (this.TextBox.FocusState == FocusState.Unfocused) return;
                string fontText = this.TextBox.Text;

                this.SetFontText(fontText);
            };

            this.CharacterButton.Click += (s, e) =>
            {
                this.TipViewModel.ShowMenuLayoutAt(MenuType.Character, this.CharacterButton);
            };
            this.FullScreenButton.Click += (s, e) =>
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

    /// <summary>
    /// <see cref="ITool"/>'s TextTool .
    /// </summary>
    public sealed partial class TextTool : UserControl
    {

        private void SetFontText(string fontText)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set font text");

            //Selection
            this.SelectionViewModel.FontText = fontText;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;
                    
                    var previous = textLayer.FontText;
                    history.UndoActions.Push(() =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        textLayer.FontText = previous;
                    });

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    textLayer.FontText = fontText;
                }
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}