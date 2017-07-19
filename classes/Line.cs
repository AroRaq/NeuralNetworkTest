using SFML.Graphics;
using SFML.System;

namespace NeuralNetworkTest
{
    public class Line : Drawable
    {
        public Line(Vector2f p1, Vector2f p2)
        {
            line.Append(new Vertex(p1));
            line.Append(new Vertex(p2));
        }
        public Line(Vector2f p1, Vector2f p2, Color c)
        {
            line.Append(new Vertex(p1, c));
            line.Append(new Vertex(p2, c));
        }
        VertexArray line = new VertexArray(PrimitiveType.Lines, 0);
        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(line);
        }
    }
}