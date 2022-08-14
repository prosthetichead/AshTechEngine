using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshTechEngine
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

        public Texture2D Texture2DFromResource(System.Resources.ResourceManager rm,  string resourceName)
        {
            string fullName = rm.BaseName + "." + resourceName;

            if (texture2Ds.ContainsKey(fullName))
            {
                return texture2Ds[fullName];
            }
            else
            {
                var bytes = (byte[])rm.GetObject(resourceName);
                using (var stream = new MemoryStream(bytes))
                {
                    var texture2d = Texture2D.FromStream(graphicsDevice, stream);
                    texture2Ds.Add(fullName, texture2d);
                    return texture2d;
                }
            }
        }
    }
}
