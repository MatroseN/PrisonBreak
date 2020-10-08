using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PrisonBreak.Entity;
using PrisonBreak.Graphing;
using System.Collections.Generic;

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
            // TODO: Add your initialization logic here
            // TODO: Remove later
            #region Testing
            // Only for testing
            _entities = new List<Entity.Entity>();
            _block = new Vector2(32, 32);
            int i = 0;
            _grid = new Vector2[25 * 25];
            for (int y = 0; y < 25 * 32; y+=32) {
                for (int x = 0; x < 25 * 32; x+=32) {
                    _grid[i] = new Vector2(x, y);
                    i++;
                }
            }

            _graph = new Graph();
            _graph.gridToGraph(_grid, _block);

            _player = new Player(this, new Vector2(0, 0));
            _player.setNode(_graph);
            _player.TextureMap = Content.Load<Texture2D>("PrisonerSpriteSheet");
            _entities.Add(_player);

            _spriteAnimator = new SpriteAnimator();

            #endregion

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Vector2[] _grid;
        private Vector2   _block;
        private Graph     _graph;

        private List<Entity.Entity> _entities;
        private Player _player;
        private SpriteAnimator _spriteAnimator;
    }
}