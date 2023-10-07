using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GWButton : MonoBehaviour
{
    [SerializeField] public GameObject previous_button;
    [SerializeField] public GameObject next_button;
    [SerializeField] public GameObject toggle;
    [SerializeField] public Sprite untoggled_image;
    [SerializeField] public Sprite toggled_image;
    [SerializeField] public ushort toggle_value;
    [SerializeField] public bool deactivate_menu;
    [SerializeField] public bool no_arrow;
    [SerializeField] public float blink_duration, original_blink_duration = 0.25f;
    public bool multiple_presses;
    public bool hovering;
    private bool toggled;
    private IEnumerator checkToggles;

    void OnEnable()
    {
        blink_duration = original_blink_duration;
        if(toggle_value != 0)
        {
            checkToggles = checkToggle();
            StartCoroutine(checkToggles);
        }
    }

    private void OnDisable()
    {
        if(toggle_value != 0)
        {
            StopCoroutine(checkToggles);
        }
        if(gameObject.GetComponent<TextMeshProUGUI>() != null)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    public void hover()
    {
        hovering = true;
        if (!no_arrow)
        {
            UITools.addArrowsToText(gameObject.GetComponent<TextMeshProUGUI>());
        }
        StartCoroutine(BlinkText());
    }

    public void unhover()
    {
        hovering = false;
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        if (!no_arrow)
        {
            UITools.removeArrowsFromText(gameObject.GetComponent<TextMeshProUGUI>());
        }
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    public void accept()
    {
        StartCoroutine(acceptEffect());
        gameObject.GetComponent<OnActivate>().onActivate();
    }

    public IEnumerator BlinkText()
    {
        if(gameObject.GetComponent<Animator>())
        {
            while(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("New State") || gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Entry"))
            {
                yield return null;
            }
        }
        float t = 0f;
        while (hovering)
        {
            t = 0f;
            while(hovering && t < blink_duration)
            {
                t += Time.deltaTime;
                gameObject.GetComponent<TextMeshProUGUI>().color = Color.clear;
                yield return null;
            }
            t = 0f;
            while (t < blink_duration)
            {
                t += Time.deltaTime;
                gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator acceptEffect()
    {
        blink_duration /= 3;
        yield return new WaitForSeconds(0.5f);
        blink_duration = original_blink_duration;
        yield return null;
    }

    IEnumerator checkToggle()
    {
        while (1 == 1)
        {
            bool old_toggle = toggled;
            switch (toggle_value)
            {
                
            }
            if (old_toggle != toggled)
            {
                if (toggled)
                {
                    UITools.changeSourceImage(toggle, toggled_image);
                }
                else
                {
                    UITools.changeSourceImage(toggle, untoggled_image);
                }
            }
            yield return null;
        }
    }
}
