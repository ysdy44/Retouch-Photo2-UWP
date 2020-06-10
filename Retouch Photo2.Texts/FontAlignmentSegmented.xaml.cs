using Microsoft.Graphics.Canvas.Text;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Texts
{
    /// <summary>
    /// Represents the segmented that is used to select font alignment.
    /// </summary>
    public sealed partial class FontAlignmentSegmented : UserControl
    {
        
        //@Delegate
        public EventHandler<CanvasHorizontalAlignment> AlignmentChanged;

        //@VisualState
        CanvasHorizontalAlignment _vsAlignment;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsAlignment)
                {
                    case CanvasHorizontalAlignment.Left: return this.Left;
                    case CanvasHorizontalAlignment.Center: return this.Center;
                    case CanvasHorizontalAlignment.Right: return this.Right;
                    case CanvasHorizontalAlignment.Justified: return this.Justified;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Alignment of <see cref = "FontAlignmentSegmented" />. </summary>
        public CanvasHorizontalAlignment Alignment
        {
            get { return (CanvasHorizontalAlignment)GetValue(AlignmentProperty); }
            set { SetValue(AlignmentProperty, value); }
        }
        /// <summary> Identifies the <see cref = "FontAlignmentSegmented.Alignment" /> dependency property. </summary>
        public static readonly DependencyProperty AlignmentProperty = DependencyProperty.Register(nameof(Alignment), typeof(CanvasHorizontalAlignment), typeof(FontAlignmentSegmented), new PropertyMetadata(CanvasHorizontalAlignment.Left, (sender, e) =>
        {
            FontAlignmentSegmented con = (FontAlignmentSegmented)sender;

            if (e.NewValue is CanvasHorizontalAlignment value)
            {
                con._vsAlignment = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        /// <summary> IsOpen of <see cref = "FontAlignmentSegmented" />. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "FontAlignmentSegmented.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(FontAlignmentSegmented), new PropertyMetadata(false));


        #endregion


        //@Construct
        public FontAlignmentSegmented()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.LeftToolTip.Content = resource.GetString("/Texts/FontAlignment_Left");
            this.LeftButton.Click += (s, e) =>
            {
                this.AlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Left);//Delegate
            };

            this.CenterToolTip.Content = resource.GetString("/Texts/FontAlignment_Center");
            this.CenterButton.Click += (s, e) =>
            {
                this.AlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Center);//Delegate
            };

            this.RightToolTip.Content = resource.GetString("/Texts/FontAlignment_Right");
            this.RightButton.Click += (s, e) =>
            {
                this.AlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Right);//Delegate
            };

            this.JustifiedToolTip.Content = resource.GetString("/Texts/FontAlignment_Justified");
            this.JustifiedButton.Click += (s, e) =>
            {
                this.AlignmentChanged?.Invoke(this, CanvasHorizontalAlignment.Justified);//Delegate
            };
        }

    }
}