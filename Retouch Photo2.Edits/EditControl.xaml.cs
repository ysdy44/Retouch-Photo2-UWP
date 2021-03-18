using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Edits
{
    /// <summary>
    /// Control of <see cref="Retouch_Photo2.Edits"/>.
    /// </summary>
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(Disabled), GroupName = nameof(CommonStates))]
    public sealed class EditControl : ContentControl
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
        /// Initializes a EditControl. 
        /// </summary>
        public EditControl(string key, string folder)
        {
            this.DefaultStyleKey = typeof(EditControl);

            this._vsIsEnabled = this.IsEnabled;
            this.VisualState = this.VisualState;//State

            this.IsEnabledChanged += (s, e) =>
            {
                this._vsIsEnabled = this.IsEnabled;
                this.VisualState = this.VisualState;//State
            };

            this.Loaded += (s, e) =>
            {
                this.Resources = new ResourceDictionary
                {
                    //@Template
                    Source = new Uri($@"ms-appx:///Retouch Photo2.Edits\{folder}Icons\{key}Icon.xaml")
                };
                {
                    //@Template
                    this.Template = this.Resources[$"{key}Icon"] as ControlTemplate;
                }
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