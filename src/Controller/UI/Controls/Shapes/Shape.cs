namespace DogAgilityCompetition.Controller.UI.Controls.Shapes;

/// <summary>
/// Represents the base class for drawable shapes.
/// </summary>
public abstract class Shape
{
    protected const int ShadowOffset = 2;

    public ShapeState State { get; set; }

    public virtual void DrawShadow(Graphics graphics)
    {
    }

    public virtual void DrawFill(Graphics graphics)
    {
    }

    public virtual void DrawBorder(Graphics graphics)
    {
    }
}
