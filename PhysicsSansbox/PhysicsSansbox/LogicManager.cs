using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox
{
internal interface LogicManager
{
    void FixedUpdate(float i_fixedDeltaTime);
    void Update(float i_deltaTime);
}
}
