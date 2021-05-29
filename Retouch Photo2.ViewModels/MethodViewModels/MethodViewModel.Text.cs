using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Texts;
using System;
using System.ComponentModel;
using Windows.UI.Text;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        //@Converter
        /// <summary>
        /// Convert font weight to boolean
        /// </summary>
        /// <param name="fontWeight"> The font weight. </param>
        /// <returns> Is Bold ? ""Normal"" : ""Bold"" </returns>
        public bool FontWeightConverter(FontWeight2 fontWeight)
        {
            switch (fontWeight)
            {
                case FontWeight2.Black:
                case FontWeight2.Bold:
                case FontWeight2.ExtraBlack:
                case FontWeight2.ExtraBold:
                case FontWeight2.SemiBold:
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// Convert font style to boolean
        /// </summary>
        /// <param name="fontStyle"> The font style. </param>
        /// <returns> Is Italic ? ""Normal"" : ""Italic"" </returns>
        public bool FontStyleConverter(FontStyle fontStyle) => fontStyle == FontStyle.Italic;


        /// <summary>
        /// Change T type for ITextLayer, save history, invalidate canvas.
        /// </summary>
        /// <typeparam name="T"> The T type property. </typeparam>
        /// <param name="set"> The sets of T. </param>
        /// <param name="type"> The history type. </param>
        /// <param name="getUndo"> The gets of history undo T. </param>
        /// <param name="setUndo"> The sets of history undo T. </param>
        public void ITextLayerChanged<T>(Action<ITextLayer> set, HistoryType type, Func<ITextLayer, T> getUndo, Action<ITextLayer, T> setUndo)
        {
            // History
            LayersPropertyHistory history = new LayersPropertyHistory(type);

            // Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type.IsText())
                {
                    ITextLayer textLayer = (ITextLayer)layer;

                    var previous = getUndo(textLayer);
                    history.UndoAction += () =>
                    {
                        // Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        setUndo(textLayer, previous);
                    };

                    // Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    set(textLayer);
                }
            });

            // History
            this.HistoryPush(history);

            this.Invalidate(); // Invalidate
        }


        public void MethodSetFontText(string fontText)
        {
            this.FontText = fontText;
            this.ITextLayerChanged<string>
            (
                set: (textLayer) => textLayer.FontText = fontText,

                type: HistoryType.LayersProperty_SetFontText,
                getUndo: (textLayer) => textLayer.FontText,
                setUndo: (textLayer, previous) => textLayer.FontText = previous
           );
        }


        public void MethodSetHorizontalAlignment(CanvasHorizontalAlignment horizontalAlignment)
        {
            this.HorizontalAlignment = horizontalAlignment;
            this.ITextLayerChanged<CanvasHorizontalAlignment>
            (
                set: (textLayer) => textLayer.HorizontalAlignment = horizontalAlignment,

                type: HistoryType.LayersProperty_SetHorizontalAlignment,
                getUndo: (textLayer) => textLayer.HorizontalAlignment,
                setUndo: (textLayer, previous) => textLayer.HorizontalAlignment = previous
           );
        }


        public void MethodSetFontWeight()
        {
            bool isBold = this.FontWeightConverter(this.FontWeight);
            FontWeight2 fontWeight = isBold ? FontWeight2.Normal : FontWeight2.Bold;

            this.MethodSetFontWeight(fontWeight);
        }
        public void MethodSetFontWeight(FontWeight2 fontWeight)
        {
            this.FontWeight = fontWeight;
            this.ITextLayerChanged<FontWeight2>
            (
                set: (textLayer) => textLayer.FontWeight = fontWeight,

                type: HistoryType.LayersProperty_SetFontWeight,
                getUndo: (textLayer) => textLayer.FontWeight,
                setUndo: (textLayer, previous) => textLayer.FontWeight = previous
           );
        }


        public void MethodSetFontStyle()
        {
            bool isNormal = this.FontStyleConverter(this.FontStyle);
            FontStyle fontStyle = isNormal ? FontStyle.Normal : FontStyle.Italic;

            this.MethodSetFontStyle(fontStyle);
        }
        public void MethodSetFontStyle(FontStyle fontStyle)
        {
            this.FontStyle = fontStyle;
            this.ITextLayerChanged<FontStyle>
            (
                set: (textLayer) => textLayer.FontStyle = fontStyle,

                type: HistoryType.LayersProperty_SetFontStyle,
                getUndo: (textLayer) => textLayer.FontStyle,
                setUndo: (textLayer, previous) => textLayer.FontStyle = previous
           );
        }


        public void MethodSetUnderline() => this.MethodSetUnderline(!this.Underline);
        public void MethodSetUnderline(bool underline)
        {
            this.Underline = underline;
            this.ITextLayerChanged<bool>
            (
                set: (textLayer) => textLayer.Underline = underline,

                type: HistoryType.LayersProperty_SetUnderline,
                getUndo: (textLayer) => textLayer.Underline,
                setUndo: (textLayer, previous) => textLayer.Underline = previous
           );
        }


        public void MethodSetFontFamily(string fontFamily)
        {
            this.FontFamily = fontFamily;
            this.ITextLayerChanged<string>
            (
                set: (textLayer) => textLayer.FontFamily = fontFamily,

                type: HistoryType.LayersProperty_SetFontFamily,
                getUndo: (textLayer) => textLayer.FontFamily,
                setUndo: (textLayer, previous) => textLayer.FontFamily = previous
           );
        }


        public void MethodSetFontSize(float fontSize)
        {
            this.FontSize = fontSize;
            this.ITextLayerChanged<float>
            (
                set: (textLayer) => textLayer.FontSize = fontSize,

                type: HistoryType.LayersProperty_SetFontSize,
                getUndo: (textLayer) => textLayer.FontSize,
                setUndo: (textLayer, previous) => textLayer.FontSize = previous
           );

            // Refactoring
            this.Transformer = this.RefactoringTransformer();
        }


        public void MethodSetDirection(CanvasTextDirection direction)
        {
            this.Direction = direction;
            this.ITextLayerChanged<CanvasTextDirection>
            (
                set: (textLayer) => textLayer.Direction = direction,

                type: HistoryType.LayersProperty_SetDirection,
                getUndo: (textLayer) => textLayer.Direction,
                setUndo: (textLayer, previous) => textLayer.Direction = previous
           );
        }


    }
}