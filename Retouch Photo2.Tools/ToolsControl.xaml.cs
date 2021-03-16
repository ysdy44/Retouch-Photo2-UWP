// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2
{
    /// <summary>
    /// Represents a tools control, that containing some buttons.
    /// </summary>
    public sealed partial class ToolsControl : UserControl
    {

        /// <summary> Children. </summary>
        public UIElementCollection Children => this.StackPanel.Children;
        /// <summary> More button's flyout panel's children. </summary>
        public UIElementCollection MoreChildren => this.MoreStackPanel.Children;



        //@Delegate
        /// <summary> Occurs when type change. </summary>
        //public EventHandler<ToolType> TypeChanged;

        //@Group
        /// <summary> Occurs when group change. </summary>
        private EventHandler<ToolType> Group;


        #region DependencyProperty


        /// <summary> Gets or sets the tool-type. </summary>
        public ToolType Type
        {
            get => (ToolType)base.GetValue(TypeProperty);
            set => base.SetValue(TypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolsControl.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(ToolType), typeof(ToolsControl), new PropertyMetadata(ToolType.Node, (sender, e) =>
        {
            ToolsControl control = (ToolsControl)sender;

            if (e.NewValue is ToolType value)
            {
                control.Group?.Invoke(control, value);//Delegate
            }
        }));


        /// <summary> Gets or sets the title. </summary>
        public object Title
        {
            get => (object)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolsControl.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(object), typeof(ToolsControl), new PropertyMetadata(null));


        #endregion


        //@Delegate
        /// <summary> Occurs when IsOpen change. </summary>
        private EventHandler<bool> IsOpenChanged;


        #region DependencyProperty


        /// <summary>
        /// Gets or sets the tools.
        /// </summary>
        public IList<ITool> Tools
        {
            private get => this.tools;
            set
            {
                this.ConstructStrings(value);

                this.tools = value;
            }
        }
        private IList<ITool> tools = null;


        /// <summary>
        /// Gets or sets the IsOpen.
        /// </summary>
        public bool IsOpen
        {
            set
            {
                if (value)
                {
                    switch (this.DeviceLayoutType)
                    {
                        case DeviceLayoutType.PC:
                        case DeviceLayoutType.Pad:
                            this.IsOpenChanged(this, value);
                            break;
                    }
                }
                else
                {
                    this.IsOpenChanged(this, value);
                }
            }
        }

        /// <summary> Gets or sets the device layout type. </summary>
        public DeviceLayoutType DeviceLayoutType { get; set; }


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ToolsControl. 
        /// </summary>
        public ToolsControl()
        {
            this.InitializeComponent();

            // Select the first Tool by default. 
            this.Loaded += (s, e) => this.Type = ToolType.Cursor;
        }



        //Strings
        private void ConstructStrings(IList<ITool> tools)
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();


            ToolGroupType toolGroupType = ToolGroupType.None;
            this.Children.Clear();
            this.MoreChildren.Clear();


            foreach (var tool in tools)
            {
                if (tool == null)
                {
                    switch (toolGroupType)
                    {
                        case ToolGroupType.Tool:
                            this.Children.Add(new Rectangle
                            {
                                Style = this.SeparatorRectangle
                            });
                            continue;
                        case ToolGroupType.Pattern:
                        case ToolGroupType.Geometry:
                            this.MoreChildren.Add(new Rectangle
                            {
                                Style = this.SeparatorRectangle
                            });
                            continue;
                    }
                }


                ToolType type = tool.Type;
                string title = resource.GetString($"Tools_{type}");



                Button button = new Button();

                //Button
                {
                    toolGroupType = tool.GroupType;
                    switch (toolGroupType)
                    {
                        case ToolGroupType.Tool:
                            tool.Icon = this.ConstructButton(button, type, title);
                            break;
                        case ToolGroupType.Pattern:
                            tool.Icon = this.ConstructButtonPattern(button, type, title);
                            break;
                        case ToolGroupType.Geometry:
                            tool.Icon = this.ConstructButtonGeometry(button, type, title);
                            break;
                    }
                    tool.Title = title;
                }

                //Group
                group(this.Type);
                this.Group += (s, groupType) => group(groupType);

                void group(ToolType groupType)
                {
                    if (groupType == type)
                    {
                        button.IsEnabled = false;

                        this.Title = title;
                    }
                    else button.IsEnabled = true;
                }
            }


            //More
            Button moreButton = new Button
            {
                Style = this.SelectedButtonStyle,
                Flyout = this.MoreFlyout,
                Content = new SymbolIcon(Symbol.More)
            };
            string moreTitle = resource.GetString("Tools_More");
            this.ConstructToolTip(moreButton, moreTitle);
            this.Children.Add(moreButton);
        }

    }
    public sealed partial class ToolsControl : UserControl
    {

        /*                
         <Button x:Name="View" Style="{StaticResource AppIconSelectedButton}">
             <Button.Resources>
                 <ResourceDictionary Source="ms-appx:///Retouch Photo2.Tools\Icons\ViewIcon.xaml"/>
             </Button.Resources>
             <Button.Tag>
                 <ContentControl Template="{StaticResource ViewIcon}"/>
             </Button.Tag>
         </Button> 
       */

        private void ConstructToolTip(Button button, string title)
        {
            ToolTip toolTip = new ToolTip
            {
                Content = title,
                Placement = PlacementMode.Right,
                Style = this.ToolTipStyle
            };
            ToolTipService.SetToolTip(button, toolTip);
            this.IsOpenChanged += (s, isOpen) => toolTip.IsOpen = isOpen;//Delegate
        }

        private ControlTemplate ConstructButton(Button button, ToolType type, string title)
        {
            this.ConstructToolTip(button, title);

            button.Style = this.SelectedButtonStyle;
            button.Resources = new ResourceDictionary
            {
                //@Template
                Source = new Uri($@"ms-appx:///Retouch Photo2.Tools\Icons\{type}Icon.xaml")
            };
            button.Content = new ContentControl
            {
                //@Template 
                Template = button.Resources[$"{type}Icon"] as ControlTemplate
            };
            //button.Click += (s, e) => this.TypeChanged?.Invoke(this, type);//Delegate
            button.Click += (s, e) => this.Type = type;

            this.Children.Add(button);

            //@Template 
            return button.Resources[$"{type}Icon"] as ControlTemplate;
        }

        private ControlTemplate ConstructButtonPattern(Button button, ToolType type, string title)
        {
            button.Content = title;
            button.Style = this.IconSelectedButtonStyle;
            button.Resources = new ResourceDictionary
            {
                //@Template
                Source = new Uri($@"ms-appx:///Retouch Photo2.Tools\IconPatterns\{type}Icon.xaml")
            };

            button.Tag = new ContentControl
            {
                //@Template
                Template = button.Resources[$"{type}Icon"] as ControlTemplate
            };
            //button.Click += (s, e) => this.TypeChanged?.Invoke(this, type);//Delegate
            button.Click += (s, e) => this.Type = type;

            this.MoreChildren.Add(button);

            //@Template
            return button.Resources[$"{type}Icon"] as ControlTemplate;
        }

        private ControlTemplate ConstructButtonGeometry(Button button, ToolType type, string title)
        {
            button.Content = title;
            button.Style = this.IconSelectedButtonStyle;
            button.Resources = new ResourceDictionary
            {
                //@Template
                Source = new Uri($@"ms-appx:///Retouch Photo2.Tools\IconGeometrys\{type}Icon.xaml")
            };

            button.Tag = new ContentControl
            {
                //@Template
                Template = button.Resources[$"{type}Icon"] as ControlTemplate
            };
            //button.Click += (s, e) => this.TypeChanged?.Invoke(this, type);//Delegate
            button.Click += (s, e) => this.Type = type;

            this.MoreChildren.Add(button);

            //@Template
            return button.Resources[$"{type}Icon"] as ControlTemplate;
        }

    }
}