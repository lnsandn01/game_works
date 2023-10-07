using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionOption : MonoBehaviour
{
    public TextBoxSelection text_box_selection;
    private GameObject Selected;

    public void Instantiate(TextBoxSelection text_box_selection)
    {
        this.text_box_selection = text_box_selection;
        this.gameObject.GetComponentInChildren<Text>().text = text_box_selection.selection_text;
        Selected = transform.Find("Selected").gameObject;
        if (text_box_selection.selected)
        {
            displaySelected(true);
        }
    }

    public void displaySelected(bool display)
    {
        if(Selected != null)
        {
            UITools.displayImage(Selected, display);
        }
    }

    public void fireEvent()
    {
        if(text_box_selection.game_event != null)
        {
            GWEventManager.gwevent_manager.TriggerEvent(text_box_selection.game_event);
        }
    }
}
