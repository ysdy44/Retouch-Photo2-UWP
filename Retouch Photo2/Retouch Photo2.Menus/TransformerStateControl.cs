using FanKit.Transformers;
using Retouch_Photo2.Tools;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary> 
    /// State of <see cref="TransformerStateControl"/>. 
    /// </summary>
    internal enum TransformerState
    {
        /// <summary> Enabled. </summary>
        Enabled,

        /// <summary> Disabled. </summary>
        Disabled
    }

    /// <summary> 
    /// Control of <see cref="TransformerState"/>. 
    /// </summary>
    internal class TransformerStateControl : ContentControl
    {

        //@Delegate
        /// <summary> Occurs when state changed. </summary>
        public Action<TransformerState, Transformer> StateChanged;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "TransformerStateControl" />'s tool type. </summary>
        public ToolType ToolType
        {
            get => (ToolType)base.GetValue(ToolTypeProperty);
            set => base.SetValue(ToolTypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransformerStateControl.ToolType" /> dependency property. </summary>
        public static readonly DependencyProperty ToolTypeProperty = DependencyProperty.Register(nameof(ToolType), typeof(ToolType), typeof(TransformerStateControl), new PropertyMetadata(ToolType.Cursor, (sender, e) =>
        {
            TransformerStateControl control = (TransformerStateControl)sender;

            if (e.NewValue is ToolType value)
            {
                switch (value)
                {
                    case ToolType.Cursor:
                    case ToolType.View:

                    case ToolType.GeometryRectangle:
                    case ToolType.GeometryEllipse:

                    case ToolType.TextFrame:
                    case ToolType.TextArtistic:

                    case ToolType.Image:
                    case ToolType.Crop:


                    //Pattern
                    case ToolType.PatternGrid:
                    case ToolType.PatternDiagonal:
                    case ToolType.PatternSpotted:


                    //Geometry1
                    case ToolType.GeometryRoundRect:
                    case ToolType.GeometryTriangle:
                    case ToolType.GeometryDiamond:

                    //Geometry2
                    case ToolType.GeometryPentagon:
                    case ToolType.GeometryStar:
                    case ToolType.GeometryCog:

                    //Geometry3
                    case ToolType.GeometryDount:
                    case ToolType.GeometryPie:
                    case ToolType.GeometryCookie:

                    //Geometry4
                    case ToolType.GeometryArrow:
                    case ToolType.GeometryCapsule:
                    case ToolType.GeometryHeart:
                        {
                            control._vsDisabledTool = false;
                            control.VisualState = control.VisualState;//State
                            return;
                        }
                }
            }

            control._vsDisabledTool = true;
            control.VisualState = control.VisualState;//State
            return;
        }));


        /// <summary> Gets or sets <see cref = "TransformerStateControl" />'s selection mode. </summary>
        public ListViewSelectionMode Mode
        {
            get => (ListViewSelectionMode)base.GetValue(ModeProperty);
            set => base.SetValue(ModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransformerStateControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(ListViewSelectionMode), typeof(TransformerStateControl), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            TransformerStateControl control = (TransformerStateControl)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
                control._vsMode = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        /// <summary> Gets or sets <see cref = "TransformerStateControl" />'s transformer. </summary>
        public Transformer Transformer
        {
            get => (Transformer)base.GetValue(TransformerProperty);
            set => base.SetValue(TransformerProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransformerStateControl.Transformer" /> dependency property. </summary>
        public static readonly DependencyProperty TransformerProperty = DependencyProperty.Register(nameof(Transformer), typeof(Transformer), typeof(TransformerStateControl), new PropertyMetadata(new Transformer(), (sender, e) =>
        {
            TransformerStateControl control = (TransformerStateControl)sender;

            if (e.NewValue is Transformer value)
            {
                control._vsTransformer = value;
                control.VisualState = control.VisualState;//State
            }
        }));

        #endregion


        //@VisualState
        bool _vsDisabledTool;
        Transformer _vsTransformer;
        ListViewSelectionMode _vsMode;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public TransformerState VisualState
        {
            get
            {
                if (this._vsDisabledTool) return TransformerState.Disabled;

                switch (this._vsMode)
                {
                    case ListViewSelectionMode.None: return TransformerState.Disabled;
                    case ListViewSelectionMode.Single:
                    case ListViewSelectionMode.Multiple:
                        return TransformerState.Enabled;
                }

                return TransformerState.Enabled;
            }
            set => this.StateChanged?.Invoke(value, this._vsTransformer);//asdsad
        }

    }
}
