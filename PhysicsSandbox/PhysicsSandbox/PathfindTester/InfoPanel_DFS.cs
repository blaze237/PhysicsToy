
using System;
using System.Numerics;
using PhysicsSandbox.Core.UI;
using PhysicsSandbox.Utils;
using PhysicsToy.Core.UI;
using Raylib_cs;
using static PhysicsSandbox.Core.UI.FontManager;
using static PhysicsSandbox.Core.UI.UIText;

namespace PhysicsToy.PathfindTester;

class InfoPanel_DFS : WorldUIManager
{
    public void Init()
    {
        
        m_elements.Add(UIManager.Instance.CreateAndRegisterRoundedBox_Relative(new Vector2(0.575f, 0.025f), new Vector2(0.4f, 0.4f), new Color(0, 0, 0, 128)));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("Instructions:", Color.White, FontStyle.Bold, new Vector2(0.6f, 0.05f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Place obstacles with LMB. Right click to remove.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.075f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Press 'R' to reset.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.125f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Press 'Enter' to switch to placing start/end.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.175f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Press Numbers to select algorithm.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.225f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("[1] BFS", Color.White, FontStyle.Bold, new Vector2(0.6f, 0.275f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("[2] DFS", Color.White, FontStyle.Bold, new Vector2(0.6f, 0.325f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("[3] A*", Color.White, FontStyle.Bold, new Vector2(0.6f, 0.375f), 20, 1));
    }
}