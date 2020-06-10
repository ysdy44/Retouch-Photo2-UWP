using FanKit.Transformers;
using Retouch_Photo2.Tools.Elements.PenModeControlIcons;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Mode-contorl of <see cref = "PenTool"/>.
    /// </summary>
    public sealed partial class PenModeControl : UserControl
    {

        //@Content    
        private SelfControlPointMode selfMode = SelfControlPointMode.None;
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



        //@Construct
        public PenModeControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructSelfMode();
            this.ConstructEachMode();
        }


        //SelfMode
        private void ConstructSelfMode()
        {
            this.AngleCheckBox.Click += (s, e) =>
            {
                switch (this.SelfMode)
                {
                    case SelfControlPointMode.None: this.SelfMode = SelfControlPointMode.Angle; break;
                    case SelfControlPointMode.Length: this.SelfMode = SelfControlPointMode.Angle; break;
                    case SelfControlPointMode.Angle: this.SelfMode = SelfControlPointMode.None; break;
                    case SelfControlPointMode.Disable: this.SelfMode = SelfControlPointMode.None; break;
                }
            };
            this.LengthCheckBox.Click += (s, e) =>
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
            this.MirroredRadioButton.Checked += (s, e) =>
            {
                this.EachLengthMode = EachControlPointLengthMode.Equal;
                this.EachAngleMode = EachControlPointAngleMode.Asymmetric;
            };
            this.DisconnectedRadioButton.Checked += (s, e) =>
            {
                this.EachLengthMode = EachControlPointLengthMode.None;
                this.EachAngleMode = EachControlPointAngleMode.None;
            };
            this.AsymmetricRadioButton.Checked += (s, e) =>
            {
                this.EachLengthMode = EachControlPointLengthMode.None;
                this.EachAngleMode = EachControlPointAngleMode.Asymmetric;
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