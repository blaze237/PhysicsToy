using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox
{
    internal abstract class RenderableObject
    {
        //-------------------
        protected RenderableObject
        (
            Renderer i_renderer
        )
        {
            i_renderer.AddRenderable(this);
        }


        //-------------------
        public abstract void Render(float i_dt);
    }
}
