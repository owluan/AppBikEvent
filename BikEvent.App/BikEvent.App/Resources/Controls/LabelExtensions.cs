using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BikEvent.App.Resources.Controls
{
    public static class LabelExtensions
    {
        public static SKSize MeasureTextSize(this Label label)
        {
            // Obtém o texto da Label
            string text = label.Text;

            // Verifica se o texto está vazio
            if (string.IsNullOrEmpty(text))
            {
                return new SKSize(0, 0); // Retorna tamanho zero se o texto estiver vazio
            }

            // Cria um Paint para definir a fonte e outros estilos
            var paint = new SKPaint
            {
                TextSize = (float)label.FontSize,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName(label.FontFamily, SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright),
            };

            // Obtém a métrica do texto
            SKRect bounds = new SKRect();
            paint.MeasureText(text, ref bounds);

            // Retorna o tamanho do texto medido
            return new SKSize(bounds.Width, bounds.Height);
        }

    }
}
