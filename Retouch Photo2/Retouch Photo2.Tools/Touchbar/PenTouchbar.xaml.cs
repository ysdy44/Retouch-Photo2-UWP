using FanKit.Transformers;
using Retouch_Photo2.Tools.Models;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Touchbar
{
    /// <summary>
    /// Touchbar of <see cref = "PenTool"/>.
    /// </summary>
    public sealed partial class PenTouchbar : UserControl
    {
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

        public EachControlPointLengthMode EachLengthMode;
        public EachControlPointAngleMode EachAngleMode;

        public Node Controller(Vector2 point, Node startingNode, bool isLeftControlPoint) => Node.Controller(this.SelfMode, this.EachLengthMode, this.EachAngleMode, point, startingNode, isLeftControlPoint);

        //@Construct
        public PenTouchbar()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => 
            {
                this.MirroredRadioButton.IsChecked = true;

                this.EachLengthMode = EachControlPointLengthMode.Equal;
                this.EachAngleMode = EachControlPointAngleMode.Asymmetric;
            };

            //SelfMode
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

            //EachMode
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
    }
}