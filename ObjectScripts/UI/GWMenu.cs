using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;

public class GWMenu : MonoBehaviour
{
    [SerializeField] public GameObject current_button;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private MoveBackgroundMainMenu move_background_script;
    private GWButton cb_script;
    private Canvas canvas;
    private Vector2 hover_position = new Vector2();
    private Dictionary<string,bool> already_accepted;

    private void OnEnable()
    {
        GWControlsManager.playerControls.Land.UpDown.performed += onUIChange;
        GWControlsManager.playerControls.Land.HoverPosition.performed += onUIChange;
        GWControlsManager.playerControls.Land.Jump.performed += onAccept;
        if (current_button == null && buttons.Length > 0)
        {
            current_button = buttons[0];
        }
        cb_script = getCurrentButtonScript(current_button);
        cb_script.hover();
        already_accepted = new Dictionary<string, bool>();
        foreach (GameObject go in buttons)
        {
            if (go != null)
            {
                if (!already_accepted.ContainsKey(go.name))
                {
                    already_accepted.Add(go.name, false);
                }
                else
                {
                    already_accepted[go.name] = false;
                }
            }
        }
        if (GWSceneManager.loading_screen)
        {
            StartCoroutine(GWSceneManager.loading_screen.JustBlendIn(0f, null));
        }
    }

    private void OnDisable()
    {
        if(GWControlsManager.playerControls != null)
        {
            GWControlsManager.playerControls.Land.UpDown.performed -= onUIChange;
            GWControlsManager.playerControls.Land.Jump.performed -= onAccept;
            GWControlsManager.playerControls.Land.HoverPosition.performed -= onUIChange;
        }
        already_accepted = new Dictionary<string, bool>();
    }

    private void Awake()
    {
        canvas = this.gameObject.GetComponent<Canvas>();
    }

    private void Start()
    {
        filterOutMissingButtons();
        already_accepted = new Dictionary<string, bool>();
        foreach(GameObject go in buttons)
        {
            if (go != null)
            {
                if (!already_accepted.ContainsKey(go.name))
                {
                    already_accepted.Add(go.name, false);
                }
                else
                {
                    already_accepted[go.name] = false;
                }
            }
        }
    }

    private void Update()
    {
        if (GWControlsManager.playerControls.Land.HoverPosition != null)
        {
            return;
        }
        Vector2 cur_pos = GWControlsManager.playerControls.Land.HoverPosition.ReadValue<Vector2>();
        if (hover_position.x < cur_pos.x - 0.05f || hover_position.x > cur_pos.x + 0.05f
            || hover_position.y < cur_pos.y - 0.05f || hover_position.y > cur_pos.y + 0.05f)
        {
            hover_position = cur_pos;
            InputAction.CallbackContext cxt = new InputAction.CallbackContext();
            onUIChange(cxt);
        }
    }

    private void onUIChange(InputAction.CallbackContext cxt)
    {
        float direction = GWControlsManager.playerControls.Land.UpDown.ReadValue<float>();
        // check if through arrow keys or hover
        if (direction == 0)
        {
            Vector2 hover_pos = GWControlsManager.playerControls.Land.HoverPosition.ReadValue<Vector2>();
            foreach (GameObject button in buttons)
            {
                if (!button)
                {
                    continue;
                }
                if (UITools.mousePositionOverObject(hover_pos, canvas, button))
                {
                    if (current_button == button)
                    {
                        return;
                    }
                    if (current_button != null)
                    {
                        cb_script.unhover();
                    }
                    current_button = button;
                    cb_script = getCurrentButtonScript(current_button);
                    cb_script.hover();
                    GWEventManager.gwevent_manager.TriggerEvent(new GameEvent(
                        GWConst.GWConstManager.SOUND_EVENT_TAG, default(System.DateTime), false,
                        new SoundEventValue(4, false, 0, true, 0.5f)));
                    return;
                }
            }
            return;
        }

        if(current_button == null)
        {
            current_button = buttons[0];
            cb_script = getCurrentButtonScript(current_button);
        }

        if(direction > 0)
        {
            // previous image
            if (cb_script.previous_button)
            {
                cb_script.unhover();
                current_button = cb_script.previous_button;
                cb_script = getCurrentButtonScript(current_button);
                cb_script.hover();
                GWEventManager.gwevent_manager.TriggerEvent(new GameEvent(
                    GWConst.GWConstManager.SOUND_EVENT_TAG, default(System.DateTime), false,
                    new SoundEventValue(4, false, 0, true, 0.5f)));
            }
        }
        else
        {
            // next image
            if (cb_script.next_button != null)
            {
                cb_script.unhover();
                current_button = cb_script.next_button;
                cb_script = getCurrentButtonScript(current_button);
                cb_script.hover();
                GWEventManager.gwevent_manager.TriggerEvent(new GameEvent(
                    GWConst.GWConstManager.SOUND_EVENT_TAG, default(System.DateTime), false,
                    new SoundEventValue(4, false, 0, true, 0.3f)));
            }
        }
    }

    private void onAccept(InputAction.CallbackContext cxt)
    {
        bool over_other_button = false;
        foreach(GameObject button in buttons)
        {
            if(GWControlsManager.playerControls.Land.HoverPosition != null && canvas != null && button != null)
            {
                if (UITools.mousePositionOverObject(GWControlsManager.playerControls.Land.HoverPosition.ReadValue<Vector2>(), canvas, button))
                {
                    over_other_button = true;
                    current_button = button;
                    cb_script = getCurrentButtonScript(current_button);
                    break;
                }
            }
        }
        if (!over_other_button && current_button == null)
        {
            return;
        }
        
        // if caused by touch -> check if over button; else check if it was a key press
        if (UITools.mousePositionOverObject(GWControlsManager.playerControls.Land.HoverPosition.ReadValue<Vector2>(), canvas, current_button)
            || (Regex.Matches(cxt.control.path, "Keyboard/[sS]pace").Count != 0))
        {
            if (already_accepted[current_button.name])
            {
                return;
            }
            if (move_background_script)
            {
                move_background_script.moveBackground();
            }
            if (!cb_script.hovering)
            {
                cb_script.hover();
                foreach (GameObject button in buttons)
                {
                    if (button && button != current_button)
                    {
                        button.GetComponent<GWButton>().unhover();
                    }
                }
            }

            cb_script.accept();
            if (!cb_script.multiple_presses)
            {
                already_accepted[current_button.name] = true;
            }
            // deactivate menu
            if (cb_script.deactivate_menu)
            {
                //this.gameObject.SetActive(false);
            }
        }
        else
        {
            cb_script.unhover();
            current_button = null;
            cb_script = null;
        }
    }

    private GWButton getCurrentButtonScript(GameObject game_object)
    {
        // get the script
        GWButton gw_button = game_object.GetComponent<GWButton>();

        filterOutMissingButtons();

        // add previous and next buttons
        if (!gw_button.previous_button || !gw_button.next_button)
        {
            for(int i = 0; i < buttons.Length; i++)
            {               
                if(buttons[i].name == current_button.name)
                {
                    // get the previous and next button from the list
                    if(i == 0)
                    {
                        gw_button.previous_button = buttons[buttons.Length - 1];
                    }
                    else
                    {
                        gw_button.previous_button = buttons[i - 1];
                    }
                    if(i == buttons.Length - 1)
                    {
                        gw_button.next_button = buttons[0];
                    }
                    else
                    {
                        gw_button.next_button = buttons[i + 1];
                    }
                }
            }
        }
        return gw_button;
    }

    private void filterOutMissingButtons()
    {
        // remove missing buttons
        ushort button_counter = 0;
        foreach (GameObject button in buttons)
        {
            if (button)
            {
                button_counter++;
            }
        }

        GameObject[] tmp_buttons = new GameObject[button_counter];
        for (ushort i = 0, j = 0; i < buttons.Length; i++)
        {
            if (buttons[i])
            {
                tmp_buttons[j] = buttons[i];
                j++;
            }
        }
        buttons = tmp_buttons;
    }
}
