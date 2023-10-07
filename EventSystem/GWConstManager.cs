
using System.Collections.Generic;
using UnityEngine;

namespace GWConst
{
    public class GWConstManager : MonoBehaviour
    {
        #region EventTags
        public const ushort LOOP_EVENT_TAG = 1;
        public const ushort CONTROLER_EVENT_TAG = 2;
        public const ushort PLAYER_EVENT_TAG = 3;
        public const ushort SOUND_EVENT_TAG = 4;
        public const ushort SCENE_EVENT_TAG = 5;
        public const ushort DYING_EVENT_TAG = 6;
        public const ushort LEVEL_START_EVENT_TAG = 7;
        public const ushort REINIT_MANAGERS_GAME_EVENT_TAG = 8;
        public const ushort CHANGE_MENU_EVENT_TAG = 9;
        public const ushort TEXT_BOX_EVENT_TAG = 10;
        public const ushort ACTIVATE_TRAP_EVENT_TAG = 11;
        public const ushort LOST_GAME_EVENT_TAG = 12;
        public const ushort START_NEW_GAME_EVENT_TAG = 13;
        public const ushort BLEND_OUT_SCENE_EVENT_TAG = 14;
        #endregion

        #region Layers
        public const string GROUND_LAYER = "ground";
        public const string DANGEROUS_LAYER = "dangerous";
        public const string WATER_LAYER = "Water";
        public const string PUSHING_LAYER = "pushing";
        public const string CAM_MOVER_LAYER = "cam_mover";
        public const string CAM_GUIDELINE_LAYER = "cam_guideline";
        #endregion

        #region Scenes
        public static string[] levels = {"StartScene","Level_1","Level_2","Level_3","Level_4"};
        public static string[] menus = {"None","StartMenu","MainMenu","SettingsMenu", "LostGameMenu","InGameUI","PauseMenu" };

        #endregion

        #region Texts
        public static string[] supported_languages = {"English", "German"};
        public static TextBoxText[] text_box = {
            new TextBoxText(1,0,2,0,0,true,"Character1Name: Watch your tongue! You\'re speaking to Ulfric Stormcloak, the true High King."),
            new TextBoxText(2,1,3,0,0,false,"Character2Name: Ulfric? The Jarl of Windhelm?"),
            new TextBoxText(3,2,3,0,0,true,"Character2Name: You\'re the leader of the rebellion. But if they captured you… Oh gods, where are they taking us?")
        };

        public static TextBoxSelection[] text_box_selections =
        {
            new TextBoxSelection(1,1,3,"Flee",false),
            new TextBoxSelection(1,1,3,"Fight",true),
            new TextBoxSelection(1,1,3,"Do nothing....",false,new GameEvent(DYING_EVENT_TAG, default(System.DateTime), false, true))
        };

        public static TextBoxText[] other_texts =
        {
            new TextBoxText(1,0,0,0,0,false,"Start Game"),
            new TextBoxText(1,0,0,1,0,false,"Starte Spiel"),
            new TextBoxText(2,0,0,0,0,false,"Settings"),
            new TextBoxText(2,0,0,1,0,false,"Einstellungen"),
        };
        #endregion

        #region Sounds
        public static SoundEventValue[] sounds =
        {
            new SoundEventValue(1,true,1f,true,1f)
        };
        #endregion
    }
}