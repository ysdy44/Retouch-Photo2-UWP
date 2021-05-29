using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed class AdaptiveWidthLayout : Control
    {

        #region DependencyProperty


        /// <summary> Type of <see cref = "AdaptiveWidthLayout" />. </summary>
        public DeviceLayoutType Type
        {
            get => (DeviceLayoutType)base.GetValue(TypeProperty);
            set => base.SetValue(TypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdaptiveWidthLayout.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(DeviceLayoutType), typeof(AdaptiveWidthLayout), new PropertyMetadata(DeviceLayoutType.PC, (sender, e) =>
        {
            AdaptiveWidthLayout control = (AdaptiveWidthLayout)sender;

            if (e.NewValue is DeviceLayoutType value)
            {
                control._vsType = value;
                control.VisualState = control.VisualState; // State
            }
        }));

        #endregion


        VisualStateGroup CommonStates;
        VisualState Normal;
        VisualState PC;
        VisualState Pad;
        VisualState Phone;


        //@VisualState
        DeviceLayoutType _vsType;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsType)
                {
                    case DeviceLayoutType.PC:
                        return this.PC;
                    case DeviceLayoutType.Pad:
                        return this.Pad;
                    case DeviceLayoutType.Phone:
                        return this.Phone;
                    default:
                        return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value?.Name ?? "Normal", false);
        }

        public AdaptiveWidthLayout()
        {
            this.DefaultStyleKey = typeof(AdaptiveWidthLayout);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CommonStates = base.GetTemplateChild(nameof(CommonStates)) as VisualStateGroup;
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.PC = base.GetTemplateChild(nameof(PC)) as VisualState;
            this.Pad = base.GetTemplateChild(nameof(Pad)) as VisualState;
            this.Phone = base.GetTemplateChild(nameof(Phone)) as VisualState;
            this.VisualState = this.VisualState; // State
        }

    }
}