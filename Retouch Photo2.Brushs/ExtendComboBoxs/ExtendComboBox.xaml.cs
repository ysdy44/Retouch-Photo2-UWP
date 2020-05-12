using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs.ExtendIcons;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents the combo box that is used to select edge behavior.
    /// </summary>
    public sealed partial class ExtendComboBox : UserControl
    {
        
        //@Delegate
        public EventHandler<CanvasEdgeBehavior> ExtendChanged;

        //@Group
        private EventHandler<CanvasEdgeBehavior> Group;

        #region DependencyProperty


        /// <summary> Gets or sets the edge behavior. </summary>
        public CanvasEdgeBehavior Extend
        {
            get { return (CanvasEdgeBehavior)GetValue(ExtendProperty); }
            set { SetValue(ExtendProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ExtendComboBox.Extend" /> dependency property. </summary>
        public static readonly DependencyProperty ExtendProperty = DependencyProperty.Register(nameof(Extend), typeof(CanvasEdgeBehavior), typeof(ExtendComboBox), new PropertyMetadata(CanvasEdgeBehavior.Clamp, (sender, e) =>
        {
            ExtendComboBox con = (ExtendComboBox)sender;

            if (e.NewValue is CanvasEdgeBehavior value)
            {
                con.Group?.Invoke(con, value);
            }
        }));


        #endregion


        //@Construct
        public ExtendComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
        }

    }

    /// <summary>
    /// Represents the combo box that is used to select CanvasEdgeBehavior.
    /// </summary>
    public sealed partial class ExtendComboBox : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructGroup(this.ClampButton, resource.GetString("/ToolElements/Extend_Clamp"), new ClampIcon(), CanvasEdgeBehavior.Clamp);
            this.ConstructGroup(this.WrapButton, resource.GetString("/ToolElements/Extend_Wrap"), new WrapIcon(), CanvasEdgeBehavior.Wrap);
            this.ConstructGroup(this.MirrorButton, resource.GetString("/ToolElements/Extend_Mirror"), new MirrorIcon(), CanvasEdgeBehavior.Mirror);
        }
        
        //Group
        private void ConstructGroup(Button button, string text, UserControl icon, CanvasEdgeBehavior behavior)
        {
            void group(CanvasEdgeBehavior groupCanvasEdgeBehavior)
            {
                if (groupCanvasEdgeBehavior == behavior)
                {
                    button.IsEnabled = false;

                    this.Button.Content = text;
                }
                else button.IsEnabled = true;
            }

            //NoneButton
            group(this.Extend);

            //Buttons
            button.Content = text;
            button.Tag = icon;
            button.Tapped += (s, e) =>
            {
                this.ExtendChanged?.Invoke(this, behavior); //Delegate
                this.Flyout.Hide();
            };

            //Group
            this.Group += (s, e) => group(e);
        }

    }
}