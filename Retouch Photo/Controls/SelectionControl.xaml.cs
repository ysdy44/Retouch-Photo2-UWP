using Retouch_Photo.Library;
using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static Retouch_Photo.Library.TransformController;

namespace Retouch_Photo.Controls
{
    public sealed partial class SelectionControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        #region DependencyProperty

        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(SelectionControl), new PropertyMetadata(null, (sender, e) =>
        {
            SelectionControl con = (SelectionControl)sender;

            if (e.NewValue is Layer value)
            {
                con.Initialize(value);
            }
            else
            {
                con.Initialize(null);
            }
        }));

        #endregion

        private bool buttonIsEnabled;
        public bool ButtonIsEnabled
        {
            get => this.buttonIsEnabled;
            set
            {
                this.Initialize(value);
                this.buttonIsEnabled = value;
            }
        }

        public SelectionControl()
        {
            this.InitializeComponent();
            
            this.CutButton.ButtonTapped += (s, e) =>this.ButtonIsEnabled=!this.ButtonIsEnabled;
            this.CopyButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;
            this.PasteButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;
            this.ClearButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;

            this.ExtractButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;
            this.MergeButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;

            this.AllButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;
            this.DeselectButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;
            this.PixelButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;
            this.InvertButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;

            this.FeatherButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;
            this.TransformButton.ButtonTapped += (s, e) => this.ButtonIsEnabled = !this.ButtonIsEnabled;
        }

        /// <summary>
        /// Initialize all button
        /// </summary>
        public void Initialize(Layer layer)
        {
            bool isEnabled = !(layer == null);

            this.Initialize(isEnabled);
        }
        public void Initialize(bool isEnabled)
        { 
            this.CutButton.ButtonIsEnabled = isEnabled;
            this.CopyButton.ButtonIsEnabled = isEnabled;
            this.PasteButton.ButtonIsEnabled = isEnabled;
            this.ClearButton.ButtonIsEnabled = isEnabled;

            this.ExtractButton.ButtonIsEnabled = isEnabled;
            this.MergeButton.ButtonIsEnabled = isEnabled;

            this.AllButton.ButtonIsEnabled = isEnabled;
            this.DeselectButton.ButtonIsEnabled = isEnabled;
            this.PixelButton.ButtonIsEnabled = isEnabled;
            this.InvertButton.ButtonIsEnabled = isEnabled;

            this.FeatherButton.ButtonIsEnabled = isEnabled;
            this.TransformButton.ButtonIsEnabled = isEnabled;
        }

    }
}
