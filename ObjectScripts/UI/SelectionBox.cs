using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using GWCon = GWConst.GWConstManager;

public class SelectionBox : MonoBehaviour
{
    [SerializeField] private GameObject option;
    private int current_option;
    private TextBoxSelection[] text_box_selections;
    private GameObject[] options;

    private void Start()
    {
        GWControlsManager.playerControls.Land.UpDown.performed += onSelectionUpDown;
        GWControlsManager.playerControls.Land.Jump.performed += onAccept;
    }

    private void OnDisable()
    {
        if (GWControlsManager.playerControls != null)
        {
            GWControlsManager.playerControls.Land.UpDown.performed -= onSelectionUpDown;
            GWControlsManager.playerControls.Land.Jump.performed -= onAccept;
        }
    }
    public void displaySelection(TextBoxSelection[] new_text_box_selections)
    {
        this.text_box_selections = new_text_box_selections;
        GWStateManager.selection_box = true;
        UITools.displayImage(this.gameObject, true);
        options = new GameObject[new_text_box_selections.Length];
        int i = 0;
        foreach(TextBoxSelection sel in text_box_selections)
        {
            GameObject new_option = Instantiate(option) as GameObject;
            new_option.transform.parent = transform;
            new_option.transform.localScale = new Vector3(1f, 1f, 1f);
            SelectionOption script = (SelectionOption)new_option.GetComponent(typeof(SelectionOption));
            script.Instantiate(sel);
            if (sel.selected)
            {
                current_option = i;
            }
            options[i] = new_option;
            i++;
        }
    }

    private void onSelectionUpDown(InputAction.CallbackContext cxt)
    {
        if (GWStateManager.selection_box)
        {
            int direction = (int)GWControlsManager.playerControls.Land.UpDown.ReadValue<float>();
            direction *= -1;
            int last_id = text_box_selections.Length - 1;
            // direction 1 go down / -1 go up
            if ((direction == -1 && current_option > 0) || (direction == 1 && current_option < last_id))
            {
                // hide current option selected
                SelectionOption current_script =
                    (SelectionOption)options[current_option]
                        .GetComponent(typeof(SelectionOption));
                current_script.displaySelected(false);
                // display next option selected
                SelectionOption script = 
                    (SelectionOption)options[current_option + direction]
                        .GetComponent(typeof(SelectionOption));
                current_option += direction;
                script.displaySelected(true);
            }
        }
    }

    private void onAccept(InputAction.CallbackContext cxt)
    {
        if (GWStateManager.selection_box)
        {
            // hide selection box and send next text event
            UITools.displayImage(this.gameObject, false);
            foreach(Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            GWStateManager.selection_box = false;
            // next text or end
            TextBox.nextText();
            // fire event of option if given
            SelectionOption script =
                (SelectionOption)options[current_option]
                    .GetComponent(typeof(SelectionOption));
            script.fireEvent();
        }
    }
}
