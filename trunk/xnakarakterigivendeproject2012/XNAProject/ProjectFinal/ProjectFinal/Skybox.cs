using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectFinal
{
    class Skybox : DrawableGameComponent
    {
        ///Videreutviklet metode fra Riemers tutorial

        //Variabler
        Model model;
        Texture2D [] texture;
        MainClass game;
        Effect effect;

        //Spill state
        public enum GameState
        {
            MainMenu,
            About,
            Playing,
            Ship,
            Info,
            Keyboard,
        }

        //Konstuktør
        public Skybox(MainClass _game, Effect _effect): base(_game)
        {
            this.game = _game;
            effect = _effect;
        }

        //Metode som laste net model
        public void load(Effect _effect, Model _model)
        {
            model = LoadModel(_model, out texture);
        }

        /// <summary>
        /// Metode fra Riemers tutorial som laster net skybox modelen og setter texturer
        /// </summary>
        /// <param name="_model"></param>
        /// <param name="textures"></param>
        /// <returns></returns>
        private Model LoadModel(Model _model, out Texture2D[] textures)
        {
            Model newModel = _model;
            textures = new Texture2D[newModel.Meshes.Count];
            int i = 0;
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    textures[i++] = currentEffect.Texture;

            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();

            return newModel;
        }

        /// <summary>
        /// Metode fra Riemers tutorial som tegner skybox
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if ((GameState)game.CurrentGameState == GameState.Playing)
            {
                SamplerState ss = new SamplerState();
                ss.AddressU = TextureAddressMode.Clamp;
                ss.AddressV = TextureAddressMode.Clamp;
                this.game.device.SamplerStates[0] = ss;

                DepthStencilState dss = new DepthStencilState();
                dss.DepthBufferEnable = true;

                this.game.device.DepthStencilState = dss;

                Matrix[] skyboxTransforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
                int i = 0;
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        Matrix worldMatrix = Matrix.CreateScale(5000.0f) * skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(this.game.cameraPosition);
                        effect.CurrentTechnique = effect.Techniques["Textured"];
                        effect.Parameters["xWorld"].SetValue(worldMatrix);
                        effect.Parameters["xView"].SetValue(game.view);
                        effect.Parameters["xProjection"].SetValue(game.projection);
                        effect.Parameters["xTexture"].SetValue(texture[i++]);
                        effect.Parameters["xEmissiveColor"].SetValue(new Vector4(1f, 1f, 1f, 0f));
                        effect.Parameters["isEmissive"].SetValue(true);
                    }
                    mesh.Draw();
                }

                dss = new DepthStencilState();
                dss.DepthBufferEnable = true;
                this.game.device.DepthStencilState = dss;
            }
            base.Draw(gameTime);
        }

    }
}
