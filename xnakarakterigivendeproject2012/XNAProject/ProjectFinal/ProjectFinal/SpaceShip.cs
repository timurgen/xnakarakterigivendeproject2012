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
        Texture2D texture, texture_purple, texture_red, texture_yellow;
        MainClass game;
        Effect effect;
        Matrix view, projection;
        public Vector3 shipPosition = new Vector3(1000, 1000, 1000);
        public Quaternion shipRotation = Quaternion.Identity;
        Matrix worldMatrix;
        BoundingSphere spaceship_bound;

        //Materiser som holder "akkumulerte" bone-transformasjonene
        public Matrix[] matrixBoneTr;

        //Tar vare på opprinnelig Bone-transformasjoner:
        public Matrix[] matrixOriginBoneTr;

        public enum GameState
        {
            MainMenu,
            About,
            Playing,
            Ship,
        }

        public enum ShipType
        {
            ShipOne,
            ShipTo,
            ShipThree,
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
            texture_purple = game.Content.Load<Texture2D>("textures-spaceship/texture-purple");
            texture_red = game.Content.Load<Texture2D>("textures-spaceship/texture-red");
            texture_yellow = game.Content.Load<Texture2D>("textures-spaceship/texture-yellow");

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
                if((ShipType)game.menu.CurrenShipType == ShipType.ShipOne)
                    worldMatrix = Matrix.CreateRotationY(0) * Matrix.CreateRotationX(MathHelper.Pi) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateFromQuaternion(shipRotation) * Matrix.CreateTranslation(shipPosition);
                if ((ShipType)game.menu.CurrenShipType == ShipType.ShipTo)
                    worldMatrix = Matrix.CreateRotationY(MathHelper.Pi * 2) * Matrix.CreateRotationX(1.4f) * Matrix.CreateRotationZ(0) * Matrix.CreateFromQuaternion(shipRotation) * Matrix.CreateTranslation(shipPosition);
                if ((ShipType)game.menu.CurrenShipType == ShipType.ShipThree)
                    worldMatrix = Matrix.CreateRotationY(0) * Matrix.CreateRotationX(-MathHelper.Pi / 2) * Matrix.CreateRotationZ(0) * Matrix.CreateFromQuaternion(shipRotation) * Matrix.CreateTranslation(shipPosition);

                if ((ShipType)game.menu.CurrenShipType == ShipType.ShipThree)
                {
                    spaceship_bound = new BoundingSphere();
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        BoundingSphere orig_bound = mesh.BoundingSphere;
                        foreach (Effect currentEffect in mesh.Effects)
                        {
                            orig_bound = mesh.BoundingSphere;
                            orig_bound = this.game.TransformBoundingSphere(orig_bound, worldMatrix);
                            spaceship_bound = BoundingSphere.CreateMerged(spaceship_bound, orig_bound);

                            currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                            currentEffect.Parameters["xEmissiveColor"].SetValue(new Vector4(1f, 1f, 1f, 1f));
                            currentEffect.Parameters["isEmissive"].SetValue(true);
                            currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                            currentEffect.Parameters["xView"].SetValue(game.view);
                            currentEffect.Parameters["xProjection"].SetValue(game.projection);

                            switch (mesh.Name)
                            {
                                case "Engine":
                                    currentEffect.Parameters["xTexture"].SetValue(this.texture_red); 
                                    break;
                                case "SpaceShip":
                                    currentEffect.Parameters["xTexture"].SetValue(this.texture_purple);
                                    break;
                                case "souz":
                                    currentEffect.Parameters["xTexture"].SetValue(this.texture_red);
                                    break;
                                case "glass":
                                    currentEffect.Parameters["xTexture"].SetValue(this.texture_yellow);
                                    break;
                                default:
                                    currentEffect.Parameters["xTexture"].SetValue(this.texture_purple);
                                    break;
                            }
                            
                        }
                        model.Tag = spaceship_bound;
                        mesh.Draw();
                    }
                }
                else
                {
                    spaceship_bound = new BoundingSphere();
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        BoundingSphere orig_bound = mesh.BoundingSphere;
                        foreach (Effect currentEffect in mesh.Effects)
                        {
                            orig_bound = mesh.BoundingSphere;
                            orig_bound = this.game.TransformBoundingSphere(orig_bound, worldMatrix);
                            spaceship_bound = BoundingSphere.CreateMerged(spaceship_bound, orig_bound);

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
                }

                

                base.Draw(gameTime);
            }

        }


        private SpaceObject DetectCollision() 
        {
            BoundingSphere obj_boundSphere;
            foreach (SpaceObject obj in game.Components) 
            {
                obj_boundSphere = (BoundingSphere)obj.model.Tag;
                if (obj_boundSphere.Intersects((BoundingSphere)model.Tag)) 
                {
                    Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    return obj;
                }
            }
            return null;
        }



    }
}
