using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Defend_Your_Castle
{
    // Contains methods for saving/loading player data
    public static class Data
    {
        // The current application data, including the version number of the app data store
        public static ApplicationData AppVersionData;

        // The version number of the app - must be a uint because of the app data storage objects
        public static uint VersionNum = 1;

        static Data()
        {
            // Get the current ApplicationData object
            AppVersionData = ApplicationData.Current;

            // Check if the data in the app data store needs to be updated
            CheckAppDataVersion();
        }

        private static async void CheckAppDataVersion()
        {
            // Check if the current version of the app data store is not up to date
            if (AppVersionData.Version < VersionNum)
            {
                // Set the new version number of the app data store
                await AppVersionData.SetVersionAsync(VersionNum, new ApplicationDataSetVersionHandler(SetVersionHandler));
            }
        }

        // ApplicationDataSetVersionHandler used to change the version number of the app data store
        private static void SetVersionHandler(SetVersionRequest request)
        {
            // Delay the setting of the app data's version number until all of the old data has been converted to the new data format
            SetVersionDeferral deferral = request.GetDeferral();

            // Perform any data conversions here

            // Set the app data version number
            deferral.Complete();
        }

        //Gets all the player helpers and returns them in HelperData form
        public static List<HelperData> GetPlayerHelpers(Player player)
        {
            List<HelperData> Helpers = new List<HelperData>();

            //For now check the type of the player's children
            for (int i = 0; i < player.GetChildren.Count; i++)
            {
                PlayerHelper helper = player.GetChildren[i] as PlayerHelper;
                //Conver the helper to a HelperData so we can store its values
                if (helper != null)
                {
                    Helpers.Add(helper.ConvertHelperToData());
                }
            }

            return Helpers;
        }

        public static async void SavePlayer(Player player)
        {
            // Create a new PlayerData object from the player
            PlayerData playerData = new PlayerData(player.GetHealth, player.GetMaxHealth, player.GetCastleLevel, player.Gold, player.HasInvincibility, GetPlayerHelpers(player));
            //playerData.SetHelpers(GetPlayerHelpers(player));

            // Create a new file for the player's save data, overwriting any existing file
            StorageFile file = await AppVersionData.LocalFolder.CreateFileAsync("player.sav", CreationCollisionOption.ReplaceExisting);

            // Open a Stream to write to the file
            using (Stream stream = await file.OpenStreamForWriteAsync())
            {
                // Create a new XmlDictionaryWriter to write to the stream
                using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateBinaryWriter(stream))
                {
                    // Initialize a new DataContractSerializer
                    DataContractSerializer serializer = new DataContractSerializer(typeof(PlayerData));

                    // Serialize the PlayerData class instance into the .sav file
                    serializer.WriteObject(writer, playerData);

                    // Flush the buffer to the file
                    writer.Flush();

                    // Dispose of all resources used by the XmlDictionaryWriter
                    writer.Dispose();
                }
            }
        }

        public static async Task<Player> LoadPlayer(GamePage page)
        {
            // Try to get the player file from its stored location
            IStorageItem storageItem = await AppVersionData.LocalFolder.TryGetItemAsync("player.sav");

            // Check if the file was found. If it wasn't, return a new player
            if (storageItem == null) return (new Player(page));

            // Convert the IStorageItem object to a StorageFile
            StorageFile file = (StorageFile)storageItem;

            // Stores the Player data object
            PlayerData playerData;

            // Open a Stream to read the contents of the StorageFile
            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                // Create a new XmlDictionaryReader to write to the stream
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
                {
                    // Initialize a new DataContractSerializer
                    DataContractSerializer serializer = new DataContractSerializer(typeof(PlayerData));

                    // Try and Catch the deserialization process
                    try
                    {
                        // Deserialize the PlayerData object
                        playerData = (PlayerData)serializer.ReadObject(reader);
                    }
                    catch // The data could not be loaded
                    {
                        // Set the PlayerData object to null
                        playerData = null;
                    }

                    // Dispose of all resources used by the XmlDictionaryReader
                    reader.Dispose();
                }
            }

            // Check to make sure player data was found
            if (playerData != null)
            {
                // It was, so create a new player
                Player player = new Player(page);

                // Load the player's data
                player.LoadPlayerData(playerData);

                // Return the player
                return player;
            }
            else // No player data was found
            {
                // Return a new player
                return (new Player(page));
            }
        }
    }

    // Class for storing player data
    [DataContract(Namespace="")]
    public class PlayerData
    {
        [DataMember]
        public int Health;
        [DataMember]
        public int MaxHealth;
        [DataMember]
        public int CastleLevel;
        [DataMember]
        public int Gold;
        [DataMember]
        public bool Invincibility;

        [DataMember]
        public List<HelperData> Helpers;

        public PlayerData(int health, int maxHealth, int castleLevel, int gold, bool invincibility, List<HelperData> helpers)
        {
            Health = health;
            MaxHealth = maxHealth;
            CastleLevel = castleLevel;
            Gold = gold;

            Invincibility = invincibility;
            Helpers = helpers;
        }

        //I'm not sure if you prefer a separate method like this or to just have it all in the constructor
        //public void SetHelpers(List<HelperData> helpers)
        //{
        //    Helpers = helpers;
        //}
    }

    //Data for the player's helpers
    [DataContract(Namespace="")]
    [KnownType("GetKnownTypes")]
    public abstract class HelperData
    {
        [DataMember]
        public int Level;

        public HelperData()
        {

        }

        //Creates the appropriate player helper when loading in the HelperData
        public abstract PlayerHelper CreateHelper();

        //The helper types
        private static Type[] GetKnownTypes()
        {
            return new Type[] { typeof(ArcherData) };
        }
    }

    [DataContract(Namespace="")]
    public class ArcherData : HelperData
    {
        public ArcherData(int currentlevel)
        {
            Level = currentlevel;
        }

        public override PlayerHelper CreateHelper()
        {
            Archer playerarcher = new Archer();
            
            //Level up the archer until its at the appropriate level; we will definitely have a SetLevel method later to avoid the unnecessary loop
            for (int i = 0; i < Level; i++)
                playerarcher.IncreaseLevel();

            return playerarcher;
        }
    }

    //Class for storing shop data; we need to set the prices of items to their correct values based on how much they were leveled up
    [DataContract(Namespace="")]
    public class ShopData
    {
        public int CastleLevel;

        public ShopData()
        {

        }
    }
}
