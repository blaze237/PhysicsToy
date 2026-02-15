using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox
{
internal interface WorldBuilder
{
    void BuildWorld(out Renderer o_renderer, out List<LogicManager> o_managers);
    void DestroyWorld(Renderer o_renderer, List<LogicManager> o_managers);
    }
}
