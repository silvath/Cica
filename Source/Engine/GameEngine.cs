using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class GameEngine
    {
        #region Attributes
            private ResourceManager _resources;
            private InputManager _inputs;
            private TouchManager _touches;
            private Canvas _canvas;
            private GameState _gameState;
            private PlayerLocal _player;
            private List<IPlayer> _computers;
            private int _viewWidth;
            private int _viewHeight;
            private CacheManager _cache;
        #endregion
        #region Properties
            public int Width 
            {
                get 
                {
                    return (this._gameState.Game.Width);
                }
            }

            public int Height
            {
                get
                {
                    return (this._gameState.Game.Height);
                }
            }

            public int ViewWidth 
            {
                set 
                {
                    this._viewWidth = value;
                    this.CalculateScale();
                }
                get 
                {
                    return (this._viewWidth);
                }
            }

            public int ViewHeight
            {
                set
                {
                    this._viewHeight = value;
                    this.CalculateScale();
                }
                get
                {
                    return (this._viewHeight);
                }
            }
            public float Scale { set; get; }
            public float ViewTop { set; get; }
            public float ViewLeft { set; get; }
            public float ClippingTop { set; get; }
            public float ClippingLeft { set; get; }
            public float ClippingWidth { set; get; }
            public float ClippingHeight { set; get; }

            private long TickLast { set; get; }
            private long TickFramesSum { set; get; }
            private long TickFrame { set; get; }
            private long TickInputsSum { set; get; }
            private long TickInput { set; get; }
            private long TickNetworksSum { set; get; }
            private long TickNetwork { set; get; }
            public bool Debug { set; get; }
            public bool Exit { set; get;}
            public bool Paused { set; get; }
            public bool Updated { set; get; }
            public Status Status{set;get;}
            public int Sleep { set; get; }
            public int SleepFrames { set; get; }
            public bool Keyboard { set; get; }
            public bool TouchButtonProximity { set; get; }
            public GameState State 
            {
                get 
                {
                    return(this._gameState);
                }
            }

            public GameEngineType Type { set; get; }
            private NetworkManager Network { set; get; }
            public QueryManager Query { set; get; }
        #endregion
        #region Constructor
            public GameEngine(ResourceManager resources, Canvas canvas, NetworkManager network, QueryManager query) 
            {
                this._resources = resources;
                this._inputs = new InputManager();
                this._touches = new TouchManager(this._inputs);
                this._cache = new CacheManager();
                this._canvas = canvas;
                this._player = new PlayerLocal(this._inputs);
                this._computers = new List<IPlayer>();
                this._computers.Add(new Computer());
                this.Debug = false;
                this.Exit = false;
                this.Status = Engine.Status.Menu;
                this.Keyboard = true;
                this.TouchButtonProximity = true;
                this.Type = GameEngineType.Single;
                this.Network = network;
                if (this.Network != null)
                    this.Network.Engine = this;
                this.Query = query;
            }
        #endregion

        #region Initialize
            public void Initialize(string game) 
            {
                //Load
                this._resources.LoadAll();
                //GameState
                this._gameState = new GameState();
                //Game
                this._gameState.Game = this._resources.GetResource(ResourceType.Game, game);
                //Control
                this.TickFrame = (10000000) / this._gameState.Game.FramesPerSecond;
                this.TickInput = (10000000) / this._gameState.Game.InputsPerSecond;
                this.TickNetwork = (10000000) / this._gameState.Game.NetworksPerSecond;
                //Screen
                this.InitializeScreen(this._gameState.Game.Screens.GetScreen(ScreenType.Start));
            }

            private void ResetTicks() 
            {
                //Tick
                this.TickLast = DateTime.Now.Ticks;
                this.TickFramesSum = this.TickInputsSum = this.TickNetworksSum = 0;
                //Sleep
                this.Sleep = this._gameState.Game.WarnUp;
                this.SleepFrames = this._gameState.Game.FramesPerSecond;
                if (this.Sleep > 0)
                    this.Status = Engine.Status.WarnUp;
            }
        #endregion

        #region Draw
            private List<Tuple<ImageCollection, SpriteMap, Frame, ResourceState>> CreateSnapshotSpriteMaps() 
            {
                List<Tuple<ImageCollection, SpriteMap, Frame, ResourceState>> snapshot = new List<Tuple<ImageCollection, SpriteMap, Frame, ResourceState>>();
                foreach(AnimationState animationState in this._gameState.AnimationStates)
                {
                    Frame frame = animationState.Animation.Frames[animationState.FrameCurrent];
                    SpriteMap spriteMap = animationState.Resource.SpritesMaps.GetSpriteMap(frame.SpriteMapName);
                    snapshot.Add(new Tuple<ImageCollection, SpriteMap, Frame, ResourceState>(animationState.ResourceRoot.Images, spriteMap, frame, animationState.ResourceState));
                }
                return (snapshot);
            }

            public Layer CreateLayer(Resource resource, string animationName, int x, int y) 
            {
                Animation animation = resource.Animations.GetAnimation(animationName);
                Frame frame = animation.Frames[0];
                SpriteMap spriteMap = resource.SpritesMaps.GetSpriteMap(frame.SpriteMapName);
                List<Layer> layers =  this.CreateLayers(resource.Images,spriteMap, x, y);
                return (layers.Count > 0 ? layers[0] : null);
            }

            public List<Layer> CreateLayers(ImageCollection images, SpriteMap spriteMap, int x, int y, bool isMultiLayer = true) 
            {
                if (spriteMap.CacheLayers == null)
                {
                    spriteMap.CacheLayers = new List<Layer>();
                    if (isMultiLayer)
                    {
                        //Multi Layer
                        foreach (Sprite sprite in spriteMap.Sprites) 
                        {
                            this._canvas.Start(sprite.Width, sprite.Height);
                            Layer layer = new Layer();
                            layer.X = sprite.X;
                            layer.Y = sprite.Y;
                            layer.Width = sprite.Width;
                            layer.Height = sprite.Height;
                            int code;
                            layer.Data = this._canvas.Transform(this._canvas.Extract(images, spriteMap, sprite, out code));
                            layer.Code = code;
                            spriteMap.CacheLayers.Add(layer);
                        }
                    }else{
                        //Single Layer
                        this._canvas.Start(spriteMap.Width, spriteMap.Heigth);
                        int code;
                        foreach (Sprite sprite in spriteMap.Sprites)
                            this._canvas.Draw(this._canvas.Extract(images, spriteMap, sprite, out code), sprite.X, sprite.Y, sprite.Width, sprite.Height);
                        Layer layer = new Layer();
                        layer.Width = spriteMap.Width;
                        layer.Height = spriteMap.Heigth;
                        layer.Data = this._canvas.Transform(this._canvas.End());
                        spriteMap.CacheLayers.Add(layer);
                    }
                }
                List<Layer> layers = new List<Layer>();
                foreach (Layer layerCache in spriteMap.CacheLayers) 
                {
                    Layer layer = layerCache.Clone();
                    layer.X += x;
                    layer.Y += y;
                    layers.Add(layer);
                }
                return (layers);
            }

            public List<Layer> Draw() 
            {
                if ((this.Paused) && (!this.Updated))
                    return(null);
                this.Updated = false;
                List<Layer> layers = new List<Layer>();
                //Background
                Layer layerBackground = new Layer();
                layerBackground.Boxes.Create(0, 0, this.ViewWidth, this.ViewHeight, 0, 0, 0, 1);
                layers.Add(layerBackground);
                //Screen
                Screen screen = GetScreen(); 
                //Animations
                if (screen.Type != ScreenType.Loading)
                {
                    List<Tuple<ImageCollection, SpriteMap, Frame, ResourceState>> spriteMaps = CreateSnapshotSpriteMaps();
                    foreach (Tuple<ImageCollection, SpriteMap, Frame, ResourceState> spriteMap in spriteMaps)
                    {
                        List<Layer> spriteMapLayers = CreateLayers(spriteMap.Item1, spriteMap.Item2, (spriteMap.Item4 != null ? spriteMap.Item4.X : 0) + spriteMap.Item3.X, (spriteMap.Item4 != null ? spriteMap.Item4.Y : 0) + spriteMap.Item3.Y);
                        if (this.Debug)
                        {
                            Layer layer = spriteMapLayers[0];
                            layer.Boxes.Clear();
                            //Damate
                            foreach(HitBox hitbox in spriteMap.Item3.HitBoxesDamage)
                                layer.Boxes.Create(layer.X + hitbox.X, layer.Y + hitbox.Y, hitbox.Width, hitbox.Height, 0, 255, 0, 0.5f);
                            //Vulnerable
                            foreach (HitBox hitbox in spriteMap.Item3.HitBoxesVulnerable)
                                layer.Boxes.Create(layer.X + hitbox.X, layer.Y + hitbox.Y, hitbox.Width, hitbox.Height, 255, 0, 0, 0.5f);
                        }
                        layers.AddRange(spriteMapLayers);
                    }
                }
                //Texts GameState
                foreach (Text text in this._gameState.Texts)
                {
                    SpriteMap spriteMap = CreateTextSpriteMap(text);
                    int x = text.X;
                    if (text.Align == TextAlign.Middle)
                        x -= spriteMap.Width / 2;
                    layers.AddRange(CreateLayers(this._gameState.Game.Images, spriteMap, x, text.Y));
                }
                if (this.Status == Engine.Status.Load)
                    this.Status = Engine.Status.Loading;
                //Scale
                List<Layer> layersScale = new List<Layer>();
                foreach (Layer layer in layers) 
                {
                    Layer layerScale = layer.Clone();
                    layerScale.X = (int)(layerScale.X * this.Scale) + (int)this.ViewLeft;
                    layerScale.Y = (int)(layerScale.Y * this.Scale) + (int)this.ViewTop;
                    foreach (Box box in layerScale.Boxes) 
                    {
                        box.Rectangle.X = (int)(box.Rectangle.X * this.Scale) + (int)this.ViewLeft;
                        box.Rectangle.Y = (int)(box.Rectangle.Y * this.Scale) + (int)this.ViewTop;
                    }
                    layersScale.Add(layerScale);
                }
                layers = layersScale;
                //Clipping
                Layer layerClipping = new Layer();
                layerClipping.Boxes.Create((int)this.ClippingLeft, (int)this.ClippingTop, (int)this.ClippingWidth, (int)this.ClippingHeight, 0, 0, 0, 1);
                layers.Add(layerClipping);
                //Virtual Keyboard
                if (this.Keyboard)
                {
                    foreach (TouchButton touchButton in screen.TouchButtons)
                    {
                        int x = touchButton.Left ?? this.ViewWidth - touchButton.Right.Value;
                        int y = touchButton.Top ?? this.ViewHeight - touchButton.Down.Value;
                        Layer layerTouchButton = CreateLayer(this._gameState.Game, touchButton.AnimationName, x, y);
                        if (this.Debug)
                            layerTouchButton.Boxes.Create(x, y, touchButton.Width, touchButton.Height, 255, 0, 0, 0.1f);
                        layers.Add(layerTouchButton);
                    }
                }
                //Debug
                if (this.Debug) 
                {
                    //Debug Information
                    Text text = new Text();
                    int players = this._gameState.ResourceStates.Count<ResourceState>(rs => (rs.Player != null) && (rs.Life > 0));
                    int playersAnimations = this._gameState.AnimationStates.Count<AnimationState>(ans => (ans.ResourceState != null) && (ans.ResourceState.Life > 0) && (ans.ResourceState.Player != null));
                    text.Value = string.Format("AS {0} > RS {1} > PL {2} > PA {3} > LA {4}",this._gameState.AnimationStates.Count, this._gameState.ResourceStates.Count, players, playersAnimations, layers.Count);
                    text.FontName = this._gameState.Game.WarnUpFontName;
                    text.X = 0;
                    text.Y = this._gameState.Game.Height - 30;
                    layers.AddRange(CreateLayers(this._gameState.Game.Images, CreateTextSpriteMap(text), text.X, text.Y));
                    Layer layer = new Layer();
                    //Touch Rectangles
                    foreach (Data.Action action in screen.Actions)
                    {
                        if (action.Touch == null)
                            continue;
                        layer.Boxes.Create((int)(action.Touch.X * this.Scale) + (int)this.ViewLeft, (int)(action.Touch.Y * this.Scale) + (int)this.ViewTop, action.Touch.Width, action.Touch.Height, 255,0,0,1);
                    }
                    if (layer.Boxes.Count > 0)
                        layers.Add(layer);
                }
                return (layers);
            }

            private Screen GetScreen()
            {
                if ((this.Status == Engine.Status.Load) || (this.Status == Engine.Status.Loading)){
                    return(this._gameState.Game.Screens.GetScreen(ScreenType.Loading));
                }else if (this.Status == Engine.Status.RoundOver) { 
                    return(this._gameState.Game.Screens.GetScreen(string.IsNullOrEmpty(this._gameState.Properties.GetPropertyValue(Property.PROPERTY_WINNER)) ? ScreenType.RoundOverDraw : ScreenType.RoundOverWin));
                }else if (this.Status == Engine.Status.MatchOver){
                    return (this._gameState.Game.Screens.GetScreen(ScreenType.MatchOver));
                }
                return (this._gameState.Screen);
            }

            private SpriteMap CreateTextSpriteMap(Text text)
            {
                string textEvaluated = EvaluateText(text.Value);
                string key = this._cache.CreateKey(text, textEvaluated);
                if (!this._cache.Contains(key))
                {
                    SpriteMap spriteMap = new SpriteMap();
                    Font font = this._gameState.Game.Fonts.GetFont(text.FontName);
                    spriteMap.ImageName = font.ImageName;
                    spriteMap.Transparency = font.Transparency;
                    int x = 0;
                    foreach (char valueChar in textEvaluated)
                    {
                        Character character = font.Characters.GetCharacter(valueChar.ToString());
                        if (character == null)
                            continue;
                        spriteMap.Sprites.Create(x, 0, character.X, character.Y, character.Width, character.Height);
                        x += character.Width;
                    }
                    this._cache.Add(key, spriteMap);
                }
                return (this._cache.Get(key));
            }

            private string EvaluateText(string text)
            {
                foreach (Property property in this._gameState.Properties)
                    text = text.Replace(string.Format("[{0}]", property.PropertyName), property.Value.ToString());
                int propertyStart = 0;
                while((propertyStart = text.IndexOf("[")) >= 0)
                {
                    int propertyEnd = text.IndexOf("]");
                    text = text.Replace(text.Substring(propertyStart, propertyEnd - (propertyStart - 1)), string.Empty);
                }
                return (text);
            }
        #endregion
        #region Update
            public bool Update() 
            {
                //Init
                bool refresh = false;
                long tickCurrent = DateTime.Now.Ticks;
                this.TickInputsSum += tickCurrent - this.TickLast;
                this.TickFramesSum += tickCurrent - this.TickLast;
                this.TickNetworksSum += tickCurrent - this.TickLast;
                this.TickLast = tickCurrent;
                //Inputs
                while ((this.Status != Engine.Status.WarnUp) && (this.TickInputsSum > this.TickFrame))
                {
                    if (UpdateInput())
                        refresh = true;
                    this.TickInputsSum -= this.TickFrame;
                }
                if (this.IsPlayingClient()) 
                {
                    //Commands
                    if (!string.IsNullOrEmpty(this.Network.ResourceStateCode)) 
                    {
                        foreach (AnimationState animationState in this._gameState.AnimationStates) 
                        {
                            if ((animationState.ResourceState == null) || (animationState.ResourceState.Code != this.Network.ResourceStateCode))
                                continue;
                            Command command = ((IPlayer)(this._player)).GetCommand(this._gameState, animationState.ResourceState);
                            if (command == null)
                                break;
                            Package package = new Package();
                            package.Type = PackageType.RequestCommand;
                            PackageItem packageItem = new PackageItem();
                            packageItem.Type = PackageItemType.Command;
                            packageItem.Data.Add(command.Name);
                            package.Items.Add(packageItem);
                            this.Network.Send(package);
                            break;
                        }
                    }
                    //Frame
                    bool update = false;
                    Package packageReceive = this.Network.Receive();
                    if ((packageReceive != null) && (packageReceive.Type == PackageType.ResponseGameState))
                        update = this.Network.Update(packageReceive, this._gameState, this._resources);
                    if (update)
                    {
                        this.TickNetworksSum = 0;
                        return (update);
                    }
                    if ((this.Status != Engine.Status.Playing) ||  (this.TickFramesSum < this.TickFrame))
                        return (update);
                }
                //Texts Screen
                this._gameState.Texts.Clear();
                Screen screen = this.GetScreen();
                foreach (Text text in screen.Texts)
                    this._gameState.Texts.Add(text);
                if (this.Paused)
                    return (false);
                if (this.Status == Engine.Status.Load) 
                {
                    if (this.Type == GameEngineType.Server) 
                    {
                        this.Status = Engine.Status.Loading;
                        return (true);
                    }
                    return (false);
                }else if (this.Status == Engine.Status.Loading) {
                    if (this.Type == GameEngineType.Single)
                        this.LoadResourcesData(this._gameState);
                    this.Status = Engine.Status.Playing;
                    //Tick
                    this.TickLast = DateTime.Now.Ticks;
                    this.TickFramesSum = this.TickInputsSum = 0;
                    //Sleep
                    this.Sleep = this._gameState.Game.WarnUp;
                    this.SleepFrames = this._gameState.Game.FramesPerSecond;
                    if (this.Sleep > 0)
                        this.Status = Engine.Status.WarnUp;
                    return (true);
                }else if (this.Status == Engine.Status.Playing) {
                    List<string> groups = this._gameState.ResourceStates.GetGroupsAlive();
                    if (groups.Count < 2)
                    {
                        //Round Over
                        this.Status = Engine.Status.RoundOver;
                        string groupWinner = groups.Count > 0 ? groups[0] : string.Empty;
                        this._gameState.Properties.Update(Property.PROPERTY_WINNER, groupWinner);
                        if (!string.IsNullOrEmpty(groupWinner))
                        {
                            int wins = this._gameState.Properties.Increment(groupWinner);
                            //Match Over
                            if (wins >= this._gameState.Game.Rounds)
                                this.Status = Engine.Status.MatchOver;
                        }
                        this.Sleep = 3;
                        this.SleepFrames = this._gameState.Game.FramesPerSecond;
                    }
                }else if (this.Status == Engine.Status.RoundOver){
                    if (this.Sleep <= 0)
                        InitializeRound(this._gameState);
                }else if (this.Status == Engine.Status.MatchOver){
                    if (this.Sleep <= 0)
                    {
                        InitializeMatch(this._gameState);
                        return (true);
                    }
                }else if (this.Status == Engine.Status.WarnUp) { 
                    //Players
                    if (!string.IsNullOrEmpty(this._gameState.Game.SystemFontName))
                    {
                        Text text = new Text();
                        text.Value = this.Sleep.ToString();
                        text.FontName = this._gameState.Game.WarnUpFontName;
                        text.X = this._gameState.Game.Width / 2;
                        text.Y = this._gameState.Game.Height / 2;
                        this._gameState.Texts.Add(text);
                        foreach (ResourceState resourceState in this._gameState.ResourceStates)
                        {
                            if (resourceState.Player == null)
                                continue;
                            text = new Text();
                            text.Value = resourceState.Player.GetName();
                            text.FontName = this._gameState.Game.SystemFontName;
                            text.X = resourceState.X;
                            text.Y = resourceState.Y;
                            this._gameState.Texts.Add(text);
                        }
                    }
                }
                //Frames
                while (this.TickFramesSum > this.TickFrame) 
                {
                    this.TickFramesSum -= this.TickFrame;
                    if ((this.Status == Engine.Status.Playing) && (UpdateFrame()))
                        refresh = true;
                    if (this.Sleep >= 0) 
                    {
                        refresh = true;
                        this.SleepFrames--;
                        if (SleepFrames <= 0) 
                        {
                            this.Sleep--;
                            this.SleepFrames = this._gameState.Game.FramesPerSecond;
                            refresh = true;
                        }
                        if (this.Sleep <= 0)
                        {
                            this.TickFramesSum = 0;
                            if (this.Status == Engine.Status.WarnUp)
                                this.Status = Engine.Status.Playing;
                        }
                    }
                }
                //Networks
                if ((this.Type == GameEngineType.Server) && (refresh) && (this.TickFrame != this.TickNetwork))
                {
                    if (this.TickNetworksSum > this.TickNetwork)
                        this.TickNetworksSum = 0;
                    else
                        refresh = false;
                }
                return (refresh);
            }

            private bool IsPlayingClient() 
            {
                if (this.Type != GameEngineType.Client)
                    return(false);
                if (this.Status == Engine.Status.Menu)
                    return (false);
                return (true);
            }

            private bool UpdateInput() 
            {
                bool refresh = false;
                this._inputs.Snapshot();
                return (refresh);
            }

            private bool UpdateFrame() 
            {
                GameState gameState = this._gameState;
                Resource resourceMap = gameState.Map;
                bool refresh = false;
                //Triggers
                if (this.Status == Engine.Status.Playing)
                {
                    foreach (TriggerState triggerState in gameState.TriggerStates)
                    {
                        if (triggerState.AnimationState != null)
                            continue;
                        if (triggerState.Activation == 0)
                            continue;
                        triggerState.Activation--;
                    }
                }
                //Update Before Animations
                if ((gameState.Map != null) && (this.Status == Engine.Status.Playing) && (UpdatePlayersLocation(gameState)))
                    refresh = true;
                //Animations
                for(int i = gameState.AnimationStates.Count - 1; i >= 0; i--)
                {
                    AnimationState animationState = gameState.AnimationStates[i];
                    Frame frame = animationState.Animation.Frames[animationState.FrameCurrent];
                    if (animationState.FrameCount < (frame.Frames - 1)) {
                        animationState.FrameCount++;
                        refresh = true;
                    }else if(animationState.FrameCurrent < (animationState.Animation.Frames.Count - 1)){
                        //Next Frame
                        animationState.FrameCurrent++;
                        animationState.FrameCount = 0;
                        refresh = true;
                    }
                    else if (((animationState.TriggerState != null) && (animationState.Animation.Loop)) || ((animationState.TriggerState == null) && (animationState.ResourceState != null) && (animationState.ResourceState.Life > 0)))
                    {
                        //Loop
                        if (animationState.FrameCurrent == 0)
                            continue;
                        refresh = true;
                        animationState.FrameCurrent = 0;
                        animationState.FrameCount = 0;
                    }else if (animationState.ResourceState != null){ 
                        //Destroy
                        gameState.AnimationStates.RemoveAt(i);
                        if (animationState.TriggerState != null)
                            animationState.TriggerState.Reset();
                        refresh = true;
                    } 
                }
                //Collisions
                for(int i = 0; i < gameState.ResourceStates.Count;i++) 
                {
                    ResourceState resourceStateBefore = gameState.ResourceStates[i];
                    if ((resourceStateBefore.AnimationState == null) || (resourceStateBefore.Life == 0))
                        continue;
                    Frame frameBefore = resourceStateBefore.AnimationState.Animation.Frames[resourceStateBefore.AnimationState.FrameCurrent];
                    for (int j = i + 1; j < gameState.ResourceStates.Count; j++) 
                    {
                        ResourceState resourceStateAfter = gameState.ResourceStates[j];
                        if ((resourceStateAfter.AnimationState == null) || (resourceStateAfter.Life == 0))
                            continue;
                        Frame frameAfter = resourceStateAfter.AnimationState.Animation.Frames[resourceStateAfter.AnimationState.FrameCurrent];
                        //Hit After
                        int hit = this.GetHit(resourceStateBefore, frameBefore, resourceStateAfter, frameAfter);
                        resourceStateAfter.Life -= hit;
                        //Hit Before
                        hit = this.GetHit(resourceStateAfter, frameAfter, resourceStateBefore, frameBefore);
                        resourceStateBefore.Life -= hit;
                        //Die After
                        if ((resourceStateAfter.Life <= 0) && (resourceStateAfter.AnimationState != null) && (resourceStateAfter.AnimationState.Animation != null) && (resourceStateAfter.AnimationState.Animation.Name != "Die"))
                        {
                            Command command = resourceStateAfter.Resource.Commands.GetCommand("Die");
                            if (command == null)
                                resourceStateAfter.AnimationState = null;
                            else
                                ExecuteCommand(gameState, resourceStateAfter, command);
                            refresh = true;
                        }
                        //Die Before
                        if ((resourceStateBefore.Life <= 0) && (resourceStateBefore.AnimationState != null) && (resourceStateBefore.AnimationState.Animation != null) && (resourceStateBefore.AnimationState.Animation.Name != "Die"))
                        {
                            Command command = resourceStateBefore.Resource.Commands.GetCommand("Die");
                            if (command == null)
                                resourceStateBefore.AnimationState = null;
                            else
                                ExecuteCommand(gameState, resourceStateBefore, command);
                            refresh = true;
                        }
                    } 
                }
                //Detroy
                for (int i = gameState.ResourceStates.Count - 1; i >= 0; i--)
                {
                    ResourceState resourceState = gameState.ResourceStates[i];
                    if ((resourceState.Life > 0) || (resourceState.IsPlayer))
                        continue;
                    gameState.AnimationStates.Remove(resourceState.AnimationState);
                    if ((resourceState.AnimationState != null) && (resourceState.AnimationState.TriggerState != null))
                        resourceState.AnimationState.TriggerState.Reset();
                    gameState.ResourceStates.RemoveAt(i);
                    refresh = true;
                }
                //Commands
                for (int i = gameState.ResourceStates.Count - 1; i >= 0; i--)
                {
                    ResourceState resourceState = gameState.ResourceStates[i];
                    if (resourceState.Player == null)
                        continue;
                    Command command = resourceState.Player.GetCommand(gameState, resourceState);
                    if (command == null)
                        continue;
                    ExecuteCommand(gameState, resourceState, command);
                    refresh = true;
                }
                //New Animations
                if (InitializeAnimations(gameState))
                    refresh = true;
                return (refresh);
            }

            private int GetHit(ResourceState resourceStateDamage, Frame frameDamage, ResourceState resourceStateVulnerable, Frame frameVulnerable)
            {
                foreach (HitBox hitDamage in frameDamage.HitBoxesDamage) 
                {
                    int x1 = resourceStateDamage.X + frameDamage.X + hitDamage.X;
                    int y1 = resourceStateDamage.Y + frameDamage.Y + hitDamage.Y;
                    int x2 = x1 + hitDamage.Width;
                    int y2 = y1 + hitDamage.Height;
                    foreach (HitBox hitVulnerable in frameVulnerable.HitBoxesVulnerable) 
                    {
                        if (x2 < (resourceStateVulnerable.X + frameVulnerable.X + hitVulnerable.X))
                            continue;
                        if (y2 < (resourceStateVulnerable.Y + frameVulnerable.Y + hitVulnerable.Y))
                            continue;
                        if (x1 > (resourceStateVulnerable.X + frameVulnerable.X + hitVulnerable.X + hitVulnerable.Width))
                            continue;
                        if (y2 > (resourceStateVulnerable.Y + frameVulnerable.Y + hitVulnerable.Y + hitVulnerable.Height))
                            continue;
                        return (hitDamage.Hit);
                    } 
                }
                return (0);
            }

            private void ExecuteCommand(GameState gameState, ResourceState resourceState, Command command)
            {
                ResourceState resourceStateCurrent = null;
                if (string.IsNullOrEmpty(command.FactoryName))
                {
                    resourceStateCurrent = resourceState;
                }else {
                    resourceStateCurrent = gameState.ResourceStates.Create(resourceState.Resource.Resources.GetResource(command.FactoryName), command.Life, resourceState.X + command.X, resourceState.Y + command.Y, command.VelocityX, command.VelocityY);
                    AnimationState animationStateCurrent = gameState.AnimationStates.GetAnimationState(resourceState);
                    if (animationStateCurrent == null)
                        return;
                    gameState.AnimationStates.Create(resourceStateCurrent, command.AnimationName, animationStateCurrent.ResourceRoot);
                }
                resourceStateCurrent.AnimationState.Animation = resourceStateCurrent.Resource.Animations.GetAnimation(command.AnimationName);
                resourceStateCurrent.AnimationState.FrameCurrent = 0;
                resourceStateCurrent.AnimationState.FrameCount = 0;
                resourceStateCurrent.VelocityX = command.VelocityX;
                resourceStateCurrent.VelocityY = command.VelocityY;
            }

            public bool InitializeAnimations(GameState gameState) 
            {
                bool created = false;
                //Triggers
                foreach (TriggerState triggerState in gameState.TriggerStates) 
                {
                    if (triggerState.AnimationState != null)
                        continue;
                    if (triggerState.Activation > 0)
                        continue;
                    gameState.AnimationStates.Create(gameState, triggerState, this.GetComputer());
                    created = true;
                }
                return (created);
            }

            protected bool UpdatePlayersLocation(GameState gameState)
            {
                bool updatedAny = false;
                int width = gameState.Map.Width;
                int height = gameState.Map.Height;
                //Actors
                foreach (ResourceState resourceState in gameState.ResourceStates)
                {
                    bool updated = false;
                    resourceState.X += resourceState.VelocityX;
                    resourceState.Y += resourceState.VelocityY;
                    if (resourceState.X < 0)
                    {
                        resourceState.X = width;
                        updated = true;
                    }
                    else if (resourceState.X > width)
                    {
                        resourceState.X = 0;
                        updated = true;
                    }
                    if (resourceState.Y < 0)
                    {
                        resourceState.Y = height;
                        updated = true;
                    }
                    else if (resourceState.Y > height)
                    {
                        resourceState.Y = 0;
                        updated = true;
                    }
                    if (updated)
                        updatedAny = true;
                    if ((!resourceState.IsPlayer) && (updated))
                        resourceState.Life = 0;
                }
                return (updatedAny);
            }
        #endregion
        #region Actions
            private void ExecuteAction(Data.Action action)
            {
                if (action.Type == ActionType.GoToScreen)
                {
                    InitializeScreen(this._gameState.Game.Screens.GetScreen(action.ScreenName));
                }else if (action.Type == ActionType.StartGame){
                    this.Type = GameEngineType.Single;
                    InitializeScreen(this._gameState.Game.Screens.GetScreen(ScreenType.Game));
                }else if (action.Type == ActionType.StartGameOnline){
                    this.Type = GameEngineType.Client;
                    if (this.Network.Connect(this._gameState.Game.Name))
                    {
                        if (this.Network.Join())
                        {
                            InitializeScreen(this._gameState.Game.Screens.GetScreen(ScreenType.Game));
                            this._gameState.Reset(true);
                        }else {
                            this._gameState.Properties.Update("ERROR", "ERROR JOIN");
                            InitializeScreen(this._gameState.Game.Screens.GetScreen(ScreenType.Error));
                        }
                    }else {
                        this._gameState.Properties.Update("ERROR", "ERROR CONNECT");
                        InitializeScreen(this._gameState.Game.Screens.GetScreen(ScreenType.Error));
                    }
                }else if (action.Type == ActionType.Quit){
                    this.Exit = true;
                }
            }
        #endregion

        #region Input
            public void Input(InputType type, bool isDown) 
            {
                if ((type == InputType.Pause) && (isDown)) 
                {
                    this.Paused = !this.Paused;
                    if (!this.Paused) 
                    {
                        this.TickLast = DateTime.Now.Ticks;
                        this.TickFramesSum = this.TickInputsSum = 0;
                    }
                    return;
                }else if ((type == InputType.Frame) && (isDown)){
                    if (this.UpdateFrame())
                        this.Updated = true;
                    return;
                }else if ((type == InputType.Escape) && (isDown)) {
                    this.Status = Engine.Status.Menu;
                    Screen screen = this._gameState.Game.Screens.GetScreen(ScreenType.Start);
                    if (screen != this._gameState.Screen)
                        InitializeScreen(screen);
                    return;
                }
                Data.Action action = this._gameState.Screen.Actions.GetAction(type);
                if (action != null)
                {
                    if (!isDown)
                        ExecuteAction(action);
                }else if (isDown){
                    this._inputs.Add(type);
                }else{
                    this._inputs.Remove(type);
                }
            }

            public List<List<InputType>> InputBuffer() 
            {
                return (this._inputs.Buffer);
            }
        #endregion
        #region Touch
            public void Touch(int x, int y, bool isDown, int? touchCode) 
            {
                Data.Action action = this._gameState.Screen.Actions.GetAction(this.Scale, this.ViewLeft, this.ViewTop,x,y);
                if (action != null)
                {
                    if (!isDown)
                        ExecuteAction(action);
                }else if (isDown){
                    TouchButton touchButton = this._gameState.Screen.TouchButtons.GetTouchButton(this.ViewWidth, this.ViewHeight, x, y, this.TouchButtonProximity);
                    if (touchButton == null)
                        return;
                    if (!touchCode.HasValue)
                        this._touches.Clear();
                    if ((touchButton.Input == InputType.Escape) && (isDown))
                    {
                        this.Status = Engine.Status.Menu;
                        Screen screen = this._gameState.Game.Screens.GetScreen(ScreenType.Start);
                        if (screen != this._gameState.Screen)
                            InitializeScreen(screen);
                        return;
                    }
                    this._touches.Add(new Touch(touchCode, x, y, touchButton));
                }else{
                    if (touchCode.HasValue)
                        this._touches.Remove(touchCode.Value);
                    else
                        this._touches.Clear();
                }
            } 
        #endregion

        #region Screen
            private void InitializeScreen(Screen screen) 
            {
                this._gameState.Reset(true);
                this._gameState.Screen = screen;
                if (screen.Type == ScreenType.Game)
                {
                    if (this.Type == GameEngineType.Client)
                    {
                        this.Status = Engine.Status.Playing;
                    }else{
                        //Map
                        InitializeMap();
                        //Triggers
                        this.InitializeTriggers(this._gameState, this._gameState.Map, this._gameState.Map);
                        //Animations
                        this.InitializeAnimations(this._gameState);
                        //Actors
                        this.InitializeActors(this._gameState, this._gameState.Map);
                    }
                }else {
                    this._gameState.AnimationStates.Create(this._gameState.Game, screen);
                }
                //Tick
                this.TickLast = DateTime.Now.Ticks;
                this.TickFramesSum = this.TickInputsSum = 0;
            }
        #endregion
        #region Scale
            private void CalculateScale() 
            {
                //Scale
                float scaleWidth = (float)Math.Round((decimal)this.ViewWidth / (decimal)this._gameState.Game.Width, 2);
                float scaleHeight = (float)Math.Round((decimal)this.ViewHeight / (decimal)this._gameState.Game.Height, 2);
                this.Scale = scaleWidth < scaleHeight ? scaleWidth : scaleHeight;
                this.ViewTop = (this.ViewHeight - (this._gameState.Game.Height * this.Scale)) / 2;
                this.ViewLeft = (this.ViewWidth - (this._gameState.Game.Width * this.Scale)) / 2;
                //Clipping 
                if (scaleWidth > scaleHeight)
                {
                    this.ClippingLeft =  this.ViewLeft + (this._gameState.Game.Width * this.Scale);
                    this.ClippingTop = 0;
                    this.ClippingWidth = this.ViewWidth - (this.ViewLeft + (this._gameState.Game.Width * this.Scale));
                    this.ClippingHeight = this.ViewHeight;
                }else{
                    this.ClippingLeft = 0;
                    this.ClippingTop = this.ViewTop + (this._gameState.Game.Height * this.Scale);
                    this.ClippingWidth = this.ViewWidth;
                    this.ClippingHeight = this.ViewHeight - (this.ViewTop + (this._gameState.Game.Height * this.Scale));
                }
            }
        #endregion
        #region Map
            public void InitializeMap(string mapCode) 
            {
                //Init
                this._gameState.AnimationStates.Clear();
                this._gameState.ResourceStates.Clear();
                this._gameState.TriggerStates.Clear();
                Resource map = this._resources.GetResource(mapCode);
                this._gameState.Map = map;
                //Load Resources
                foreach (Resource resource in this._resources.GetResources())
                    LoadResourceData(resource);
            }

            private void InitializeMap()
            {
                //Choose Map
                List<Resource> maps = this._resources.GetResources(ResourceType.Map);
                Random rand = new Random(DateTime.Now.Millisecond);
                int index = rand.Next(0, maps.Count);
                //Initialize Map
                Resource map = maps[index];
                //Map
                this._gameState.Map = map;
                if ((this.Type == GameEngineType.Client) || (this._gameState.Game.Screens.GetScreen(ScreenType.Loading) == null))
                    this.Status = Engine.Status.Playing;
                else
                    this.Status = Engine.Status.Load;
            }

            public void InitializeTriggers(GameState gameState, Resource resourceRoot, Resource resource)
            {
                //Triggers
                foreach (Trigger trigger in resource.Triggers)
                    gameState.TriggerStates.Create(trigger, resourceRoot, resource);
                //Resources Internal
                foreach (Resource resourceInternal in resource.Resources)
                    InitializeTriggers(gameState, resourceRoot, resourceInternal);
            }
        #endregion
        #region Actors
            private IPlayer GetComputer()
            {
                //HACK: Make this random in the future
                return (this._computers[0]);
            }

            public void InitializeActors(GameState gameState, Resource map)
            {
                List<Resource> actors = this._resources.GetResources(ResourceType.Actor);
                Dictionary<string, int> groups = new Dictionary<string, int>();
                int spawnPlayer = 0;
                if (this.Type == GameEngineType.Server)
                    spawnPlayer = -1;
                for (int i = 0; i < map.Spawns.Count; i++)
                {
                    Spawn spawn = map.Spawns[i];
                    if ((!string.IsNullOrEmpty(spawn.GroupName)) && (groups.ContainsKey(spawn.GroupName)))
                        continue;
                    groups.Add(spawn.GroupName, 0);
                    gameState.Properties.Create(string.Empty, string.Format("{0}", spawn.GroupName), "0");
                    Resource actor = actors.FirstOrDefault(a => a.Animations.GetAnimation(spawn.AnimationName) != null);
                    if (actor == null)
                        continue;
                    actors.Remove(actor);
                    //Human or Computer 
                    AnimationState animationState = gameState.AnimationStates.Create(gameState.ResourceStates.Create(actor, spawn, i == spawnPlayer ? this._player : this.GetComputer(),  spawn.GroupName), spawn);
                    animationState.ResourceState.GroupIndex = groups[spawn.GroupName];
                    groups[spawn.GroupName]++;
                    if (string.IsNullOrEmpty(spawn.GroupName))
                        continue;
                    for (int j = i + 1; j < map.Spawns.Count; j++)
                    {
                        Spawn spawnGroup = map.Spawns[j];
                        if (spawnGroup.GroupName != spawn.GroupName)
                            continue;
                        animationState = gameState.AnimationStates.Create(gameState.ResourceStates.Create(actor, spawnGroup, this.GetComputer(), spawn.GroupName), spawnGroup);
                        animationState.ResourceState.GroupIndex = groups[spawn.GroupName];
                        groups[spawn.GroupName]++;
                    }
                }
            }

            public void InitializeActorsAnimations() 
            {
                List<ResourceState> playersInserted = new List<ResourceState>();
                GameState gameState = this._gameState;
                for (int i = 0; i < gameState.Map.Spawns.Count; i++)
                {
                    Spawn spawn = gameState.Map.Spawns[i];
                    foreach (ResourceState resourceState in this._gameState.ResourceStates)
                    {
                        if (resourceState.Player == null)
                            continue;
                        if (resourceState.Player.IsHuman())
                            continue;
                        if (playersInserted.Contains(resourceState))
                            continue;
                        playersInserted.Contains(resourceState);
                        gameState.AnimationStates.Create(resourceState, spawn);
                    }
                }
            }

            public ResourceState AddPlayer(IPlayer player) 
            {
                foreach (ResourceState resourceState in this._gameState.ResourceStates) 
                {
                    if (resourceState.Player == null)
                        continue;
                    if (resourceState.Player.IsHuman())
                        continue;
                    resourceState.Player = player;
                    return (resourceState);
                }
                return (null);
            }

            public bool RemovePlayer(IPlayer player) 
            {
                foreach (ResourceState resourceState in this._gameState.ResourceStates)
                {
                    if (resourceState.Player == null)
                        continue;
                    if (resourceState.Player != player)
                        continue;
                    resourceState.Player = this.GetComputer();
                    return (true);
                }
                return (false);
            }
        #endregion
        #region Resource
            private void LoadResourcesData(GameState gameState)
            {
                foreach (ResourceState resourceState in this._gameState.ResourceStates)
                    this.LoadResourceData(resourceState.Resource);
                this.LoadResourceData(gameState.Map);
            }

            private void LoadResourceData(Resource resource)
            {
                foreach (SpriteMap spriteMap in resource.SpritesMaps)
                    if (spriteMap.CacheLayers == null)
                        CreateLayers(resource.Images, spriteMap, 0, 0);
            }
        #endregion

        #region Round
            private void InitializeRound(GameState gameState) 
            {
                //Players
                List<ResourceState> resourceStatePlayers = this._gameState.ExtractResourceStatePlayers();
                //Reset
                gameState.Reset(false);
                //Triggers
                this.InitializeTriggers(this._gameState, this._gameState.Map, this._gameState.Map);
                //Animations
                this.InitializeAnimations(this._gameState);
                //Actors
                InitializeActors(gameState, gameState.Map);
                //Players
                this._gameState.DefineResourceStatePlayers(resourceStatePlayers);
                //Status
                this.Status = Engine.Status.Playing;
                //Reset Ticks
                this.ResetTicks();
            }
        #endregion
        #region Match
            public void InitializeMatch() 
            {
                this.InitializeMatch(this._gameState);
            } 

            private void InitializeMatch(GameState gameState) 
            {
                //Players
                List<ResourceState> resourceStatePlayers = this._gameState.ExtractResourceStatePlayers();
                //Reset
                gameState.Reset(true);
                //Map
                InitializeMap();
                //Triggers
                this.InitializeTriggers(this._gameState, this._gameState.Map, this._gameState.Map);
                //Animations
                this.InitializeAnimations(this._gameState);
                //Actors
                this.InitializeActors(this._gameState, this._gameState.Map);
                //Players
                this._gameState.DefineResourceStatePlayers(resourceStatePlayers);
                //Load Resources
                if (this.Type == GameEngineType.Server)
                    this.LoadResourcesData(this._gameState);
            } 
        #endregion
        #region Multiplayer
            public void CreateSession() 
            {
                //Screen
                this._gameState.Screen = this._gameState.Game.Screens.GetScreen(ScreenType.Game);
                //Map
                InitializeMap();
                //Actors
                InitializeActors(this._gameState, this._gameState.Map);
                //Game State
                for (int i = this._gameState.ResourceStates.Count - 1; i >= 0; i--) 
                {
                    ResourceState resourceState = this._gameState.ResourceStates[i];
                    if (resourceState.Player == null)
                        this._gameState.ResourceStates.RemoveAt(i);
                }
                this._gameState.AnimationStates = new AnimationStateCollection();
                this._gameState.TriggerStates = new TriggerStateCollection();
            }

            public void StartSession()
            {
                //Reset Ticks
                this.ResetTicks();
                //Clear Animations
                this._gameState.AnimationStates.Clear();
                //Triggers
                this.InitializeTriggers(this._gameState, this._gameState.Map, this._gameState.Map);
                //Animations
                this.InitializeAnimations(this._gameState);
                //Actors
                this.InitializeActorsAnimations();
                //Start
                InitializeRound(this._gameState);
            }
        #endregion
    }
}
