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
    /// <see cref="GeometryTool"/>'s TextFrameTool.
    /// </summary>
    public partial class TextFrameTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content 
        public ToolType Type => ToolType.TextFrame;
        public ToolGroupType GroupType => ToolGroupType.Tool;
        public string Title => this.TextPage.Title;
        public ControlTemplate Icon => this.TextPage.Icon;
        public FrameworkElement Page => this.TextPage;
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.TextPage.IsOpen; set => this.TextPage.IsOpen = value; }
        readonly TextPage TextPage = new TextPage(ToolType.TextFrame);


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new TextFrameLayer
            {
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandTextStyle,
            };
        }

    }
}