using PhysicsSansbox.TileRender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox.PathfindTester
{
    internal class PathfindWorld : WorldBuilder
    {
        //-----------------------
        public void BuildWorld
        (
            out Renderer o_renderer,
            out List<LogicManager> o_managers
        )
        {
            o_renderer = new TileRenderer(25, Program.c_screenWidth, Program.c_screenHeight);
            o_managers = new List<LogicManager>();
        }

        //-----------------------
        public void DestroyWorld
        (
            Renderer o_renderer,
            List<LogicManager> o_managers
        )
        {
            
        }
    }
}
