
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
    public void Init
    (
        bool i_randomNeighbourExploration,
        bool i_diagonalMovement
    )
    {
        m_elements.Add(UIManager.Instance.CreateAndRegisterRoundedBox_Relative(new Vector2(0.575f, 0.025f), new Vector2(0.4f, 0.5f), new Color(0, 0, 0, 128)));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("Algorithm: DFS", Color.White, FontStyle.Bold, new Vector2(0.6f, 0.05f), 25, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("Instructions:", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.1f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Place obstacles with LMB. Right click to remove.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.15f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Press 'R' to reset.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.2f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Press 'Enter' to switch to placing start/end.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.25f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Random neighbour ordering: " + i_randomNeighbourExploration.ToString(), Color.White, FontStyle.Regular, new Vector2(0.6f, 0.3f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Diagonal movement: " + i_diagonalMovement.ToString(), Color.White, FontStyle.Regular, new Vector2(0.6f, 0.35f), 20, 1));


    }
}