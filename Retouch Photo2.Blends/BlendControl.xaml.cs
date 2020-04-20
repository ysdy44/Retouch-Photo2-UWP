using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends.Icons;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Retouch_Photo2 Blends 's Control.
    /// </summary>
    public sealed partial class BlendControl : UserControl
    {

        //@Delegate
        public EventHandler<BlendEffectMode?> BlendTypeChanged;


        #region DependencyProperty


        /// <summary> Gets or sets the BlendType. </summary>
        public BlendEffectMode? BlendType
        {
            get { return (BlendEffectMode?)GetValue(BlendTypeProperty); }
            set { SetValue(BlendTypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BlendControl.BlendType" /> dependency property. </summary>
        public static readonly DependencyProperty BlendTypeProperty = DependencyProperty.Register(nameof(BlendType), typeof(BlendEffectMode?), typeof(BlendControl), new PropertyMetadata(null, (sender, e) =>
        {
            BlendControl con = (BlendControl)sender;

            if (e.NewValue is BlendEffectMode value)
            {
                con.SelectedButtons(value);
            }
            else
            {
                con.SelectedNormalButton();
            }
        }));


        /// <summary> Gets or sets the Title. </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BlendControl.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(BlendControl), new PropertyMetadata(null));


        #endregion

        //NormalButton
        private BlendButton NormalButton;
        //Buttons
        private IList<BlendButton> Buttons;


        //@Construct
        public BlendControl()
        {
            this.ConstructStrings();
            this.InitializeComponent();

            this.NormalButton.IsSelected = true;
            this.Title = this.NormalButton.Text;
        }


        private void InitializeComponent()
        {
            StackPanel stackPanel = new StackPanel();

            this.Content = new ScrollViewer
            {
                Content = stackPanel
            };

            //NormalButton
            {
                stackPanel.Children.Add(this.NormalButton);
                this.NormalButton.Tapped += (s, e) =>
                {
                    this.BlendTypeChanged?.Invoke(this, null);//Delegate
                };
            }

            //Buttons
            foreach (BlendButton item in this.Buttons)
            {
                if (item == null)
                {
                    stackPanel.Children.Add(new BlendSeparator());
                }
                else
                {
                    stackPanel.Children.Add(item);
                    item.Tapped += (s, e) =>
                    {
                        this.BlendTypeChanged?.Invoke(this, item.BlendType);//Delegate
                    };
                }
            }
        }

        //NormalButton
        private void SelectedNormalButton()
        {
            this.NormalButton.IsSelected = true;
            this.Title = this.NormalButton.Text;

            foreach (BlendButton item in this.Buttons)
            {
                if (item == null) continue;
                item.IsSelected = false;
            }
        }
        //Buttons
        private void SelectedButtons(BlendEffectMode value)
        {
            this.NormalButton.IsSelected = false;

            foreach (BlendButton item in this.Buttons)
            {
                if (item == null) continue;

                bool isSelected = (item.BlendType == value);

                if (isSelected)
                {
                    this.Title = item.Text;
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }

    }

    /// <summary>
    /// Retouch_Photo2 Blends 's Control.
    /// </summary>
    public sealed partial class BlendControl : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            //NormalButton
            this.NormalButton = new BlendButton
            {
                Text = resource.GetString("/Blends/Normal"),
                CenterContent = new NormalIcon(),
                BlendType = null,
            };

            //Buttons
            this.Buttons = new List<BlendButton>
            {

                null,

                new BlendButton
                {
                    Text = resource.GetString("/Blends/Multiply"),
                    CenterContent = new MultiplyIcon(),
                    BlendType = BlendEffectMode.Multiply,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/Screen"),
                    CenterContent = new ScreenIcon(),
                    BlendType = BlendEffectMode.Screen,
                },

                null,

                new BlendButton
                {
                    Text = resource.GetString("/Blends/Darken"),
                    CenterContent = new DarkenIcon(),
                    BlendType = BlendEffectMode.Darken,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/Lighten"),
                    CenterContent = new LightenIcon(),
                    BlendType = BlendEffectMode.Lighten,
                },

                null,

                new BlendButton
                {
                    Text = resource.GetString("/Blends/Dissolve"),
                    CenterContent = new DissolveIcon(),
                    BlendType = BlendEffectMode.Dissolve,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/ColorBurn"),
                    CenterContent = new ColorBurnIcon(),
                    BlendType = BlendEffectMode.ColorBurn,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/LinearBurn"),
                    CenterContent = new LinearBurnIcon(),
                    BlendType = BlendEffectMode.LinearBurn,
                },

                null,

                new BlendButton
                {
                    Text = resource.GetString("/Blends/DarkerColor"),
                    CenterContent = new DarkerColorIcon(),
                    BlendType = BlendEffectMode.DarkerColor,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/LighterColor"),
                    CenterContent = new LighterColorIcon(),
                    BlendType = BlendEffectMode.LighterColor,
                },

                null,

                new BlendButton
                {
                    Text = resource.GetString("/Blends/ColorDodge"),
                    CenterContent = new ColorDodgeIcon(),
                    BlendType = BlendEffectMode.ColorDodge,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/LinearDodge"),
                    CenterContent = new LinearDodgeIcon(),
                    BlendType = BlendEffectMode.LinearDodge,
                },

                null,

                new BlendButton
                {
                    Text = resource.GetString("/Blends/Overlay"),
                    CenterContent = new OverlayIcon(),
                    BlendType = BlendEffectMode.Overlay,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/SoftLight"),
                    CenterContent = new SoftLightIcon(),
                    BlendType = BlendEffectMode.SoftLight,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/HardLight"),
                    CenterContent = new HardLightIcon(),
                    BlendType = BlendEffectMode.HardLight,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/VividLight"),
                    CenterContent = new VividLightIcon(),
                    BlendType = BlendEffectMode.VividLight,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/LinearLight"),
                    CenterContent = new LinearLightIcon(),
                    BlendType = BlendEffectMode.LinearLight,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/PinLight"),
                    CenterContent = new PinLightIcon(),
                    BlendType = BlendEffectMode.PinLight,
                },

                null,

                new BlendButton
                {
                    Text = resource.GetString("/Blends/HardMix"),
                    CenterContent = new HardMixIcon(),
                    BlendType = BlendEffectMode.HardMix,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/Difference"),
                    CenterContent = new DifferenceIcon(),
                    BlendType = BlendEffectMode.Difference,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/Exclusion"),
                    CenterContent = new ExclusionIcon(),
                    BlendType = BlendEffectMode.Exclusion,
                },

                null,

                new BlendButton
                {
                    Text = resource.GetString("/Blends/Hue"),
                    CenterContent = new HueIcon(),
                    BlendType = BlendEffectMode.Hue,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/Saturation"),
                    CenterContent = new SaturationIcon(),
                    BlendType = BlendEffectMode.Saturation,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/Color"),
                    CenterContent = new ColorIcon(),
                    BlendType = BlendEffectMode.Color,
                },

                null,

                new BlendButton
                {
                    Text = resource.GetString("/Blends/Luminosity"),
                    CenterContent = new LuminosityIcon(),
                    BlendType = BlendEffectMode.Luminosity,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/Subtract"),
                    CenterContent = new SubtractIcon(),
                    BlendType = BlendEffectMode.Subtract,
                },
                new BlendButton
                {
                    Text = resource.GetString("/Blends/Division"),
                    CenterContent = new DivisionIcon(),
                    BlendType = BlendEffectMode.Division,
                },
            };
        }

    }
}