using System;
using System.Windows;
using System.Windows.Media;

namespace wpf_css_mix_blend_mode;

/// <summary>Interaction logic for MainWindow.xaml</summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var defaultForeground = textBlock.Foreground as SolidColorBrush;
        var newForeground = GetReadableForeground(
            backgroundColor: ((SolidColorBrush) textBlock.Background).Color,
            defaultForeground: defaultForeground);
        textBlock.Foreground = newForeground;
    }
    
    public static double GetContrast(Color color1, Color color2)
    {
        var l1 = GetRelativeLuminance(color: color1);
        var l2 = GetRelativeLuminance(color: color2);

        var lighter = Math.Max(val1: l1, val2: l2);
        var darker = Math.Min(val1: l1, val2: l2);

        return (lighter + 0.05) / (darker + 0.05);
    }

    public static SolidColorBrush GetReadableForeground(Color backgroundColor, SolidColorBrush defaultForeground)
    {
        double contrastThreshold = 9; // آستانه کنتراست متوسط

        var contrast = GetContrast(color1: backgroundColor, color2: defaultForeground.Color);
        if (contrast >= contrastThreshold)
        {
            return defaultForeground;
        }
        else
        {
            var luminance = GetRelativeLuminance(color: backgroundColor);
            var newForeground = luminance > 0.5 ? Brushes.Black : Brushes.White;
            return newForeground;
        }
    }

    public static double GetRelativeLuminance(Color color)
    {
        var r = color.R / 255.0;
        var g = color.G / 255.0;
        var b = color.B / 255.0;

        var rgb = new double[]
        {
            r,
            g,
            b
        };

        for (var i = 0; i < 3; i++)
        {
            if (rgb[i] <= 0.03928)
            {
                rgb[i] = rgb[i] / 12.92;
            }
            else
            {
                rgb[i] = Math.Pow(x: (rgb[i] + 0.055) / 1.055, y: 2.4);
            }
        }

        return 0.2126 * rgb[0] + 0.7152 * rgb[1] + 0.0722 * rgb[2];
    }
}
