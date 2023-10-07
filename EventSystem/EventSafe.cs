using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class EventSafe : MonoBehaviour
{
    public static List<GameEvent> all_events = new List<GameEvent>();

    public static List<GameEvent> loop_events = new List<GameEvent>();
    public static List<GameEvent> controler_events = new List<GameEvent>();
    public static List<GameEvent> player_events = new List<GameEvent>();
    public static List<GameEvent> sound_events = new List<GameEvent>();

    public static Dictionary<string,ushort> dict_loop_events = new Dictionary<string, ushort>()
    {
        {"testeventloop",1}
    };
    public static Dictionary<string, ushort> dict_player_events = new Dictionary<string, ushort>()
    {
        {"gained_xp", GWCon.XP_EVENT_TAG},
    };
    public static Dictionary<string, ushort> dict_controler_events = new Dictionary<string, ushort>()
    { };
    public static Dictionary<string, ushort> dict_player_events = new Dictionary<string, ushort>()
    { };
    public static Dictionary<string, ushort> dict_sound_events = new Dictionary<string, ushort>()
    { };

    public static GameInfo game_info = new GameInfo();

    void Awake()
    {
        game_info.lifes = 1;
        game_info.xp = 1;
        game_info.volume = 8;
        game_info.language = 0;
        loadCache();
        GWEventManager.xp_event += appendList;
        GWEventManager.settings_event += appendList;
        GWEventManager.scene_event += appendList;
    }

    private void loadCache()
    {
        // if file exists load from file, else create file and write empty object into it
        if(File.Exists(Application.persistentDataPath + "/game_info.gameinfo"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/game_info.gameinfo", FileMode.Open);
            try
            {
                game_info = (GameInfo)bf.Deserialize(file);
            }
            catch(SerializationException e)
            {
                Debug.Log(e.Message);
                file.Close();
            }
            loadValues();
        }
        else
        {
            //TODO build in warning for creating a new game save
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = null;
            try
            {
                file = File.Create(Application.persistentDataPath + "/game_info.gameinfo");
            }
            catch(IOException e)
            {
                Debug.Log(e.Message);
                File.Delete(Application.persistentDataPath + "/game_info.gameinfo");
                file = File.Create(Application.persistentDataPath + "/game_info.gameinfo");
            }
            if(file != null)
            {
                bf.Serialize(file, game_info);
                file.Close();
            }
        }
        if (File.Exists(Application.persistentDataPath + "/all_events.events"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/all_events.events", FileMode.Open);
            try
            {
                all_events = (List<GameEvent>)bf.Deserialize(file);
            }
            catch (SerializationException e)
            {
                Debug.Log(e.Message);
                file.Close();
            }
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = null;
            try
            {
                file = File.Create(Application.persistentDataPath + "/all_events.events");
            }
            catch (IOException e)
            {
                Debug.Log(e.Message);
                File.Delete(Application.persistentDataPath + "/all_events.events");
                file = File.Create(Application.persistentDataPath + "/all_events.events");
            }
            if (file != null)
            {
                bf.Serialize(file, all_events);
                file.Close();
            }
        }
    }

    private void loadValues()
    {
        // update state manager values
        GWStateManager.lives = (ushort)GWGameManager.livesPerXp(game_info.xp);
        GWStateManager.start_lives = (ushort)GWGameManager.livesPerXp(game_info.xp);
        GWStateManager.xp = game_info.xp;
        GWStateManager.volume = game_info.volume;
        GWStateManager.language = game_info.language;
    }

    private static void updateCache(GameEvent new_game_event, GameInfo new_game_info = null)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = null;
        if (new_game_info != null)
        {
            try
            {
                file = File.Create(Application.persistentDataPath + "/game_info.gameinfo");
            }
            catch(IOException e)
            {
                Debug.Log(e);
                file.Close();
                return;
            }
            bf.Serialize(file, game_info);
            file.Close();
        }
        if(new_game_event != null)
        {
            // first, add to all_events safe
            List<GameEvent> new_all_events = new List<GameEvent>();
            file = File.Open(Application.persistentDataPath + "/all_events.events", FileMode.Open);
            try
            {
                new_all_events = (List<GameEvent>)bf.Deserialize(file);
            }
            catch (SerializationException e)
            {
                Debug.Log(e.Message);
                file.Close();
                return;
            }
            new_all_events.Add(new_game_event);
            try
            {
                file = File.Create(Application.persistentDataPath + "/all_events.events");
            }
            catch (IOException e)
            {
                Debug.Log(e);
                file.Close();
                return;
            }
            bf.Serialize(file, new_all_events);
            file.Close();

            // then add to resptective event safe
            if(dict_loop_events.ContainsValue(new_game_event.tag)){
                List<GameEvent> new_loop_events = new List<GameEvent>();
                file = File.Open(Application.persistentDataPath + "/loop_events.events", FileMode.Open);
                try
                {
                    new_loop_events = (List<GameEvent>)bf.Deserialize(file);
                }
                catch (SerializationException e)
                {
                    Debug.Log(e.Message);
                    file.Close();
                    return;
                }
                new_loop_events.Add(new_game_event);
                try
                {
                    file = File.Create(Application.persistentDataPath + "/loop_events.events");
                }
                catch (IOException e)
                {
                    Debug.Log(e);
                    file.Close();
                    return;
                }
                bf.Serialize(file, new_loop_events);
                file.Close();
            }else if (dict_controler_events.ContainsValue(new_game_event.tag))
            {
                List<GameEvent> new_controler_events = new List<GameEvent>();
                file = File.Open(Application.persistentDataPath + "/controler_events.events", FileMode.Open);
                try
                {
                    new_controler_events = (List<GameEvent>)bf.Deserialize(file);
                }
                catch (SerializationException e)
                {
                    Debug.Log(e.Message);
                    file.Close();
                    return;
                }
                new_controler_events.Add(new_game_event);
                try
                {
                    file = File.Create(Application.persistentDataPath + "/controler_events.events");
                }
                catch (IOException e)
                {
                    Debug.Log(e);
                    file.Close();
                    return;
                }
                bf.Serialize(file, new_controler_events);
                file.Close();
            }
            else if (dict_player_events.ContainsValue(new_game_event.tag))
            {
                List<GameEvent> new_player_events = new List<GameEvent>();
                file = File.Open(Application.persistentDataPath + "/player_events.events", FileMode.Open);
                try
                {
                    new_player_events = (List<GameEvent>)bf.Deserialize(file);
                }
                catch (SerializationException e)
                {
                    Debug.Log(e.Message);
                    file.Close();
                    return;
                }
                new_player_events.Add(new_game_event);
                try
                {
                    file = File.Create(Application.persistentDataPath + "/player_events.events");
                }
                catch (IOException e)
                {
                    Debug.Log(e);
                    file.Close();
                    return;
                }
                bf.Serialize(file, new_player_events);
                file.Close();
            }else
            {
                Debug.Log("Event doesnt exist yet in Dictionary");
            }
        }
    }

    public static void appendList(GameEvent new_game_event)
    {
        // add event to respective 
        new_game_event.order_id = all_events.Count;
        all_events.Add(new_game_event);
        if (dict_loop_events.ContainsValue(new_game_event.tag))
        {
            new_game_event.order_id = loop_events.Count;
            loop_events.Add(new_game_event);
        }
        else if (dict_controler_events.ContainsValue(new_game_event.tag))
        {
            new_game_event.order_id = controler_events.Count;
            controler_events.Add(new_game_event);
        }
        else if (dict_player_events.ContainsValue(new_game_event.tag))
        {
            new_game_event.order_id = player_events.Count;
            player_events.Add(new_game_event);
            switch (new_game_event.tag)
            {
                case 19:
                    update_game_info = true;
                    game_info.xp += (uint)new_game_event.value;
                    game_info.lifes = (ushort)GWGameManager.livesPerXp(game_info.xp);
                    break;
            }
        }
        else if (dict_sound_events.ContainsValue(new_game_event.tag))
        {
            new_game_event.order_id = sound_events.Count;
            sound_events.Add(new_game_event);
        }
        else if (dict_settng_events.ContainsValue(new_game_event.tag))
        {
            switch (new_game_event.tag)
            {
                case 20:
                    update_game_info = true;
                    switch(((SettingsEventValue)new_game_event.value).setting_name)
                    {
                        case "AdjustVolume":
                            game_info.volume = GWStateManager.volume;
                            break;
                        case "ChangeLanguage":
                            game_info.language = GWStateManager.language;
                            break;
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("event couldn't be added to the event safe! group " + new_game_event.tag + " unknown!");
        }

        // update safe file if bool update_cache set
        if (new_game_event.update_cache)
        {
            updateCache(new_game_event);
        }
    }
}
