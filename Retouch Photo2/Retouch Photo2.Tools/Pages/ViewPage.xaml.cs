using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.Tools.Pages.ViewPages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "ViewTool"/>.
    /// </summary>
    public sealed partial class ViewPage : Page
    {
        //@ViewModel
        public ViewModel ViewModel => App.ViewModel;
        public TipViewModel TipViewModel => App.TipViewModel;

        //@Touchbar
        RadianTouchbarSlider _radianTouchbarSlider { get; } = new RadianTouchbarSlider();
        ScaleTouchbarSlider _scaleTouchbarSlider { get; } = new ScaleTouchbarSlider();

        //@Converter
        private string RadianToString(float radian) => ViewConverter.RadianToString(radian);
        private string ScaleToString(float radian) => ViewConverter.ScaleToString(radian);


        bool _isSetting;

        /// <summary> Type of ViewPage. </summary>
        public ViewPageType Type
        {
            get => this.type;
            set
            {
                if (this.type == value) return;

                if (this._isSetting) return;
                this._isSetting = true;

                switch (value)
                {
                    case ViewPageType.None:
                        {
                            this.RadianToggleButton.IsChecked = false;
                            this.ScaleToggleButton.IsChecked = false;

                            this.TipViewModel.Touchbar = null;//Touchbar
                        }
                        break;
                    case ViewPageType.Radian:
                        {
                            this.RadianToggleButton.IsChecked = true;
                            this.ScaleToggleButton.IsChecked = false;

                            this.TipViewModel.Touchbar = this._radianTouchbarSlider;//Touchbar
                        }
                        break;
                    case ViewPageType.Scale:
                        {
                            this.RadianToggleButton.IsChecked = false;
                            this.ScaleToggleButton.IsChecked = true;

                            this.TipViewModel.Touchbar = this._scaleTouchbarSlider;//Touchbar
                        }
                        break;
                }

                this._isSetting = false;

                this.type=value;
            }
        }
        private ViewPageType type;


        //@Construct
        public ViewPage()
        {
            this.InitializeComponent();

            //Radian
            this.RadianToggleButton.Unchecked += (s, e) => this.Type = ViewPageType.None;
            this.RadianToggleButton.Checked += (s, e) => this.Type = ViewPageType.Radian;

            //Scale
            this.ScaleToggleButton.Unchecked += (s, e) => this.Type = ViewPageType.None;
            this.ScaleToggleButton.Checked += (s, e) => this.Type = ViewPageType.Scale;
        }
    }
}