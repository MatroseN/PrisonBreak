using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PrisonBreak.Entity;
using PrisonBreak.Graphing;
using PrisonBreak.Searching;
using System.Collections.Generic;
using monogameVector2 = Microsoft.Xna.Framework.Vector2;
using monogameVector3 = Microsoft.Xna.Framework.Vector3;

namespace PrisonBreak {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Remove later
            #region Testing
            // Only for testing
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();

            setCameraPosition(0, 0);

            _basicEffect = new BasicEffect(_graphics.GraphicsDevice);
            _basicEffect.VertexColorEnabled = true;

            _basicEffect.World = Matrix.Identity;
             monogameVector3 cameraUp = monogameVector3.Transform(new monogameVector3(0, -1, 0), Matrix.CreateRotationZ(camera2DrotationZ));
            _basicEffect.View = Matrix.CreateLookAt(camera2DScrollPosition, camera2DScrollLookAt, cameraUp);
            _basicEffect.Projection = Matrix.CreateScale(1, -1, 1) * Matrix.CreateOrthographicOffCenter(0, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height, 0, 0, 1);

            _vertecies = new VertexPositionColor[3];

            _vertecies[0].Position = new monogameVector3(400, 400, 0);
            _vertecies[0].Color = new Color(50, 50, 0, 0);

            _vertecies[1].Position = new monogameVector3(300, 200, 0);
            _vertecies[1].Color = new Color(50, 50, 0, 0);

            _vertecies[2].Position = new monogameVector3(500, 200, 0);
            _vertecies[2].Color = new Color(50, 50, 0, 0);


            _entities = new List<Entity.Entity>();
            _block = new monogameVector2(32, 32);
            int i = 0;
            _grid = new monogameVector2[25 * 25];
            for (int y = 0; y < 25 * (int)_block.Y; y+=(int)_block.Y) {
                for (int x = 0; x < 25 * (int)_block.X; x+=(int)_block.X) {
                    _grid[i] = new monogameVector2(x, y);
                    i++;
                }
            }

            _graph = new Graph();
            _graph.gridToGraph(_grid, _block);
            _bfs = new BFS(_graph);

            _player = new Player(this, new monogameVector2(0, 0), _block);
            _player.setNode(_graph);

            _entities.Add(_player);

            _enemy = new Enemy(this, new monogameVector2(10, 10), _block);
            _enemy.setNode(_graph);
            _entities.Add(_enemy);

            _spriteAnimator = new SpriteAnimator();
            #endregion

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _player.TextureMap = Content.Load<Texture2D>("PrisonerSpriteSheet");
            _enemy.TextureMap = Content.Load<Texture2D>("GuardSpriteSheet");
            _pixel = Content.Load<Texture2D>("Pixel");
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (!_enemy.isGuided && _enemy.checkIfPlayerInVision(_player.HitBox)) {
                _enemy.isGuided = true;
            }

            if (_enemy.isGuided && _enemy.Position != _player.Position) {

                _allPaths = _bfs.allPaths(_graph.Adjecent[_player.Position]);
                _enemy.guidedMovement(_graph, _allPaths, _player.Node);
            } else if(_enemy.Position == _player.Position) {
                Initialize();
            }

            foreach (Entity.Entity entity in _entities) {
                entity.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach (Entity.Entity entity in _entities) {
                _spriteBatch.Draw(entity.TextureMap, entity.Position, _spriteAnimator.textureChooser(entity.TextureMap, entity.Direction, _block), Color.White);
            }

            _spriteBatch.End();

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rs;

            _basicEffect.CurrentTechnique.Passes[0].Apply();

            if (!_enemy.isGuided) {
                foreach (VertexPositionColor[] triangle in _enemy.Triangles) {
                    _graphics.GraphicsDevice.DrawUserPrimitives(
                    PrimitiveType.TriangleList, triangle, 0, 1);
                }
            }

            base.Draw(gameTime);
        }

        private void setCameraPosition(int x, int y) {
            camera2DScrollPosition.X = x;
            camera2DScrollPosition.Y = y;
            camera2DScrollPosition.Z = -1;
            camera2DScrollLookAt.X = x;
            camera2DScrollLookAt.Y = y;
            camera2DScrollLookAt.Z = 0;
        }

        private void setStates() {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        }

        private monogameVector2[] _grid;
        private monogameVector2   _block;
        private Graph     _graph;

        private List<Entity.Entity> _entities;
        private Player _player;
        private Enemy _enemy;

        private Texture2D _pixel;
        private SpriteAnimator _spriteAnimator;
        private VertexPositionColor[] _vertecies;

        private monogameVector3 camera2DScrollPosition = new monogameVector3(0, 0, -1);
        private monogameVector3 camera2DScrollLookAt = new monogameVector3(0, 0, 0);
        private float camera2DrotationZ = 0f;

        private BFS _bfs;
        private Dictionary<Node, Node> _allPaths;
    }
}