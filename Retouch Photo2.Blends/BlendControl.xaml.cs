using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Control of Blend.
    /// </summary>
    public sealed partial class BlendControl : UserControl
    {
        //@Delegate
        public EventHandler<BlendType> BlendTypeChanged;
        bool canInvoke = true;

        #region DependencyProperty

        /// <summary> Gets or sets the BlendType. </summary>
        public BlendType BlendType
        {
            get { return (BlendType)GetValue(BlendTypeProperty); }
            set { SetValue(BlendTypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BlendControl.BlendType" /> dependency property. </summary>
        public static readonly DependencyProperty BlendTypeProperty = DependencyProperty.Register(nameof(BlendType), typeof(BlendType), typeof(BlendControl), new PropertyMetadata(BlendType.None, (sender, e) =>
        {
            BlendControl con = (BlendControl)sender;

            if (e.NewValue is BlendType value)
            {
                con.canInvoke = false;

                int index = (int)value;
                con.ListView.SelectedIndex= index;

                con.canInvoke = true;
            }
        }));
        
        #endregion
        
        //@Construct
        public BlendControl()
        {
            this.InitializeComponent();
            this.ListView.SelectionChanged += (s, e) =>
            {
                if (this.canInvoke)
                {
                    int index = this.ListView.SelectedIndex;
                    BlendType blendType = (BlendType)index;

                    this.BlendTypeChanged?.Invoke(this, blendType); //Delegate
                }
            };
        }
    }
}