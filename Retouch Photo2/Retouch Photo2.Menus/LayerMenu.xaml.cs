// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Elements;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Layers"/>.
    /// </summary>
    public sealed partial class LayerMenu : Expander
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility NameToVisibilityConverter(string name) => string.IsNullOrEmpty(name) ? Visibility.Visible : Visibility.Collapsed;
        private bool VisibilityToBooConverter(Visibility visibility) => visibility == Visibility.Visible;
        private int OpacityToNumberConverter(float opacity) => (int)(opacity * 100.0f);
        private Visibility SelectionUnSingleToVisibilityConverter(bool isSingle) => isSingle == false ? Visibility.Visible : Visibility.Collapsed;
        private Visibility ZeroToVisibilityConverter(int count) => count == 0 ? Visibility.Visible : Visibility.Collapsed;
        private Visibility SelectionSingleToVisibilityConverter(bool isSingle) => isSingle ? Visibility.Visible : Visibility.Collapsed;


        //@Construct
        /// <summary>
        /// Initializes a LayerMenu. 
        /// </summary>
        public LayerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.NameButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowRename?.Invoke();
            this.ConstructVisibility();
            this.ConstructOpacity1();
            this.ConstructOpacity2();
            this.ConstructOpacity3();
            this.ConstructBlendMode();
            this.ConstructTagType();
        }
    }

    public sealed partial class LayerMenu : Expander
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.NameTextBlock.Text = resource.GetString("Menus_Layer_Name");
            this.OpacityTextBlock.Text = resource.GetString("Menus_Layer_Opacity");
            this.BlendModeTextBlock.Text = resource.GetString("Menus_Layer_BlendMode");
            this.VisibilityTextBlock.Text = resource.GetString("Menus_Layer_Visibility");
            this.TagTypeTextBlock.Text = resource.GetString("Menus_Layer_TagType");

            this.NamePlaceholderTextBlock.Text = resource.GetString("Menus_Layer_Rename");

            this.AdjustmentTextBlock.Text = resource.GetString("Menus_Adjustment");
            this.AdjustmentDisableTextBlock.Text = resource.GetString("Menus_Adjustment_DisableTip");
            this.AdjustmentZeroTextBlock.Text = resource.GetString("Menus_Adjustment_ZeroTip");
        }


        //Visibility
        private void ConstructVisibility()
        {
            this.VisibilityButton.Tapped += (s, e) =>
            {
                Visibility value = (this.SelectionViewModel.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
                this.SelectionViewModel.Visibility = value;

                this.MethodViewModel.ILayerChanged<Visibility>
                (
                    set: (layer) => layer.Visibility = value,

                    type: HistoryType.LayersProperty_SetVisibility,
                    getUndo: (layer) => layer.Visibility,
                    setUndo: (layer, previous) => layer.Visibility = previous
                );
            };
        }


        //Opacity
        private void ConstructOpacity1()
        {
            this.OpacityPicker.Unit = "%";
            this.OpacityPicker.Minimum = 0;
            this.OpacityPicker.Maximum = 100;
            this.OpacityPicker.ValueChanged += (s, value) =>
            {
                float opacity = (float)value / 100.0f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    type: HistoryType.LayersProperty_SetOpacity,
                    getUndo: (layer) => layer.Opacity,
                    setUndo: (layer, previous) => layer.Opacity = previous
                );
            };
        }

        private void ConstructOpacity2()
        {
            this.OpacitySlider.Minimum = 0.0d;
            this.OpacitySlider.Maximum = 1.0d;
            this.OpacitySlider.ValueChangeStarted += (s, value) => this.MethodViewModel.ILayerChangeStarted(cache: (layer) => layer.CacheOpacity());
            this.OpacitySlider.ValueChangeDelta += (s, value) =>
            {
                float opacity = (float)value;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChangeDelta(set: (layer) => layer.Opacity = opacity);
            };
            this.OpacitySlider.ValueChangeCompleted += (s, value) =>
            {
                float opacity = (float)value;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChangeCompleted<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    type: HistoryType.LayersProperty_SetOpacity,
                    getUndo: (layer) => layer.StartingOpacity,
                    setUndo: (layer, previous) => layer.Opacity = previous
                );
            };
        }

        private void ConstructOpacity3()
        {
            this.Opacity0Button.Tapped += (s, e) =>
            {
                float opacity = 0.0f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    type: HistoryType.LayersProperty_SetOpacity_000,
                    getUndo: (layer) => layer.Opacity,
                    setUndo: (layer, previous) => layer.Opacity = previous
                );
            };

            this.Opacity25Button.Tapped += (s, e) =>
            {
                float opacity = 0.25f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    type: HistoryType.LayersProperty_SetOpacity_025,
                    getUndo: (layer) => layer.Opacity,
                    setUndo: (layer, previous) => layer.Opacity = previous
                );
            };

            this.Opacity50Button.Tapped += (s, e) =>
            {
                float opacity = 0.5f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    type: HistoryType.LayersProperty_SetOpacity_050,
                    getUndo: (layer) => layer.Opacity,
                    setUndo: (layer, previous) => layer.Opacity = previous
                );
            };

            this.Opacity75Button.Tapped += (s, e) =>
            {
                float opacity = 0.75f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    type: HistoryType.LayersProperty_SetOpacity_075,
                    getUndo: (layer) => layer.Opacity,
                    setUndo: (layer, previous) => layer.Opacity = previous
                );
            };

            this.Opacity100Button.Tapped += (s, e) =>
            {
                float opacity = 1.0f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    type: HistoryType.LayersProperty_SetOpacity_100,
                    getUndo: (layer) => layer.Opacity,
                    setUndo: (layer, previous) => layer.Opacity = previous
                );
            };
        }


        //Blend Mode
        private void ConstructBlendMode()
        {
            this.BlendModeComboBox.ModeChanged += (s, mode) =>
            {
                BlendEffectMode? blendMode = mode;
                this.SelectionViewModel.BlendMode = blendMode;

                this.MethodViewModel.ILayerChanged<BlendEffectMode?>
                (
                    set: (layer) => layer.BlendMode = blendMode,

                    type: HistoryType.LayersProperty_SetBlendMode,
                    getUndo: (layer) => layer.BlendMode,
                    setUndo: (layer, previous) => layer.BlendMode = previous
                );
            };
            this.BlendModeComboBox.Closed += (s, e) => this.SettingViewModel.RegisteKey();//Setting
            this.BlendModeComboBox.Opened += (s, e) => this.SettingViewModel.UnregisteKey();//Setting
        }


        //Tag Type
        private void ConstructTagType()
        {
            this.TagTypeSegmented.TypeChanged += (s, type) =>
            {
                TagType tagType = (TagType)type;
                this.SelectionViewModel.TagType = tagType;

                this.MethodViewModel.ILayerChanged<Retouch_Photo2.Blends.TagType>
                (
                    set: (layer) => layer.TagType = tagType,

                    type: HistoryType.LayersProperty_SetTagType,
                    getUndo: (layer) => layer.TagType,
                    setUndo: (layer, previous) => layer.TagType = previous
                );
            };
        }

    }
}