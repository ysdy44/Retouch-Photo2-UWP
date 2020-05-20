﻿using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "FootPageControl" />. 
    /// </summary>
    public sealed partial class HeadBarControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Content
        /// <summary> DocumentButton. </summary>   
        public Button DocumentButton => this._DocumentButton;
        /// <summary> DocumentUnSaveButton. </summary>   
        public Button DocumentUnSaveButton => this._DocumentUnSaveButton;
        /// <summary> DocumentFlyout. </summary>   
        public Flyout DocumentFlyout => this._DocumentFlyout;

        /// <summary> ExportButton. </summary>   
        public ExpandAppbarButton ExportButton => this._ExportButton;
        /// <summary> UndoButton. </summary>   
        public ExpandAppbarButton UndoButton => this._UndoButton;
        /// <summary> SetupButton. </summary>   
        public ExpandAppbarButton SetupButton => this._SetupButton;
        /// <summary> RulerButton. </summary>   
        public ExpandAppbarToggleButton RulerButton => this._RulerButton;
        /// <summary> FullScreenButton. </summary>   
        public ExpandAppbarButton FullScreenButton => this._FullScreenButton;

        /// <summary> RightStackPanel's Children. </summary>   
        public UIElementCollection RightChildren => this.HeadRightStackPanel.Children;


        //@VisualState
        bool _vsIsHeadLeft;
        public VisualState VisualState
        {
            get => this._vsIsHeadLeft ? this.HeadLeftStar : this.HeadRightStar;
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        public HeadBarControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            //Document            
            this._DocumentButton.Holding += (s, e) => this._DocumentFlyout.ShowAt(this._DocumentButton);
            this._DocumentButton.RightTapped += (s, e) => this._DocumentFlyout.ShowAt(this._DocumentButton);

            //HeadGrid
            this.Loaded += (s, e) => this.HeadGridSizeChange(this.ActualWidth);
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                {
                    this.HeadGridSizeChange(e.NewSize.Width);
                }
            };
        }


        private void HeadGridSizeChange(double width)
        {
            double arrangeWidth = width - 70 - 40;
            double measureWidth = this.HeadRightStackPanel.ActualWidth;

            bool isHeadLeft = arrangeWidth > measureWidth;
            if (this._vsIsHeadLeft != isHeadLeft)
            {
                this._vsIsHeadLeft = isHeadLeft;
                this.VisualState = this.VisualState;//State 
            }
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._DocumentButton.Content = resource.GetString("/$DrawPage/Document");
            this._DocumentUnSaveButton.Content = resource.GetString("/$DrawPage/DocumentUnSave");

            this._ExportToolTip.Content =
            this._ExportButton.Text = resource.GetString("/$DrawPage/Export");
            this._UndoToolTip.Content =
            this._UndoButton.Text = resource.GetString("/$DrawPage/Undo");
            //this.RedoToolTip.Content = resourceLoader.GetString("/$DrawPage/Redo");
            //this._RedoButton.Text =
            this._SetupToolTip.Content =
            this._SetupButton.Text = resource.GetString("/$DrawPage/Setup");
            this._SnapToolTip.Content =
            this._SnapButton.Text = resource.GetString("/$DrawPage/Snap");
            this._RulerToolTip.Content =
            this._RulerButton.Text = resource.GetString("/$DrawPage/Ruler");
            this._FullScreenToolTip.Content =
            this._FullScreenButton.Text = resource.GetString("/$DrawPage/FullScreen");
            this._TipButton.Text = resource.GetString("/$DrawPage/Tip");
        }
    }
}