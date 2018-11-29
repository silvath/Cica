using Data;
using Engine;
using EngineWindows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapWindows
{
    public partial class FormMap : Form
    {
        #region Attributes
            private ResourceManager _resources = new ResourceManager(new StorageWindows(@"C:\Files\Projects\Cica\Games\Airplane\Resources\"));
        #endregion
        public FormMap()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateAirplane();
        }

        public void CreateAirplane() 
        {
            string path = @"C:\Files\Projects\Cica\Games\Airplane\Images\";
            string imageName = "1945";
            string transparency = "0043AB";
            this._resources.Clear();
            //Game
            int border = 60;
            int wingman = 50;
            Resource game = this._resources.Create(ResourceType.Game, "Airplane");
            game.Width = 600;
            game.Height = 600;
            game.FramesPerSecond = 30;
            Data.Image image = game.Images.Create(imageName, new List<byte>(System.IO.File.ReadAllBytes(string.Format("{0}{1}.png", path, imageName))));

            //Map Ocean
            Resource map = this._resources.Create(ResourceType.Map, "Ocean");
            map.Width = 600;
            map.Height = 600;
            int velocity = 10;
            map.Spawns.Create(1, (game.Width / 2) - wingman, game.Height - border, -velocity / 2, -velocity / 2, "UpLeft", "Down");
            map.Spawns.Create(1, (game.Width / 2), game.Height - (border * 2), 0, -velocity, "Up", "Down");
            map.Spawns.Create(1, (game.Width / 2) + wingman, game.Height - border, velocity / 2, -velocity / 2, "UpRight", "Down");
            map.Spawns.Create(1, border, (game.Height / 2) - wingman, velocity / 2, -velocity / 2, "UpRight", "Left");
            map.Spawns.Create(1, (border * 2), (game.Height / 2), velocity, 0, "Right", "Left");
            map.Spawns.Create(1, border, (game.Height / 2) + wingman, velocity / 2, velocity / 2, "DownRight", "Left");
            map.Spawns.Create(1, (game.Width) - border, (game.Height / 2) - wingman, -velocity / 2, -velocity / 2, "UpLeft", "Right");
            map.Spawns.Create(1, (game.Width) - (border * 2), (game.Height / 2), -velocity, 0, "Left", "Right");
            map.Spawns.Create(1, (game.Width) - border, (game.Height / 2) + wingman, -velocity / 2, velocity / 2, "DownLeft", "Right");
            map.Spawns.Create(1, (game.Width / 2) - wingman, border, -velocity / 2, velocity / 2, "DownLeft", "Up");
            map.Spawns.Create(1, (game.Width / 2), (border * 2), 0, velocity, "Down", "Up");
            map.Spawns.Create(1, (game.Width / 2) + wingman, border, velocity / 2, velocity / 2, "DownRight", "Up");
            image = map.Images.Create(imageName, new List<byte>(System.IO.File.ReadAllBytes(string.Format("{0}{1}.png", path, imageName)))); 
            SpriteMap spriteMap = map.SpritesMaps.Create(image.Name, "Ocean"); 
            int rows = 20;
            int columns = 20;
            int width = 30;
            int height = 30;
            for (int row = 0; row < rows; row++)
                for (int column = 0; column < columns; column++)
                    spriteMap.Sprites.Create(column * width, row * height, 269, 368, width, height);
            Animation animation = map.Animations.Create("Ocean");
            animation.Frames.Create("Ocean", 1);
            map.Triggers.CreateAnimation("Ocean", 0, 0);
            //Map Ocean Island1
            Resource island1 = map.Create("Island1");
            island1.SpritesMaps.Create(image.Name, "Island1x1", transparency).Sprites.Create(200, 200, 103, 499, 64, 64);
            animation = island1.Animations.Create("Island1");
            animation.Frames.Create("Island1x1", 1);
            island1.Triggers.CreateAnimation("Island1", 0, 0);
            //Map Ocean Island2
            Resource island2 = map.Create("Island2");
            island2.SpritesMaps.Create(image.Name, "Island2x1", transparency).Sprites.Create(400, 100, 168, 499, 64, 64);
            animation = island2.Animations.Create("Island2");
            animation.Frames.Create("Island2x1", 1);
            island2.Triggers.CreateAnimation("Island2", 0, 0);
            //Map Ocean Island3
            Resource island3 = map.Create("Island3");
            island3.SpritesMaps.Create(image.Name, "Island3x1", transparency).Sprites.Create(500, 50, 233, 499, 64, 64);
            animation = island3.Animations.Create("Island3");
            animation.Frames.Create("Island3x1", 1);
            island3.Triggers.CreateAnimation("Island3", 0, 0);

            //Map Ocean Boat1
            Resource boat1 = map.Create("Boat1");
            boat1.SpritesMaps.Create(image.Name, "Boat1x6", transparency).Sprites.Create(100, 100, 367, 103, 31, 97);
            boat1.SpritesMaps.Create(image.Name, "Boat1x5", transparency).Sprites.Create(100, 100, 400, 103, 31, 97);
            boat1.SpritesMaps.Create(image.Name, "Boat1x4", transparency).Sprites.Create(100, 100, 433, 103, 31, 97);
            boat1.SpritesMaps.Create(image.Name, "Boat1x3", transparency).Sprites.Create(100, 100, 466, 103, 31, 97);
            boat1.SpritesMaps.Create(image.Name, "Boat1x2", transparency).Sprites.Create(100, 100, 499, 103, 31, 97);
            boat1.SpritesMaps.Create(image.Name, "Boat1x1", transparency).Sprites.Create(100, 100, 532, 103, 31, 97);
            //Animation 
            animation = boat1.Animations.Create("UpDown");
            animation.Frames.Create("Boat1x1", 10);
            animation.Frames.Create("Boat1x2", 10);
            animation.Frames.Create("Boat1x3", 10);
            animation.Frames.Create("Boat1x4", 10);
            animation.Frames.Create("Boat1x5", 10);
            animation.Frames.Create("Boat1x6", 100);
            animation.Frames.Create("Boat1x5", 10);
            animation.Frames.Create("Boat1x4", 10);
            animation.Frames.Create("Boat1x3", 10);
            animation.Frames.Create("Boat1x2", 10);
            animation.Frames.Create("Boat1x1", 10);
            //Trigger
            boat1.Triggers.CreateAnimation("UpDown", 30, 150);

            //Map AirplaneGreen
            Resource airplaneGreen = map.Create("AirplaneGreen");
            airplaneGreen.SpritesMaps.Create(image.Name, "AirplaneGreenx1", transparency).Sprites.Create(0, 0, 268, 400, 64, 64);
            airplaneGreen.SpritesMaps.Create(image.Name, "AirplaneGreenx2", transparency).Sprites.Create(0, 0, 334, 400, 64, 64);
            airplaneGreen.SpritesMaps.Create(image.Name, "AirplaneGreenx3", transparency).Sprites.Create(0, 0, 400, 400, 64, 64);
            animation = airplaneGreen.Animations.Create("Fly");
            int frame = 1;
            for (int y = game.Height; y >= 0; y = y - 10)
            {
                animation.Frames.Create(string.Format("AirplaneGreenx{0}", frame), 5, 300, y);
                frame++;
                if (frame > 3)
                    frame = 1;

            }
            //Trigger
            airplaneGreen.Triggers.CreateAnimation("Fly", 100, 100);
            //Actor Blue
            CreateActor("Blue", imageName, transparency, path, 0, (game.Width / 2), (game.Height / 2) - border, "Up");
            //Actor Green
            CreateActor("Green", imageName, transparency, path, 1, border, (game.Height / 2), "Right");
            //Actor White
            CreateActor("White", imageName, transparency, path, 2, (game.Width / 2), -border, "Left");
            //Actor Gray
            CreateActor("Gray", imageName, transparency, path, 3, (game.Width / 2), border, "Down");
            this._resources.SaveAll();
        }

    }
}
