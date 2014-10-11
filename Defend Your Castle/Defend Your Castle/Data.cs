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
                //Convert the helper to a HelperData so we can store its values
                if (helper != null)
                {
                    Helpers.Add(helper.ConvertHelperToData());
                }
            }

            return Helpers;
        }

        // Saves the game data
        public static async void SaveGameData(Shop shop, Level level)
        {
            // Create a new PlayerData object from the player
            PlayerData playerData = new PlayerData(level.GetPlayer.GetHealth, level.GetPlayer.GetMaxHealth, level.GetPlayer.GetCastleLevel,
                                                   level.GetPlayer.Gold, level.GetPlayer.HasInvincibility, GetPlayerHelpers(level.GetPlayer));
            //playerData.SetHelpers(GetPlayerHelpers(player));

            // Create a new ShopData object from the shop
            ShopData shopData = new ShopData(shop.ShopUpgrades, shop.ShopPrepareRepairs, shop.ShopItems);

            // Create a new LevelData object from the level
            LevelData levelData = new LevelData(level.GetLevelNum);

            // Create a new GameData object
            GameData gameData = new GameData(playerData, shopData, levelData);
            
            // Create a new file for the game's save data, overwriting any existing file
            StorageFile file = await AppVersionData.LocalFolder.CreateFileAsync("game.sav", CreationCollisionOption.ReplaceExisting);

            // Open a Stream to write to the file
            using (Stream stream = await file.OpenStreamForWriteAsync())
            {
                // Create a new XmlDictionaryWriter to write to the stream
                using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateBinaryWriter(stream))
                {
                    // Initialize a new DataContractSerializer
                    DataContractSerializer serializer = new DataContractSerializer(typeof(GameData));

                    // Serialize the GameData class instance into the .sav file
                    serializer.WriteObject(writer, gameData);

                    // Flush the buffer to the file
                    writer.Flush();

                    // Dispose of all resources used by the XmlDictionaryWriter
                    writer.Dispose();
                }
            }
        }

        // Loads the game's data. Returns an object array containing the Shop and Level, in that order
        public static async Task<object[]> LoadGameData(GamePage page, Game1 game)
        {
            // Try to get the player file from its stored location
            IStorageItem storageItem = await AppVersionData.LocalFolder.TryGetItemAsync("game.sav");
            
            // Check if the file was found
            if (storageItem == null)
            {
                goto NoData;
            }

            // Convert the IStorageItem object to a StorageFile
            StorageFile file = (StorageFile)storageItem;

            // Stores the GameData object
            GameData gameData;

            // Open a Stream to read the contents of the StorageFile
            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                // Create a new XmlDictionaryReader to write to the stream
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
                {
                    // Initialize a new DataContractSerializer
                    DataContractSerializer serializer = new DataContractSerializer(typeof(GameData));

                    // Try and Catch the deserialization process
                    try
                    {
                        // Deserialize the GameData object
                        gameData = (GameData)serializer.ReadObject(reader);
                    }
                    catch // The data could not be loaded
                    {
                        // Set the GameData object to null
                        gameData = null;
                    }

                    // Dispose of all resources used by the XmlDictionaryReader
                    reader.Dispose();
                }
            }

            // Check to make sure game data was found
            if (gameData != null)
            {
                // State that the player has saved game data
                Game1.HasSavedData = true;

                // It was, so create a new Player
                Player player = new Player(page);

                // Load the player's data
                player.LoadPlayerData(gameData.playerData);

                // Create a new Shop
                Shop shop = new Shop(page, player);

                // Load the shop data
                shop.LoadShopData(gameData.shopData);
                
                // Create a new Level
                Level level = new Level(player, game);

                // Load the level data
                level.LoadLevelData(gameData.levelData);

                // Return the Shop and the Level since the player is contained in the level
                return (new object[2] { shop, level });
            }
            else // No player data was found
            {
                goto NoData;
            }

            NoData:
                // Create new Player, Shop, and Level objects
                Player thePlayer = new Player(page);
                Shop theShop = new Shop(page, thePlayer);
                Level theLevel = new Level(thePlayer, game);

                // Return the Shop and the Level
                return (new object[2] { theShop, theLevel });
        }
    }

    // Class for storing the game's data
    [DataContract(Namespace="")]
    public class GameData
    {
        [DataMember]
        public PlayerData playerData;

        [DataMember]
        public ShopData shopData;

        [DataMember]
        public LevelData levelData;

        public GameData(PlayerData playerdata, ShopData shopdata, LevelData leveldata)
        {
            playerData = playerdata;
            shopData = shopdata;
            levelData = leveldata;
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
    }

    //Data for the player's helpers
    [DataContract(Namespace="")]
    [KnownType("GetKnownTypes")]
    public abstract class HelperData
    {
        [DataMember]
        public int Level;

        [DataMember]
        public int Index;

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
        public ArcherData(int currentlevel, int index)
        {
            Level = currentlevel;
            Index = index;
        }

        public override PlayerHelper CreateHelper()
        {
            Archer playerarcher = new Archer(Index);
            
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
        [DataMember]
        public List<ShopItemData> ShopUpgrades; // List of available Upgrades

        [DataMember]
        public List<ShopItemData> ShopPrepareRepairs; // List of available Prepare/Repair ShopItems

        [DataMember]
        public List<ShopItemData> ShopItems; // List of available Items

        public ShopData(List<ShopItem> shopUpgrades, List<ShopItem> shopPrepareRepairs, List<ShopItem> shopItems)
        {
            // Initialize the ShopItemData lists
            ShopUpgrades = new List<ShopItemData>();
            ShopPrepareRepairs = new List<ShopItemData>();
            ShopItems = new List<ShopItemData>();

            // Loop through all of the upgrade shop items
            for (int i = 0; i < shopUpgrades.Count; i++)
            {
                // Store the level of each upgrade shop item
                ShopUpgrades.Add(new ShopItemData(shopUpgrades[i].GetCurrentLevel));
            }

            // Loop through all of the prepare/repair shop items
            for (int i = 0; i < shopPrepareRepairs.Count; i++)
            {
                // Store the level of each prepare/repair shop item
                ShopPrepareRepairs.Add(new ShopItemData(shopPrepareRepairs[i].GetCurrentLevel));
            }

            // Loop through all of the one-use shop items
            for (int i = 0; i < shopItems.Count; i++)
            {
                // Store the level of each one-use shop item
                ShopItems.Add(new ShopItemData(shopItems[i].GetCurrentLevel));
            }
        }
    }

    [DataContract(Namespace="")]
    public class ShopItemData
    {
        [DataMember]
        public int CurLevel;

        public ShopItemData(int currentlevel)
        {
            CurLevel = currentlevel;
        }
    }

    [DataContract(Namespace="")]
    public class LevelData
    {
        [DataMember]
        public int LevelNum;

        public LevelData(int levelNum)
        {
            LevelNum = levelNum;
        }
    }


}
