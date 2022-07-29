using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.UI.Controls;

/// <summary>
/// Represents a <see cref="Label" /> whose font size adapts to the control size, preserving aspect ratio.
/// </summary>
public sealed class AutoScalingLabel : Label
{
    private float? sizeCache;
    private PointF? drawPointCache;
    private string innerText = string.Empty;

    // AutoSize defeats the whole purpose of this control when set to True, so block it.
    [Browsable(false)]
    [DefaultValue(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override bool AutoSize
    {
        get => false;
        set
        {
            // ignore
        }
    }

    public override Font Font
    {
        get => base.Font;
        set
        {
            base.Font = value;
            ResetCache();
        }
    }

    public override string Text
    {
        get => innerText;
        set
        {
            // ReSharper disable once ConstantNullCoalescingCondition
            // Justification: Although this property is not nullable, caller could still pass in 'null' anyway.
            value ??= string.Empty;

            if (value != innerText)
            {
                ResetCache();
                innerText = value;
                Invalidate();
            }
        }
    }

    public override ContentAlignment TextAlign
    {
        get => base.TextAlign;
        set
        {
            if (value != base.TextAlign)
            {
                base.TextAlign = value;
                ResetCache();
            }
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Guard.NotNull(e, nameof(e));

        EnsureCache(e.Graphics);
        RenderText(e.Graphics);
    }

    private void ResetCache()
    {
        sizeCache = null;
        drawPointCache = null;
    }

    private void EnsureCache(Graphics graphics)
    {
        if (sizeCache == null || drawPointCache == null)
        {
            Font font = Font;
            Size layoutSize = ClientRectangle.Size;

            SizeF extent = graphics.MeasureString(innerText, font);

            float heightRatio = layoutSize.Height / extent.Height;
            float widthRatio = layoutSize.Width / extent.Width;
            float ratio = heightRatio < widthRatio ? heightRatio : widthRatio;

            float newSize = font.Size * ratio;

            if (!float.IsInfinity(newSize))
            {
                using var drawFont = new Font(font.FontFamily, newSize, font.Style);
                extent = graphics.MeasureString(innerText, drawFont);
                drawPointCache = CalculateDrawPoint(extent, heightRatio, widthRatio);
            }

            sizeCache = newSize;
        }
    }

    private PointF CalculateDrawPoint(SizeF extent, float heightRatio, float widthRatio)
    {
        float offsetX = 0;
        float offsetY = 0;

        if (widthRatio < heightRatio)
        {
            // offsetY depends on alignment
            switch (TextAlign)
            {
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    offsetY = (ClientRectangle.Height - extent.Height) / 2;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    offsetY = ClientRectangle.Height - extent.Height;
                    break;
            }
        }
        else
        {
            // offsetX depends on alignment
            switch (TextAlign)
            {
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    offsetX = (ClientRectangle.Width - extent.Width) / 2;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    offsetX = ClientRectangle.Width - extent.Width;
                    break;
            }
        }

        return new PointF(offsetX, offsetY);
    }

    private void RenderText(Graphics graphics)
    {
        if (sizeCache != null && drawPointCache != null && !float.IsInfinity(sizeCache.Value))
        {
            Font currentFont = Font;

            using var drawFont = new Font(currentFont.FontFamily, sizeCache.Value, currentFont.Style);
            using var brush = new SolidBrush(ForeColor);

            graphics.DrawString(innerText, drawFont, brush, drawPointCache.Value);
        }
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        ResetCache();
    }
}
