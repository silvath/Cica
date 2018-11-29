using Data;
using Engine;
using EngineWindows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Color = System.Drawing.Color;

namespace ResourceBuilderWindows
{
    public partial class FormResourceBuilder : Form
    {
        #region Properties
            private StorageWindows _storage = new StorageWindows(StorageWindows.GetPathFolderResources());
            private ResourceManager _resources = null;
            public GameEngine _game = null;
            public CanvasWindows _canvasWindows = null;
            public Canvas _canvas = null;
            private Resource Resource { set; get; }
            private int FrameCurrent { set; get; }
        #endregion
        #region Constructors
            public FormResourceBuilder()
            {
                InitializeComponent();
                this._resources = new ResourceManager(this._storage);
                this._canvasWindows = new CanvasWindows();
                this._canvasWindows.Transform = false;
                this._canvas = new Canvas(this._canvasWindows);
                this._game = new GameEngine(this._resources, this._canvas, null, null);
            }
        #endregion

        #region Events
            #region Menu
            private void openToolStripMenuItemOpen_Click(object sender, EventArgs e)
            {
                FormResourceList form = new FormResourceList(this._resources);
                if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                this.Resource = form.Resource;
                this.RefreshResorce();
            }

            private void gameToolStripMenuItemNewGame_Click(object sender, EventArgs e)
            {
                //this.Resource = this._resources.Create(ResourceType.Game, "New Game");
                this.RefreshResorce();
            }

            private void mapToolStripMenuItemNewMap_Click(object sender, EventArgs e)
            {
                //this.Resource = this._resources.Create(ResourceType.Map, "New Map");
                this.RefreshResorce();

            }

            private void actorToolStripMenuItemNewActor_Click(object sender, EventArgs e)
            {
                //this.Resource = this._resources.Create(ResourceType.Actor, "New Actor");
                this.RefreshResorce();
            }

            private void saveToolStripMenuItemSave_Click(object sender, EventArgs e)
            {
                if (this.Resource == null)
                    return;
                this._resources.Save(this.Resource);
            }

            private void airplaneToolStripMenuItem_Click(object sender, EventArgs e)
            {
                CreateGameAirplane(true);
            }

            private void airplaneBlankToolStripMenuItem_Click(object sender, EventArgs e)
            {
                CreateGameAirplane(false);
            }
            #endregion
            #region Properties
            private void textBoxName_TextChanged(object sender, EventArgs e)
            {
                this.Resource.Name = this.textBoxName.Text;
                this.RefreshResorce();
            }

            private void textBoxWidth_TextChanged(object sender, EventArgs e)
            {
                this.Resource.Width = Int32.Parse(this.textBoxWidth.Text);
                this.RefreshResorce();
            }

            private void textBoxHeight_TextChanged(object sender, EventArgs e)
            {
                this.Resource.Height = Int32.Parse(this.textBoxHeight.Text);
                this.RefreshResorce();
            }
            #endregion
            #region Images
            private void buttonImageNew_Click(object sender, EventArgs e)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Images (*.png)|*.png";
                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                string fileName = ofd.FileName;
                CreateImage(this.Resource, fileName);
                this.RefreshResorce();
            }

            private Data.Image CreateImage(Resource resource, string filePath) 
            {
                string name = Path.GetFileNameWithoutExtension(filePath);
                System.Drawing.Image image = Bitmap.FromFile(filePath);
                List<byte> data = new List<byte>(System.IO.File.ReadAllBytes(filePath));
                return(resource.Images.Create(name, image.Width, image.Height, data));
            }

            private void buttonImageEdit_Click(object sender, EventArgs e)
            {
                MessageBox.Show("Maybe later");
            }

            private void buttonImageDelete_Click(object sender, EventArgs e)
            {
                if (this.listBoxImages.SelectedItem == null)
                    return;
                this.Resource.Images.Remove(this.listBoxImages.SelectedItem as Data.Image);
                this.RefreshResorce();
            }

            private void listBoxImages_SelectedIndexChanged(object sender, EventArgs e)
            {
                if(this.listBoxImages.SelectedItem == null)
                    return;
                MemoryStream memo = new MemoryStream((this.listBoxImages.SelectedItem as Data.Image).Data.ToArray());
                this.pictureBoxImage.Image = Bitmap.FromStream(memo);
            }
            #endregion
            #region SpriteMaps
            private void listBoxSpriteMaps_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (this.listBoxSpriteMaps.SelectedItem == null)
                    return;
                Layer layer = _game.CreateLayers(this.Resource.Images, this.listBoxSpriteMaps.SelectedItem as SpriteMap, 0, 0)[0];
                MemoryStream memo = new MemoryStream(layer.Data.ToArray());
                this.pictureBoxSpriteMaps.Image = Bitmap.FromStream(memo);
            }

            private void buttonSpriteMapsNew_Click(object sender, EventArgs e)
            {
                FormSpriteMap formSpriteMap = new FormSpriteMap(this._game, this.Resource, null);
                if (formSpriteMap.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.RefreshResorce();
            }

            private void buttonSpriteMapsEdit_Click(object sender, EventArgs e)
            {
                if (this.listBoxSpriteMaps.SelectedItem == null)
                    return;
                FormSpriteMap formSpriteMap = new FormSpriteMap(this._game, this.Resource, this.listBoxSpriteMaps.SelectedItem as SpriteMap);
                if (formSpriteMap.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.RefreshResorce();
            }

            private void buttonSpriteMapsDelete_Click(object sender, EventArgs e)
            {
                if (this.listBoxSpriteMaps.SelectedItem == null)
                    return;
                SpriteMap spriteMap = this.listBoxSpriteMaps.SelectedItem as SpriteMap;
                this.Resource.SpritesMaps.Remove(spriteMap);
                this.RefreshResorce();
            }
            #endregion
            #region Animations
            private void buttonAnimationNew_Click(object sender, EventArgs e)
            {
                Animation animation = new Animation();
                this.Resource.Animations.Add(animation);
                this.RefreshResorce();
            }

            private void buttonAnimationDelete_Click(object sender, EventArgs e)
            {
                if (this.listBoxAnimations.SelectedItem == null)
                    return;
                Animation animation = this.listBoxAnimations.SelectedItem as Animation;
                this.Resource.Animations.Remove(animation);
                this.RefreshResorce();
            }

            private void listBoxAnimations_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (this.listBoxAnimations.SelectedItem == null)
                    return;
                Animation animation = this.listBoxAnimations.SelectedItem as Animation;
                RefreshAnimation(animation); 
            }

            private void textBoxAnimationName_TextChanged(object sender, EventArgs e)
            {
                if (this.listBoxAnimations.SelectedItem == null)
                    return;
                Animation animation = this.listBoxAnimations.SelectedItem as Animation;
                animation.Name = this.textBoxAnimationName.Text;
                RefreshAnimation(animation); 
            }

            private void listBoxFrames_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (this.listBoxFrames.SelectedItem == null)
                    return;
                Frame frame = this.listBoxFrames.SelectedItem as Frame;
                SpriteMap spriteMap = this.Resource.SpritesMaps.GetSpriteMap(frame.SpriteMapName);
                Layer layer = _game.CreateLayers(this.Resource.Images, spriteMap, 0, 0)[0];
                MemoryStream memo = new MemoryStream(layer.Data.ToArray());
                this.pictureBoxAnimationPreview.Image = Bitmap.FromStream(memo);
            }

            private void timerAnimations_Tick(object sender, EventArgs e)
            {
                if (this.listBoxFrames.Items.Count == 0)
                    return;
                if (this.listBoxFrames.SelectedItem == null)
                    this.listBoxFrames.SelectedIndex = 0;
                Frame frame = this.listBoxFrames.SelectedItem as Frame;
                this.FrameCurrent++;
                if (this.FrameCurrent <= frame.Frames)
                    return;
                this.FrameCurrent = 0;
                if (this.listBoxFrames.SelectedIndex < (this.listBoxFrames.Items.Count - 1))
                    this.listBoxFrames.SelectedIndex++;
                else
                    this.listBoxFrames.SelectedIndex = 0;
            }

            private void buttonAnimationStart_Click(object sender, EventArgs e)
            {
                this.StartAnimation();
            }

            private void buttonAnimationStop_Click(object sender, EventArgs e)
            {
                this.StopAnimation();
            }

            private void buttonFrameDown_Click(object sender, EventArgs e)
            {
                Animation animation = this.listBoxAnimations.SelectedItem as Animation;
                if (animation == null)
                    return;
                Frame frame = this.listBoxFrames.SelectedItem as Frame;
                if (frame == null)
                    return;
                int index = animation.Frames.IndexOf(frame);
                if (index >= (animation.Frames.Count - 1))
                    return;
                animation.Frames[index] = animation.Frames[index + 1];
                animation.Frames[index + 1] = frame;
                this.RefreshAnimation(animation);
                this.listBoxFrames.SelectedItem = frame;
            }

            private void buttonFrameUp_Click(object sender, EventArgs e)
            {
                Animation animation = this.listBoxAnimations.SelectedItem as Animation;
                if (animation == null)
                    return;
                Frame frame = this.listBoxFrames.SelectedItem as Frame;
                if (frame == null)
                    return;
                int index = animation.Frames.IndexOf(frame);
                if (index == 0)
                    return;
                animation.Frames[index] = animation.Frames[index - 1];
                animation.Frames[index - 1] = frame;
                this.RefreshAnimation(animation);
                this.listBoxFrames.SelectedItem = frame;
            }

            private void buttonFrameNew_Click(object sender, EventArgs e)
            {
                Animation animation = this.listBoxAnimations.SelectedItem as Animation;
                if (animation == null)
                    return;
                FormFrame formFrame = new FormFrame(this.Resource,animation, null);
                if (formFrame.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                this.RefreshAnimation(animation);
            }

            private void buttonFrameEdit_Click(object sender, EventArgs e)
            {
                Animation animation = this.listBoxAnimations.SelectedItem as Animation;
                if (animation == null)
                    return;
                Frame frame = this.listBoxFrames.SelectedItem as Frame;
                if (frame == null)
                    return;
                FormFrame formFrame = new FormFrame(this.Resource,animation, frame);
                if (formFrame.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                this.RefreshAnimation(animation);
            }

            private void buttonFrameDelete_Click(object sender, EventArgs e)
            {
                Animation animation = this.listBoxAnimations.SelectedItem as Animation;
                if (animation == null)
                    return;
                Frame frame = this.listBoxFrames.SelectedItem as Frame;
                if (frame == null)
                    return;
                animation.Frames.Remove(frame);
                this.RefreshAnimation(animation);
            }
            #endregion
            #region Triggers
            private void buttonTriggerNew_Click(object sender, EventArgs e)
            {
                Trigger trigger = new Trigger();
                this.Resource.Triggers.Add(trigger);
                this.RefreshTriggers(this.Resource); 
            }

            private void buttonTriggerDelete_Click(object sender, EventArgs e)
            {
                Trigger trigger = listBoxTriggers.SelectedItem as Trigger;
                if (trigger == null)
                    return;
                this.Resource.Triggers.Remove(trigger);
                this.RefreshTriggers(this.Resource); 
            }

            private void listBoxTriggers_SelectedIndexChanged(object sender, EventArgs e)
            {
                Trigger trigger = listBoxTriggers.SelectedItem as Trigger;
                if (trigger == null)
                    return;
                this.comboBoxAnimation.SelectedItem = this.Resource.Animations.GetAnimation(trigger.AnimationName);
                this.textBoxTriggerStart.Text = trigger.Activation.Start.ToString();
                this.textBoxTriggerEnd.Text = trigger.Activation.End != null ? trigger.Activation.End.ToString() : string.Empty;
            }

            private void comboBoxAnimation_SelectedIndexChanged(object sender, EventArgs e)
            {
                Trigger trigger = listBoxTriggers.SelectedItem as Trigger;
                if (trigger == null)
                    return;
                Animation animation = comboBoxAnimation.SelectedItem as Animation;
                trigger.AnimationName = animation == null ? string.Empty : animation.Name;
            }

            private void textBoxTriggerStart_TextChanged(object sender, EventArgs e)
            {
                Trigger trigger = listBoxTriggers.SelectedItem as Trigger;
                if (trigger == null)
                    return;
                trigger.Activation.Start = Int32.Parse(this.textBoxTriggerStart.Text);
            }

            private void textBoxTriggerEnd_TextChanged(object sender, EventArgs e)
            {
                Trigger trigger = listBoxTriggers.SelectedItem as Trigger;
                if (trigger == null)
                    return;
                trigger.Activation.End = string.IsNullOrEmpty(this.textBoxTriggerEnd.Text) ? null : (int?)Int32.Parse(this.textBoxTriggerEnd.Text);
            }
            #endregion
            #region Spawns
            private void buttonSpawnNew_Click(object sender, EventArgs e)
            {
                Spawn spawn = new Spawn();
                this.Resource.Spawns.Add(spawn);
                this.RefreshResorce();
                this.listBoxSpawns.SelectedItem = spawn;
            }

            private void buttonSpawnDelete_Click(object sender, EventArgs e)
            {
                Spawn spawn = this.listBoxSpawns.SelectedItem as Spawn;
                if (spawn == null)
                    return;
                this.Resource.Spawns.Remove(spawn);
                this.RefreshResorce();
            }

            private void listBoxSpawns_SelectedIndexChanged(object sender, EventArgs e)
            {
                Spawn spawn = this.listBoxSpawns.SelectedItem as Spawn;
                if (spawn == null)
                    return;
                this.textBoxSpawnGroupName.Text = spawn.GroupName;
                this.textBoxSpawnAnimationName.Text = spawn.AnimationName;
                this.textBoxSpawnGroupName.Text = spawn.GroupName;
                this.textBoxSpawnX.Text = spawn.X.ToString();
                this.textBoxSpawnY.Text = spawn.Y.ToString();
                this.textBoxSpawnLife.Text = spawn.Life.ToString();
                this.textBoxSpawnVelocityX.Text = spawn.VelocityX.ToString();
                this.textBoxSpawnVelocityY.Text = spawn.VelocityY.ToString();
            }

            private void textBoxSpawnGroupName_TextChanged(object sender, EventArgs e)
            {
                Spawn spawn = this.listBoxSpawns.SelectedItem as Spawn;
                if (spawn == null)
                    return;
                spawn.GroupName = this.textBoxSpawnGroupName.Text;
                this.RefreshResorce();
            }

            private void textBoxSpawnAnimationName_TextChanged(object sender, EventArgs e)
            {
                Spawn spawn = this.listBoxSpawns.SelectedItem as Spawn;
                if (spawn == null)
                    return;
                spawn.AnimationName = this.textBoxSpawnAnimationName.Text;
                this.RefreshResorce();
            }

            private void textBoxSpawnLife_TextChanged(object sender, EventArgs e)
            {
                Spawn spawn = this.listBoxSpawns.SelectedItem as Spawn;
                if (spawn == null)
                    return;
                spawn.Life = Int32.Parse(this.textBoxSpawnLife.Text);
                this.RefreshResorce();
            }

            private void textBoxSpawnX_TextChanged(object sender, EventArgs e)
            {
                Spawn spawn = this.listBoxSpawns.SelectedItem as Spawn;
                if (spawn == null)
                    return;
                spawn.X = Int32.Parse(this.textBoxSpawnX.Text);
                this.RefreshResorce();
            }

            private void textBoxSpawnY_TextChanged(object sender, EventArgs e)
            {
                Spawn spawn = this.listBoxSpawns.SelectedItem as Spawn;
                if (spawn == null)
                    return;
                spawn.Y = Int32.Parse(this.textBoxSpawnY.Text);
                this.RefreshResorce();
            }

            private void textBoxSpawnVelocityX_TextChanged(object sender, EventArgs e)
            {
                Spawn spawn = this.listBoxSpawns.SelectedItem as Spawn;
                if (spawn == null)
                    return;
                spawn.VelocityX = Int32.Parse(this.textBoxSpawnVelocityX.Text);
                this.RefreshResorce();
            }

            private void textBoxSpawnVelocityY_TextChanged(object sender, EventArgs e)
            {
                Spawn spawn = this.listBoxSpawns.SelectedItem as Spawn;
                if (spawn == null)
                    return;
                spawn.VelocityY = Int32.Parse(this.textBoxSpawnVelocityY.Text);
                this.RefreshResorce();
            }
            #endregion
        #endregion

        #region UI
            private void RefreshResorce() 
            {
                this.RefreshResource(this.Resource);
            }

            private void RefreshResource(Resource resource)
            {
                //Properties
                this.textBoxName.Text = resource.Name;
                this.textBoxWidth.Text = resource.Width.ToString();
                this.textBoxHeight.Text = resource.Height.ToString();
                //Images
                this.listBoxImages.Items.Clear();
                foreach (Data.Image image in resource.Images)
                    this.listBoxImages.Items.Add(image);
                //SpriteMaps
                this.listBoxSpriteMaps.Items.Clear();
                foreach (SpriteMap spriteMap in resource.SpritesMaps)
                    this.listBoxSpriteMaps.Items.Add(spriteMap);
                //Animations
                this.listBoxAnimations.Items.Clear();
                this.comboBoxAnimation.Items.Clear();
                foreach (Animation animation in resource.Animations)
                {
                    this.listBoxAnimations.Items.Add(animation);
                    this.comboBoxAnimation.Items.Add(animation);
                }
                //Triggers
                this.RefreshTriggers(resource); 
                //Spawns
                this.listBoxSpawns.Items.Clear();
                foreach (Spawn spawn in resource.Spawns)
                    this.listBoxSpawns.Items.Add(spawn);
            }

            private void RefreshAnimation(Animation animation) 
            {
                this.textBoxAnimationName.Text = animation.Name;
                this.listBoxFrames.Items.Clear();
                foreach (Frame frame in animation.Frames)
                    this.listBoxFrames.Items.Add(frame);
            }

            private void RefreshTriggers(Resource resource) 
            {
                this.listBoxTriggers.Items.Clear();
                foreach (Trigger trigger in resource.Triggers)
                    this.listBoxTriggers.Items.Add(trigger);
            } 
        #endregion

        #region Animations
            private void StopAnimation() 
            {
                this.timerAnimations.Enabled = false;
            }

            private void StartAnimation()
            {
                this.FrameCurrent = 0;
                this.timerAnimations.Interval = 1000 / (this.Resource.FramesPerSecond > 0 ? this.Resource.FramesPerSecond : 30);
                this.timerAnimations.Enabled = true;
            }
        #endregion

        #region Airplane
            private void CreateGameAirplane(bool maps) 
            {
                ResourceGenerator generator = new ResourceGenerator("A");
                string path = StorageWindows.GetPathFolderImages();
                string imageName = "1945";
                string imageLogoName = "Logo";
                string imageFontLogoName = "Font2";
                string transparency = "0043AB";
                this._resources.Clear();
                //Game
                int border = 30;
                Resource game = this._resources.Create(ResourceType.Game, generator.GetCode(), "Airplane");
                game.Width = 800;
                game.Height = 600;
                game.WarnUp = 3;
                game.FramesPerSecond = 30;
                game.InputsPerSecond = 3;
                game.NetworksPerSecond = 10;
                game.Rounds = 3;
                Data.Image image = CreateImage(game, string.Format("{0}{1}.png", path, imageName));
                Data.Image imageLogo = CreateImage(game, string.Format("{0}{1}.png", path, imageLogoName));
                Data.Image imageFontLogo = CreateImage(game, string.Format("{0}{1}.png", path, imageFontLogoName)); 
                //Fonts
                Data.Font font = CreateFonts(game, imageName, "000000");
                Data.Font fontLogo = CreateFontLogo(game, imageFontLogoName, "FFFFFF");
                game.WarnUpFontName = font.Name;
                game.SystemFontName = font.Name;
                //Screens
                CreateScreens(game, path, imageLogoName, font, fontLogo, transparency);
                if (maps)
                {
                    //Map Ocean
                    CreateGameAirplaneMapOcean(generator, game, path, imageName, transparency, border);
                }else {
                    //Map Blank
                    CreateGameAirplaneMapBlank(generator, game, path, imageName, transparency, border);
                }
                //Actor Blue
                CreateGameAirplaneActor(generator, "Blue", imageName, transparency, path, 0);
                //Actor Green
                CreateGameAirplaneActor(generator, "Green", imageName, transparency, path, 1);
                //Actor White
                CreateGameAirplaneActor(generator, "White", imageName, transparency, path, 2);
                //Actor Gray
                CreateGameAirplaneActor(generator, "Gray", imageName, transparency, path, 3);
                this._resources.SaveAll();
            }

            private void CreateScreens(Data.Resource game, string path, string imageName, Data.Font font, Data.Font fontLogo, string transparency)
            {
                SpriteMap spriteMap = game.SpritesMaps.Create(imageName,"Main");
                Color colorLogo = Color.FromArgb(0, 67, 171);
                Color colorGame = Color.FromArgb(0,67,171);
                int width = 647;
                int height = 483;
                spriteMap.Sprites.Create((game.Width - width) / 2, (game.Height - height) / 2, 0,0,width,height);  
                Animation animation = game.Animations.Create("Main");
                animation.Frames.Create("Main");
                //SinglePlayer
                Data.Screen screen = game.Screens.Create(ScreenType.Start, "SinglePlayer");
                screen.AnimationName = "Main";
                int x = 150;
                int xSelector = x - 40;
                int y = 300;
                height = 50;
                width = 200;
                screen.Texts.Create(fontLogo.Name, "SINGLE PLAYER", x, y);
                screen.Texts.Create(fontLogo.Name, "0", xSelector, y - 1);
                screen.Texts.Create(fontLogo.Name, "MULTI PLAYER", x, y + height);
                screen.Texts.Create(fontLogo.Name, "QUIT", x, y + (height * 3));
                screen.Actions.Create(InputType.Button1, ActionType.GoToScreen, "Quit");
                screen.Actions.Create(InputType.Right, ActionType.StartGame);
                screen.Actions.Create(InputType.Down, ActionType.GoToScreen, "MultiPlayer");
                screen.Actions.Create(x, y, 437, height, ActionType.StartGame);
                screen.Actions.Create(x, y + height, 437, height, ActionType.StartGameOnline);
                screen.Actions.Create(x, y + (height * 3), 126, height, ActionType.Quit);
                //MultiPlayer
                screen = game.Screens.Create(ScreenType.Intermediate, "MultiPlayer");
                screen.AnimationName = "Main";
                screen.Texts.Create(fontLogo.Name, "SINGLE PLAYER", x, y);
                screen.Texts.Create(fontLogo.Name, "MULTI PLAYER", x, y + height);
                screen.Texts.Create(fontLogo.Name, "QUIT", x, y + (height * 3));
                screen.Texts.Create(fontLogo.Name, "0", xSelector, y + height - 1);
                screen.Actions.Create(InputType.Button1, ActionType.GoToScreen, "SinglePlayer");
                screen.Actions.Create(InputType.Right, ActionType.StartGameOnline);
                screen.Actions.Create(InputType.Down, ActionType.GoToScreen, "Quit");
                //Quit
                screen = game.Screens.Create(ScreenType.Intermediate, "Quit");
                screen.AnimationName = "Main";
                screen.Texts.Create(fontLogo.Name, "SINGLE PLAYER", x, y);
                screen.Texts.Create(fontLogo.Name, "MULTI PLAYER", x, y + height);
                screen.Texts.Create(fontLogo.Name, "QUIT", x, (y + (height * 3)));
                screen.Texts.Create(fontLogo.Name, "0", xSelector, (y + (height * 3)));
                screen.Actions.Create(InputType.Button1, ActionType.GoToScreen, "MultiPlayer");
                screen.Actions.Create(InputType.Right, ActionType.Quit);
                screen.Actions.Create(InputType.Down, ActionType.GoToScreen, "SinglePlayer");
                screen.Actions.Create(x, y, 437, height, ActionType.StartGame);
                screen.Actions.Create(x, y + (height * 3), 126, height, ActionType.Quit);
                //Loading
                screen = game.Screens.Create(ScreenType.Loading, "LoadGame");
                screen.Texts.Create(font.Name, "LOADING", (game.Width / 2), game.Height / 2, TextAlign.Middle);
                //Error
                screen = game.Screens.Create(ScreenType.Error, "Error");
                screen.Texts.Create(font.Name, "ERROR", (game.Width / 2), game.Height / 2, TextAlign.Middle);
                screen.Texts.Create(font.Name, "[ERROR]", (game.Width / 2), (game.Height / 2) + height, TextAlign.Middle);
                screen.Actions.Create(0, 0, width, height, ActionType.GoToScreen, "SinglePlayer");
                //Round Over Win
                screen = game.Screens.Create(ScreenType.RoundOverWin, "RoundOverWin");
                screen.Texts.Create(fontLogo.Name, string.Format("[{0}] WINS", Property.PROPERTY_WINNER), (game.Width / 2) - 20, game.Height / 2, TextAlign.Middle);
                //Round Over Draw
                screen = game.Screens.Create(ScreenType.RoundOverDraw, "RoundOverDraw");
                screen.Texts.Create(fontLogo.Name, "DRAW", (game.Width / 2), game.Height / 2, TextAlign.Middle);
                //Match Over 
                screen = game.Screens.Create(ScreenType.MatchOver, "MatchOver");
                screen.Texts.Create(fontLogo.Name, "CONGRATULATIONS", (game.Width / 2), (game.Height / 2) - 40, TextAlign.Middle);
                screen.Texts.Create(fontLogo.Name, string.Format("[{0}] IS THE WINNER", Property.PROPERTY_WINNER), (game.Width / 2), game.Height / 2, TextAlign.Middle);
                //Game
                screen = game.Screens.Create(ScreenType.Game, "Game");
                screen.Texts.Create(font.Name, "BLUE  [BLUE]", 10, 10);
                screen.Texts.Create(font.Name, "GREEN [GREEN]", 10, 30);
                screen.Texts.Create(font.Name, "WHITE [WHITE]", 10, 50);
                screen.Texts.Create(font.Name, "GRAY  [GRAY]", 10, 70);
                //Touch Buttons
                int buttonSize = 60;
                int buttonMargin = 20;
                int buttonMarginDown = 50;
                string buttonAnimationEscape = "button_escape";
                string buttonAnimationLeft = "button_left";
                string buttonAnimationRight = "button_right";
                string buttonAnimationFire = "button_fire";
                //Escape
                Data.Image image = CreateImage(game, string.Format("{0}{1}.png", path, buttonAnimationEscape));
                spriteMap = game.SpritesMaps.Create(image.Name, image.Name);
                spriteMap.Sprites.Create(0, 0, 0, 0, buttonSize, buttonSize);
                animation = game.Animations.Create(image.Name);
                animation.Frames.Create(image.Name);
                screen.TouchButtons.Create(buttonMargin, null, buttonMargin, null, buttonSize, buttonSize, InputType.Escape, image.Name);
                //Left
                image = CreateImage(game, string.Format("{0}{1}.png", path, buttonAnimationLeft));
                spriteMap = game.SpritesMaps.Create(image.Name, image.Name);
                spriteMap.Sprites.Create(0, 0, 0, 0, buttonSize, buttonSize);
                animation = game.Animations.Create(image.Name);
                animation.Frames.Create(image.Name);
                screen.TouchButtons.Create(null, buttonMarginDown + buttonSize, buttonMargin, null, buttonSize, buttonSize, InputType.Left, buttonAnimationLeft);
                //Right
                image = CreateImage(game, string.Format("{0}{1}.png", path, buttonAnimationRight));
                spriteMap = game.SpritesMaps.Create(image.Name, image.Name);
                spriteMap.Sprites.Create(0, 0, 0, 0, buttonSize, buttonSize);
                animation = game.Animations.Create(image.Name);
                animation.Frames.Create(image.Name);
                screen.TouchButtons.Create(null, buttonMarginDown + buttonSize, buttonSize + (buttonMargin * 2), null, buttonSize, buttonSize, InputType.Right, buttonAnimationRight);
                //Fire
                image = CreateImage(game, string.Format("{0}{1}.png", path, buttonAnimationFire));
                spriteMap = game.SpritesMaps.Create(image.Name, image.Name);
                spriteMap.Sprites.Create(0, 0, 0, 0, buttonSize, buttonSize);
                animation = game.Animations.Create(image.Name);
                animation.Frames.Create(image.Name);
                screen.TouchButtons.Create(null, buttonMarginDown + buttonSize, null, buttonMargin + buttonSize, buttonSize, buttonSize, InputType.Button1, buttonAnimationFire); 
            }

            private Data.Font CreateFonts(Resource game, string imageName, string transparency)
            {
                Data.Font font = game.Fonts.Create("Font1", imageName, transparency);
                string characters = " ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                List<int> xs = new List<int>() { 403, 415, 427, 439, 450, 461, 472, 482, 494, 507, 514, 524, 537, 547, 561, 572, 584, 596, 607, 620, 630, 641, 653, 663, 678, 689, 700, 709 };
                int y = 521;
                for(int i = 0; i < characters.Length;i++)    
                    font.Characters.Create(characters[i].ToString(), xs[i], y, xs[i + 1] - xs[i], 14);
                characters = "0123456789";
                xs = new List<int>() { 579, 592, 601, 611, 621, 632, 642, 652, 662, 672, 682 };
                y = 107;
                for (int i = 0; i < characters.Length; i++) 
                    font.Characters.Create(characters[i].ToString(), xs[i], y, xs[i + 1] - xs[i], 14);
                font.Characters.Create(">", 569, 389, 8, 13);
                return (font);
            }

            private Data.Font CreateFontLogo(Data.Resource game, string imageFontLogoName, string transparency)
            {
                Data.Font font = game.Fonts.Create(imageFontLogoName, imageFontLogoName, transparency);
                string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
                List<int> xs = new List<int>() { 16, 50, 86, 121, 155, 191, 226, 262, 296, 316, 351, 386, 419, 457, 492, 527, 562, 597, 633, 667, 703, 738, 773, 811, 845, 880, 916, 950, 975, 1008, 1045, 1080, 1115, 1151, 1186, 1221, 1256, 1290, 1320 };
                int y = 8;
                for (int i = 0; i < characters.Length; i++)
                    font.Characters.Create(characters[i].ToString(), xs[i], y, xs[i + 1] - xs[i], 37);
                return (font);
            }

            private void CreateGameAirplaneMapBlank(ResourceGenerator generator, Resource game, string path, string imageName, string transparency, int border)
            {
                Resource map = this._resources.Create(ResourceType.Map, generator.GetCode(), "Blank");
                map.Width = 600;
                map.Height = 600;
                int velocity = 10;
                map.Spawns.Create(1, (border * 2), (game.Height / 2), velocity, 0, "Right", "BLUE");
                map.Spawns.Create(1, (game.Width) - (border * 2), (game.Height / 2), -velocity, 0, "Left", "GREEN");
                Data.Image image = CreateImage(map, string.Format("{0}{1}.png", path, imageName)); 
                SpriteMap spriteMap = map.SpritesMaps.Create(image.Name, "Blank", transparency);
                Animation animation = map.Animations.Create("Blank");
                animation.Frames.Create("Blank", 1);
                map.Triggers.CreateAnimation("Blank", new IntRandom(0, 0));
            }

            private void CreateGameAirplaneMapOcean(ResourceGenerator generator, Resource game, string path, string imageName, string transparency, int border) 
            {
                //int wingman = 50;
                Resource map = this._resources.Create(ResourceType.Map, generator.GetCode(), "Ocean");
                map.Width = game.Width;
                map.Height = game.Height;
                int velocity = 10;
                map.Spawns.Create(1, (game.Width / 2), game.Height - (border * 2), 0, -velocity, "Up", "BLUE");
                map.Spawns.Create(1, (game.Width / 2), (border * 2), 0, velocity, "Down", "GRAY");
                map.Spawns.Create(1, (border * 2), (game.Height / 2), velocity, 0, "Right", "GREEN");
                map.Spawns.Create(1, (game.Width) - (border * 2), (game.Height / 2), -velocity, 0, "Left", "WHITE");
                //map.Spawns.Create(1, (game.Width / 2) - wingman, game.Height - border, -velocity / 2, -velocity / 2, "UpLeft", "BLUE");
                //map.Spawns.Create(1, (game.Width / 2) + wingman, game.Height - border, velocity / 2, -velocity / 2, "UpRight", "BLUE");
                //map.Spawns.Create(1, border, (game.Height / 2) - wingman, velocity / 2, -velocity / 2, "UpRight", "GREEN");
                //map.Spawns.Create(1, border, (game.Height / 2) + wingman, velocity / 2, velocity / 2, "DownRight", "GREEN");
                //map.Spawns.Create(1, (game.Width) - border, (game.Height / 2) - wingman, -velocity / 2, -velocity / 2, "UpLeft", "WHITE");
                //map.Spawns.Create(1, (game.Width) - border, (game.Height / 2) + wingman, -velocity / 2, velocity / 2, "DownLeft", "WHITE");
                //map.Spawns.Create(1, (game.Width / 2) - wingman, border, -velocity / 2, velocity / 2, "DownLeft", "GRAY");
                //map.Spawns.Create(1, (game.Width / 2) + wingman, border, velocity / 2, velocity / 2, "DownRight", "GRAY");
                Data.Image image = CreateImage(map, string.Format("{0}{1}.png", path, imageName)); 
                SpriteMap spriteMap = map.SpritesMaps.Create(image.Name, "Ocean", transparency);
                int width = 30;
                int height = 30;
                int rows = 20;
                int columns = 27;
                for (int row = 0; row < rows; row++)
                    for (int column = 0; column < columns; column++)
                        spriteMap.Sprites.Create(column * width, row * height, 269, 368, width, height, false);
                //Map Ocean Island1
                spriteMap.Sprites.Create(200, 200, 103, 499, 64, 64);
                //Map Ocean Island2
                spriteMap.Sprites.Create(400, 100, 168, 499, 64, 64);
                //Map Ocean Island3
                spriteMap.Sprites.Create(500, 50, 233, 499, 64, 64);
                Animation animation = map.Animations.Create("Ocean");
                animation.Frames.Create("Ocean", 1);
                map.Triggers.CreateAnimation("Ocean", new IntRandom(0, 0));
                //Map Ocean Boat1
                Resource boat1 = map.Create(generator.GetCode(),"Boat1");
                boat1.SpritesMaps.Create(image.Name, "Boat1x6", transparency).Sprites.Create(0, 0, 367, 103, 31, 97);
                boat1.SpritesMaps.Create(image.Name, "Boat1x5", transparency).Sprites.Create(0, 0, 400, 103, 31, 97);
                boat1.SpritesMaps.Create(image.Name, "Boat1x4", transparency).Sprites.Create(0, 0, 433, 103, 31, 97);
                boat1.SpritesMaps.Create(image.Name, "Boat1x3", transparency).Sprites.Create(0, 0, 466, 103, 31, 97);
                boat1.SpritesMaps.Create(image.Name, "Boat1x2", transparency).Sprites.Create(0, 0, 499, 103, 31, 97);
                boat1.SpritesMaps.Create(image.Name, "Boat1x1", transparency).Sprites.Create(0, 0, 532, 103, 31, 97);
                //Animation 
                animation = boat1.Animations.Create("UpDown", false);
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
                foreach (Animation animationHitBoxes in boat1.Animations)
                {
                    foreach (Frame frame in animationHitBoxes.Frames)
                    {
                        frame.HitBoxesDamage.Create(10, 10, 11, 77).Hit = 1;
                        frame.HitBoxesVulnerable.Create(10, 10, 11, 77);
                    }
                }
                //Projectile
                Resource projectile = boat1.Create(generator.GetCode(), "Projectile");
                projectile.SpritesMaps.Create(image.Name, "Projectile", transparency).Sprites.Create(0, 0, 268, 169, 31, 31);
                projectile.Animations.Create("Projectile").Frames.Create("Projectile", 1);
                foreach (Animation animationHitBoxes in projectile.Animations)
                {
                    foreach (Frame frame in animationHitBoxes.Frames)
                    {
                        frame.HitBoxesDamage.Create(10, 10, 11, 11).Hit = 1;
                        frame.HitBoxesVulnerable.Create(10, 10, 11, 11);
                    }
                }
                //Commands
                boat1.Commands.Create("Die", "Die", 0, 0, "System");
                boat1.Commands.Create("Fire", "Projectile", 15, -15, "UpDown", "Projectile", 1, 45, -45).Sequences.Create().Add(InputType.Button1);
                boat1.Commands.Create("Fire", "Projectile", 0, -20, "UpDown", "Projectile", 1, 0, -60).Sequences.Create().Add(InputType.Button1);
                boat1.Commands.Create("Fire", "Projectile", -15, -15, "UpDown", "Projectile", 1, -45, -45).Sequences.Create().Add(InputType.Button1);
                boat1.Commands.Create("Fire", "Projectile", -20, 0, "UpDown", "Projectile", 1, -60, 0).Sequences.Create().Add(InputType.Button1);
                boat1.Commands.Create("Fire", "Projectile", -15, 15, "UpDown", "Projectile", 1, -45, 45).Sequences.Create().Add(InputType.Button1);
                boat1.Commands.Create("Fire", "Projectile", 0, 20, "UpDown", "Projectile", 1, 0, 60).Sequences.Create().Add(InputType.Button1);
                boat1.Commands.Create("Fire", "Projectile", 15, 15, "UpDown", "Projectile", 1, 45, 45).Sequences.Create().Add(InputType.Button1);
                boat1.Commands.Create("Fire", "Projectile", 20, 0, "UpDown", "Projectile", 1, 60, 0).Sequences.Create().Add(InputType.Button1);
                //Trigger
                boat1.Triggers.CreateFactory("UpDown", new IntRandom(30, 150), new IntRandom(1000), new IntRandom(50, game.Width - 50), new IntRandom(300, game.Height - 50), new IntRandom(0,0), new IntRandom(0,0));
                //Map AirplaneGreen
                Resource airplaneGreen = map.Create(generator.GetCode(),"AirplaneGreen");
                airplaneGreen.SpritesMaps.Create(image.Name, "AirplaneGreenx1", transparency).Sprites.Create(0, 0, 268, 400, 64, 64);
                airplaneGreen.SpritesMaps.Create(image.Name, "AirplaneGreenx2", transparency).Sprites.Create(0, 0, 334, 400, 64, 64);
                airplaneGreen.SpritesMaps.Create(image.Name, "AirplaneGreenx3", transparency).Sprites.Create(0, 0, 400, 400, 64, 64);
                animation = airplaneGreen.Animations.Create("Fly");
                animation.Frames.Create("AirplaneGreenx1", 5);
                animation.Frames.Create("AirplaneGreenx2", 5);
                animation.Frames.Create("AirplaneGreenx3", 5);
                foreach (Animation animationHitBoxes in airplaneGreen.Animations)
                {
                    foreach (Frame frame in animationHitBoxes.Frames)
                    {
                        frame.HitBoxesDamage.Create(20, 10, 24, 50).Hit = 1;
                        frame.HitBoxesVulnerable.Create(20, 10, 24, 50);
                    }
                }
                //Projectile
                projectile = airplaneGreen.Create(generator.GetCode(), "Projectile");
                projectile.SpritesMaps.Create(image.Name, "Projectile", transparency).Sprites.Create(0, 0, 268, 169, 31, 31);
                projectile.Animations.Create("Projectile").Frames.Create("Projectile", 1);
                foreach (Animation animationHitBoxes in projectile.Animations)
                {
                    foreach (Frame frame in animationHitBoxes.Frames)
                    {
                        frame.HitBoxesDamage.Create(10, 10, 11, 11).Hit = 1;
                        frame.HitBoxesVulnerable.Create(10, 10, 11, 11);
                    }
                }
                //Commands
                airplaneGreen.Commands.Create("Die", "Die", 0, 0, "System");
                airplaneGreen.Commands.Create("Fire", "Projectile", 15, -15, "Fly", "Projectile", 1, 45, -45).Sequences.Create().Add(InputType.Button1);
                airplaneGreen.Commands.Create("Fire", "Projectile", 0, -20, "Fly", "Projectile", 1, 0, -60).Sequences.Create().Add(InputType.Button1);
                airplaneGreen.Commands.Create("Fire", "Projectile", -15, -15, "Fly", "Projectile", 1, -45, -45).Sequences.Create().Add(InputType.Button1);
                //Trigger
                airplaneGreen.Triggers.CreateFactory("Fly", new IntRandom(100), new IntRandom(1000), new IntRandom(50, game.Width - 50), new IntRandom(game.Height - 50), new IntRandom(0,0), new IntRandom(-5));
            }

            private void CreateGameAirplaneActor(ResourceGenerator generator, string name, string imageName, string transparency, string path, int player)
            {
                Resource actor = this._resources.Create(ResourceType.Actor, generator.GetCode(), name);
                Data.Image image = CreateImage(actor, string.Format("{0}{1}.png", path, imageName));
                int xImage = 4;
                int yImage = 37 + (player * 33);
                actor.SpritesMaps.Create(image.Name, "Down", transparency).Sprites.Create(0, 0, xImage + (33 * 0), yImage, 31, 31);
                actor.SpritesMaps.Create(image.Name, "DownRight", transparency).Sprites.Create(0, 0, xImage + (33 * 1), yImage, 31, 31);
                actor.SpritesMaps.Create(image.Name, "Right", transparency).Sprites.Create(0, 0, xImage + (33 * 2), yImage, 31, 31);
                actor.SpritesMaps.Create(image.Name, "UpRight", transparency).Sprites.Create(0, 0, xImage + (33 * 3), yImage, 31, 31);
                actor.SpritesMaps.Create(image.Name, "Up", transparency).Sprites.Create(0, 0, xImage + (33 * 4), yImage, 31, 31);
                actor.SpritesMaps.Create(image.Name, "UpLeft", transparency).Sprites.Create(0, 0, xImage + (33 * 5), yImage, 31, 31);
                actor.SpritesMaps.Create(image.Name, "Left", transparency).Sprites.Create(0, 0, xImage + (33 * 6), yImage, 31, 31);
                actor.SpritesMaps.Create(image.Name, "DownLeft", transparency).Sprites.Create(0, 0, xImage + (33 * 7), yImage, 31, 31);
                actor.SpritesMaps.Create(image.Name, "Die1", transparency).Sprites.Create(0, 0, 70, 169, 31, 31);
                actor.SpritesMaps.Create(image.Name, "Die2", transparency).Sprites.Create(0, 0, 103, 169, 31, 31);
                actor.SpritesMaps.Create(image.Name, "Die3", transparency).Sprites.Create(0, 0, 136, 169, 31, 31);
                actor.SpritesMaps.Create(image.Name, "Die4", transparency).Sprites.Create(0, 0, 169, 169, 31, 31);
                actor.SpritesMaps.Create(image.Name, "Die5", transparency).Sprites.Create(0, 0, 202, 169, 31, 31);
                actor.SpritesMaps.Create(image.Name, "Die6", transparency).Sprites.Create(0, 0, 235, 169, 31, 31);
                actor.SpritesMaps.Create(image.Name, "Blank", transparency).Sprites.Create(0, 0, 268, 202, 31, 31);
                actor.Animations.Create("Down").Frames.Create("Down", 1);
                actor.Animations.Create("DownRight").Frames.Create("DownRight", 1);
                actor.Animations.Create("Right").Frames.Create("Right", 1);
                actor.Animations.Create("UpRight").Frames.Create("UpRight", 1);
                actor.Animations.Create("Up").Frames.Create("Up", 1);
                actor.Animations.Create("UpLeft").Frames.Create("UpLeft", 1);
                actor.Animations.Create("Left").Frames.Create("Left", 1);
                actor.Animations.Create("DownLeft").Frames.Create("DownLeft", 1);
                foreach (Animation animationHitBoxes in actor.Animations)
                {
                    foreach (Frame frame in animationHitBoxes.Frames)
                    {
                        frame.HitBoxesDamage.Create(10, 10, 11, 11).Hit = 1;
                        frame.HitBoxesVulnerable.Create(10, 10, 11, 11);
                    }
                }
                Animation animation = actor.Animations.Create("Die");
                animation.Frames.Create("Die6", 5);
                animation.Frames.Create("Die5", 5);
                animation.Frames.Create("Die4", 5);
                animation.Frames.Create("Die3", 5);
                animation.Frames.Create("Die2", 5);
                animation.Frames.Create("Die1", 5);
                animation.Frames.Create("Die2", 5);
                animation.Frames.Create("Die3", 5);
                animation.Frames.Create("Die4", 5);
                animation.Frames.Create("Die5", 5);
                animation.Frames.Create("Die6", 5);
                animation.Frames.Create("Blank", 5);
                //Projectile
                Resource projectile = actor.Create(generator.GetCode(), "Projectile");
                projectile.SpritesMaps.Create(image.Name, "Projectile", transparency).Sprites.Create(0, 0, 268, 169, 31, 31);
                projectile.Animations.Create("Projectile").Frames.Create("Projectile", 1);
                foreach (Animation animationHitBoxes in projectile.Animations)
                {
                    foreach (Frame frame in animationHitBoxes.Frames)
                    {
                        frame.HitBoxesDamage.Create(10, 10, 11, 11).Hit = 1;
                        frame.HitBoxesVulnerable.Create(10, 10, 11, 11);
                    }
                }
                //Commands
                actor.Commands.Create("Die", "Die", 0, 0, "System");
                actor.Commands.Create("Left", "Up", 0, -10, "UpRight").Sequences.Create().Add(InputType.Left);
                actor.Commands.Create("Left", "UpLeft", -5, -5, "Up").Sequences.Create().Add(InputType.Left);
                actor.Commands.Create("Left", "Left", -10, 0, "UpLeft").Sequences.Create().Add(InputType.Left);
                actor.Commands.Create("Left", "DownLeft", -5, +5, "Left").Sequences.Create().Add(InputType.Left);
                actor.Commands.Create("Left", "Down", 0, 10, "DownLeft").Sequences.Create().Add(InputType.Left);
                actor.Commands.Create("Left", "DownRight", 5, 5, "Down").Sequences.Create().Add(InputType.Left);
                actor.Commands.Create("Left", "Right", 10, 0, "DownRight").Sequences.Create().Add(InputType.Left);
                actor.Commands.Create("Left", "UpRight", 5, -5, "Right").Sequences.Create().Add(InputType.Left);
                actor.Commands.Create("Right", "Up", 0, -10, "UpLeft").Sequences.Create().Add(InputType.Right);
                actor.Commands.Create("Right", "UpRight", 5, -5, "Up").Sequences.Create().Add(InputType.Right);
                actor.Commands.Create("Right", "Right", 10, 0, "UpRight").Sequences.Create().Add(InputType.Right);
                actor.Commands.Create("Right", "DownRight", 5, 5, "Right").Sequences.Create().Add(InputType.Right);
                actor.Commands.Create("Right", "Down", 0, 10, "DownRight").Sequences.Create().Add(InputType.Right);
                actor.Commands.Create("Right", "DownLeft", -5, +5, "Down").Sequences.Create().Add(InputType.Right);
                actor.Commands.Create("Right", "Left", -10, 0, "DownLeft").Sequences.Create().Add(InputType.Right);
                actor.Commands.Create("Right", "UpLeft", -5, -5, "Left").Sequences.Create().Add(InputType.Right);
                actor.Commands.Create("Fire", "Projectile", 15, -15, "UpRight", "Projectile", 1, 45, -45).Sequences.Create().Add(InputType.Button1);
                actor.Commands.Create("Fire", "Projectile", 0, -20, "Up", "Projectile", 1, 0, -60).Sequences.Create().Add(InputType.Button1);
                actor.Commands.Create("Fire", "Projectile", -15, -15, "UpLeft", "Projectile", 1, -45, -45).Sequences.Create().Add(InputType.Button1);
                actor.Commands.Create("Fire", "Projectile", -20, 0, "Left", "Projectile", 1, -60, 0).Sequences.Create().Add(InputType.Button1);
                actor.Commands.Create("Fire", "Projectile", -15, 15, "DownLeft", "Projectile", 1, -45, 45).Sequences.Create().Add(InputType.Button1);
                actor.Commands.Create("Fire", "Projectile", 0, 20, "Down", "Projectile", 1, 0, 60).Sequences.Create().Add(InputType.Button1);
                actor.Commands.Create("Fire", "Projectile", 15, 15, "DownRight", "Projectile", 1, 45, 45).Sequences.Create().Add(InputType.Button1);
                actor.Commands.Create("Fire", "Projectile", 20, 0, "Right", "Projectile", 1, 60, 0).Sequences.Create().Add(InputType.Button1);
            }
        #endregion
        #region Tank
            private void tankFulllToolStripMenuItem_Click(object sender, EventArgs e)
            {
                ResourceGenerator generator = new ResourceGenerator("T");
                StorageWindows.PathDefault = @"C:\Files\Projects\Cica\Games\Tank";
                this._storage.Path = StorageWindows.GetPathFolderResources();
                string path = StorageWindows.GetPathFolderImages();
                string imageName = "Tanks";
                string transparency = "A3CD3F";
                this._resources.Clear();
                //Game
                int border = 30;
                Resource game = this._resources.Create(ResourceType.Game, generator.GetCode(), "Tank");
                game.Width = 800;
                game.Height = 600;
                game.WarnUp = 0;
                game.FramesPerSecond = 30;
                game.InputsPerSecond = 3;
                game.NetworksPerSecond = 5;
                game.Rounds = 3;
                Data.Image image = CreateImage(game, string.Format("{0}{1}.png", path, imageName));
                //Fonts
                //Screens
                CreateScreensTank(game, path);
                //Maps 
                CreatMapsTank(generator, game, path, imageName, transparency, border);
                //Actor Yellow
                CreateTansksActor(generator, "Yellow", "yellow", transparency, path);
                //Actor White
                CreateTansksActor(generator, "Grey", "grey", transparency, path);
                //Actor Green
                CreateTansksActor(generator, "Green", "green", transparency, path);
                //Actor Blue
                CreateTansksActor(generator, "Blue", "blue", transparency, path);
                this._resources.SaveAll();
            }

            private void CreateScreensTank(Data.Resource game, string path)
            {
                //Game
                Data.Screen screen = game.Screens.Create(ScreenType.Game, "Game");
            }

            private void CreatMapsTank(ResourceGenerator generator, Resource game, string path, string imageName, string transparency, int border)
            {
                Resource map = this._resources.Create(ResourceType.Map, generator.GetCode(), "Blank");
                map.Width = 800;
                map.Height = 600;
                int velocity = 10;
                map.Spawns.Create(1, (game.Width / 2), game.Height - (border * 2), 0, -velocity, "Up", "BLUE");
                map.Spawns.Create(1, (game.Width / 2), (border * 2), 0, velocity, "Down", "GRAY");
                map.Spawns.Create(1, (border * 2), (game.Height / 2), velocity, 0, "Right", "GREEN");
                map.Spawns.Create(1, (game.Width) - (border * 2), (game.Height / 2), -velocity, 0, "Left", "WHITE");
                Data.Image image = CreateImage(map, string.Format("{0}{1}.png", path, imageName));
                SpriteMap spriteMap = map.SpritesMaps.Create(image.Name, "Blank", transparency);
                Animation animation = map.Animations.Create("Blank");
                animation.Frames.Create("Blank", 1);
                map.Triggers.CreateAnimation("Blank", new IntRandom(0, 0));
            }

            private void CreateTansksActor(ResourceGenerator generator, string name, string imageBaseName, string transparency, string path)
            {
                Resource actor = this._resources.Create(ResourceType.Actor, generator.GetCode(), name);
                //Left
                Data.Image image = CreateImage(actor, string.Format("{0}t_{1}1.png", path, imageBaseName));
                actor.SpritesMaps.Create(image.Name, "Left", transparency).Sprites.Create(0, 0, 0, 0, image.Width, image.Height);
                //Right
                image = CreateImage(actor, string.Format("{0}t_{1}2.png", path, imageBaseName));
                actor.SpritesMaps.Create(image.Name, "Right", transparency).Sprites.Create(0, 0, 0, 0, image.Width, image.Height);
                //Up
                image = CreateImage(actor, string.Format("{0}t_{1}3.png", path, imageBaseName));
                actor.SpritesMaps.Create(image.Name, "Up", transparency).Sprites.Create(0, 0, 0, 0, image.Width, image.Height);
                //Down
                image = CreateImage(actor, string.Format("{0}t_{1}4.png", path, imageBaseName));
                actor.SpritesMaps.Create(image.Name, "Down", transparency).Sprites.Create(0, 0, 0, 0, image.Width, image.Height);
                //Die
                image = CreateImage(actor, string.Format("{0}explosion1.png", path));
                actor.SpritesMaps.Create(image.Name, "Die1", transparency).Sprites.Create(0, 0, 0, 0, image.Width, image.Height);
                image = CreateImage(actor, string.Format("{0}explosion2.png", path));
                actor.SpritesMaps.Create(image.Name, "Die2", transparency).Sprites.Create(0, 0, 0, 0, image.Width, image.Height);
                image = CreateImage(actor, string.Format("{0}explosion3.png", path));
                actor.SpritesMaps.Create(image.Name, "Die3", transparency).Sprites.Create(0, 0, 0, 0, image.Width, image.Height);
                image = CreateImage(actor, string.Format("{0}explosion4.png", path));
                actor.SpritesMaps.Create(image.Name, "Die4", transparency).Sprites.Create(0, 0, 0, 0, image.Width, image.Height);
                image = CreateImage(actor, string.Format("{0}explosion5.png", path));
                actor.SpritesMaps.Create(image.Name, "Die5", transparency).Sprites.Create(0, 0, 0, 0, image.Width, image.Height);
                actor.SpritesMaps.Create(image.Name, "Blank", transparency).Sprites.Create(0, 0, 0, 0, 1, 1);
                //Animations
                actor.Animations.Create("Down").Frames.Create("Down", 1);
                actor.Animations.Create("Right").Frames.Create("Right", 1);
                actor.Animations.Create("Up").Frames.Create("Up", 1);
                actor.Animations.Create("Left").Frames.Create("Left", 1);
                foreach (Animation animationHitBoxes in actor.Animations)
                {
                    foreach (Frame frame in animationHitBoxes.Frames)
                    {
                        frame.HitBoxesDamage.Create(10, 10, 11, 11).Hit = 1;
                        frame.HitBoxesVulnerable.Create(10, 10, 11, 11);
                    }
                }
                Animation animation = actor.Animations.Create("Die");
                animation.Frames.Create("Die1", 5);
                animation.Frames.Create("Die2", 5);
                animation.Frames.Create("Die3", 5);
                animation.Frames.Create("Die4", 5);
                animation.Frames.Create("Die5", 5);
                animation.Frames.Create("Blank", 5);
                //Projectile
                Resource projectile = actor.Create(generator.GetCode(), "Mine");
                image = CreateImage(actor, string.Format("{0}m_{1}.png", path, imageBaseName));
                projectile.SpritesMaps.Create(image.Name, "Mine", transparency).Sprites.Create(0, 0, 0, 0, image.Width, image.Height);
                projectile.Animations.Create("Mine").Frames.Create("Mine", 1);
                foreach (Animation animationHitBoxes in projectile.Animations)
                {
                    foreach (Frame frame in animationHitBoxes.Frames)
                    {
                        frame.HitBoxesDamage.Create(0, 0, 10, 10).Hit = 1;
                        frame.HitBoxesVulnerable.Create(0, 0, 10, 10);
                    }
                }
                //Commands
                actor.Commands.Create("Die", "Die", 0, 0, "System");
                actor.Commands.Create("Up", "Up", 0, -10).Sequences.Create().Add(InputType.Top);
                actor.Commands.Create("Left", "Left", -10, 0).Sequences.Create().Add(InputType.Left);
                actor.Commands.Create("Down", "Down", 0, 10).Sequences.Create().Add(InputType.Down);
                actor.Commands.Create("Right", "Right", 10, 0).Sequences.Create().Add(InputType.Right);
                actor.Commands.Create("Mine", "Mine", 0, 0, "Up", "Mine", 1, 5, 20).Sequences.Create().Add(InputType.Button1);
                actor.Commands.Create("Mine", "Mine", 0, 0, "Left", "Mine", 1, 20, 5).Sequences.Create().Add(InputType.Button1);
                actor.Commands.Create("Mine", "Mine", 0, 0, "Down", "Mine", 1, 5, -20).Sequences.Create().Add(InputType.Button1);
                actor.Commands.Create("Mine", "Mine", 0, 0, "Right", "Mine", 1, -20, 5).Sequences.Create().Add(InputType.Button1);
            }
        #endregion
    }
}
