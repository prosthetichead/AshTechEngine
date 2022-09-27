using AshTechEngine.ScreenDisplay;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        public string prettyJSON()
        {
            var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });
            return jsonString;
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

        /// <summary>
        /// Used by the console can take 2 strings and atempts to set the value
        /// Config is applyed but not saved
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SetConfigFromStrings(string name, string value)
        {
            switch (name)
            {
                case "horizontalResolution":
                    if(int.TryParse(value, out int result))
                    {
                        config.horizontalResolution = result;
                        ApplyConfig();
                        return "success";
                    }
                    else
                    {
                        return "error cant convert value to int";
                    }
                case "verticalResolution":
                    if (int.TryParse(value, out int verticalResult))
                    {
                        config.verticalResolution = verticalResult;
                        ApplyConfig();
                        return "success";
                    }
                    else
                    {
                        return "error cant convert value to int";
                    }
                case "fullScreen":
                    if(bool.TryParse(value, out bool fullscreenResult))
                    {
                        config.fullScreen = fullscreenResult;
                        ApplyConfig();
                        return "success";
                    }
                    else
                    {
                        return "error cant convert value to bool";
                    }
                case "allowResize":
                    if (bool.TryParse(value, out bool allowResizeResult))
                    {
                        config.allowResize = allowResizeResult;
                        ApplyConfig();
                        return "success";
                    }
                    else
                    {
                        return "error cant convert value to bool";
                    }
                default:
                    return "config name not found";
            }
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
            screenManager.Graphics.ApplyChanges();
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
