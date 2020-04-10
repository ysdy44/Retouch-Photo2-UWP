using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary> 
    /// Page of <see cref = "ImagePage"/>.
    /// </summary>
    public sealed partial class ImagePage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Static
        public static Action Select;
        public static Action Replace;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        /// <summary> Tip. </summary>
        public void TipSelect() => this.EaseStoryboard.Begin();//Storyboard

        //@Construct
        public ImagePage()
        {
            this.InitializeComponent();

            this.SelectButton.Tapped += (s, e) => ImagePage.Select?.Invoke();
            this.ReplaceButton.Tapped += (s, e) => ImagePage.Replace?.Invoke();
            this.ClearButton.Tapped += (s, e) => this.SelectionViewModel.ImageStr = new ImageStr();//ImageRe
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}