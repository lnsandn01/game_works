using UnityEngine;

[System.Serializable]
class BlendOutEventValue
{
    // false blend out
    public bool blend_out { get; set; }
    // if scene change from death then with cam zoom delay
    public bool with_cam_zoom { get; set; }

    public BlendOutEventValue(bool blend_out, bool with_cam_zoom)
    {
        this.blend_out = blend_out;
        this.with_cam_zoom = with_cam_zoom;
    }
}
