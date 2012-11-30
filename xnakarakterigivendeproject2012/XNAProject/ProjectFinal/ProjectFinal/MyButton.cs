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
        Texture2D texture;
        Vector2 position;
        Rectangle rectangel;
        public bool isClicked = false;
        public Vector2 size;
        Color color = new Color(255, 255, 255, 255);

        public MyButton(Texture2D _texture, GraphicsDevice _graphics, Vector2 _size)
        {
            texture = _texture;

            size = _size;
        }

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

        public void setPosition(Vector2 _position)
        {
            position = _position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangel, color);
        }
    }
}
