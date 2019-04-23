using Retouch_Photo2.Effects;
using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    public enum EffectsControlState
    {
        None,
        Disable,
        Effects,
        Edit
    }

    public sealed partial class EffectsControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        private EffectsControlState state;
        public EffectsControlState State
        {
            get => this.state;
            set
            {
                this.ItemsControl.Visibility = (value == EffectsControlState.Edit) ? Visibility.Collapsed : Visibility.Visible;
                this.BackButton.Visibility = this.ResetButton.Visibility = this.Frame.Visibility = (value == EffectsControlState.Edit) ? Visibility.Visible : Visibility.Collapsed;

                if (value == EffectsControlState.Disable)
                {
                    //Controls foreach
                    foreach (var page in EffectPage.PageList)
                    {
                        page.Control.Button.IsEnabled =
                        page.Control.ToggleSwitch.IsEnabled = false;
                    }
                }

                this.state = value;
            }
        }

        private EffectPage page;
        public EffectPage Page
        {
            get => this.page;
            set
            {
                this.Frame.Child = value;

                this.page = value;
            }
        }


        #region DependencyProperty


        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(EffectsControl), new PropertyMetadata(null, (sender, e) =>
        {
            EffectsControl con = (EffectsControl)sender;

            if (e.NewValue is Layer layer)
            {
                con.Invalidate(layer.EffectManager);
            }
            else
            {
                con.State = EffectsControlState.Disable;
            }
        }));

        #endregion


        public EffectsControl()
        {
            this.InitializeComponent();
            this.State = EffectsControlState.Disable;
            this.ItemsControl.ItemsSource = from item in EffectPage.PageList select item.Control;

            //Effect
            Retouch_Photo2.Effects.EffectManager.Invalidate = () => this.ViewModel.Invalidate();

            //Button
            this.BackButton.Tapped += (sender, e) => this.Close();
            this.ResetButton.Tapped += (sender, e) => this.Reset();

            //Controls foreach
            foreach (var page in EffectPage.PageList)
            {
                page.Control.Button.Tapped += (s, e) => this.Edit(page);
                page.Control.ToggleSwitch.Toggled += (s, e) => this.Togge(page);
            }
        }


        /// <summary> Togge the Effect. </summary>
        public void Togge(EffectPage page)
        {
            if (this.Layer == null) return;
            if (this.Layer.EffectManager == null) return;

            bool isOn = page.Control.ToggleSwitch.IsOn;
            page.Control.Button.IsEnabled = isOn;
            page.SetIsOn(this.Layer.EffectManager, isOn);

            this.ViewModel.Invalidate();
        }
        /// <summary> Edit the Effect. </summary>
        public void Edit(EffectPage page)
        {
            if (page == null) return;
            if (this.Layer == null) return;
            if (this.Layer.EffectManager == null) return;

            this.Page = page;
            this.Page.SetManager(this.Layer.EffectManager);

            this.State = EffectsControlState.Edit;
        }
        /// <summary> Clear the Effect. </summary>
        private void Close()
        {
            if (this.Page == null) return;

            this.Page.Close();
            this.Page = null;

            this.State = EffectsControlState.Effects;
        }
        /// <summary> Reset the Effect. </summary>
        private void Reset()
        {
            if (this.Page == null) return;

            this.Page.Reset();

            this.ViewModel.Invalidate();
        }

        /// <summary> Invalidate Effect ItemsControl. </summary>
        public void Invalidate(EffectManager manager)
        {
            if (manager == null) return;

            //Controls foreach
            foreach (var page in EffectPage.PageList)
            {
                page.Control.ToggleSwitch.IsEnabled = true;
                page.Control.ToggleSwitch.IsOn = page.GetIsOn(manager);
            }
            this.State = EffectsControlState.Effects;
        }
    }
}
