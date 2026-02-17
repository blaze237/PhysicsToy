using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox.Utils;
internal class List2D<T>
{

    //--------------------
    public List2D
    (
        int width,
        int height
    )
    {
        m_width = width;
        m_height = height;
        m_data = new T[width * height];
    }

    //--------------------
    public T this[int x, int y]
    {
        get => m_data[y * m_width + x];
        set => m_data[y * m_width + x] = value;
    }

    //Members
    private readonly T[] m_data;
    public readonly int m_width;
    public readonly int m_height;
}   