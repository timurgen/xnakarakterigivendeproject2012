using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectFinal
{
    class MyButton
    {
        /// <summary>
        /// Videreutvilket metode fra menu tutorial
        /// http://www.youtube.com/watch?v=54L_w0PiRa8&feature=g-user
        /// </summary>
        

        ///Variabler
        Texture2D texture;
        Vector2 position;
        Rectangle rectangel;
        public bool isClicked = false;
        public Vector2 size;
        Color color = new Color(255, 255, 255, 255);

        /// <summary>
        /// Konstruktør
        /// Det var endret flere ganger så finnes det ubrukkelig variabler
        /// </summary>
        /// <param name="_texture">Knappe textur</param>
        /// <param name="_graphics">Grafikk kort</param>
        /// <param name="_size">Knappe størelse</param>
        public MyButton(Texture2D _texture, GraphicsDevice _graphics, Vector2 _size)
        {
            texture = _texture;
            size = _size;
        }

        /// <summary>
        /// Oppdatering mus posisjon og sjekker "click"
        /// </summary>
        /// <param name="mouse"></param>
        public void Update(MouseState mouse)
        {
            rectangel = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                System.Threading.Thread.Sleep(100);
                Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
                if (mouseRectangle.Intersects(rectangel))
                {
                    isClicked = true;
                }
                else
                {
                    isClicked = false;
                }
            }
        }

        /// <summary>
        /// Mus posisjon
        /// </summary>
        /// <param name="_position"></param>
        public void setPosition(Vector2 _position)
        {
            position = _position;
        }

        /// <summary>
        /// Metode som tegner knappe
        /// </summary>
        /// <param name="spriteBatch">2d tegning</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangel, color);
        }
    }
}
