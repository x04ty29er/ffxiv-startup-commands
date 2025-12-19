namespace FfxivStartupCommands
{
    using System;
    using FFXIVClientStructs.FFXIV.Client.System.String;
    using FFXIVClientStructs.FFXIV.Client.UI;
    using FFXIVClientStructs.FFXIV.Component.GUI;


    public class GameClient
    {
        private readonly GetUiModuleDelegate getUI;
        private readonly IntPtr uiModulePointer;

        private delegate void GetChatBoxModuleDelegate(IntPtr uiModule, IntPtr message, IntPtr unused, byte a4);
        private delegate IntPtr GetUiModuleDelegate(IntPtr basePtr);

        
        public GameClient()
        {
            if (Services.PluginInterface == null)
                return;
        }


        /// <summary>
        /// Returns whether the Chat window is visible (and thus interactable). 
        /// </summary>
        public unsafe bool GetChatVisible()
        {
            try
            {
                AtkUnitBase* chatLog = (AtkUnitBase*)Services.GameGui.GetAddonByName("ChatLog", 1);

                if (chatLog != null)
                    return chatLog->IsVisible;
            }
            catch (Exception)
            {
                // Something went wrong getting the chat log information
            }

            return false;
        }
        
        
        /// <summary>
        /// Convenience function to change active chat channel.
        /// </summary>
        public void ChangeChatChannel()
        {
            if (Plugin.Configuration.DefaultChatChannel == ChatChannel.None)
                return;
            
            Plugin.GameClient.SubmitToChat(Plugin.Configuration.DefaultChatChannel.ToCommand());
        }


        /// <summary>
        /// Submit text/command to outgoing chat.
        /// Can be used to enter chat commands.
        /// </summary>
        /// <param name="text">Text to submit.</param>
        public unsafe void SubmitToChat(string text)
        {
            var uiModule = UIModule.Instance();
            using (Utf8String convertedCommand = new(text))
            {
                Utf8String* textPointer = &convertedCommand;
                uiModule->ProcessChatBoxEntry(textPointer, IntPtr.Zero, false);
            }
        }
    }

    
}