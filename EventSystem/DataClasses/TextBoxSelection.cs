[System.Serializable]
public class TextBoxSelection
{
    public int id { get; set; }
    public int text_box_text_id { get; set; }
    public int trigger_object_id { get; set; }
    public string selection_text { get; set; }
    public bool selected { get; set; }
    public GameEvent game_event { get; set; }

    public TextBoxSelection(
        int id, 
        int text_box_text_id, 
        int trigger_object_id, 
        string selection_text,
        bool selected,
        GameEvent game_event = null
    )
    {
        this.id = id;
        this.text_box_text_id = text_box_text_id;
        this.trigger_object_id = trigger_object_id;
        this.selection_text = selection_text;
        this.selected = selected;
        this.game_event = game_event;
    }
}