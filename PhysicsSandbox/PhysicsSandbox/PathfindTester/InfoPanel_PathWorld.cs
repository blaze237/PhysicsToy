
using System;
using System.Numerics;
using PhysicsSandbox.Core.UI;
using PhysicsSandbox.Utils;
using PhysicsToy.Core.UI;
using Raylib_cs;
using static PhysicsSandbox.Core.UI.FontManager;
using static PhysicsSandbox.Core.UI.UIText;

namespace PhysicsToy.PathfindTester;

class InfoPanel_PathWorld : WorldUIManager
{
    private UIText m_statusText;
    private UIText m_algorithmText;

    //--------------------------
    public void SetStatusText
    (
        string i_text,
        Color i_color
    )
    {
        m_statusText.Text = "Status: " + i_text;
        m_statusText.Color = i_color;
    }
    //--------------------------
    public void SetAlgorithmText
    (
        string i_text,
        Color i_color
    )
    {
        m_algorithmText.Text = "Algorithm: " + i_text;
        m_algorithmText.Color = i_color;
    }
    
    //--------------------------
    public void Init()
    {

        m_elements.Add(UIManager.Instance.CreateAndRegisterRoundedBox_Relative(new Vector2(0.575f, 0.025f), new Vector2(0.4f, 0.65f), new Color(0, 0, 0, 128)));
        // Shift first element down by 25%
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("Instructions:", Color.White, FontStyle.Bold, new Vector2(0.6f, 0.05f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Place obstacles with LMB. Right click to remove.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.08125f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Press 'R' to reset.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.11875f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Press 'Enter' to switch to placing start/end.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.15625f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("- Press Numbers to select algorithm.", Color.White, FontStyle.Regular, new Vector2(0.6f, 0.19375f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("[1] BFS", Color.White, FontStyle.Bold, new Vector2(0.6f, 0.23125f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("[2] DFS", Color.White, FontStyle.Bold, new Vector2(0.6f, 0.26875f), 20, 1));
        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("[3] A*", Color.White, FontStyle.Bold, new Vector2(0.6f, 0.30625f), 20, 1));

        //Add a divider
        m_elements.Add(UIManager.Instance.CreateAndRegisterRoundedBox_Relative(new Vector2(0.6f, 0.35f), new Vector2(0.35f, 0.0025f), Color.LightGray));

        m_elements.Add(UIManager.Instance.CreateAndRegisterText_Relative("Info:", Color.White, FontStyle.Bold, new Vector2(0.6f, 0.375f), 20, 1));
        //Algorithm text
        {
            Vector2Int position = UIScaler.RelativeToAbsolute(new Vector2(0.6f, 0.4125f));
            m_algorithmText = new UIText("Algorithm: BFS", Color.Gold, FontStyle.Regular, position, 20, UIManager.Instance.GetNextID(), false /*No Scaling*/);
            UIManager.Instance.RegisterElement(m_algorithmText);
            m_elements.Add(m_algorithmText.ID);
        }
        //Status text
        {
            Vector2Int position = UIScaler.RelativeToAbsolute(new Vector2(0.6f, 0.45f));
            m_statusText = new UIText("Status: Obstacles", Color.White, FontStyle.Regular, position, 20, UIManager.Instance.GetNextID(), false /*No Scaling*/);
            UIManager.Instance.RegisterElement(m_statusText);
            m_elements.Add(m_statusText.ID);
        } 
    }
}