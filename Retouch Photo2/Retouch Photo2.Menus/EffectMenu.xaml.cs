// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Effects;
using Retouch_Photo2.Effects.Pages;
using Retouch_Photo2.Elements;
using System;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Effects"/>.
    /// </summary>
    public sealed partial class EffectMenu : Expander
    {

        //@Delegate
        /// <summary> Occurs after the splitview pane is closed. </summary>
        public event TypedEventHandler<SplitView, object> PaneClosed { add => this.SplitView.PaneClosed += value; remove => this.SplitView.PaneClosed -= value; }
        /// <summary> Occurs when the splitview pane is opened. </summary>
        public event TypedEventHandler<SplitView, object> PaneOpened { add => this.SplitView.PaneOpened += value; remove => this.SplitView.PaneOpened -= value; }

        public bool MenuIsEnabled { get => this.SplitView.IsEnabled; set => this.SplitView.IsEnabled = value; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "EffectMenu" />'s effect. </summary>
        public Effect Effect
        {
            get => this.effect;
            set
            {
                if (value == null)
                {
                    this.MenuIsEnabled = false;
                }
                else
                {
                    this.MenuIsEnabled = true;

                    this.GaussianBlur.IsSelected = value.GaussianBlur_IsOn;
                    this.DirectionalBlur.IsSelected = value.DirectionalBlur_IsOn;
                    this.Sharpen.IsSelected = value.Sharpen_IsOn;
                    this.OuterShadow.IsSelected = value.OuterShadow_IsOn;
                    this.Edge.IsSelected = value.Edge_IsOn;
                    this.Morphology.IsSelected = value.Morphology_IsOn;
                    this.Emboss.IsSelected = value.Emboss_IsOn;
                    this.Straighten.IsSelected = value.Straighten_IsOn;
                }

                this.SplitView.IsPaneOpen = true;
                this.effect = value;
            }
        }
        private Effect effect;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a EffectMenu. 
        /// </summary>
        public EffectMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructGroup();
            base.Loaded += (s, e) => this.ConstructLanguages();

            this.SplitView.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.SplitView.OpenPaneLength = e.NewSize.Width;
            };
            this.CloseButton.Tapped += (s, e) =>
            {
                this.ContentPresenter.Content = null;
                this.SplitView.IsPaneOpen = true;
            };
        }
    }

    public sealed partial class EffectMenu : Expander
    {

        // Languages
        private void ConstructLanguages()
        {
            if (string.IsNullOrEmpty(ApplicationLanguages.PrimaryLanguageOverride) == false)
            {
                if (ApplicationLanguages.PrimaryLanguageOverride != base.Language)
                {
                    this.ConstructStrings();
                }
            }
        }

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            foreach (UIElement child in this.StackPanel.Children)
            {
                if (child is ListViewItem item)
                {
                    if (item.Content is Grid grid)
                    {
                        if (grid.Children.First() is ContentControl control)
                        {
                            string key = item.Name;
                            string title = resource.GetString($"Effects_{key}");

                            control.Content = title;
                        }
                    }
                }
            }

            this.CloseButton.Content = resource.GetString("Menus_Close");
        }


        //@Group
        private void ConstructGroup()
        {
            foreach (UIElement child in this.StackPanel.Children)
            {
                if (child is ListViewItem item)
                {
                    if (item.Content is Grid grid)
                    {
                        if (grid.Children.First() is ContentControl control)
                        {
                            if (grid.Children.Last() is CheckControl check)
                            {
                                string key = item.Name;
                                EffectType type = EffectType.None;
                                try
                                {
                                    type = (EffectType)Enum.Parse(typeof(EffectType), key);
                                }
                                catch (Exception) { }


                                // Button
                                item.Tapped += (s, e) =>
                                {
                                    if (item.IsSelected)
                                    {
                                        IEffectPage effectPage = Retouch_Photo2.Effects.XML.CreateEffectPage(typeof(GaussianBlurPage), type);
                                        effectPage.FollowPage(this.Effect);
                                        this.ContentPresenter.Content = effectPage.Self;
                                        this.SplitView.IsPaneOpen = false;
                                    }
                                    else Switch();
                                };


                                // Button
                                check.Tapped += (s, e) => { Switch(); e.Handled = true; };


                                void Switch()
                                {
                                    bool isOn = !check.IsChecked;

                                    IEffectPage effectPage = Retouch_Photo2.Effects.XML.CreateEffectPage(typeof(GaussianBlurPage), type);
                                    effectPage?.Switch(isOn);

                                    check.IsChecked = isOn;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}