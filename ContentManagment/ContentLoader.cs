using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;


namespace AshTechEngine.ContentManagment
{
    public class ContentLoader
    {

        private Dictionary<string, Texture2D> texture2Ds;
        private GraphicsDevice graphicsDevice;


        public ContentLoader(GraphicsDevice graphicsDevice)
        {
            texture2Ds = new Dictionary<string, Texture2D>();
            this.graphicsDevice = graphicsDevice;
            string[] all = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceNames();          
            
        }

        public Texture2D Texture2DFromFile(string path)
        {
            //check if path is in the dictonary yet.
            if (texture2Ds.ContainsKey(path))
            {
                return texture2Ds[path];
            }
            else
            {
                var texture2D = Texture2D.FromFile(graphicsDevice, path);
                texture2Ds.Add(path, texture2D);
                return texture2D;
            }
        }

        /// <summary>
        /// Loads the bitmap font resources and adds them to the Fonts Class for later use.
        /// The font can be found in the Fonts Class using the provided FontName eg. Fonts.GetSpriteFontBase(fontName) .
        /// </summary>
        /// <param name="rm"></param>
        /// <param name="fontName"></param>
        /// <param name="fontDataName"></param>
        /// <param name="fontTextureName"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public void BitmapFontFromResource(System.Resources.ResourceManager rm, string fontName, string fontDataName, string fontTextureName, Point offset)
        {
            var spriteFontBase = Fonts.GetSpriteFontBase(fontName);
            if (spriteFontBase == null) // font isnt in our font manager yet so lets add it then return it
            {
                var bytes = (byte[])rm.GetObject(fontTextureName);
                var fontTexture = Texture2DFromBytes(bytes);
                spriteFontBase = StaticSpriteFont.FromBMFont(rm.GetString(fontDataName), filenname => new TextureWithOffset(fontTexture, offset));

                Fonts.AddSpriteFontBase(fontName, spriteFontBase);  //add it for next time
            }            
        }

        public Texture2D Texture2DFromResource(System.Resources.ResourceManager rm, string resourceName)
        {
            string fullName = rm.BaseName + "." + resourceName;

            if (texture2Ds.ContainsKey(fullName))
            {
                return texture2Ds[fullName];
            }
            else
            {
                var bytes = (byte[])rm.GetObject(resourceName);
                if (bytes == null)
                {
                    throw new Exception("No Bytes Loaded, Check Resource Name is corrent: " + resourceName);
                }
                var texture2d = Texture2DFromBytes(bytes);
                texture2Ds.Add(fullName, texture2d);
                return texture2d;
            }
        }

        private Texture2D Texture2DFromBytes(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var texture2d = Texture2D.FromStream(graphicsDevice, stream);
                return texture2d;
            }
        }
    }
}
