// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="GeometryTool"/>'s GeometryCapsuleTool.
    /// </summary>
    public partial class GeometryCapsuleTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryCapsule;
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title => this.GeometryCapsulePage.Title;
        public ControlTemplate Icon => this.GeometryCapsulePage.Icon;
        public FrameworkElement Page => this.GeometryCapsulePage;
        readonly GeometryCapsulePage GeometryCapsulePage = new GeometryCapsulePage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.GeometryCapsulePage.IsOpen; set => this.GeometryCapsulePage.IsOpen = value; }


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryCapsuleLayer
            {
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryTool"/>.
    /// </summary>
    internal partial class GeometryCapsulePage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryCapsulePage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryCapsulePage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryCapsulePage), new PropertyMetadata(false));


        #endregion


        //@Content 
        public string Title { get; private set; }
        public ControlTemplate Icon => this.IconContentControl.Template;


        //@Construct
        /// <summary>
        /// Initializes a GeometryCapsulePage. 
        /// </summary>
        public GeometryCapsulePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConvertToCurvesButton.Click += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                this.MethodViewModel.MethodConvertToCurves();

                //Change tools group value.
                this.TipViewModel.ToolType = ToolType.Node;
            };

            this.MoreCreateButton.Click += (s, e) => Retouch_Photo2.DrawPage.ShowMoreCreate?.Invoke(this, this.MoreCreateButton);
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Title = resource.GetString("Tools_GeometryCapsule");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_MoreCreate");
        }

    }
}