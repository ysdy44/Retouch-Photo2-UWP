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

        /// <summary> Gets or sets the self control-point's mode </summary>
        public SelfControlPointMode ControlPointMode
        {
            get => this.controlPointMode;
            set
            {
                switch (value)
                {
                    case SelfControlPointMode.None:
                        this.AngleCheckBox.IsChecked = false;
                        this.LengthCheckBox.IsChecked = false;
                        break;
                    case SelfControlPointMode.Length:
                        this.AngleCheckBox.IsChecked = false;
                        this.LengthCheckBox.IsChecked = true;
                        break;
                    case SelfControlPointMode.Angle:
                        this.AngleCheckBox.IsChecked = true;
                        this.LengthCheckBox.IsChecked = false;
                        break;
                    case SelfControlPointMode.Disable:
                        this.AngleCheckBox.IsChecked = true;
                        this.LengthCheckBox.IsChecked = true;
                        break;
                }

                this.controlPointMode = value;
            }
        }
        private SelfControlPointMode controlPointMode = SelfControlPointMode.None;


        //@Construct
        /// <summary>
        /// Initializes a PenModeControl. 
        /// </summary>
        public NodeModeControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.AngleCheckBox.Tapped += (s, e) =>
            {
                switch (this.ControlPointMode)
                {
                    case SelfControlPointMode.None: this.ControlPointMode = SelfControlPointMode.Angle; break;
                    case SelfControlPointMode.Length: this.ControlPointMode = SelfControlPointMode.Angle; break;
                    case SelfControlPointMode.Angle: this.ControlPointMode = SelfControlPointMode.None; break;
                    case SelfControlPointMode.Disable: this.ControlPointMode = SelfControlPointMode.None; break;
                }
            };

            this.LengthCheckBox.Tapped += (s, e) =>
            {
                switch (this.ControlPointMode)
                {
                    case SelfControlPointMode.None: this.ControlPointMode = SelfControlPointMode.Length; break;
                    case SelfControlPointMode.Length: this.ControlPointMode = SelfControlPointMode.None; break;
                    case SelfControlPointMode.Angle: this.ControlPointMode = SelfControlPointMode.Length; break;
                    case SelfControlPointMode.Disable: this.ControlPointMode = SelfControlPointMode.None; break;
                }
            };
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

            this.RestrictionTextBlock.Text = resource.GetString("Tools_Node_NodeMode_Restriction");
            this.AngleCheckBox.Content = resource.GetString("Tools_Node_NodeMode_Restriction_Angle");
            this.LengthCheckBox.Content = resource.GetString("Tools_Node_NodeMode_Restriction_Length");

            this.ModeTextBlock.Text = resource.GetString("Tools_Node_NodeMode_Mode");
            this.MirroredRadioButton.Content = resource.GetString("Tools_Node_NodeMode_Mode_Mirrored");
            this.DisconnectedRadioButton.Content = resource.GetString("Tools_Node_NodeMode_Mode_Disconnected");
            this.AsymmetricRadioButton.Content = resource.GetString("Tools_Node_NodeMode_Mode_Asymmetric");
        }
    }
}