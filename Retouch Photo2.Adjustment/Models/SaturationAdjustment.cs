﻿// Core:              ★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s SaturationAdjustment.
    /// </summary>
    public class SaturationAdjustment : IAdjustment
    {

        //@Content
        public AdjustmentType Type => AdjustmentType.Saturation;
        public Visibility PageVisibility => Visibility.Visible;

        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }

        public SaturationAdjustment()
        {
            this.Edit = new AdjustmentCommand(() => AdjustmentCommand.Edit(this));
            this.Remove = new AdjustmentCommand(() => AdjustmentCommand.Remove(this));
        }

        /// <summary> Gets or sets the saturation intensity for effect. </summary>
        public float Saturation = 1.0f;
        public float StartingSaturation { get; private set; }
        public void CacheSaturation() => this.StartingSaturation = this.Saturation;
        

        public IAdjustment Clone()
        {
            return new SaturationAdjustment
            {
                Saturation = this.Saturation,
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("Saturation", this.Saturation));
        }
        public void Load(XElement element)
        {
            if (element.Attribute("Saturation") is XAttribute saturation) this.Saturation = (float)saturation;
        }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new SaturationEffect
            {
                Saturation = this.Saturation,
                Source = image
            };
        }

    }
}