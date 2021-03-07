using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        /// <summary> Children. </summary>
        public UIElementCollection Children => this.StackPanel.Children;
        /// <summary> More button's flyout panel's children. </summary>
        public UIElementCollection MoreChildren => this.MoreStackPanel.Children;


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
                switch (this.DeviceLayoutType)
                {
                    case DeviceLayoutType.PC:
                    case DeviceLayoutType.Pad:
                        foreach (ToolTip toolTip in this.ToolTips)
                        {
                            toolTip.IsOpen = value;
                        }
                        break;
                }
            }
        }


        /// <summary> Gets or sets the device layout type. </summary>
        public DeviceLayoutType DeviceLayoutType { get; set; }


        private readonly IList<ToolTip> ToolTips = new List<ToolTip>();


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ToolsControl. 
        /// </summary>
        public ToolsControl()
        {
            this.InitializeComponent();
        }



        //Strings
        private void ConstructStrings(IList<ITool> tools)
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();


            ToolGroupType groupType = ToolGroupType.None;
            this.Children.Clear();
            this.MoreChildren.Clear();


            foreach (ITool tool in tools)
            {
                if (tool == null)
                {
                    switch (groupType)
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


                ToolType key = tool.Type;
                string text = resource.GetString($"Tools_{key}");
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

                groupType = tool.GroupType;
                switch (groupType)
                {
                    case ToolGroupType.Tool:
                        this.ConstructTool(tool, key, text);
                        break;
                    case ToolGroupType.Pattern:
                        this.ConstructPattern(tool, key, text);
                        break;
                    case ToolGroupType.Geometry:
                        this.ConstructGeometry(tool, key, text);
                        break;
                }
            }


            //More
            this.Children.Add(new Button
            {
                Style = this.SelectedButtonStyle,
                Flyout = this.MoreFlyout,
                Content = new SymbolIcon(Symbol.More)
            });


            // Select the first Tool by default. 
            {
                ITool tool = tools.FirstOrDefault();
                if (tool != null)
                {
                    ToolManager.Instance = tool;
                }
            }
        }

    }
    public sealed partial class ToolsControl : UserControl
    {

        private void ConstructTool(ITool tool, ToolType key, string text)
        {
            Button button = new Button
            {
                Style = this.SelectedButtonStyle,
                Resources = new ResourceDictionary
                {
                    //@Template
                    Source = new Uri($@"ms-appx:///Retouch Photo2.Tools\Icons\{key}Icon.xaml")
                }
            };

            ToolTip toolTip = new ToolTip
            {
                Content = text,
                Placement = PlacementMode.Right,
                Style = this.ToolTipStyle
            };
            ToolTipService.SetToolTip(button, toolTip);
            this.ToolTips.Add(toolTip);

            button.Content = new ContentControl
            {
                Template = button.Resources[$"{key}Icon"] as ControlTemplate
            };
            button.Click += (s, e) => this.ItemClick(tool);
            this.Children.Add(button);

            //@Template 
            tool.Icon = button.Resources[$"{key}Icon"] as ControlTemplate;
            tool.Title = text;
        }


        private void ConstructPattern(ITool tool, ToolType key, string text)
        {
            Button button = new Button
            {
                Content = text,
                Style = this.IconSelectedButtonStyle,
                Resources = new ResourceDictionary
                {
                    //@Template
                    Source = new Uri($@"ms-appx:///Retouch Photo2.Tools\IconPatterns\{key}Icon.xaml")
                }
            };
            button.Tag = new ContentControl
            {
                //@Template
                Template = button.Resources[$"{key}Icon"] as ControlTemplate
            };
            button.Click += (s, e) => this.ItemClick(tool);
            this.MoreChildren.Add(button);

            //@Template
            tool.Icon = button.Resources[$"{key}Icon"] as ControlTemplate;
            tool.Title = text;
        }

        private void ConstructGeometry(ITool tool, ToolType key, string text)
        {
            Button button = new Button
            {
                Content = text,
                Style = this.IconSelectedButtonStyle,
                Resources = new ResourceDictionary
                {
                    //@Template
                    Source = new Uri($@"ms-appx:///Retouch Photo2.Tools\IconGeometrys\{key}Icon.xaml")
                }
            };
            button.Tag = new ContentControl
            {
                //@Template
                Template = button.Resources[$"{key}Icon"] as ControlTemplate
            };
            button.Click += (s, e) => this.ItemClick(tool);
            this.MoreChildren.Add(button);

            //@Template
            tool.Icon = button.Resources[$"{key}Icon"] as ControlTemplate;
            tool.Title = text;
        }



        private void ItemClick(ITool tool)
        {
            //Change tools group value.
            ToolManager.Instance = tool;
            this.SelectionViewModel.ToolType = tool.Type;

            this.ViewModel.TipTextBegin(tool.Title);
            this.ViewModel.Invalidate();//Invalidate
        }

    }
}