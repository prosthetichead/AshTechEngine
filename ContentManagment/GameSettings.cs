using AshTechEngine.ScreenDisplay;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AshTechEngine.ContentManagment
{
    public class Config
    {
        public int horizontalResolution { get; set; }
        public int verticalResolution { get; set; }
        public bool fullScreen { get; set; }
        public bool allowResize { get; set; }

        public Config()
        {
            horizontalResolution = 1920;
            verticalResolution = 1080;
            fullScreen = false;
            allowResize = false;
        }
    }

    public class GameSettings
    {
        public Config config;
        public Config previousConfig;
        private ScreenManager screenManager;
        public GameSettings(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            LoadConfig();
        }

        /// <summary>
        /// Read the config from a local Config.json if one exisits
        /// once loaded Apply the config file to the game object.
        /// </summary>
        public void LoadConfig()
        {
            //try load the config file
            if (System.IO.File.Exists("ashtech.config"))
            {
                //file exits so read it and deserialize into the config object
                string configJSON = System.IO.File.ReadAllText("ashtech.config");
                config = JsonConvert.DeserializeObject<Config>(configJSON);
            }
            else
            {
                //no file exists yet.
                config = new Config();
                SaveConfig();
            }
            previousConfig = config;

            ApplyConfig();
        }

        public void SaveConfig()
        {
            string configJSON = JsonConvert.SerializeObject(config, Formatting.Indented);
            System.IO.File.WriteAllText("ashtech.config", configJSON); //Need If Android handeling

        }

        /// <summary>
        /// Apply any settings to the Game 
        /// </summary>
        public void ApplyConfig()
        {
            screenManager.Graphics.ApplyChanges();
            screenManager.Game.Window.AllowUserResizing = config.allowResize;
            screenManager.Graphics.PreferredBackBufferWidth = config.horizontalResolution;
            screenManager.Graphics.PreferredBackBufferHeight = config.verticalResolution;
            screenManager.Graphics.IsFullScreen = config.fullScreen;
            screenManager.Graphics.ApplyChanges();
            previousConfig = config;
        }

        public string resolutionText
        {
            get
            {
                return config.horizontalResolution + "x" + config.verticalResolution;
            }
            set
            {
                string[] res = value.Split('x');
                //1920x1080
                config.horizontalResolution = Convert.ToInt32(res[0]);
                config.verticalResolution = Convert.ToInt32(res[1]);

            }
        }
    }
}
