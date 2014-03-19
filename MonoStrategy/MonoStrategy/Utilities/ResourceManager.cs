using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace MonoStrategy.Utility
{
    public class ResourceManager
    {
        private ContentManager content;
        private GraphicsDevice graphics;

        //Effects
        private Effect regulareffect, quadeffect;

        //Model
        private Dictionary<string, Texture2D> textures;
        private Dictionary<String, Model> models;

        #region Getters & setters
        public Effect Quadeffect
        {
            get { return quadeffect; }
            set { quadeffect = value; }
        }

        public Effect Regulareffect
        {
            get { return regulareffect; }
            set { regulareffect = value; }
        }
        #endregion

        public ResourceManager(ContentManager content, GraphicsDevice graphics) 
        {
            this.content = content;
            textures = new Dictionary<string, Texture2D>();
            models = new Dictionary<string, Model>();
            this.graphics = graphics;

            //Load effects
            BinaryReader Reader = new BinaryReader(File.Open("Effects\\RegularEffect.mgfxo", FileMode.Open));
            regulareffect = new Effect(graphics, Reader.ReadBytes((int)Reader.BaseStream.Length)); 
        }

        public ContentManager Content 
        {
            get
            {
                return content;
            }
            private set
            {
                content = value;
            }
        }

        public void Update(float elapsedTime)
        {
        }

        public Effect GetEffect(string effect)
        {
            Effect e;
            switch(effect)
            {
                case "regulareffect":
                    e = regulareffect;
                    break;
                case "quadeffect":
                    e = quadeffect;
                    break;
                default:
                    e = null;
                    break;
            }
            return e;
        }

        public Texture2D GetTexture(string texture)
        {
            if (texture == "")
                return null;

            if (!textures.ContainsKey(texture))
            {   
                if(Content.Load<Texture2D>(texture) != null)
                    textures.Add(texture, Content.Load<Texture2D>(texture));
            }
            return textures[texture];
        }

        public int CountFrames(Texture2D texture, Vector2 frameSize)
        {
            int xCount, yCount;
            yCount = texture.Height / (int)frameSize.Y + ((int)frameSize.Y % texture.Height == 0 ? 0 : 1);
            xCount = texture.Width / (int)frameSize.X + ((int)frameSize.X % texture.Width == 0 ? 0 : 1);
            return xCount + yCount;
        }

        //Splits up a spritesheet into frames
        public Texture2D[] Split(Texture2D original, int partWidth, int partHeight, out int numberOfFrames)
        {
            int xCount, yCount;
            yCount = original.Height / partHeight;// + (partHeight % original.Height == 0 ? 0 : 1); //The number of textures in each vertical column
            xCount = original.Width / partWidth; // +(partWidth % original.Width == 0 ? 0 : 1); //The number of textures in each horizontal row
            numberOfFrames = xCount*yCount;
            Texture2D[] r = new Texture2D[numberOfFrames];//Number of parts
            int dataPerPart = partWidth * partHeight;//Number of pixels in each of the split parts

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData<Color>(originalData);

            int index = 0;
            for (int y = 0; y < yCount * partHeight; y += partHeight)
                for (int x = 0; x < xCount * partWidth; x += partWidth)
                {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    //The data for part
                    Color[] partData = new Color[dataPerPart];

                    //Fill the part data with colors from the original texture
                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++)
                        {
                            int partIndex = px + py * partWidth;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + py >= original.Height || x + px >= original.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                        }

                    //Fill the part with the extracted data
                    part.SetData<Color>(partData);
                    //Stick the part in the return array:                    
                    r[index++] = part;
                }
            //Return the array of parts.
            return r;
        }

        public Model GetModel(String model)
        {
            if (!models.ContainsKey(model))
            {
                if (Content.Load<Model>(model) != null)
                    models.Add(model, Content.Load<Model>(model));
            }
            return models[model];
        }

        internal SpriteFont GetSpriteFont(string p)
        {
            return content.Load<SpriteFont>(p);
        }
    }
}
