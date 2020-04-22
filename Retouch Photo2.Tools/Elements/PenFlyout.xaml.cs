using FanKit.Transformers;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Flyout of <see cref = "PenTool"/>.
    /// </summary>
    public sealed partial class PenFlyout : UserControl
    {

        //@Content
        private SelfControlPointMode selfMode;
        public SelfControlPointMode SelfMode
        {
            get => this.selfMode;
            set
            {
                switch (value)
                {
                    case SelfControlPointMode.None:
                        {
                            this.AngleCheckBox.IsChecked = false;
                            this.LengthCheckBox.IsChecked = false;
                        }
                        break;
                    case SelfControlPointMode.Length:
                        {
                            this.AngleCheckBox.IsChecked = false;
                            this.LengthCheckBox.IsChecked = true;
                        }
                        break;
                    case SelfControlPointMode.Angle:
                        {
                            this.AngleCheckBox.IsChecked = true;
                            this.LengthCheckBox.IsChecked = false;
                        }
                        break;
                    case SelfControlPointMode.Disable:
                        {
                            this.AngleCheckBox.IsChecked = true;
                            this.LengthCheckBox.IsChecked = true;
                        }
                        break;
                }
                this.selfMode = value;
            }
        }

        public EachControlPointLengthMode EachLengthMode = EachControlPointLengthMode.Equal;
        public EachControlPointAngleMode EachAngleMode = EachControlPointAngleMode.Asymmetric;


        /// <summary> 
        /// Controllers for control points. 
        /// </summary>
        public Node Controller(Vector2 point, Node startingNode, bool isLeftControlPoint)
        {
            return Node.Controller(this.SelfMode, this.EachLengthMode, this.EachAngleMode, point, startingNode, isLeftControlPoint);
        }


        //@Construct
        public PenFlyout()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructSelfMode();
            this.ConstructEachMode();
            this.Loaded += (s, e) =>
            {
                this.MirroredRadioButton.IsChecked = true;

                this.EachLengthMode = EachControlPointLengthMode.Equal;
                this.EachAngleMode = EachControlPointAngleMode.Asymmetric;
            };
        }


        //SelfMode
        private void ConstructSelfMode()
        {
            this.AngleCheckBox.Tapped += (s, e) =>
            {
                switch (this.SelfMode)
                {
                    case SelfControlPointMode.None: this.SelfMode = SelfControlPointMode.Angle; break;
                    case SelfControlPointMode.Length: this.SelfMode = SelfControlPointMode.Angle; break;
                    case SelfControlPointMode.Angle: this.SelfMode = SelfControlPointMode.None; break;
                    case SelfControlPointMode.Disable: this.SelfMode = SelfControlPointMode.None; break;
                }
            };
            this.LengthCheckBox.Tapped += (s, e) =>
            {
                switch (this.SelfMode)
                {
                    case SelfControlPointMode.None: this.SelfMode = SelfControlPointMode.Length; break;
                    case SelfControlPointMode.Length: this.SelfMode = SelfControlPointMode.None; break;
                    case SelfControlPointMode.Angle: this.SelfMode = SelfControlPointMode.Length; break;
                    case SelfControlPointMode.Disable: this.SelfMode = SelfControlPointMode.None; break;
                }
            };
        }

        //EachMode
        private void ConstructEachMode()
        {
            this.MirroredRadioButton.Tapped += (s, e) =>
            {
                this.EachLengthMode = EachControlPointLengthMode.Equal;
                this.EachAngleMode = EachControlPointAngleMode.Asymmetric;
            };
            this.DisconnectedRadioButton.Tapped += (s, e) =>
            {
                this.EachLengthMode = EachControlPointLengthMode.None;
                this.EachAngleMode = EachControlPointAngleMode.None;
            };
            this.AsymmetricRadioButton.Tapped += (s, e) =>
            {
                this.EachLengthMode = EachControlPointLengthMode.None;
                this.EachAngleMode = EachControlPointAngleMode.Asymmetric;
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RestrictionTextBlock.Text = resource.GetString("/Tools/PenFlyout_Restriction");
            this.AngleCheckBox.Content = resource.GetString("/Tools/PenFlyout_Restriction_Angle");
            this.LengthCheckBox.Content = resource.GetString("/Tools/PenFlyout_Restriction_Length");

            this.ModeTextBlock.Text = resource.GetString("/Tools/PenFlyout_Mode");
            this.MirroredRadioButton.Tag = resource.GetString("/Tools/PenFlyout_Mode_Mirrored");
            this.DisconnectedRadioButton.Tag = resource.GetString("/Tools/PenFlyout_Mode_Disconnected");
            this.AsymmetricRadioButton.Tag = resource.GetString("/Tools/PenFlyout_Mode_Asymmetric");
        }

    }
}