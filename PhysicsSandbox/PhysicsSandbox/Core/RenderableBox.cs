using System.Numerics;
using PhysicsSandbox.Utils;
using Raylib_cs;

namespace PhysicsSandbox.Core;


public class RenderableBox : RenderableObject
{
    private Vector2Int m_position;
    private float m_width;      
    private float m_height;

    //take in relative and color and rotation
    public RenderableBox(Vector2Int position, float width, float height)
    {
       m_position = position;
       m_width = width;
       m_height = height;
    }

    public override void Render(float i_alpha)
    {
      //  Raylib.DrawRectangle(m_position.X, (m_position.Y, (int)m_width, (int)m_height, Color.WHITE);
    }
}
