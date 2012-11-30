using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectFinal
{
    class SpaceShip : DrawableGameComponent
    {
        Model model;
        Texture2D texture;
        MainClass game;
        Effect effect;
        Matrix view, projection;
        public Vector3 shipPosition = new Vector3(500, 500, 500);
        public Quaternion shipRotation = Quaternion.Identity; 

        public SpaceShip(MainClass _game,  Matrix _view,  Matrix _projection) : base(_game)
        {
            this.game = _game;
            this.view = _view;
            this.projection = _projection;
        }



        public void load(Effect _effect, Model _model, Texture2D _texture)
        {
            this.model = _model;
            this.effect = _effect;
            this.texture = _texture;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect.Clone();
                }
            }
                
                    
            
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix worldMatrix =  Matrix.CreateRotationY(MathHelper.Pi*2)*Matrix.CreateRotationX(1.4f)* Matrix.CreateRotationZ(0) *Matrix.CreateFromQuaternion(shipRotation) * Matrix.CreateTranslation(shipPosition);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xEmissiveColor"].SetValue(new Vector4(1f, 1f, 1f, 1f));
                    currentEffect.Parameters["isEmissive"].SetValue(true);
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(game.view);
                    currentEffect.Parameters["xProjection"].SetValue(game.projection);
                    currentEffect.Parameters["xTexture"].SetValue(this.texture);
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }






    }
}
