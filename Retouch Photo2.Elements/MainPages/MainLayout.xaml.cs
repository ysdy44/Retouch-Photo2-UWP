using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary> 
    /// <see cref = "MainPage" />'s layout. 
    /// </summary>
    public sealed partial class MainLayout : UserControl
    {

        //@Content
        /// <summary> RefreshButton. </summary>
        public Button RefreshButton => this._RefreshButton;
        /// <summary> SettingButton. </summary>
        public Button SettingButton => this._SettingButton;
        
        /// <summary> SelectCheckBox. </summary>
        public CheckBox SelectCheckBox => this._SelectCheckBox;
        /// <summary> Select's Text. </summary>
        public string SelectText { set => this.SelectCountRun.Text = value; }
        /// <summary> SelectAllButton. </summary>
        public Button SelectAllButton => this._SelectAllButton;
        
        /// <summary> InitialBorder's Child. </summary>
        public UIElement InitialChild { get => this.InitialBorder.Child; set => this.InitialBorder.Child = value; }
        /// <summary> GridView's ItemsSource. </summary>
        public object ItemsSource { get => this.GridView.ItemsSource; set => this.GridView.ItemsSource = value; }

        /// <summary> MainBorder's Child. </summary>
        public UIElement MainChild { get => this.MainBorder.Child; set => this.MainBorder.Child = value; }
        /// <summary> PicturesBorder's Child. </summary>
        public UIElement PicturesChild { get => this.PicturesBorder.Child; set => this.PicturesBorder.Child = value; }
        /// <summary> RenameBorder's Child. </summary>
        public UIElement RenameChild { get => this.RenameBorder.Child; set => this.RenameBorder.Child = value; }
        /// <summary> SaveBorder's Child. </summary>
        public UIElement SaveChild { get => this.SaveBorder.Child; set => this.SaveBorder.Child = value; }
        /// <summary> ShareBorder's Child. </summary>
        public UIElement ShareChild { get => this.ShareBorder.Child; set => this.ShareBorder.Child = value; }
        /// <summary> DeleteBorder's Child. </summary>
        public UIElement DeleteChild { get => this.DeleteBorder.Child; set => this.DeleteBorder.Child = value; }
        /// <summary> DuplicateBorder's Child. </summary>
        public UIElement DuplicateChild { get => this.DuplicateBorder.Child; set => this.DuplicateBorder.Child = value; }


        //@VisualState
        MainPageState _vsState = MainPageState.Main;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsState)
                {
                    case MainPageState.None: return this.Normal;
                    case MainPageState.Initial: return this.Initial;
                    case MainPageState.Main: return this.Main;
                    case MainPageState.Dialog: return this.Dialog;
                    case MainPageState.Pictures: return this.Pictures;
                    case MainPageState.Rename: return this.Rename;
                    case MainPageState.Delete: return this.Delete;
                    case MainPageState.Duplicate: return this.Duplicate;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        /// <summary>
        /// Gets or sets the mainpage-state.
        /// </summary>
        public MainPageState MainPageState
        {
            get => this._vsState;
            set
            {
                this._vsState = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@Construct
        public MainLayout()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) =>
            {
                 this.VisualState = this.VisualState;//State
            };
        }

    }
}