using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.TestApp.ViewModels;
using Retouch_Photo2.Transformers;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public sealed partial class LayerControl : UserControl
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;
        SelectionViewModel Selection => Retouch_Photo2.TestApp.App.Selection;
        MezzanineViewModel Mezzanine => Retouch_Photo2.TestApp.App.Mezzanine;
        

        //@Converter
        private double OpacityToValueConverter(float opacity) => opacity * 100.0d;
        private float ValueToOpacityConverter(double value) => (float)value / 100.0f;

        private double VisibilityToOpacityConverter(Visibility visibility) => (visibility == Visibility.Visible) ? 1.0 : 0.4;
        private bool GroupLayerToBoolConverter(GroupLayer groupLayer) => (groupLayer == null) ? false : true;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "LayerControl" />'s selection mode. </summary>
        public ListViewSelectionMode Mode
        {
            get { return (ListViewSelectionMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "LayerControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Transformer), typeof(ListViewSelectionMode), typeof(LayerControl), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            LayerControl con = (LayerControl)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
                switch (value)
                {
                    case ListViewSelectionMode.None:
                        {
                            con.OpacitySlider.IsEnabled = false;
                            con.BlendControl.IsEnabled = false;
                            con.VisualButton.IsEnabled = false;
                            con.DuplicateButton.IsEnabled = false;
                            con.RemoveButton.IsEnabled = false;
                        }
                        break;
                    case ListViewSelectionMode.Single:
                    case ListViewSelectionMode.Multiple:
                        {
                            con.OpacitySlider.IsEnabled = true;
                            con.BlendControl.IsEnabled = true;
                            con.VisualButton.IsEnabled = true;
                            con.DuplicateButton.IsEnabled = true;
                            con.RemoveButton.IsEnabled = true;
                        }
                        break;
                }

                switch (value)
                {
                    case ListViewSelectionMode.None:
                    case ListViewSelectionMode.Single:
                        {
                            con.GroupButton.IsEnabled = false;
                        }
                        break;

                    case ListViewSelectionMode.Multiple:
                        {
                            con.GroupButton.IsEnabled = true;
                        }
                        break;

                }

                switch (value)
                {
                    case ListViewSelectionMode.Single:
                        {
                            con.ChildrenTextBlock.Visibility = Visibility.Visible;
                            con.ChildrenBorder.Visibility = Visibility.Visible;
                        }
                        break;
                    case ListViewSelectionMode.None:
                    case ListViewSelectionMode.Multiple:
                        {
                            con.ChildrenTextBlock.Visibility = Visibility.Collapsed;
                            con.ChildrenBorder.Visibility = Visibility.Collapsed;
                        }
                        break;
                }
            }
        }));


        #endregion


        //@Construct
        public LayerControl()
        {
            this.InitializeComponent();
                       

            //Layer
            this.OpacitySlider.ValueChanged += (s, e) =>
            {
                float opacity = this.ValueToOpacityConverter(e.NewValue);

                //Selection
                this.Selection.Opacity = opacity;
                this.Selection.SetValue((layer) =>
                {
                    layer.Opacity = opacity;
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.BlendControl.TypeChanged += (type) =>
            {
                //Selection
                this.Selection.BlendType = type;
                this.Selection.SetValue((layer) =>
                {
                    layer.BlendType = type;
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            
            //Layer
            this.VisualButton.Tapped += (s, e) =>
            {
                Visibility value = (this.Selection.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                //Selection
                this.Selection.Visibility = value;
                this.Selection.SetValue((layer) =>
                {
                    layer.Visibility = value;
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.DuplicateButton.Tapped += (s, e) =>
            {
                int index = this.Mezzanine.GetfFrstIndex(this.ViewModel.Layers);

                this.Selection.SetValue((layer) =>
                {
                    var cloneLayer = layer.Clone(this.ViewModel.CanvasDevice);//Clone

                    //IsChecked
                    cloneLayer.IsChecked = true;
                    layer.IsChecked = false;

                    this.ViewModel.Layers.Insert(index, cloneLayer);//Insert
                });

                this.Selection.SetModeNone();//Selection
                this.ViewModel.Invalidate();//Invalidate
            };

            this.RemoveButton.Tapped += (s, e) =>
            {
                switch (this.Selection.Mode)
                {
                    case ListViewSelectionMode.None:
                        break;
                    case ListViewSelectionMode.Single:
                        {
                            this.ViewModel.Layers.Remove(this.Selection.Layer);

                            this.Selection.SetModeNone();//Selection
                            this.ViewModel.Invalidate();//Invalidate
                        }
                        break;
                    case ListViewSelectionMode.Multiple:
                        {
                            foreach (Layer layer in this.Selection.Layers)
                            {
                                this.ViewModel.Layers.Remove(this.Selection.Layer);
                            }

                            this.Selection.SetModeNone();//Selection
                            this.ViewModel.Invalidate();//Invalidate
                        }
                        break;
                }
            };


            //Group
            this.GroupButton.Tapped += (s, e) =>
            {
                if (this.ViewModel.LayerMenuLayoutState == Elements.MenuLayoutState.FlyoutShow)
                {
                    this.ViewModel.LayerMenuLayoutState = Elements.MenuLayoutState.FlyoutHide;
                }


                //TransformerMatrix
                TransformerMatrix transformerMatrix = new TransformerMatrix(this.Selection.Transformer);
                //GroupLayer
                GroupLayer groupLayer = new GroupLayer
                {
                    IsChecked = true,
                    TransformerMatrix = transformerMatrix
                };


                //Index
                int index = this.Mezzanine.GetfFrstIndex(this.ViewModel.Layers);
                //Selection
                this.Selection.SetValue((layer) =>
                {
                    groupLayer.Children.Add(layer);//Add
                });


                foreach (Layer layer in groupLayer.Children)
                {
                    this.ViewModel.Layers.Remove(layer);//Remove
                }

                this.Selection.SetModeSingle(groupLayer);//Selection
                this.ViewModel.Layers.Insert(index, groupLayer);//Insert
                this.ViewModel.Invalidate();//Invalidate
            };

            this.UnGroupButton.Tapped += (s, e) =>
            {
                if (this.Selection.IsGroupLayer == false) return;

                if (this.Selection.Layer is GroupLayer groupLayer)
                {
                    //Index
                    int index = this.Mezzanine.GetfFrstIndex(this.ViewModel.Layers);

                    //Selection
                    this.Selection.SetValue((layer) =>
                    {
                        layer.IsChecked = false;
                    });

                    //Insert
                    this.ViewModel.Layers.Remove(groupLayer);
                    foreach (Layer layer in groupLayer.Children)
                    {
                        this.ViewModel.Layers.Insert(index, layer);//Insert
                    }

                    //SetMode
                    IEnumerable<Layer> layers = groupLayer.Children;
                    Transformer transformer = groupLayer.TransformerMatrix.Destination;
                    this.Selection.SetModeMultiple(layers, transformer);//Selection

                    this.ViewModel.Invalidate();//Invalidate
                }
            };
        }

        //@DataTemplate
        /// <summary> DataTemplate's Button Tapped. </summary>
        private void VisibilityButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            LayersControl.GetButtonDataContext(sender, out Grid rootGrid, out Layer layer);

            layer.Visibility = (layer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

            this.ViewModel.Invalidate();//Invalidate

            e.Handled = true;
        }
    }
}