using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PrisonBreak.Entity;
using PrisonBreak.Graphing;
using System.Collections.Generic;
using System.Diagnostics;

namespace PrisonBreak {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;
        private float _angle;

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
            Vector3 cameraUp = Vector3.Transform(new Vector3(0, -1, 0), Matrix.CreateRotationZ(camera2DrotationZ));
            _basicEffect.View = Matrix.CreateLookAt(camera2DScrollPosition, camera2DScrollLookAt, cameraUp);
            _basicEffect.Projection = Matrix.CreateScale(1, -1, 1) * Matrix.CreateOrthographicOffCenter(0, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height, 0, 0, 1);

            _vertecies = new VertexPositionColor[3];

            _vertecies[0].Position = new Vector3(400, 400, 0);
            _vertecies[0].Color = new Color(50, 50, 0, 0);

            _vertecies[1].Position = new Vector3(300, 200, 0);
            _vertecies[1].Color = new Color(50, 50, 0, 0);

            _vertecies[2].Position = new Vector3(500, 200, 0);
            _vertecies[2].Color = new Color(50, 50, 0, 0);


            _entities = new List<Entity.Entity>();
            _block = new Vector2(32, 32);
            int i = 0;
            _grid = new Vector2[25 * 25];
            for (int y = 0; y < 25 * (int)_block.Y; y+=(int)_block.Y) {
                for (int x = 0; x < 25 * (int)_block.X; x+=(int)_block.X) {
                    _grid[i] = new Vector2(x, y);
                    i++;
                }
            }

            _graph = new Graph();
            _graph.gridToGraph(_grid, _block);

            _player = new Player(this, new Vector2(0, 0), _block);
            _player.setNode(_graph);

            _entities.Add(_player);

            _enemy = new Enemy(this, new Vector2(10, 10), _block);
            _enemy.setNode(_graph);
            _entities.Add(_enemy);

            _spriteAnimator = new SpriteAnimator();
            _lineOfSight = new LineOfSight();
            _bresenhams = new Bresenhams();
            _rays = new List<List<Vector2>>();
            _triangles = new List<VertexPositionColor[]>();

            _rays = _lineOfSight.calculateLineOfSight(400, 400, Direction.NORTH, 10);

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
            foreach (Entity.Entity entity in _entities) {
                entity.Update(gameTime);
            }

            _triangles = new List<VertexPositionColor[]>();
            _vertexPositions = new List<List<Vector2>>();
            foreach (List<Vector2> ray in _rays) {
                Vector2 anchor = ray[0];
                Vector2 final = ray[ray.Count - 1];
                List<Vector2> points = new List<Vector2>();
                points.Add(anchor);
                points.Add(final);

                _vertexPositions.Add(points);
            }

            for (int i = _vertexPositions.Count -1; i > 0; i--) {

                VertexPositionColor[] vertecies = new VertexPositionColor[3];
                vertecies[0].Color = new Color(50, 50, 0, 0);
                vertecies[1].Color = new Color(50, 50, 0, 0);
                vertecies[2].Color = new Color(50, 50, 0, 0);

                // Current
                vertecies[0].Position = new Vector3(_vertexPositions[i][0].X, _vertexPositions[i][0].Y, 0);
                vertecies[1].Position = new Vector3(_vertexPositions[i][1].X, _vertexPositions[i][1].Y, 0);

                // Next
                vertecies[2].Position = new Vector3(_vertexPositions[i -1][1].X, _vertexPositions[i -1][1].Y, 0);

                _triangles.Add(vertecies);
            }

            Debug.WriteLine(_triangles);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach (Entity.Entity entity in _entities) {
                _spriteBatch.Draw(entity.TextureMap, entity.Position, _spriteAnimator.textureChooser(entity.TextureMap, entity.Direction, _block), Color.White);
            }

            for (int i = 0; i < _rays.Count; i++) {
                for (int j = 0; j < _rays[i].Count; j++) {
                    _spriteBatch.Draw(_pixel, _rays[i][j], Color.White);
                }
            }

            _spriteBatch.End();

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rs;

            _basicEffect.CurrentTechnique.Passes[0].Apply();

            foreach (VertexPositionColor[] triangle in _triangles) {
                _graphics.GraphicsDevice.DrawUserPrimitives(
                PrimitiveType.TriangleList, triangle, 0, 1);
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

        private Vector2[] _grid;
        private Vector2   _block;
        private Graph     _graph;

        private List<Entity.Entity> _entities;
        private Player _player;
        private Enemy _enemy;

        private Texture2D _pixel;
        private SpriteAnimator _spriteAnimator;
        private LineOfSight _lineOfSight;
        private Bresenhams _bresenhams;
        private List<List<Vector2>> _rays;
        private VertexPositionColor[] _vertecies;
        private List<List<Vector2>> _vertexPositions;
        private List<VertexPositionColor[]> _triangles;

        private Vector3 camera2DScrollPosition = new Vector3(0, 0, -1);
        private Vector3 camera2DScrollLookAt = new Vector3(0, 0, 0);
        private float camera2DrotationZ = 0f;
    }
}