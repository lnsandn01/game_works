using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using GWConst;

public class UITools : MonoBehaviour
{

    public static void displayImage(GameObject obj, bool display)
    {
        if (display)
        {
            Image im = obj.GetComponent<Image>();
            SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
            if(im == null)
            {
                if (sp == null)
                {
                    return;
                }
                Color color = sp.color;
                color = new Color(color.r, color.g, color.b, 1f);
                sp.color = color;
            }
            else
            {
                Color color = im.color;
                color = new Color(color.r, color.g, color.b, 1f);
                im.color = color;
            }
        }
        else
        {
            Image im = obj.GetComponent<Image>();
            SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
            if (im == null)
            {
                if (sp == null)
                {
                    return;
                }
                Color color = sp.color;
                color = new Color(color.r, color.g, color.b, 0f);
                sp.color = color;
            }
            else
            {
                Color color = im.color;
                color = new Color(color.r, color.g, color.b, 0f);
                im.color = color;
            }
        }
    }

    public static void displayText(GameObject obj, bool display, bool in_children)
    {
        Color text_color = new Color();

        TextMeshProUGUI tmp = obj.GetComponentInChildren<TextMeshProUGUI>();
        if (!in_children)
        {
            tmp = obj.GetComponent<TextMeshProUGUI>();
        }
        if (!tmp)
        {
            Text t = obj.GetComponentInChildren<Text>();
            if (!in_children)
            {
                t = obj.GetComponent<Text>();
            }
            text_color = t.color;
            text_color = new Color(text_color.r, text_color.g, text_color.b, (display?1f:0f));
            t.color = text_color;
        }
        else
        {
            text_color = tmp.color;
            text_color = new Color(text_color.r, text_color.g, text_color.b, (display ? 1f : 0f));
            tmp.color = text_color;
        }
    }

    public static void changeSourceImage(GameObject obj, Sprite new_image)
    {
        obj.GetComponent<Image>().sprite = new_image;
    }

    public static void changeOpacityTo(GameObject obj, float val)
    {
        Image im = obj.GetComponent<Image>();
        SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
        if (im == null)
        {
            if (sp == null)
            {
                return;
            }
            Color color = sp.color;            
            color = new Color(color.r, color.g, color.b, val);
            sp.color = color;
        }
        else
        {
            Color color = im.color;
            float new_a = color.a;
            color = new Color(color.r, color.g, color.b, val);
            im.color = color;
        }
    }

    /**
     * input: position in screen coordinates; like from a touch or mouse position
     * game object you are comparing to
     * output: true if the position is within the bounds of the game object; false otherwise
     */
    public static bool mousePositionOverObject(Vector2 screen_pos, Canvas canvas, GameObject game_object)
    {
        Rect rect;
        // check if ui object or in game object
        if (game_object.GetComponent<CanvasRenderer>() != null)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(game_object.GetComponent<RectTransform>(), screen_pos, canvas.worldCamera);
        }
        else
        {
            rect = GetRect(canvas, game_object);
        }
        return rect.Contains(screen_pos);
    }

    public static Rect GetRect(Canvas canvas, GameObject go)
    {
        var canvasRT = canvas.transform as RectTransform;
        var camera = canvas.worldCamera;
        if (camera == null) camera = Camera.main;
        var renderer = go.GetComponent<Renderer>();
        if (camera == null || canvasRT == null || renderer == null) return Rect.zero;
        var bounds = renderer.bounds;
        var cen = bounds.center;
        var ext = bounds.extents;
        var extMin = cen - ext;
        var extMax = cen + ext;
        var extentPoints = new[]
        {
          new Vector3(extMax.x, extMin.y, extMin.z),
          new Vector3(extMin.x, extMin.y, extMax.z),
          new Vector3(extMax.x, extMin.y, extMax.z),
          new Vector3(extMin.x, extMax.y, extMin.z),
          new Vector3(extMax.x, extMax.y, extMin.z),
          new Vector3(extMin.x, extMax.y, extMax.z),
          extMax
      };
        var min = camera.WorldToScreenPoint(extMin);
        var max = min;
        foreach (var v3 in extentPoints)
        {
            var v = camera.WorldToScreenPoint(v3);
            min = Vector2.Min(min, v);
            max = Vector2.Max(max, v);
        }
        var sizeDelta = canvasRT.sizeDelta / 2f;
        return new Rect(min.x - sizeDelta.x, min.y - sizeDelta.y, max.x - min.x, max.y - min.y);
    }

    public static void addArrowsToText(TextMeshProUGUI tmp)
    {
        if (!tmp.text.Contains(" <"))
        {
            tmp.text = tmp.text.Insert(tmp.text.Length, " <");
        }
    }

    public static void removeArrowsFromText(TextMeshProUGUI tmp)
    {
        tmp.text = tmp.text.Replace(" <", "");
    }

    public static IEnumerator moveTowards(GameObject gameObject, Vector3 towards, float speed)
    {
        while(Vector3.Distance(gameObject.transform.position, towards) > 0.001f)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, towards, Time.deltaTime * speed);
            yield return null;
        }
        yield return null;
    }

    public static IEnumerator scale(GameObject gameObject, Vector3 scale, float speed)
    {
        bool x_bigger = false, y_bigger = false, z_bigger = false, scaled_x = false, scaled_y = false, scaled_z = false;
       
        if(gameObject.transform.localScale.x > scale.x)
        {
            x_bigger = true;
        }
        if (gameObject.transform.localScale.y > scale.y)
        {
            y_bigger = true;
        }
        if (gameObject.transform.localScale.z > scale.z)
        {
            z_bigger = true;
        }
        while (!scaled_x && !scaled_y && ! scaled_z)
        {
            gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, Vector3.one + (scale * Time.deltaTime * speed));
            
            if((x_bigger && gameObject.transform.localScale.x < scale.x)
                || (!x_bigger && gameObject.transform.localScale.x > scale.x))
            {
                scaled_x = true;
            }
            if ((y_bigger && gameObject.transform.localScale.y < scale.y)
                || (!y_bigger && gameObject.transform.localScale.y > scale.y))
            {
                scaled_y = true;
            }
            if ((z_bigger && gameObject.transform.localScale.z < scale.z)
                || (!z_bigger && gameObject.transform.localScale.z > scale.z))
            {
                scaled_z = true;
            }
            yield return null;
        }
        yield return null;
    }

    public static IEnumerator changeColorLerp(SpriteRenderer sprite_renderer, Color color, float speed)
    {
        while(sprite_renderer.color != color)
        {
            if (GWStateManager.interrupted || GWStateManager.paused || GWStateManager.dying)
            {
                yield return null;
            }
            sprite_renderer.color = new Color(Mathf.Lerp(sprite_renderer.color.r, color.r, Time.deltaTime * speed),
                Mathf.Lerp(sprite_renderer.color.g, color.g, Time.deltaTime * speed),
                Mathf.Lerp(sprite_renderer.color.b, color.b, Time.deltaTime * speed),
                Mathf.Lerp(sprite_renderer.color.a, color.a, Time.deltaTime * speed));
            yield return null;
        }
        yield return null;
    }

    public static void changeTextLanguage(TextMeshProUGUI textmp, int id)
    {
        TextBoxText tbt = Array.Find(
        GWConstManager.other_texts, item => item.id == id && GWStateManager.language == item.language);
        if (tbt == null)
        {
            Debug.LogWarning("text_id" + id + " not translated into given language " + GWStateManager.language);
            tbt = Array.Find(GWConstManager.other_texts, item => item.id == id);
        }
        if (tbt == null)
        {
            Debug.LogWarning("text_id" + id + " not given at all in GWCon.other_texts");
            return;
        }
        textmp.text = tbt.text;
    }
}
