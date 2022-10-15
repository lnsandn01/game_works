
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
        public const ushort OPEN_MAIN_MENU_EVENT_TAG = 6;
        public const ushort GROUNDED_EVENT_TAG = 7;
        public const ushort JUMP_EVENT_TAG = 8;
        public const ushort DYING_EVENT_TAG = 9;
        public const ushort LEVEL_START_EVENT_TAG = 10;
        public const ushort REINIT_MANAGERS_GAME_EVENT_TAG = 11;
        public const ushort CHANGE_MENU_EVENT_TAG = 12;
        public const ushort SPEECH_BUBBLE_EVENT_TAG = 13;
        public const ushort TEXT_BOX_EVENT_TAG = 14;
        public const ushort ACTIVATE_TRAP_EVENT_TAG = 15;
        public const ushort LOST_GAME_EVENT_TAG = 16;
        public const ushort START_NEW_GAME_EVENT_TAG = 17;
        public const ushort BLEND_OUT_SCENE_EVENT_TAG = 18;
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
        public static string[] levels = {"StartScene","lvl0","lvl1.2","lvl2","lvl3","lvl5"};
        public static string[] menus = {"None","StartMenu","MainMenu","OptionsMenu","UnlockMoreLivesMenu","EndMenu","LostGameMenu","InGameUI","PauseMenu" };

        #endregion

        #region Texts
        public static TextBoxText[] text_box = {
            new TextBoxText(1,0,2,0,0,true,"Seni: Hmm? Music? Something is going on in the Village..."),
            new TextBoxText(2,1,3,0,0,false,"LittleTurtle: SENI!!! You are here"),
            new TextBoxText(3,2,4,0,0,false,"LittleTurtle: The village came together and assigned you to a quest to find the sacred plant ZINGIBER."),
            new TextBoxText(4,3,5,0,0,false,"LittleTurtle: Someone has to do something and cure our little turtles, or we all will end up sick."),
            new TextBoxText(5,4,6,0,0,false,"LittleTurtle: You are the oldest of the young folk and fit perfectly for the job."),
            new TextBoxText(6,5,7,0,0,false,"LittleTurtle: We old ones can't make it across the old mountains anymore..."),
            new TextBoxText(7,6,8,0,0,false,"Seni: The OLD MOUNTAINS?!!!"),
            new TextBoxText(8,7,10,0,0,false,"LittleTurtle: Yes, across the old mountains. There lies an ancient jungle where Zingiber grows."),
            new TextBoxText(10,8,11,0,0,false,"LittleTurtle: You will have to take the dangerous path through the mountains."),
            new TextBoxText(11,10,12,0,0,false,"LittleTurtle: It leads through dark holes, high snowy mountain tops, hot deserts and around a big lake."),
            new TextBoxText(12,11,13,0,0,false,"LittleTurtle: Anyways, go already! We don't have time and you are just standing around rooted like a tree."),
            new TextBoxText(13,12,14,0,0,false,"LittleTurtle: You know the way to the entrance, it is right behind me up the hill."),
            new TextBoxText(14,13,15,0,0,true,"Seni: ......"),
            new TextBoxText(15,14,16,0,0,false,"Seni: This is the entrance to the path......."),
            new TextBoxText(16,15,17,0,0,false,"Seni: Ohhhhh noooo..."),
            new TextBoxText(17,16,18,0,0,false,"Seni: Sighhhhhh...."),
            new TextBoxText(18,17,19,0,0,false,"Seni: But I can't go back empty handed"),
            new TextBoxText(19,18,20,0,0,true,"Seni: .........................!"),
            new TextBoxText(20,19,21,0,0,false,"Seni: AAAAAAAAAAAAHHHHHHHHH"),
            new TextBoxText(21,20,22,0,0,true,"Seni: WHAT WAS THAT!!!!!!\nWhy did I agree to this"),
        };

        public static TextBoxSelection[] text_box_selections =
        {
            /*new TextBoxSelection(1,1,21,"Follow Music",false),
            new TextBoxSelection(1,1,22,"Go Back",true),
            new TextBoxSelection(1,1,22,"Do nothing....",false,new GameEvent(DYING_EVENT_TAG, default(System.DateTime), false, true))*/
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