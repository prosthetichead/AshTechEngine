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

            //add the default console font on load
            var fontSystem = new FontSystem();
            fontSystem.AddFont(File.ReadAllBytes("Content/fonts/pixellocale.ttf"));
            fontSystems.Add("console", fontSystem);
        }

        public static void AddFont(string fontName, params string[] fontPath)
        {
            var fontSystem = new FontSystem();

            foreach (string path in fontPath)
                fontSystem.AddFont(File.ReadAllBytes(path));

            fontSystems.Add(fontName, fontSystem);
        }

        public static void DrawString(SpriteBatch spriteBatch, string fontName, int fontSize, string text, Vector2 position, Color color,
                                      Vector2? scale = null, float rotation = 0, Vector2 origin = default, float layerDepth = 0, float characterSpacing = 0, float lineSpacing = 0)
        {
            DrawString(spriteBatch, fontName, fontSize, text, position, new Color[]{color}, scale, rotation, origin, layerDepth, characterSpacing, lineSpacing);
        }
        public static void DrawString(SpriteBatch spriteBatch, string fontName, int fontSize, string text, Vector2 position, Color[] colors,
                                      Vector2? scale = null, float rotation = 0, Vector2 origin = default, float layerDepth = 0, float characterSpacing = 0, float lineSpacing = 0)
        {
            if(fontSystems.TryGetValue(fontName, out var fontSystem))
            {
                SpriteFontBase font = fontSystem.GetFont(fontSize);
                spriteBatch.DrawString(font, text, position, colors, scale, rotation, origin, layerDepth, characterSpacing, lineSpacing );
            }
        }

        public static void DrawString(SpriteBatch spriteBatch, string fontName, int fontSize, string text, Rectangle rectangle, Alignment alignment, Color color,
                                      Vector2? scale = null, float rotation = 0, float layerDepth = 0, float characterSpacing = 0, float lineSpacing = 0)
        {
            DrawString(spriteBatch, fontName, fontSize, text, rectangle, alignment, new Color[] { color }, scale, rotation, layerDepth, characterSpacing, lineSpacing);
        }
        public static void DrawString(SpriteBatch spriteBatch, string fontName, int fontSize, string text, Rectangle rectangle, Alignment alignment, Color[] colors,
                                      Vector2? scale = null, float rotation = 0, float layerDepth = 0, float characterSpacing = 0, float lineSpacing = 0)
        {
            if (fontSystems.TryGetValue(fontName, out var fontSystem))
            {
                SpriteFontBase font = fontSystem.GetFont(fontSize);
                Vector2 stringSize = font.MeasureString(text);

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

                if (alignment == Alignment.CenterCenter)
                {
                    spriteBatch.DrawString(font, text, rectangle.Center.ToVector2(), colors, origin: new Vector2((int)stringSize.X / 2, (int)stringSize.Y / 2), scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing);
                }
                
            }
        }
    }
}
