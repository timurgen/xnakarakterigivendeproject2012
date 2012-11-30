using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectFinal
{
    public class SpaceShip : DrawableGameComponent
    {
        public Model model { get; set; }
        Texture2D texture;
        MainClass game;
        Effect effect;
        Matrix view, projection;
        public Vector3 shipPosition = new Vector3(500, 500, 500);
        public Quaternion shipRotation = Quaternion.Identity;

        public enum GameState
        {
            MainMenu,
            About,
            Playing,
            Ship,
        } 

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
            if ((GameState)game.CurrentGameState == GameState.Playing)
            {
                Matrix worldMatrix = Matrix.CreateRotationY(0) * Matrix.CreateRotationX(MathHelper.Pi) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateFromQuaternion(shipRotation) * Matrix.CreateTranslation(shipPosition);
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
}
