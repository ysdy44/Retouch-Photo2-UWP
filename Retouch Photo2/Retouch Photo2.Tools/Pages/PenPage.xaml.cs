using FanKit.Win2Ds;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "PenTool"/>.
    /// </summary>
    public sealed partial class PenPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        List<Node> Nodes => this.SelectionViewModel.CurveLayer.Nodes;

        //@Converter
        private Visibility FalseToVisibilityConverter(bool value) => value ? Visibility.Collapsed : Visibility.Visible;
        private Visibility TrueToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;
        
        //@Construct
        public PenPage()
        {
            this.InitializeComponent();

            this.RemoveButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.CurveLayer == null) return;

                //Remove
                int removeIndex= -1;
                do
                {
                    if (removeIndex >=0)
                        if (this.Nodes[removeIndex].IsChecked)
                            this.Nodes.RemoveAt(removeIndex);

                    removeIndex = -1;
                    for (int i = 0; i < this.Nodes.Count; i++)
                        if (this.Nodes[i].IsChecked)
                            removeIndex = i;
                }
                while (removeIndex >= 0);

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AddButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.CurveLayer == null) return;

            };

            this.SharpButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.CurveLayer == null) return;

                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    if (this.Nodes[i].IsChecked)
                    {
                        if (this.Nodes[i].IsSmooth)
                        {
                            Vector2 vector = this.Nodes[i].Point;

                            Node node = new Node
                            {
                                Point = vector,
                                LeftControlPoint = vector,
                                RightControlPoint = vector,
                                IsChecked = true,
                                IsSmooth = false,
                            };
                            this.Nodes[i] = node;
                        }
                    }
                }

                this.ViewModel.Invalidate();//Invalidate
            };
            this.SmoothButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.CurveLayer == null) return;

                //First     
                {
                    int first = 0;
                    int next = 1;
                    if (this.Nodes[first].IsChecked)
                    {
                        Vector2 space = (this.Nodes[next].Point - this.Nodes[first].Point) / 3;
                        this._smoothNode(first, space);
                    }
                }
                //Last
                {
                    int last = this.Nodes.Count - 1;
                    int previous = this.Nodes.Count - 2;
                    if (this.Nodes[last].IsChecked)
                    {
                        Vector2 space = (this.Nodes[last].Point - this.Nodes[previous].Point) / 3;
                        this._smoothNode(last, space);
                    }
                }
                //Nodes
                if (this.Nodes.Count > 2)
                {
                    for (int i = 1; i < this.Nodes.Count - 1; i++)
                    {
                        if (this.Nodes[i].IsChecked)
                        {
                            Vector2 space = (this.Nodes[i + 1].Point - this.Nodes[i - 1].Point) / 6;
                            this._smoothNode(i, space);
                        }
                    }
                }

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void _smoothNode(int index, Vector2 space)
        {
            Vector2 vector = this.Nodes[index].Point;

            Node node = new Node
            {
                Point = vector,
                LeftControlPoint = vector + space,
                RightControlPoint = vector - space,
                IsChecked = true,
                IsSmooth = true,
            };
            this.Nodes[index] = node;
        }

    }
}