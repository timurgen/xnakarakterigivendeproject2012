﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
    /// <summary>
    /// Klassen representerer et objekt i solar systemet slik som Sola, planetter og satelitter (samt kunstige satellitter)
    /// 
    /// </summary>
    class SpaceObject : DrawableGameComponent
    {
        /// <summary>
        /// Parent spaceobject, brukes dersom en object er "child element" i systemmet
        /// </summary>
        private SpaceObject parent;
        /// <summary>
        /// 3d model som brukes ved tegning 
        /// </summary>
        private Model model;
        /// <summary>
        /// Matriser til grunnlegende transformasjoner
        /// </summary>
        private Matrix world, view, projection;
        /// <summary>
        /// Størrelse av objekt
        /// </summary>
        private float size;
        /// <summary>
        /// baneradius 
        /// </summary>
        private float orbitRadius;
        /// <summary>
        /// Vunkel fra y akse til objekt
        /// </summary>
        private float akseAngle;
        /// <summary>
        /// Banehastighet
        /// </summary>
        private float orbitalSpeed;
        /// <summary>
        /// 
        /// </summary>
        private float orbitAngle;
        /// <summary>
        /// Rotasjonshastighet (om y akse)
        /// </summary>
        private float rotSpeed;
        /// <summary>
        /// Texture til objekt
        /// </summary>
        private Texture2D texture;
        /// <summary>
        /// Shader som brukes ved tegning
        /// </summary>
        private Effect effect;
        /// <summary>
        /// Spill som bojekt tilhører
        /// </summary>
        private MainClass game;
        /// <summary>
        /// Shaderteknikk som brukes
        /// </summary>
        private String currentTechnique;
        /// <summary>
        /// Settes til <code>true</code>dersom objekt må bruke emissiv belysning, ellers <code>false</code>
        /// </summary>
        public bool isEmissive { get; set; }
        /// <summary>
        /// Brukes ved beregning av startpunkt i bane
        /// </summary>
        private Matrix orbitStart;
        /// <summary>
        /// Tilfeldig tall generatør 
        /// </summary>
        private Random g;

        /// <summary>
        /// Brukes til å lage nytt objekt
        /// </summary>
        /// <param name="size">Størrelse av planet</param>
        /// <param name="orbitRadius">planetens baneradius</param>
        /// <param name="akseAngle"> vinkelen til rotasjonsakse</param>
        /// <param name="orbitalSpeed">banehastiget</param>
        /// <param name="orbitAngle">banevinkel i forhold til xz plane</param>
        /// <param name="parent">parent planet</param>
        public SpaceObject(Game _game, float _size, float _orbitRadius, float _akseAngle, float _orbitalSpeed, float _orbitAngle, float _rotSpeed, SpaceObject _parent): base(_game)
        {
            this.size = _size;
            this.orbitRadius = _orbitRadius;
            this.akseAngle = _akseAngle;
            this.orbitalSpeed = _orbitalSpeed;
            this.orbitAngle = _orbitAngle;
            this.rotSpeed = _rotSpeed;
            this.game = (MainClass)_game;
            if (_parent != null)
            {
                this.parent = _parent;
            }
            //her for planetter tilfeldige oppstartspunkter i bane
            g = new Random(this.GetHashCode());
            //System.Threading.Thread.Sleep(5);
            Matrix.CreateRotationY((float)(Math.PI * g.NextDouble()), out orbitStart);
        }


        /// <summary>
        ///  Metode skal kalles i load metode av main class
        /// </summary>
        /// <param name="game">reference til spill, objekt tilhører</param>
        public void load(Effect _effect, Model _model, Texture2D _texture)
        {
            this.effect = _effect;
            this.model = _model;
            this.texture = _texture;
            
            foreach (ModelMesh mesh in this.model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = this.effect.Clone();
                }
            }
            
        }//end of load


        /// <summary>
        /// hjelpematriser
        /// </summary>
        float RotY, orbRotY;
        float orbRotX;
        /// <summary>
        /// metode skal kalles i update metode av main class
        /// </summary>
        public override void Update(GameTime gt)
        {
            Matrix matIdentity, matTrans, matRotateY, matRotateX, matScale, matOrbT, matOrbR, matOrbTX, matOrbRX;
            
            matIdentity = Matrix.Identity;
            
            matScale = Matrix.CreateScale(this.size);
            
            //rotasjon om y akse
            RotY += this.rotSpeed * (float)gt.ElapsedGameTime.Milliseconds / 1000f;
            RotY = RotY % (float)(2 * Math.PI);
            matRotateY = Matrix.CreateRotationY(RotY);

            //vinkel til jordakse
            matRotateX = Matrix.CreateRotationX(this.akseAngle);

            //orbital rotasjon
            matOrbT = Matrix.CreateTranslation(this.orbitRadius, 0, this.orbitRadius);
            orbRotY += this.orbitalSpeed * (float)gt.ElapsedGameTime.Milliseconds / 5000.0f;
            orbRotY = orbRotY % (float)(2 * Math.PI);
            matOrbR = Matrix.CreateRotationY((orbRotY));

            //matOrbTX = Matrix.CreateTranslation(this.orbitAngle, this.orbitAngle, 0);
            //orbRotX += this.orbitAngle * (float)gt.ElapsedGameTime.Milliseconds / 5000.0f;
            //orbRotX = orbRotX % (float)(2 * Math.PI);
            //matOrbRX = Matrix.CreateRotationX((orbRotX));
            matOrbRX = Matrix.CreateRotationX(this.orbitAngle);

            matTrans = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            //matTrans = Matrix.CreateTranslation(this.orbitAngle, this.orbitAngle, this.orbitAngle);
            
            //Kumulativ world‐matrise;
            if (parent == null)
            {
                world = matIdentity * matScale * matRotateY * matRotateX * (matOrbT  * matOrbR * orbitStart) * matTrans*matOrbRX;
            }
            else
            {
                world = matIdentity * matScale * matRotateY * matRotateX * (matOrbT * matOrbR * orbitStart) * matTrans *matOrbRX* parent.world;
            }

            view = game.getEffect().Parameters["xView"].GetValueMatrix();

        }//end of update


        /// <summary>
        /// Tegner model, den metoden kalles fra draw i main class
        /// </summary>
        public override void Draw(GameTime gt)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect e in mesh.Effects)
                {
                    e.CurrentTechnique = e.Techniques["Textured"];
                    e.Parameters["xWorld"].SetValue(this.world);
                    e.Parameters["xView"].SetValue(view);
                    e.Parameters["xTexture"].SetValue(this.texture);
                    e.Parameters["xEmissiveColor"].SetValue(new Vector4(1f, 1f, 1f, 0f));
                    e.Parameters["isEmissive"].SetValue(isEmissive);
                    e.Parameters["xLightsWorldViewProjection"].SetValue(this.world * effect.Parameters["xView"].GetValueMatrix() * effect.Parameters["xProjection"].GetValueMatrix()); 
                }
                
                mesh.Draw();
            }

        }//end of draw

        public void setShaderTechnique(String _Technique)
        {
            this.currentTechnique = _Technique;
        }

        public Effect getEffect()
        {
            return this.effect;
        }

    }
}
