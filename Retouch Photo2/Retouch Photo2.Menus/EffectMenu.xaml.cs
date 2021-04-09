// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Effects;
using Retouch_Photo2.Elements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Effects"/>.
    /// </summary>
    public sealed partial class EffectMenu : UserControl, ICollection<IEffectPage>
    {

        private readonly IList<IEffectPage> Items = new List<IEffectPage>();
        private readonly IList<(EffectType, IEffectPage, Button, CheckControl)> ItemsCore = new List<(EffectType, IEffectPage, Button, CheckControl)>();

        

        public int Count => this.Items.Count;
        public bool IsReadOnly => false;


        public void Add(IEffectPage effectPage)
        {
            if (effectPage == null) return;

            EffectType type = effectPage.Type;

            Button button = new Button
            {
                IsEnabled = false,
                Content = effectPage.Title,
                Style = this.IconButton,
                Tag = new ContentControl
                {
                    Template = effectPage.Icon
                }
            };
            button.Click += this.Button_Click;
            this.ButtonsStackPanel.Children.Add(button);

            CheckControl check = new CheckControl
            {
                Height = button.Height
            };
            check.Tapped += this.Check_Tapped;
            this.ChecksStackPanel.Children.Add(check);

            this.ItemsCore.Add((type, effectPage, button, check));
            this.Items.Add(effectPage);
        }

        public bool Remove(IEffectPage effectPage)
        {
            if (effectPage == null) return false;

            bool isContains = this.Items.Contains(effectPage);
            if (isContains == false) return false;

            var item = this.ItemsCore.First(e => e.Item2 == effectPage);

            Button button = item.Item3;
            button.Click -= this.Button_Click;
            this.ButtonsStackPanel.Children.Remove(button);

            CheckControl check = item.Item4;
            check.Tapped -= this.Check_Tapped;
            this.ChecksStackPanel.Children.Remove(check);

            this.ItemsCore.Remove(item);
            this.Items.Remove(effectPage);
            return true;
        }

        public void Clear()
        {
            foreach (var item in this.ItemsCore)
            {
                Button button = item.Item3;
                button.Click -= this.Button_Click;

                CheckControl check = item.Item4;
                check.Tapped -= this.Check_Tapped;
            }

            this.ButtonsStackPanel.Children.Clear();

            this.ChecksStackPanel.Children.Clear();

            this.ItemsCore.Clear();
            this.Items.Clear();
        }


        public bool Contains(IEffectPage item) => this.Items.Contains(item);
        public void CopyTo(IEffectPage[] array, int arrayIndex) => this.Items.CopyTo(array, arrayIndex);

        public IEnumerator<IEffectPage> GetEnumerator() => this.Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.Items.GetEnumerator();

    }


    public sealed partial class EffectMenu : UserControl, ICollection<IEffectPage>
    {

        /// <summary> Gets or sets <see cref = "EffectMenu" />'s effect pages. </summary>
        public ICollection<IEffectPage> EffectPages => this;


        /// <summary> Gets or sets <see cref = "EffectMenu" />'s effect. </summary>
        public Effect Effect
        {
            get => this.effect;
            set
            {
                if (value != null)
                {
                    foreach (var item in ItemsCore)
                    {
                        IEffectPage effectPage = item.Item2;
                        bool isOn = effectPage.FollowButton(value);

                        Button button = item.Item3;
                        button.IsEnabled = isOn;

                        CheckControl check = item.Item4;
                        check.IsEnabled = true;
                        check.IsChecked = isOn;
                    }
                }
                else
                {
                    foreach (var item in ItemsCore)
                    {
                        Button button = item.Item3;
                        button.IsEnabled = false;

                        CheckControl check = item.Item4;
                        check.IsEnabled = false;
                    }
                }


                this.effect = value;
            }
        }
        private Effect effect;


        //@Construct
        /// <summary>
        /// Initializes a EffectMainPage. 
        /// </summary>
        public EffectMenu()
        {
            this.InitializeComponent();
            this.ConstructStringsss();

            base.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.SplitView.OpenPaneLength = e.NewSize.Width;
            };

            this.CloseButton.Click += (s, e) =>
            {
                this.ContentPresenter.Content = null;
                this.SplitView.IsPaneOpen = true;
            };
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                IEffectPage effectPage = this.ItemsCore.First(p => p.Item3 == button).Item2;

                this.ContentPresenter.Content = effectPage?.Self;
                this.SplitView.IsPaneOpen = false;
            }
        }
        private void Check_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is CheckControl check)
            {
                bool isOn = !check.IsChecked;

                var item = this.ItemsCore.First(p => p.Item4 == check);
                IEffectPage effectPage = item.Item2;
                effectPage?.Switch(isOn);

                Button button = item.Item3;
                button.IsEnabled = isOn;

                check.IsEnabled = true;
                check.IsChecked = isOn;
                check.IsChecked = isOn;
            }
        }


        public void ConstructStringsss()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.CloseButton.Content = resource.GetString("Menus_Close");
        }

    }
}