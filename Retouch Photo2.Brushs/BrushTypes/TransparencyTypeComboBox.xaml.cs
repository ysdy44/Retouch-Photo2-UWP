// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// ComboBox of Transparency<see cref="BrushType"/>
    /// </summary>
    public sealed partial class TransparencyTypeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when type change. </summary>
        public EventHandler<BrushType> TypeChanged;

        //@Group
        /// <summary> Occurs when group change. </summary>
        private EventHandler<BrushType> Group;


        #region DependencyProperty


        /// <summary> Gets or sets the transparency. </summary>
        public IBrush Transparency
        {
            get => (IBrush)base.GetValue(TransparencyProperty);
            set => base.SetValue(TransparencyProperty, value);
        }
        /// <summary> Identifies the <see cref = "BrushTypeComboBox.Fill" /> dependency property. </summary>
        public static readonly DependencyProperty TransparencyProperty = DependencyProperty.Register(nameof(Transparency), typeof(IBrush), typeof(TransparencyTypeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            TransparencyTypeComboBox control = (TransparencyTypeComboBox)sender;

            if (e.NewValue is IBrush value)
            {
                control.Type = value.Type;
            }
            else
            {
                control.Type = BrushType.None;
            }
        }));

        /// <summary> Gets or sets the transparency type. </summary>
        public BrushType Type
        {
            get => this.type;
            set
            {
                this.Group?.Invoke(this, value);//Delegate
                this.type = value;
            }
        }
        private BrushType type = BrushType.None;


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TransparencyTypeComboBox. 
        /// </summary>
        public TransparencyTypeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Click += (s, e) => this.Flyout.ShowAt(this);
        }

    }

    /// <summary>
    /// ComboBox of Transparency<see cref="BrushType"/>
    /// </summary>
    public sealed partial class TransparencyTypeComboBox : UserControl
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
                        BrushType type = XML.CreateBrushType(key);
                        string title = resource.GetString($"Tools_Brush_Type_{key}");


                        //Button
                        this.ConstructButton(button, key, type, title);


                        //Group
                        group(this.Type);
                        this.Group += (s, groupMode) => group(groupMode);

                        void group(BrushType groupType)
                        {
                            if (groupType == type)
                            {
                                button.IsEnabled = false;

                                this.Button.Content = title;
                            }
                            else button.IsEnabled = true;
                        }
                    }
                }
            }
        }

        private void ConstructButton(Button button, string key, BrushType type, string title)
        {
            /*                
             <Button x:Name="None" Style="{StaticResource AppIconSelectedButton}">
                 <Button.Resources>
                     <ResourceDictionary Source="ms-appx:///Retouch Photo2.Brushs\BrushTypes\TransparencyTypeIcons\NoneIcon.xaml"/>
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
                Source = new Uri($@"ms-appx:///Retouch Photo2.Brushs\BrushTypes\TransparencyTypeIcons\{key}Icon.xaml")
            };
            button.Tag = new ContentControl
            {
                //@Template
                Template = button.Resources[$"{key}Icon"] as ControlTemplate
            };
            button.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, type);//Delegate

                this.Flyout.Hide();
            };
        }

    }
}