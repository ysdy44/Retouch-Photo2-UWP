// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.Graphics.Canvas.Text;
using System.Linq;
using Windows.Globalization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.TestApp.Pages
{
    public class FontPickerComboBox : UserControl
    {
     private readonly   ComboBox comboBox = new ComboBox();

        public FontPickerComboBox()
        {
            var fontFamilyNames = CanvasTextFormat.GetSystemFontFamilies(ApplicationLanguages.Languages).OrderBy(k => k);

            foreach (string fontFamilyName in fontFamilyNames)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = fontFamilyName,
                    FontFamily = new FontFamily(fontFamilyName)
                };
                this.comboBox.Items.Add(item);
            }

            this.comboBox.SelectionChanged += this.ComboBox_SelectionChanged;

            base.Content = this.comboBox;

            this.SelectDefaultFont();
        }

        public event SelectionChangedEventHandler SelectionChanged
        {
            add { this.comboBox.SelectionChanged += value;}
            remove { this.comboBox.SelectionChanged -= value; }
        }

        private void SelectDefaultFont()
        {
            SelectFont("Arial");
        }

        public void SelectFont(string name)
        {
            for (int i = 0; i < this.comboBox.Items.Count; ++i)
            {
                ComboBoxItem item = this.comboBox.Items[i] as ComboBoxItem;
                if ((item.Content as string) == name)
                {
                    this.comboBox.SelectedIndex = i;
                    return;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Updates the style of the currently-displayed item to reflect the font.
            FontFamily = (this.comboBox.SelectedItem as ComboBoxItem).FontFamily;
        }

        public string CurrentFontFamily
        {
            get
            {
                return (this.comboBox.SelectedItem as ComboBoxItem).Content as string;
            }
        }
    }
}
