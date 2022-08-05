using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AshTechEngine
{
    public static class Fonts
    {
        private static Dictionary<string, FontSystem> fontSystems;
        private static Dictionary<string, SpriteFontBase> bitmapFonts;


        public enum Alignment
        {
            TopLeft,
            TopCenter,
            TopRight,

            CenterLeft,
            CenterCenter,
            CenterRight,

            BottomLeft,
            BottomCenter,
            BottomRight,
        }

        static Fonts()
        {
            fontSystems = new Dictionary<string, FontSystem>();
            bitmapFonts = new Dictionary<string, SpriteFontBase>();
        }

        public static void AddBitmapFont(GraphicsDevice graphicsDevice, string fontName, string fontPath)
        {
            var fntData = File.ReadAllText(fontPath);
            SpriteFontBase font = StaticSpriteFont.FromBMFont(fntData, fileName => File.OpenRead(fileName), graphicsDevice);

            bitmapFonts.Add(fontName,font);
        }        

        public static void AddFont(string fontName, params string[] fontPath)
        {
            FontSystemSettings fontSettings = new FontSystemSettings
            {
                KernelWidth = 2,
                KernelHeight = 2,
                FontResolutionFactor = 2,
            };

            var fontSystem = new FontSystem(fontSettings);

            foreach (string path in fontPath)
                fontSystem.AddFont(File.ReadAllBytes(path));

            fontSystems.Add(fontName, fontSystem);
        }

        private static SpriteFontBase GetSpriteFontBase(string fontName, int fontSize)
        {
            if (fontSystems.TryGetValue(fontName, out var fontSystem))
            {
                return fontSystem.GetFont(fontSize);
            }
            else if(bitmapFonts.TryGetValue(fontName, out var bitmapFont))
            {
                return bitmapFont;
            }

            return null;
        }

        public static Vector2 MeasureString(string fontName, int fontSize, string text, Vector2? scale = null, float characterSpacing = 0, float lineSpacing = 0)
        {
            SpriteFontBase font = GetSpriteFontBase(fontName, fontSize);
            return MeasureString(font, text, scale, characterSpacing, lineSpacing);
        }
        public static Vector2 MeasureString(SpriteFontBase font, string text, Vector2? scale = null, float characterSpacing = 0, float lineSpacing = 0)
        {
            Vector2 stringSize = font.MeasureString(text, scale, characterSpacing, lineSpacing);
            return stringSize;
        }

        public static void DrawString(SpriteBatch spriteBatch, string fontName, int fontSize, string text, Vector2 position, Color color,
                                      Vector2? scale = null, float rotation = 0, Vector2 origin = default, float layerDepth = 0, float characterSpacing = 0, float lineSpacing = 0)
        {
            DrawString(spriteBatch, fontName, fontSize, text, position, new Color[]{color}, scale, rotation, origin, layerDepth, characterSpacing, lineSpacing);
        }
        public static void DrawString(SpriteBatch spriteBatch, string fontName, int fontSize, string text, Vector2 position, Color[] colors,
                                      Vector2? scale = null, float rotation = 0, Vector2 origin = default, float layerDepth = 0, float characterSpacing = 0, float lineSpacing = 0)
        {
            SpriteFontBase font = GetSpriteFontBase(fontName, fontSize);
            spriteBatch.DrawString(font, text, position, colors, scale, rotation, origin, layerDepth, characterSpacing, lineSpacing );            
        }

        public static void DrawString(SpriteBatch spriteBatch, string fontName, int fontSize, string text, Rectangle rectangle, Alignment alignment, Color color,
                                      Vector2? scale = null, float rotation = 0, float layerDepth = 0, float characterSpacing = 0, float lineSpacing = 0)
        {
            DrawString(spriteBatch, fontName, fontSize, text, rectangle, alignment, new Color[] { color }, scale, rotation, layerDepth, characterSpacing, lineSpacing);
        }
        public static void DrawString(SpriteBatch spriteBatch, string fontName, int fontSize, string text, Rectangle rectangle, Alignment alignment, Color[] colors,
                                      Vector2? scale = null, float rotation = 0, float layerDepth = 0, float characterSpacing = 0, float lineSpacing = 0)
        {
            
            SpriteFontBase font = GetSpriteFontBase(fontName, fontSize);
            Vector2 stringSize = font.MeasureString(text, scale, characterSpacing, lineSpacing);

            if(alignment == Alignment.TopLeft)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.X, rectangle.Y), colors, origin: new Vector2(0,0), scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing);
            }
            if (alignment == Alignment.TopCenter)
            {
                spriteBatch.DrawString(font, text, new Vector2((int)rectangle.Width/2 + rectangle.X, rectangle.Y), colors, origin: new Vector2((int)stringSize.X/2, 0), scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing);
            }
            if (alignment == Alignment.TopRight)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.Width + rectangle.X, rectangle.Y), colors, origin: new Vector2((int)stringSize.X, 0), scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing);
            }
            if(alignment == Alignment.CenterLeft)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.X, rectangle.Height/2 + rectangle.Y), colors, origin: new Vector2(0, (int)stringSize.Y / 2), scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing);
            }
            if (alignment == Alignment.CenterCenter)
            {
                spriteBatch.DrawString(font, text, rectangle.Center.ToVector2(), colors, origin: new Vector2((int)stringSize.X / 2, (int)stringSize.Y / 2), scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing);
            }
            if (alignment == Alignment.CenterRight)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.Width + rectangle.X, rectangle.Height / 2 + rectangle.Y), colors, origin: new Vector2(stringSize.X, (int)stringSize.Y / 2), scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing);
            }
            if (alignment == Alignment.BottomLeft)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.X, rectangle.Height + rectangle.Y), colors, origin: new Vector2(0, (int)stringSize.Y), scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing);
            }
            if (alignment == Alignment.BottomCenter)
            {
                spriteBatch.DrawString(font, text, new Vector2((int)rectangle.Width / 2 + rectangle.X, rectangle.Height + rectangle.Y), colors, origin: new Vector2((int)stringSize.X / 2, stringSize.Y), scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing);
            }
            if (alignment == Alignment.BottomRight)
            {
                spriteBatch.DrawString(font, text, new Vector2((int)rectangle.Width + rectangle.X, rectangle.Height + rectangle.Y), colors, origin: new Vector2((int)stringSize.X, stringSize.Y), scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing);
            }
            
        }
    }
}
