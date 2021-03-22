using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Operates
{
    /// <summary>
    /// Control of <see cref="Retouch_Photo2.Operates"/>.
    /// </summary>
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(Disabled), GroupName = nameof(CommonStates))]
    public sealed class OperateControl : ContentControl
    {

        VisualStateGroup CommonStates;
        VisualState Normal;
        VisualState Disabled;


        //@VisualState
        bool _vsIsEnabled;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsEnabled) return this.Normal;
                else return this.Disabled;
            }
            set => VisualStateManager.GoToState(this, value?.Name ?? "Normal", true);
        }

        //@Construct
        /// <summary>
        /// Initializes a OperateControl. 
        /// </summary>
        public OperateControl()
        {
            this.DefaultStyleKey = typeof(OperateControl);

            this._vsIsEnabled = this.IsEnabled;
            this.VisualState = this.VisualState;//State

            this.IsEnabledChanged += (s, e) =>
            {
                this._vsIsEnabled = this.IsEnabled;
                this.VisualState = this.VisualState;//State
            };
        }


        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CommonStates = base.GetTemplateChild(nameof(CommonStates)) as VisualStateGroup;
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.Disabled = base.GetTemplateChild(nameof(Disabled)) as VisualState;
            this._vsIsEnabled = this.IsEnabled;
            this.VisualState = this.VisualState;//State
        }

    }
}