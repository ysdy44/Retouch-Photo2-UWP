using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using System;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Retouch_Photo2's the only <see cref = "ColorMenu" />. 
    /// </summary>
    public sealed partial class ColorMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Construct
        public ColorMenu()
        {
            this.InitializeComponent();
            this._button.CenterContent = new ColorEllipse
             (
                  dataContext: this.SelectionViewModel,
                  path: nameof(this.SelectionViewModel.Color),
                  dp: ColorEllipse.ColorProperty
             );
            this.ConstructStrings();
            this.ConstructMenu();

            this.ConstructColor1();
            this.ConstructColor2();
        }



        private void ConstructColor1()
        {
            this.ColorPicker.ColorChanged += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        {
                            //History
                            IHistoryBase history = new IHistoryBase("Set fill");

                            //Selection
                            this.SelectionViewModel.Color = value;
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                //History
                                var previous = layer.Style.Fill;
                                int index = layer.Control.Index;
                                history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                                Style.Fill = previous.Clone());

                                layer.Style.Fill = BrushBase.ColorBrush(value);
                            });

                            //History
                            this.ViewModel.Push(history);

                            this.ViewModel.Invalidate();//Invalidate
                        }
                        break;

                    case FillOrStroke.Stroke:
                        {
                            //History
                            IHistoryBase history = new IHistoryBase("Set stroke");

                            //Selection
                            this.SelectionViewModel.Color = value;
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                //History
                                var previous = layer.Style.Stroke;
                                int index = layer.Control.Index;
                                history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                                Style.Stroke = previous.Clone());

                                layer.Style.Stroke = BrushBase.ColorBrush(value);
                            });

                            //History
                            this.ViewModel.Push(history);

                            this.ViewModel.Invalidate();//Invalidate
                        }
                        break;
                }
            };
        }


        private void ConstructColor2()
        {
            //History
            IHistoryBase history = null;


            //Color
            this.ColorPicker.ColorChangeStarted += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        {
                            history = new IHistoryBase("Set fill");

                            //Selection
                            this.SelectionViewModel.Color = value;
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.Style.CacheFill();
                            });

                            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                        }
                        break;
                    case FillOrStroke.Stroke:
                        {
                            history = new IHistoryBase("Set stroke");

                            //Selection
                            this.SelectionViewModel.Color = value;
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.Style.CacheStroke();
                            });

                            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                        }
                            break;
                }
            };
            this.ColorPicker.ColorChangeDelta += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        {
                            //Selection
                            this.SelectionViewModel.Color = value;
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.Style.Fill = BrushBase.ColorBrush(value);
                            });

                            this.ViewModel.Invalidate();//Invalidate
                        }
                            break;
                    case FillOrStroke.Stroke:
                        {
                            //Selection
                            this.SelectionViewModel.Color = value;
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.Style.Stroke = BrushBase.ColorBrush(value);
                            });

                            this.ViewModel.Invalidate();//Invalidate
                        }
                        break;
                }
            };
            this.ColorPicker.ColorChangeCompleted += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        {
                            //Selection
                            this.SelectionViewModel.Color = value;
                            this.SelectionViewModel.Fill = BrushBase.ColorBrush(value);
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                //History
                                var previous = layer.Style.StartingFill.Clone();
                                int index = layer.Control.Index;
                                history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                                Style.Fill = previous.Clone());

                                layer.Style.Fill = BrushBase.ColorBrush(value);
                            });

                            //History
                            this.ViewModel.Push(history);

                            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
                        }
                            break;
                    case FillOrStroke.Stroke:
                        {
                            //Selection
                            this.SelectionViewModel.Color = value;
                            this.SelectionViewModel.Stroke = BrushBase.ColorBrush(value);
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                //History
                                var previous = layer.Style.StartingStroke.Clone();
                                int index = layer.Control.Index;
                                history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                                Style.Stroke = previous.Clone());

                                layer.Style.Stroke = BrushBase.ColorBrush(value);
                            });

                            //History
                            this.ViewModel.Push(history);

                            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
                        }
                        break;
                }
            };
        }


    }
        
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TransformerMenu" />. 
    /// </summary>
    public sealed partial class ColorMenu : UserControl, IMenu
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = 
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/Color");
        }

        //Menu
        public MenuType Type => MenuType.Transformer;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton();

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }
}
