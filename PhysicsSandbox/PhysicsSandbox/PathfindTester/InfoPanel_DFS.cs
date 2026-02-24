
using System;
using System.Numerics;
using PhysicsSandbox.Core.UI;
using PhysicsSandbox.Utils;
using PhysicsToy.Core.UI;
using Raylib_cs;
using static PhysicsSandbox.Core.UI.UIText;

namespace PhysicsToy.PathfindTester;

class InfoPanel_DFS : WorldUIManager
{
    public override void Init()
    {
        m_elements.Add(UIManager.Instance.CreateAndRegisterRoundedBox_Relative(new Vector2(0.5f, 0.5f), new Vector2(0.25f, 0.25f), new Color(0, 0, 0, 128)));
      //  m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("Mode: DFS", Color.White, FontStyle.Bold, new Vector2(0.75f, 0.25f), 25, 1));
    }
}