using Godot;
using System;
using Steamworks;




public class steamManager : Node2D
{
    public override void _Ready()
    {
        //Sanity Check
        if (!Packsize.Test())
            GD.Print("[STEAMWORKS.NET] The Packsize test returned false, the wrong version of Steam is being run on this platform.");
        if(!DllCheck.Test())
            GD.Print("[STEAMWORKS.NET] The DllCheck test returned false, one or more of the Steamworks binaries seem to be the wrong version.");
        
        //Check if application is being run through the Steam Client
        try 
        {
            if(SteamAPI.RestartAppIfNecessary((AppId_t)480))
            {
                GD.Print("Restarting through Steam...");
                GetTree().Quit();
            }
        } 
        catch (System.DllNotFoundException e)
        {
            GD.Print("[STEAMWORKS.NET] Could not load [lib]steam_api.dll/so/dylib. Its likely not in the correct location. \n" + e);
        }

        // Try to initialize steam
        if(SteamAPI.Init())
        {
            GD.Print(SteamFriends.GetPersonaName());
        }
        else
        {
            GD.Print("Failed to initialize Steam. Please make sure that Steam client is open.");
        }

   }

    public override void _Process(float delta)
    {
       //Run callbacks
       SteamAPI.RunCallbacks();
    }

}
