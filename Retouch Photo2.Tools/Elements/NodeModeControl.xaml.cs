using FanKit.Transformers;
using Retouch_Photo2.Tools.Elements.NodeModeIcons;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Mode contorl of <see cref="NodeTool"/>.
    /// </summary>
    public sealed partial class NodeModeControl : UserControl
    {
        
        //@Content    
        /// <summary> Gets or sets the each control-point's length-mode </summary>
        public EachControlPointLengthMode ControlLengthMode = EachControlPointLengthMode.Equal;
        /// <summary> Gets or sets the each control-point's angle-mode </summary>
        public EachControlPointAngleMode ControlAngleMode = EachControlPointAngleMode.Asymmetric;


        #region DependencyProperty

        /// <summary> Gets or sets the self control-point's mode </summary>
        public SelfControlPointMode ControlPointMode
        {
            get => (SelfControlPointMode)base.GetValue(ControlPointModeProperty);
            set => base.SetValue(ControlPointModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "NodeModeControl.ControlPointMode" /> dependency property. </summary>
        public static readonly DependencyProperty ControlPointModeProperty = DependencyProperty.Register(nameof(ControlPointMode), typeof(SelfControlPointMode), typeof(NodeModeControl), new PropertyMetadata(SelfControlPointMode.None, (sender, e) =>
        {
            NodeModeControl control = (NodeModeControl)sender;

            if (e.NewValue is SelfControlPointMode value)
            {
                switch (value)
                {
                    case SelfControlPointMode.None:
                        {
                            control.AngleCheckBox.IsChecked = false;
                            control.LengthCheckBox.IsChecked = false;
                        }
                        break;
                    case SelfControlPointMode.Length:
                        {
                            control.AngleCheckBox.IsChecked = false;
                            control.LengthCheckBox.IsChecked = true;
                        }
                        break;
                    case SelfControlPointMode.Angle:
                        {
                            control.AngleCheckBox.IsChecked = true;
                            control.LengthCheckBox.IsChecked = false;
                        }
                        break;
                    case SelfControlPointMode.Disable:
                        {
                            control.AngleCheckBox.IsChecked = true;
                            control.LengthCheckBox.IsChecked = true;
                        }
                        break;
                }
            }
        }));

        #endregion


        //@Construct
        /// <summary>
        /// Initializes a PenModeControl. 
        /// </summary>
        public NodeModeControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructControlPointMode();
            this.ConstructControlLengthAngleMode();
        }


        //ControlPointMode
        private void ConstructControlPointMode()
        {
            this.AngleCheckBox.Click += (s, e) =>
            {
                switch (this.ControlPointMode)
                {
                    case SelfControlPointMode.None: this.ControlPointMode = SelfControlPointMode.Angle; break;
                    case SelfControlPointMode.Length: this.ControlPointMode = SelfControlPointMode.Angle; break;
                    case SelfControlPointMode.Angle: this.ControlPointMode = SelfControlPointMode.None; break;
                    case SelfControlPointMode.Disable: this.ControlPointMode = SelfControlPointMode.None; break;
                }
            };
            this.LengthCheckBox.Click += (s, e) =>
            {
                switch (this.ControlPointMode)
                {
                    case SelfControlPointMode.None: this.ControlPointMode = SelfControlPointMode.Length; break;
                    case SelfControlPointMode.Length: this.ControlPointMode = SelfControlPointMode.None; break;
                    case SelfControlPointMode.Angle: this.ControlPointMode = SelfControlPointMode.Length; break;
                    case SelfControlPointMode.Disable: this.ControlPointMode = SelfControlPointMode.None; break;
                }
            };
        }

        //ControlLengthAngleMode
        private void ConstructControlLengthAngleMode()
        {
            this.MirroredRadioButton.Checked += (s, e) =>
            {
                this.ControlLengthMode = EachControlPointLengthMode.Equal;
                this.ControlAngleMode = EachControlPointAngleMode.Asymmetric;
            };
            this.DisconnectedRadioButton.Checked += (s, e) =>
            {
                this.ControlLengthMode = EachControlPointLengthMode.None;
                this.ControlAngleMode = EachControlPointAngleMode.None;
            };
            this.AsymmetricRadioButton.Checked += (s, e) =>
            {
                this.ControlLengthMode = EachControlPointLengthMode.None;
                this.ControlAngleMode = EachControlPointAngleMode.Asymmetric;
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RestrictionTextBlock.Text = resource.GetString("/ToolElements/PenMode_Restriction");
            this.AngleCheckBox.Content = resource.GetString("/ToolElements/PenMode_Restriction_Angle");
            this.LengthCheckBox.Content = resource.GetString("/ToolElements/PenMode_Restriction_Length");

            this.ModeTextBlock.Text = resource.GetString("/ToolElements/PenMode_Mode");
            this.MirroredRadioButton.Content = resource.GetString("/ToolElements/PenMode_Mode_Mirrored");
            this.MirroredRadioButton.Tag = new MirroredIcon();
            this.DisconnectedRadioButton.Content = resource.GetString("/ToolElements/PenMode_Mode_Disconnected");
            this.DisconnectedRadioButton.Tag = new DisconnectedIcon();
            this.AsymmetricRadioButton.Content = resource.GetString("/ToolElements/PenMode_Mode_Asymmetric");
            this.AsymmetricRadioButton.Tag = new AsymmetricIcon();
        }

    }
}