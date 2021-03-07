// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas.Effects;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// ComboBox of <see cref="BlendEffect"/>
    /// </summary>
    public sealed partial class BlendModeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when mode change. </summary>
        public EventHandler<BlendEffectMode?> ModeChanged;

        //@Group
        /// <summary> Occurs when group change. </summary>
        private EventHandler<BlendEffectMode?> Group;
        
        #region DependencyProperty


        /// <summary> Gets or sets the blend-type. </summary>
        public BlendEffectMode? Mode
        {
            get => (BlendEffectMode?)base.GetValue(ModeProperty);
            set => base.SetValue(ModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "BlendModeComboBox.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(BlendEffectMode?), typeof(BlendModeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BlendModeComboBox control = (BlendModeComboBox)sender;

            if (e.NewValue is BlendEffectMode value)
            {
                control.Group?.Invoke(control, value);//Delegate
            }
            else
            {
                control.Group?.Invoke(control, null);//Delegate
            }
        }));


        /// <summary> Gets or sets the title. </summary>
        public object Title
        {
            get  => (object)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "BlendModeComboBox.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(object), typeof(BlendModeComboBox), new PropertyMetadata(null));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a BlendModeComboBox. 
        /// </summary>
        public BlendModeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Represents the combo box that is used to select blend mode.
    /// </summary>
    public sealed partial class BlendModeComboBox : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();


            foreach (UIElement child in this.StackPanel.Children)
            {
                if (child is Button button)
                {

                    //@Group
                    //void constructGroup(Button button)
                    {
                        string key = button.Name;
                        BlendEffectMode? mode = XML.CreateBlendMode(key);
                        string title = resource.GetString($"Blends_{key}");


                        //Button
                        this.ConstructButton(button, key, mode, title);


                        //Group
                        group(this.Mode);
                        this.Group += (s, groupMode) => group(groupMode);

                        void group(BlendEffectMode? groupMode)
                        {
                            if (groupMode == mode)
                            {
                                button.IsEnabled = false;

                                this.Title = title;
                            }
                            else button.IsEnabled = true;
                        }
                    }
                }
            }


            
        }


        private void ConstructButton(Button button, string key, BlendEffectMode? mode, string title)
        {
            /*                
             <Button x:Name="None" Style="{StaticResource AppIconSelectedButton}">
                 <Button.Resources>
                     <ResourceDictionary Source="ms-appx:///Retouch Photo2.Blends\Icons\NoneIcon.xaml"/>
                 </Button.Resources>
                 <Button.Tag>
                     <ContentControl Template="{StaticResource NoneIcon}"/>
                 </Button.Tag>
             </Button> 
           */
            button.Content = title;
            button.Resources = new ResourceDictionary
            {
                //@Template
                Source = new Uri($@"ms-appx:///Retouch Photo2.Blends\Icons\{key}Icon.xaml")
            };
            button.Tag = new ContentControl
            {
                //@Template
                Template = button.Resources[$"{key}Icon"] as ControlTemplate
            };
            button.Click += (s, e) => this.ModeChanged?.Invoke(this, mode);//Delegate

        }

    }
}