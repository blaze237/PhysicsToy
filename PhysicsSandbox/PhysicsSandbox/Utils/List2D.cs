using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSandbox.Utils;
public class List2D<T> : IEnumerable<T>
{
    // Members
    private readonly T[] m_data;
    public readonly int m_width;
    public readonly int m_height;

    // Methods

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

    //--------------------
    public IEnumerator<T> GetEnumerator()
    {
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                yield return this[x, y];
            }
        }
    }

    //--------------------
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}

