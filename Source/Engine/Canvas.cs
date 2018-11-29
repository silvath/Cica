using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Canvas
    {
        #region Attributes
            private ICanvas _canvas;
            private byte[] _data;
            private int _spriteCode = 0;
        #endregion
        #region Constructor
            public Canvas(ICanvas canvas) 
            {
                this._canvas = canvas;
            }
        #endregion

        #region Start
            public void Start(int width, int height) 
            {
                _data = this._canvas.Create(width, height);
            }
        #endregion
        #region End
            public byte[] End() 
            {
                return (this._data);
            } 
        #endregion
        #region Draw
            public void Draw(byte[] data, int x, int y, int width, int height) 
            {
                this._data = this._canvas.Draw(this._data, data, x, y, width, height);
            }
        #endregion
        #region Extract
            public byte[] Extract(ImageCollection images, SpriteMap spriteMap, Sprite sprite, out int spriteCode) 
            {
                spriteCode = 0;
                byte[] data = spriteMap.GetCacheSprite(sprite.XImage, sprite.YImage, sprite.Width, sprite.Height, out spriteCode);
                if (data != null)
                    return (data);
                Image image = images.GetImage(spriteMap.ImageName);
                data = this._canvas.Extract(image, sprite.XImage, sprite.YImage, sprite.Width, sprite.Height, sprite.Transparency ? spriteMap.Transparency : string.Empty);
                spriteMap.SetCacheSprite(_spriteCode, sprite.XImage, sprite.YImage, sprite.Width, sprite.Height, data);
                spriteCode = _spriteCode;
                _spriteCode++;
                return (data);
            } 
        #endregion
        #region Transform
            public byte[] Transform(byte[] data)
            {
                return(this._canvas.Transform(data));
            }
        #endregion
    }
}
