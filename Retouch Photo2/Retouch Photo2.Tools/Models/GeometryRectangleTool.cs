﻿// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="GeometryTool"/>'s GeometryRectangleTool.
    /// </summary>
    public partial class GeometryRectangleTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content 
        public ToolType Type => ToolType.GeometryRectangle;
        public ToolGroupType GroupType => ToolGroupType.Tool;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page { get; } = new GeometryPage(ToolType.GeometryRectangle);
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryRectangleLayer
            {
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

    }
}