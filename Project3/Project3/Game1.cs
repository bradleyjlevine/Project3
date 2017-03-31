﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Primitives;


namespace Project3
{
    #region Structs
    struct CubeData
    {
        public Vector3 position;
        public float scale;
        public float xScale;
        public float yScale;
        public float zScale;
        public Color color;
        public Vector3 velocity;
    }

    struct SphereData
    {
        public Vector3 position;
        public Color color;
        public Vector3 velocity;
    }
    #endregion
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Cube cube;
        CubeData[] cubeData;
        SpherePrimitive sphere;
        SphereData[] sphereData;
        LineCube gameField;
        BasicEffect effect;

        Matrix world;
        Matrix view;
        Matrix projection;
        Vector3 cameraPosition = new Vector3(0, 0, 100f);
        float cameraSpeed = 5, pitch = 0, yaw = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            cube = new Cube();
            gameField = new LineCube();
            sphere = new SpherePrimitive(GraphicsDevice, 10f, 20);
            cubeData = new CubeData[3];
            sphereData = new SphereData[1];
            effect = new BasicEffect(GraphicsDevice);

            world = Matrix.Identity;
            view = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 0, 0), Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);

            //line cube creation
            cubeData[0].position = new Vector3(1, 1, 1);
            cubeData[0].xScale = 25f;
            cubeData[0].yScale = 25f;
            cubeData[0].zScale = 50f;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //gets the state of the keyboards
            KeyboardState state = Keyboard.GetState();

            #region Rotation and Translations of View
            Matrix rotation;

            if (state.IsKeyDown(Keys.Up))
                pitch += 0.05f;
            if (state.IsKeyDown(Keys.Down))
                pitch -= 0.05f;
            if (state.IsKeyDown(Keys.Left))
                yaw += 0.05f;
            if (state.IsKeyDown(Keys.Right))
                yaw -= 0.05f;

            rotation = Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f);

            if (state.IsKeyDown(Keys.A))
            {
                cameraPosition += rotation.Left * cameraSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
            else if (state.IsKeyDown(Keys.S))
            {
                cameraPosition += rotation.Backward * cameraSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
            else if (state.IsKeyDown(Keys.D))
            {
                cameraPosition += rotation.Right * cameraSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
            else if (state.IsKeyDown(Keys.W))
            {
                cameraPosition += rotation.Forward * cameraSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            view = Matrix.CreateLookAt(cameraPosition, cameraPosition + rotation.Forward, rotation.Up);

            #endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            RasterizerState orginal = GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = RasterizerState.CullNone;
            GraphicsDevice.RasterizerState = rasterizerState;
            effect.VertexColorEnabled = true;
            effect.EnableDefaultLighting();

            foreach(EffectTechnique technique in effect.Techniques)
            {
                foreach(EffectPass pass in technique.Passes)
                {
                    effect.World = world * Matrix.CreateScale(cubeData[0].xScale, cubeData[0].yScale, cubeData[0].zScale) * Matrix.CreateTranslation(cubeData[0].position);
                    effect.View = view;
                    effect.Projection = projection;
                }
            }

            GraphicsDevice.RasterizerState = orginal;

            base.Draw(gameTime);
        }
    }
}
