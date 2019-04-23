using Retouch_Photo2.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Library;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;

namespace Retouch_Photo2.Tools.Pages
{
    public sealed partial class PenPage : ToolPage
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        bool IsOn
        {
            set
            {
                this.RemoveButton.IsEnabled = value;
                this.AddButton.IsEnabled = value;
                this.SharpButton.IsEnabled = value;
                this.SmoothButton.IsEnabled = value;
                this.MoreButton.IsEnabled = value;
            }
        }
        public PenPage()
        {
            this.InitializeComponent();

            //EditMode
            this.IsOn = false;
            this.Switch.IsOn = false;
            this.Switch.IsOnChanged += (isOn) =>
            {
                this.IsOn = isOn;
                NodeEditMode mode = isOn ? NodeEditMode.EditMove : NodeEditMode.Add;
                this.ViewModel.CurveNodes.EditMode = mode;
            };


            //Button
            this.RemoveButton.Tapped += (s, e) => this.Operator((curveNodes, nodes) =>
            {
                int unRemoveCount = 0;
                foreach (Node node in nodes)
                {
                    if (node.ChooseMode==NodeChooseMode.None)
                        unRemoveCount++;                    
                }

                if (unRemoveCount > 2) curveNodes.Remove(nodes);
                else
                {
                    this.ViewModel.RenderLayer.Remove(this.ViewModel.CurrentCurveLayer);
                    this.ViewModel.CurrentLayer = null;
                }
            });
            this.AddButton.Tapped += (s, e) => this.Operator((curveNodes, nodes) => curveNodes.Interpolation(nodes));
            this.SharpButton.Tapped += (s, e) => this.Operator((curveNodes, nodes) => curveNodes.Sharp(nodes));
            this.SmoothButton.Tapped += (s, e) => this.Operator((curveNodes, nodes) => curveNodes.Smooth(nodes));

            //More
            this.MoreButton.Tapped += (s, e) => this.Flyout.ShowAt(this.MoreButton);
         
            //Mirrored
            this.MirroredButton.IsChecked = this.ViewModel.CurveNodes.ControlMode == NodeControlMode.Mirrored;
            this.MirroredButton.Tapped += (s, e) => this.ViewModel.CurveNodes.ControlMode = NodeControlMode.Mirrored;
            //Disconnected
            this.DisconnectedButton.IsChecked = this.ViewModel.CurveNodes.ControlMode == NodeControlMode.Disconnected;
            this.DisconnectedButton.Tapped += (s, e) => this.ViewModel.CurveNodes.ControlMode = NodeControlMode.Disconnected;
            //Asymmetric
            this.AsymmetricButton.IsChecked = this.ViewModel.CurveNodes.ControlMode == NodeControlMode.Asymmetric;
            this.AsymmetricButton.Tapped += (s, e) => this.ViewModel.CurveNodes.ControlMode = NodeControlMode.Asymmetric;
        }

        private void Operator(Action<CurveNodes,List<Node>> action)
        {
            if (this.ViewModel.CurrentCurveLayer == null) return;
           
            action(this.ViewModel.CurveNodes,this.ViewModel.CurrentCurveLayer.Nodes);//Action

            this.ViewModel.CurrentCurveLayer.NodesGeometry = Retouch_Photo2.Models.Layers.GeometryLayers.CurveLayer.GetNodesGeometry
                (
               this.ViewModel.CanvasDevice,
               this.ViewModel.CurrentCurveLayer.Nodes
                );
            this.ViewModel.CurrentCurveLayer.ResetTransformer();
            this.ViewModel.Invalidate();
        }

        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
            this.Switch.IsOn=!(this.ViewModel.CurveNodes.EditMode== NodeEditMode.Add);
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }
    }
}
