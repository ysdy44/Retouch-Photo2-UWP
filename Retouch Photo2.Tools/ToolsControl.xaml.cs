// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Represents a tools control, that containing some buttons.
    /// </summary>
    public sealed class ToolsControl : ContentControl
    {

        /// <summary>
        /// Gets or sets the tools.
        /// </summary>
        public IList<ITool> Tools
        {
            get => this.tools;
            set
            {
                this.tools = value;
                if (this.isLoaded) this.ToolsControlCore = new ToolsControlCore(value);
            }
        }
        private IList<ITool> tools;


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
                if (control.ToolsControlCore != null)
                {
                    control.ToolsControlCore.Type = value;
                }
            }
        }));


        #endregion


        /// <summary>
        /// Gets or sets the IsOpen.
        /// </summary>
        public bool IsOpen
        {
            set
            {
                if (this.ToolsControlCore != null)
                {
                    switch (this.DeviceLayoutType)
                    {
                        case DeviceLayoutType.PC:
                        case DeviceLayoutType.Pad:
                            this.IsOpen = value;
                            break;
                    }
                }
            }
        }

        /// <summary> Gets or sets the device layout type. </summary>
        public DeviceLayoutType DeviceLayoutType { get; set; }


        bool isLoaded;
        public ToolsControl()
        {
            this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            this.Loaded += (s, e) =>
            {
                this.isLoaded = true;

                if (this.Tools != null) this.ToolsControlCore = new ToolsControlCore(this.Tools);
            };
        }


        private ToolsControlCore ToolsControlCore
        {
            get => this.toolsControlCore;
            set
            {
                if (this.toolsControlCore != null)
                {
                    this.Content = null;
                    this.toolsControlCore.TypeChanged -= this.ToolsControlCore_TypeChanged;
                }

                this.toolsControlCore = value;

                if (this.toolsControlCore != null)
                {
                    this.Content = this.ToolsControlCore;
                    this.toolsControlCore.TypeChanged += this.ToolsControlCore_TypeChanged;
                }
            }
        }
        private ToolsControlCore toolsControlCore;

        private void ToolsControlCore_TypeChanged(object sender, ToolType toolType)
        {
            this.Type = toolType;
        }

    }


    internal sealed partial class ToolsControlCore : UserControl
    {

        //@Delegate
        /// <summary> Occurs when type change. </summary>
        internal EventHandler<ToolType> TypeChanged;


        //@Group
        /// <summary> Occurs when IsOpen change. </summary>
        private EventHandler<bool> IsOpenChanged;
        /// <summary> Occurs when group change. </summary>
        private EventHandler<ToolType> Group;


        //@Content
        internal bool IsOpen
        {
            set
            {
                this.IsOpenChanged?.Invoke(this, value);//Delegate
            }
        }
        internal ToolType Type
        {
            get => this.type;
            set
            {
                this.Group?.Invoke(this, value);//Delegate
                this.type = value;
            }
        }
        private ToolType type= ToolType.Node;


        /// <summary> Children. </summary>
        private UIElementCollection Children => this.StackPanel.Children;
        /// <summary> More button's flyout panel's children. </summary>
        private UIElementCollection MoreChildren => this.MoreStackPanel.Children;


        //@Construct
        /// <summary>
        /// Initializes a ToolsControlCore. 
        /// </summary>
        public ToolsControlCore(IList<ITool> tools)
        {
            this.InitializeComponent();
            this.ConstructStrings(tools);

            // Select the first Tool by default. 
            this.Loaded += (s, e) => this.TypeChanged?.Invoke(this, ToolType.Cursor); //Delegate
        }



        //Strings
        private void ConstructStrings(IList<ITool> tools)
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();


            ToolGroupType toolGroupType = ToolGroupType.None;
            this.Children.Clear();
            this.MoreChildren.Clear();


            foreach (ITool tool in tools)
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
                string title = tool.Title;


                //Button
                Button button = null;
                {
                    toolGroupType = tool.GroupType;
                    switch (toolGroupType)
                    {
                        case ToolGroupType.Tool:
                            button = new Button
                            {
                                Style = this.SelectedButtonStyle,
                                Content = new ContentControl
                                {
                                    Template = tool.Icon
                                }
                            };
                            this.ConstructToolTip(button, title);
                            this.Children.Add(button);
                            button.Click += (s, e) => this.TypeChanged?.Invoke(this, type); //Delegate 
                            break;
                        case ToolGroupType.Pattern:
                        case ToolGroupType.Geometry:
                            button = new Button
                            {
                                Content = title,
                                Style = this.IconSelectedButtonStyle,
                                Tag = new ContentControl
                                {
                                    Template = tool.Icon
                                }
                            };
                            this.MoreChildren.Add(button);
                            button.Click += (s, e) => this.TypeChanged?.Invoke(this, type); //Delegate 
                            break;
                    }
                }


                //Group
                group(this.Type);
                this.Group += (s, groupType) => group(groupType);

                void group(ToolType groupType)
                {
                    if (groupType == type)
                    {
                        button.IsEnabled = false;

                        //this.Title = title;
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

    }
}