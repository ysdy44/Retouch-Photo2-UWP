// Core:              ★★
// Referenced:   ★★
// Difficult:         ★
// Only:              
// Complete:      ★★★
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
        public List<UIElement> Children { get; set; } = new List<UIElement>();

        IList<IExpandAppbarElement> _elements { get; set; } = new List<IExpandAppbarElement>();

        //@Static
        /// <summary> Is this Loaded? </summary>
        static bool _lockIsLoaded = false;

        #region Mode

        int Index;
        /// <summary>
        /// Left: Button show
        /// RightL button hid
        /// Center: middle
        /// </summary>
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
                            foreach (IExpandAppbarElement element in this._elements)
                            {
                                element.IsSecondPage = false;
                                this.StackPanel.Children.Add(element.Self);
                            }
                            this.MoreButton.Visibility = Visibility.Collapsed;
                        }
                        break;
                    case HorizontalAlignment.Center:
                        {
                            {
                                for (int i = 0; i < this._elements.Count; i++)
                                {
                                    IExpandAppbarElement element = this._elements[i];

                                    if (i < this.Index)
                                    {
                                        element.IsSecondPage = false;
                                        this.StackPanel.Children.Add(element.Self);
                                    }
                                    else
                                    {
                                        element.IsSecondPage = true;
                                        this.SecondStackPanel.Children.Add(element.Self);
                                    }
                                }
                                this.MoreButton.Visibility = Visibility.Visible;
                            }
                        }
                        break;
                    case HorizontalAlignment.Right:
                        {
                            foreach (IExpandAppbarElement element in this._elements)
                            {
                                element.IsSecondPage = true;
                                this.SecondStackPanel.Children.Add(element.Self);
                            }
                            this.MoreButton.Visibility = Visibility.Visible;
                        }
                        break;
                }
            }
        }

        #endregion

        //@Construct
        /// <summary>
        /// Initializes a ExpandAppbar.
        /// </summary>
        public ExpandAppbar()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) =>
            {
                if (ExpandAppbar._lockIsLoaded) return;
                ExpandAppbar._lockIsLoaded = true;
                
                foreach (UIElement element in this.Children)
                {
                    if (element is IExpandAppbarElement appbarElement)
                    {
                        this._elements.Add(appbarElement);
                    }
                }
                
                //Width
                double width = this.ActualWidth - 50;
                int index = ExpandAppbar.Measure(width, this._elements);

                //Index
                this.Index = index;
                this.Mode = ExpandAppbar.Arrange(index, this._elements.Count);
            };
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                //Width
                double width = e.NewSize.Width - 40;
                int index = ExpandAppbar.Measure(width, this._elements);

                //Index
                if (this.Index != index)
                {
                    this.Index = index;
                    this.Mode = ExpandAppbar.Arrange(index, this._elements.Count);
                }
            };
            this.MoreButton.Click += (s, e) => this.Flyout.ShowAt(this.MoreButton);
            this.SecondStackPanel.Tapped += (s, e) => this.Flyout.Hide();
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