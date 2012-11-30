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
        Model model;
        Texture2D [] texture;
        MainClass game;
        Effect effect;

        public Skybox(MainClass _game, Effect _effect): base(_game)
        {
            this.game = _game;
            effect = _effect;
        }

        public void load(Effect _effect, Model _model)
        {
            model = LoadModel(_model, out texture);
        }

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

        public override void Draw(GameTime gameTime)
        {
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            this.game.device.SamplerStates[0] = ss;

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;

            this.game.device.DepthStencilState = dss;

            Matrix[] skyboxTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
            int i = 0;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    Matrix worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(this.game.cameraPosition);
                    effect.CurrentTechnique = effect.Techniques["Textured"];
                    effect.Parameters["xWorld"].SetValue(worldMatrix);
                    effect.Parameters["xView"].SetValue(game.view);
                    effect.Parameters["xProjection"].SetValue(game.projection);
                    effect.Parameters["xTexture"].SetValue(texture[i++]);
                }
                mesh.Draw();
            }

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            this.game.device.DepthStencilState = dss;
            base.Draw(gameTime);
        }

    }
}
