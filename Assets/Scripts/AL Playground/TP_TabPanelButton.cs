using UnityEngine;

public class TP_TabPanelButton : MonoBehaviour
{
    public TP_Tabs tabController;
    public int tabIndex;
    public int panelIndex;

    public void OpenAssignedPanel()
    {
        if (tabController == null)
        {
            return;
        }

        tabController.OpenTabPanel(tabIndex, panelIndex);
    }
}