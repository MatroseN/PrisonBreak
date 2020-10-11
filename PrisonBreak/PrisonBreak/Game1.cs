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

            for (int x = (int)_rays[0][_rays[0].Count -1].X; x < (int)_rays[_rays.Count-1][_rays[_rays.Count - 1].Count - 1].X; x++) {
                int y = (int)_rays[0][_rays[0].Count - 1].Y;

                _spriteBatch.Draw(_pixel, new Vector2(x, y), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
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
    }
}