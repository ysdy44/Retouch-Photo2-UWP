using FanKit.Transformers;
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
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RestrictionTextBlock.Text = resource.GetString("Tools_Node_NodeMode_Restriction");
            this.AngleCheckBox.Content = resource.GetString("Tools_Node_NodeMode_Restriction_Angle");
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
            this.LengthCheckBox.Content = resource.GetString("Tools_Node_NodeMode_Restriction_Length");
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

            this.ModeTextBlock.Text = resource.GetString("Tools_Node_NodeMode_Mode");        
            this.MirroredRadioButton.Content = resource.GetString("Tools_Node_NodeMode_Mode_Mirrored");
            this.MirroredRadioButton.Checked += (s, e) =>
            {
                this.ControlLengthMode = EachControlPointLengthMode.Equal;
                this.ControlAngleMode = EachControlPointAngleMode.Asymmetric;
            };
            this.DisconnectedRadioButton.Content = resource.GetString("Tools_Node_NodeMode_Mode_Disconnected");
            this.DisconnectedRadioButton.Checked += (s, e) =>
            {
                this.ControlLengthMode = EachControlPointLengthMode.None;
                this.ControlAngleMode = EachControlPointAngleMode.None;
            };
            this.AsymmetricRadioButton.Content = resource.GetString("Tools_Node_NodeMode_Mode_Asymmetric");
            this.AsymmetricRadioButton.Checked += (s, e) =>
            {
                this.ControlLengthMode = EachControlPointLengthMode.None;
                this.ControlAngleMode = EachControlPointAngleMode.Asymmetric;
            };
        }

    }
}