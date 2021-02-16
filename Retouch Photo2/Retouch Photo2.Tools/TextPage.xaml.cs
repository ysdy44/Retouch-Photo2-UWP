// Core:              ★★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
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
    /// Page of <see cref="TextTool"/>.
    /// </summary>
    internal sealed partial class TextPage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        

        //@VisualState
        bool _vsIsFullScreen;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get => this._vsIsFullScreen ? this.FullScreen : this.Normal;
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        /// <summary>
        /// Initializes a TextPage. 
        /// </summary>
        public TextPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            if (this.TextBox is TextBox textBox)
            {
                //textBox.IsEnabled = false;
                //this.ColorFlyout.Opened += (s, e) => textBox.IsEnabled = true;
                //this.ColorFlyout.Closed += (s, e) => textBox.IsEnabled = false;
                textBox.GotFocus += (s, e) => this.SettingViewModel.UnRegisteKey();
                textBox.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            }

            this.TextBox.TextChanged += (s, e) =>
            {
                if (this.TextBox.FocusState == FocusState.Unfocused) return;
                string fontText = this.TextBox.Text;

                this.SetFontText(fontText);
            };

            this.TextButton.Click += (s, e) =>
            {
                this.TipViewModel.ShowMenuLayoutAt(MenuType.Text, this.TextButton);
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

            this.TextBox.PlaceholderText = resource.GetString("Tools_Text_PlaceholderText");
        }

    }

    /// <summary>
    /// Page of <see cref="TextTool"/>.
    /// </summary>
    internal sealed partial class TextPage : Page
    {
        
        private void SetFontText(string fontText)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetFontText);

            //Selection
            this.SelectionViewModel.FontText = fontText;
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;
                    
                    var previous = textLayer.FontText;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        textLayer.FontText = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    textLayer.FontText = fontText;
                }
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}