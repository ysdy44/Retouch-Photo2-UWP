using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Represents a tools control, that containing some <see cref="ToolButton"/>。
    /// </summary>
    public sealed partial class ToolsControl : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        /// <summary> Left panel of Tool. </summary>
        public UIElementCollection ToolLeft => this.StackPanel.Children;
        /// <summary> Left more button's flyout panel's children. </summary>
        public UIElementCollection ToolLeftMore = null;


        /// <summary>
        /// Gets or sets the tools.
        /// </summary>
        public IList<ITool> Tools
        {
            private get => this.tools;
            set
            {
                foreach (ITool tool in value)
                {
                    if (tool == null)
                    {
                        Rectangle rectangle = new Rectangle
                        {
                            Style = this.SeparatorRectangle
                        };

                        if (this.ToolLeftMore == null)
                            this.ToolLeft.Add(rectangle);
                        else
                            this.ToolLeftMore.Add(rectangle);
                    }
                    else if (tool.Type == ToolType.None)
                    {
                    }
                    else if (tool.Type == ToolType.More && tool.Button.Self is ToolMoreButton moreButton)
                    {
                        this.ToolLeft.Add(moreButton);
                        this.ToolLeftMore = moreButton.Children;
                    }
                    else if (tool.Button.Self is FrameworkElement element)
                    {
                        if (this.ToolLeftMore == null)
                            this.ToolLeft.Add(element);
                        else
                            this.ToolLeftMore.Add(element);

                        element.Tapped += (s, e) =>
                        {
                            //Change tools group value.
                            ToolBase.Instance = tool;
                            this.SelectionViewModel.ToolType = tool.Type;

                            this.ViewModel.TipTextBegin(tool.Button.Title);
                            this.ViewModel.Invalidate();//Invalidate
                        };
                    }
                }

                // Select the first Tool by default. 
                {
                    ITool tool = value.FirstOrDefault();
                    if (tool != null)
                    {
                        ToolBase.Instance = tool;
                    }
                }

                this.tools = value;
            }
        }
        private IList<ITool> tools = null;


        //@Construct
        /// <summary>
        /// Initializes a ToolsControl. 
        /// </summary>
        public ToolsControl()
        {
            this.InitializeComponent();
        }

    }
}