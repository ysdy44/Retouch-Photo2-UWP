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
    /// ComboBox OF <see cref="Retouch_Photo2.Brushs.FillOrStroke"/>.
    /// </summary>
    public sealed partial class FillOrStrokeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when fill or stroke change. </summary>
        public EventHandler<FillOrStroke> FillOrStrokeChanged;

        //@Group
        /// <summary> Occurs when group change. </summary>
        private EventHandler<FillOrStroke> Group;

        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            get => (FillOrStroke)base.GetValue(FillOrStrokeProperty);
            set => base.SetValue(FillOrStrokeProperty, value);
        }
        /// <summary> Identifies the <see cref = "FillOrStrokeComboBox.FillOrStroke" /> dependency property. </summary>
        public static readonly DependencyProperty FillOrStrokeProperty = DependencyProperty.Register(nameof(FillOrStroke), typeof(FillOrStroke), typeof(FillOrStrokeComboBox), new PropertyMetadata(FillOrStroke.Fill, (sender, e) =>
        {
            FillOrStrokeComboBox control = (FillOrStrokeComboBox)sender;

            if (e.NewValue is FillOrStroke value)
            {
                control.Group?.Invoke(control, value);
            }
        }));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a FillOrStrokeComboBox. 
        /// </summary>
        public FillOrStrokeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Click += (s, e) => this.Flyout.ShowAt(this);
        }

    }

    /// <summary>
    /// Represents the combo box that is used to select fill or stroke.
    /// </summary>
    public sealed partial class FillOrStrokeComboBox : UserControl
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
                        FillOrStroke fillOrStroke = key == "Fill" ? FillOrStroke.Fill : FillOrStroke.Stroke;
                        string title = resource.GetString($"Tools_{key}");


                        //Button
                        this.ConstructButton(button, key, fillOrStroke, title);


                        //Group
                        group(this.FillOrStroke);
                        this.Group += (s, groupMode) => group(groupMode);

                        void group(FillOrStroke groupMode)
                        {
                            if (groupMode == fillOrStroke)
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

        private void ConstructButton(Button button, string key, FillOrStroke fillOrStroke, string title)
        {
            /*             
                 <Button x:Name="Fill" Style="{StaticResource AppIconSelectedButton}">
                    <Button.Resources>
                        <ResourceDictionary Source="ms-appx:///Retouch Photo2.Brushs\FillOrStrokes\FillOrStrokeIcons\FillIcon.xaml"/
                    </Button.Resources>
                    <Button.Tag>
                        <ContentControl Template="{StaticResource FillIcon}"/>
                    </Button.Tag>
                </Button>
           */
            button.Content = title;
            button.Resources = new ResourceDictionary
            {
                //@Template
                Source = new Uri($@"ms-appx:///Retouch Photo2.Brushs\FillOrStrokes\FillOrStrokeIcons\{key}Icon.xaml")
            };
            button.Tag = new ContentControl
            {
                //@Template
                Template = button.Resources[$"{key}Icon"] as ControlTemplate
            };
            button.Click += (s, e) => this.FillOrStrokeChanged?.Invoke(this, fillOrStroke);//Delegate
        }

    }
}