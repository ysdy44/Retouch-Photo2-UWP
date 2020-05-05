using Retouch_Photo2.Brushs.BrushTypeIcons;
using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents the contorl that is used to select brush type.
    /// </summary>
    public sealed partial class BrushTypeControl : UserControl
    {

        //@Delegate
        public EventHandler<BrushType> FillTypeChanged;
        public EventHandler<BrushType> StrokeTypeChanged;

        //Buttons
        private IList<Button> Buttons = new List<Button>();


        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            set
            {
                this._vsFillOrStroke=value;
                this.VisualState = this.VisualState;//State
            }
        }

        /// <summary> Gets or sets the fill-brush. </summary>
        public IBrush FillBrush
        {
            set
            {
                this._vsFillType = value.Type;
                this.VisualState = this.VisualState;//State
            }
        }

        /// <summary> Gets or sets the stroke-brush. </summary>
        public IBrush StrokeBrush
        {
            set
            {
                this._vsStrokeType = value.Type;
                this.VisualState = this.VisualState;//State
            }
        }


        #endregion


        //@VisualState
        FillOrStroke _vsFillOrStroke;
        BrushType _vsFillType;
        BrushType _vsStrokeType;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsFillOrStroke)
                {
                    case FillOrStroke.Fill: return this._getTypeVisualState(this._vsFillType);
                    case FillOrStroke.Stroke: return this._getTypeVisualState(this._vsStrokeType);
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        private VisualState _getTypeVisualState(BrushType type)
        {
            switch (type)
            {
                case BrushType.None: return this.None;

                case BrushType.Color: return this.Color;

                case BrushType.LinearGradient: return this.LinearGradient;
                case BrushType.RadialGradient: return this.RadialGradient;
                case BrushType.EllipticalGradient: return this.EllipticalGradient;

                case BrushType.Image: return this.Image;
            }

            return this.Normal;
        }


        //@Construct
        public BrushTypeControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this);
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructButton(this.NoneButton, resource.GetString("/ToolElements/BrushType_None"), new NoneIcon(), BrushType.None);

            this.ConstructButton(this.ColorButton, resource.GetString("/ToolElements/BrushType_Color"), new ColorIcon(), BrushType.Color);

            this.ConstructButton(this.LinearGradientButton, resource.GetString("/ToolElements/BrushType_LinearGradient"), new LinearGradientIcon(), BrushType.LinearGradient);
            this.ConstructButton(this.RadialGradientButton, resource.GetString("/ToolElements/BrushType_RadialGradient"), new RadialGradientIcon(), BrushType.RadialGradient);
            this.ConstructButton(this.EllipticalGradientButton, resource.GetString("/ToolElements/BrushType_EllipticalGradient"), new EllipticalGradientIcon(), BrushType.EllipticalGradient);

            this.ConstructButton(this.ImageButton, resource.GetString("/ToolElements/BrushType_Image"), new ImageIcon(), BrushType.Image);
        }

        private void ConstructButton(Button button, string text, UserControl icon, BrushType brushType)
        {
            button.Content = text;
            button.Tag = icon;
            button.Tapped += (s, e) =>
            {
                switch (this._vsFillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillTypeChanged?.Invoke(this, brushType);//Delegate
                        this.Flyout.Hide();
                        break;

                    case FillOrStroke.Stroke:
                        this.StrokeTypeChanged?.Invoke(this, brushType);//Delegate
                        this.Flyout.Hide();
                        break;
                }
            };

            this.Buttons.Add(button);
        }
    }
}
