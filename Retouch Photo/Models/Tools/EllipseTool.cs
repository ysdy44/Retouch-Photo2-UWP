﻿using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class EllipseTool : Tool
    {
        public EllipseTool()
        {
            base.Type = ToolType.Ellipse;
            base.Icon = new EllipseControl();
            base.WorkIcon = new EllipseControl();
            base.Page = new EllipsePage();
            base.ViewModel = new EllipseViewModel();
        }
    }
}