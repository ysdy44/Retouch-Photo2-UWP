// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Represents a tools control, that containing some buttons.
    /// </summary>
    public sealed partial class ToolsControl : UserControl
    {

        //@Delegate
        /// <summary> Occurs when type change. </summary>
        public EventHandler<ToolType> TypeChanged;

        //@Group
        /// <summary> Occurs when group change. </summary>
        private EventHandler<ToolType> Group;


        #region DependencyProperty


        /// <summary> Gets or sets the assembly type. </summary>
        public Type AssemblyType { get; set; }


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

                control.Tool = Retouch_Photo2.Tools.XML.CreateTool(control.AssemblyType, value);
            }
        }));


        /// <summary> Gets or sets the tool. </summary>
        public ITool Tool
        {
            get => (ITool)base.GetValue(ToolProperty);
            set => base.SetValue(ToolProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolsControl.Tool" /> dependency property. </summary>
        public static readonly DependencyProperty ToolProperty = DependencyProperty.Register(nameof(Tool), typeof(ITool), typeof(ToolsControl), new PropertyMetadata(new NoneTool(), (sender, e) =>
        {
            ToolsControl control = (ToolsControl)sender;

            //The current tool becomes the active tool.
            if (e.OldValue is ITool oldTool)
            {
                oldTool.OnNavigatedFrom();
            }

            //The current tool does not become an active tool.
            if (e.NewValue is ITool newTool)
            {
                newTool.OnNavigatedTo();
            }
        }));


        /// <summary> Gets or sets the tool-type. </summary>
        public bool IsOpenCore
        {
            get => (bool)base.GetValue(IsOpenCoreProperty);
            set => base.SetValue(IsOpenCoreProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolsControl.IsOpenCore" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenCoreProperty = DependencyProperty.Register(nameof(IsOpenCore), typeof(bool), typeof(ToolsControl), new PropertyMetadata(false));


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
                        {
                            this.IsOpenCore = value;
                            this.Tool.IsOpen = value;
                        }
                        break;
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
            this.ConstructStrings();
            this.ConstructGroup();

            this.More.Tapped += (s, e) => this.MoreFlyout.ShowAt(this.More);
        }
    }

    public sealed partial class ToolsControl : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            foreach (UIElement child in this.StackPanel.Children)
            {
                if (child is ListViewItem item)
                {
                    if (ToolTipService.GetToolTip(item) is ToolTip toolTip)
                    {
                        string key = item.Name;
                        string title = resource.GetString($"Tools_{key}");

                        toolTip.Content = title;
                    }
                }
            }

            {
                if (ToolTipService.GetToolTip(this.More) is ToolTip toolTip)
                {
                    string key = this.More.Name;
                    string title = resource.GetString($"Tools_{key}");

                    toolTip.Content = title;
                }
            }

            foreach (UIElement child in this.MoreStackPanel.Children)
            {
                if (child is Border border)
                {
                    if (border.Child is TextBlock textBlock)
                    {
                        string key = textBlock.Name;
                        string title = resource.GetString($"Tools_{key}");

                        textBlock.Text = title;
                    }
                }

                if (child is ListViewItem item)
                {
                    if (item.Content is ContentControl control)
                    {
                        string key = item.Name;
                        string title = resource.GetString($"Tools_{key}");

                        control.Content = title;
                    }
                }
            }
        }


        //@Group
        private void ConstructGroup()
        {
            foreach (UIElement child in this.StackPanel.Children)
            {
                this.ConstructGroupCore(child);
            }

            foreach (UIElement child in this.MoreStackPanel.Children)
            {
                this.ConstructGroupCore(child);
            }
        }
        private void ConstructGroupCore(UIElement child)
        {
            if (child is ListViewItem item)
            {
                if (item == this.More) return;

                string key = item.Name;
                ToolType type = ToolType.None;
                try
                {
                    type = (ToolType)Enum.Parse(typeof(ToolType), key);
                }
                catch (Exception) { }


                //Button
                item.Tapped += (s, e) => this.Type = type;


                //Group
                group(this.Type);
                this.Group += (s, groupType) => group(groupType);

                void group(ToolType groupType)
                {
                    if (groupType == type)
                    {
                        item.IsSelected = true;
                    }
                    else item.IsSelected = false;
                }
            }
        }
    }
}