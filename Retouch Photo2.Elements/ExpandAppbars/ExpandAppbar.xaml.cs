using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Expand children if children-width is more than size-width.
    /// </summary>
    public sealed partial class ExpandAppbar : UserControl
    {
        //@Content
        /// <summary> Gets and sets children elements. </summary>
        public List<IExpandAppbarElement> Children { get; set; } = new List<IExpandAppbarElement>();

        #region Mode

        int Index;
        HorizontalAlignment Mode
        {
            set
            {
                this.StackPanel.Children.Clear();
                this.SecondStackPanel.Children.Clear();

                switch (value)
                {
                    case HorizontalAlignment.Left:
                        {
                            foreach (UIElement element in this.Children)
                            {
                                this.StackPanel.Children.Add(element);
                            }
                            this.MoreButton.Visibility = Visibility.Collapsed;
                        }
                        break;
                    case HorizontalAlignment.Center:
                        {
                            {
                                for (int i = 0; i < this.Children.Count; i++)
                                {
                                    UIElement element = this.Children[i].Self;

                                    if (i < this.Index) this.StackPanel.Children.Add(element);
                                    else this.SecondStackPanel.Children.Add(element);
                                }
                                this.MoreButton.Visibility = Visibility.Visible;
                            }
                        }
                        break;
                    case HorizontalAlignment.Right:
                        {
                            foreach (UIElement element in this.Children)
                            {
                                this.SecondStackPanel.Children.Add(element);
                            }
                            this.MoreButton.Visibility = Visibility.Visible;
                        }
                        break;
                }
            }
        }

        #endregion

        //@Construct
        public ExpandAppbar()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) =>
            {
                if (this.Children != null)
                {
                    //Width
                    double width = this.ActualWidth - 50;
                    int index = ExpandAppbar.Measure(width, this.Children);

                    //Index
                    this.Index = index;
                    this.Mode = ExpandAppbar.Arrange(index, this.Children.Count);
                }
            };
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                //Width
                double width = e.NewSize.Width - 70;
                int index = ExpandAppbar.Measure(width, this.Children);

                //Index
                if (this.Index != index)
                {
                    this.Index = index;
                    this.Mode = ExpandAppbar.Arrange(index, this.Children.Count);
                }
            };
            this.MoreButton.Tapped += (s, e) => this.Flyout.ShowAt(this.MoreButton);
        }


        //@Static
        private static int Measure(double sizeWidth, IList<IExpandAppbarElement> children)
        {
            double addWidth = 0;

            for (int i = 0; i < children.Count; i++)
            {
                double width = children[i].ExpandWidth;
                addWidth += width;

                if (addWidth > sizeWidth)
                {
                    return i;
                }
            }

            return children.Count;
        }
        private static HorizontalAlignment Arrange(int index, int count)
        {
            if (index <= 0)
            {
                return HorizontalAlignment.Right;
            }
            else if (index > count - 3)
            {
                return HorizontalAlignment.Left;
            }
            else
            {
                return HorizontalAlignment.Center;
            }
        }
    }
}