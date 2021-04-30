// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas.Effects;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// ComboBox of <see cref="BlendEffect"/>
    /// </summary>
    public sealed partial class BlendModeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when mode change. </summary>
        public event EventHandler<BlendEffectMode?> ModeChanged;
        /// <summary> Occurs after the flyout is closed. </summary>
        public event EventHandler<object> Closed { add => this.Flyout.Closed += value; remove => this.Flyout.Closed -= value; }
        /// <summary> Occurs when the flyout is opened. </summary>
        public event EventHandler<object> Opened { add => this.Flyout.Opened += value; remove => this.Flyout.Opened -= value; }


        //@Group
        /// <summary> Occurs when group change. </summary>
        private event EventHandler<BlendEffectMode?> Group;


        #region DependencyProperty


        /// <summary> Gets or sets the blend-type. </summary>
        public BlendEffectMode? Mode
        {
            get => (BlendEffectMode?)base.GetValue(ModeProperty);
            set => base.SetValue(ModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "BlendModeComboBox.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(BlendEffectMode?), typeof(BlendModeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BlendModeComboBox control = (BlendModeComboBox)sender;

            if (e.NewValue is BlendEffectMode value)
            {
                control.Group?.Invoke(control, value);//Delegate
            }
            else
            {
                control.Group?.Invoke(control, null);//Delegate
            }
        }));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a BlendModeComboBox. 
        /// </summary>
        public BlendModeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructGroup();

            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
        }
    }

    public sealed partial class BlendModeComboBox : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            foreach (UIElement child in this.StackPanel.Children)
            {
                if (child is ListViewItem item)
                {
                    if (item.Content is ContentControl control)
                    {
                        string key = control.Name;
                        string title = resource.GetString($"Blends_{key}");

                        control.Content = title;
                    }
                }
            }
        }


        //@Group
        private void ConstructGroup()
        {
            foreach (UIElement child in this.StackPanel.Children)
            {
                if (child is ListViewItem item)
                {
                    if (item.Content is ContentControl control)
                    {
                        string key = control.Name;
                        BlendEffectMode? mode = null;
                        try
                        {
                            mode = (BlendEffectMode)Enum.Parse(typeof(BlendEffectMode), key);
                        }
                        catch (Exception) { }


                        //Button
                        item.Tapped += (s, e) => this.ModeChanged?.Invoke(this, mode);//Delegate


                        //Group
                        group(this.Mode);
                        this.Group += (s, groupMode) => group(groupMode);

                        void group(BlendEffectMode? groupMode)
                        {
                            if (groupMode == mode)
                            {
                                item.IsSelected = true;

                                this.Control.Content = control.Content as string;
                            }
                            else item.IsSelected = false;
                        }
                    }
                }
            }
        }
    }
}