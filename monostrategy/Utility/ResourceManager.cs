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


namespace monostrategy.Utility
{
    public class ResourceManager
    {
        private static ResourceManager instance;
        private static ContentManager content;

        //Effects
        private Effect regulareffect, quadeffect;

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

        //Model
        private Dictionary<string, Texture2D> textures;
        private Dictionary<String, Model> models;

        //Audio
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;

        private ResourceManager() {}

        public static ResourceManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ResourceManager();
                }
             return instance;
            }
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
            audioEngine.Update();
        }

        public void PlaySound(String soundEffect, Camera camera, Vector3 pos, bool blocked)
        {
            Cue cue = soundBank.GetCue(soundEffect);
            AudioListener listener = new AudioListener();
            listener.Position = camera.Position;
            listener.Forward = camera.GetForward();
            listener.Up = Vector3.Up;

            int iblocked;
            if(blocked)
                iblocked = 1;
            else
                iblocked = 0;
            AudioEmitter emitter = new AudioEmitter();
            emitter.Up = Vector3.Up;
            emitter.Forward = Vector3.Normalize(camera.Position - pos);
            emitter.Position = pos;
            cue.SetVariable("Distance", (Math.Min((camera.Position - pos).Length(), 10000)));
            cue.SetVariable("Blocked", iblocked);
            cue.Apply3D(listener, emitter);
            cue.Play();
        }

        public static void Initialize(ContentManager c)
        {
            Instance.Content = c;
            Instance.textures = new Dictionary<string, Texture2D>();
            Instance.models = new Dictionary<string, Model>();
            Instance.quadeffect = LoadEffect("Effects/QuadEffect");
            Instance.regulareffect = LoadEffect("Effects/Regulareffect");
        }

        public static Effect LoadEffect(string effect)
        {
            return Instance.Content.Load<Effect>(effect);
        }

        public static Texture2D LoadTexture(string texture)
        {
            if (texture == "")
                return null;

            if (!Instance.textures.ContainsKey(texture))
            {   
                if(Instance.Content.Load<Texture2D>(texture) != null)
                    Instance.textures.Add(texture, Instance.Content.Load<Texture2D>(texture));
            }
            return Instance.textures[texture];
        }

        public static int CountFrames(Texture2D texture, Vector2 frameSize)
        {
            int xCount, yCount;
            yCount = texture.Height / (int)frameSize.Y + ((int)frameSize.Y % texture.Height == 0 ? 0 : 1);
            xCount = texture.Width / (int)frameSize.X + ((int)frameSize.X % texture.Width == 0 ? 0 : 1);
            return xCount + yCount;
        }

        //Splits up a spritesheet into frames
        public static Texture2D[] Split(Texture2D original, int partWidth, int partHeight, out int numberOfFrames)
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

        public static Model LoadModel(String model)
        {
            if (!Instance.models.ContainsKey(model))
            {
                if (Instance.Content.Load<Model>(model) != null)
                    Instance.models.Add(model, Instance.Content.Load<Model>(model));
            }
            return Instance.models[model];
        }
    }
}
