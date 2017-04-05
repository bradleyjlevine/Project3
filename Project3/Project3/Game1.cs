using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        #region Fields
        //fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Cube cube;
        LineCube field;
        CubeData[] cubeData;
        SpherePrimitive sphere;
        SphereData[] sphereData;
        BasicEffect effect;
        BasicEffect ballEffect;
        BasicEffect paddleEffect;
        BoundingBox fieldBoundingBox;
        BoundingBox playerPaddle1;
        BoundingBox playerPaddle2;
        BoundingSphere ballBoundingSphere;

        Matrix world;
        Matrix view;
        Matrix projection;
        Vector3 cameraPosition = new Vector3(0, 0, 50f);
        float ballSpeed = 10f, paddleSpeed1 = 10f, pitch = MathHelper.PiOver2, yaw = 0;
        #endregion

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
            //constructs necessary objects
            cube = new Cube();
            sphere = new SpherePrimitive(GraphicsDevice, 1f, 20);
            effect = new BasicEffect(GraphicsDevice);
            ballEffect = new BasicEffect(GraphicsDevice);
            paddleEffect = new BasicEffect(GraphicsDevice);
            field = new LineCube();

            cubeData = new CubeData[4];
            sphereData = new SphereData[1];

            //creats cube objects
            CreateCubes();

            //sets the info for the ball
            sphereData[0].position = Vector3.Zero;
            sphereData[0].velocity = new Vector3(0, 0, ballSpeed);
            sphereData[0].color = new Color(NextFloat(0.5f, 1), NextFloat(0, 1), NextFloat(0, 1));

            //creates the bounding box
            fieldBoundingBox = new BoundingBox(new Vector3(-cubeData[0].xScale + cubeData[0].position.X, -cubeData[0].yScale + cubeData[0].position.Y, -cubeData[0].zScale + cubeData[0].position.Z), new Vector3(cubeData[0].xScale + cubeData[0].position.X, cubeData[0].yScale + cubeData[0].position.Y, cubeData[0].zScale + cubeData[0].position.Z));
            playerPaddle2 = new BoundingBox(new Vector3(-cubeData[2].xScale + cubeData[2].position.X, -cubeData[2].yScale + cubeData[2].position.Y, -cubeData[2].zScale + cubeData[2].position.X), new Vector3(cubeData[2].xScale + cubeData[2].position.X, cubeData[2].yScale + cubeData[2].position.Y, cubeData[2].zScale + cubeData[2].position.Z));

            world = Matrix.Identity;
            view = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 0, 0), Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);

            base.Initialize();
        }

        /// <summary>
        /// Creates the bonding box and two paddles
        /// </summary>
        public void CreateCubes()
        {
            //sets info for the stencil of the bounding box
            cubeData[0].xScale = 10;
            cubeData[0].yScale = 10;
            cubeData[0].zScale = 20;
            cubeData[0].position = Vector3.Zero;

            //paddles info player 1
            cubeData[1].xScale = 1;
            cubeData[1].yScale = 1;
            cubeData[1].zScale = 0.2f;
            cubeData[1].position = new Vector3(0, 0, 20f);
            cubeData[1].color = new Color(NextFloat(0.5f, 1), 0, NextFloat(0.5f, 1));

            //paddles info player 2
            cubeData[2].xScale = 1;
            cubeData[2].yScale = 1;
            cubeData[2].zScale = 0.2f;
            cubeData[2].position = new Vector3(0, 0, -20f);
            cubeData[2].color = new Color(NextFloat(0.5f, 1), NextFloat(0.5f, 1), 0);
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

            Vector3 p = Vector3.Zero;
            float deltaX = 0, deltaY = 0;
            //checks if the ball intersects with the bound box and inverts the apporiate velocity
            for (int i = 0; i < sphereData.Length; i++)
            {

                UpdateBall(sphereData[i], gameTime.ElapsedGameTime.Milliseconds / 1000f, out p);
                sphereData[i].position = p;

                //creates the bounding sphere around the ball
                ballBoundingSphere = new BoundingSphere(sphereData[i].position, 1f);
                playerPaddle1 = new BoundingBox(new Vector3(-cubeData[1].xScale + cubeData[1].position.X, -cubeData[1].yScale + cubeData[1].position.Y, -cubeData[1].zScale + cubeData[1].position.Z), new Vector3(cubeData[1].xScale + cubeData[1].position.X, cubeData[1].yScale + cubeData[1].position.Y, cubeData[1].zScale + cubeData[1].position.Z));

                if (ballBoundingSphere.Intersects(playerPaddle1))
                {
                    deltaX = sphereData[i].position.X - cubeData[1].position.X;
                    deltaY = sphereData[i].position.Y - cubeData[1].position.Y;

                    sphereData[i].velocity.Normalize();

                    sphereData[i].velocity.X += deltaX;
                    sphereData[i].velocity.Y += deltaY;

                    sphereData[i].velocity.Normalize();

                    sphereData[i].velocity *= ballSpeed;
                    sphereData[i].velocity.Z = ballSpeed;

                    Console.WriteLine(sphereData[i].velocity);
                }

                //checks if the ball insterected with the bounding box
                if (ballBoundingSphere.Intersects(fieldBoundingBox))
                {
                    if (sphereData[i].position.X >= cubeData[0].xScale || sphereData[i].position.X <= -cubeData[0].xScale) sphereData[i].velocity *= new Vector3(-1f, 1f, 1f);
                    if (sphereData[i].position.Y >= cubeData[0].yScale || sphereData[i].position.Y <= -cubeData[0].yScale) sphereData[i].velocity *= new Vector3(1f, -1f, 1f);
                    if (sphereData[i].position.Z >= cubeData[0].zScale || sphereData[i].position.Z <= -cubeData[0].zScale) sphereData[i].velocity *= new Vector3(1f, 1f, -1f);
                }
            }

            //gets the state of the keyboards
            KeyboardState state = Keyboard.GetState();

            #region Rotation and Translations of View

            //paddle1 movement
            if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Up))
            {
                cubeData[1].position.Y += paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                cubeData[1].position.X -= paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Up))
            {
                cubeData[1].position.Y += paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                cubeData[1].position.X += paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
            else if (state.IsKeyDown(Keys.Up))
            {
                cubeData[1].position.Y += paddleSpeed1 * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Down))
            {
                cubeData[1].position.Y -= paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                cubeData[1].position.X -= paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
            else if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Down))
            {
                cubeData[1].position.Y -= paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                cubeData[1].position.X += paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                    cubeData[1].position.Y -= paddleSpeed1 * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            if (state.IsKeyDown(Keys.Left))
            {
                cubeData[1].position.X -= paddleSpeed1 * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            if (state.IsKeyDown(Keys.Right))
            {
                cubeData[1].position.X += paddleSpeed1 * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            //rotation = Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f);

            if (state.IsKeyDown(Keys.A) && pitch < MathHelper.Pi)
            {
                pitch += 0.05f;
            }

            if (state.IsKeyDown(Keys.S) && yaw > -MathHelper.PiOver2)
            {
                yaw -= 0.05f;
            }

            if (state.IsKeyDown(Keys.D) && pitch > 0)
            {
                pitch -= 0.05f;
            }

            if (state.IsKeyDown(Keys.W) && yaw < MathHelper.PiOver2)
            {
                yaw += 0.05f;
            }

            cameraPosition = new Vector3((float)Math.Cos(pitch) * 30, (float)Math.Sin(yaw) * 30, 50);

            view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);

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

            effect.VertexColorEnabled = true;

            //renders bounding box
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                effect.World = world * Matrix.CreateScale(cubeData[0].xScale, cubeData[0].yScale, cubeData[0].zScale) * Matrix.CreateTranslation(cubeData[0].position);
                effect.View = view;
                effect.Projection = projection;

                field.Render(GraphicsDevice);
            }

            Vector3 lightDirection = new Vector3(1f, 1f, -1f);

            foreach (EffectPass pass in paddleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                paddleEffect.World = world * Matrix.CreateScale(cubeData[1].xScale, cubeData[1].yScale, cubeData[1].zScale) * Matrix.CreateTranslation(cubeData[1].position);
                paddleEffect.View = view;
                paddleEffect.Projection = projection;

                paddleEffect.LightingEnabled = true;
                paddleEffect.DirectionalLight0.DiffuseColor = cubeData[1].color.ToVector3();
                paddleEffect.DirectionalLight0.Direction = lightDirection;
                paddleEffect.DirectionalLight0.SpecularColor = Vector3.One;

                paddleEffect.AmbientLightColor = cubeData[1].color.ToVector3();

                cube.Render(GraphicsDevice);
            }

            foreach (EffectPass pass in paddleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                paddleEffect.World = world * Matrix.CreateScale(cubeData[2].xScale, cubeData[2].yScale, cubeData[2].zScale) * Matrix.CreateTranslation(cubeData[2].position);
                paddleEffect.View = view;
                paddleEffect.Projection = projection;

                paddleEffect.LightingEnabled = true;
                paddleEffect.DirectionalLight0.DiffuseColor = cubeData[2].color.ToVector3();
                paddleEffect.DirectionalLight0.Direction = lightDirection;
                paddleEffect.DirectionalLight0.SpecularColor = Vector3.One;

                paddleEffect.AmbientLightColor = cubeData[2].color.ToVector3();

                cube.Render(GraphicsDevice);
            }

            //renders the ball
            foreach (EffectPass pass in ballEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                ballEffect.World = world * Matrix.CreateTranslation(sphereData[0].position);
                ballEffect.View = view;
                ballEffect.Projection = projection;

                ballEffect.LightingEnabled = true;
                ballEffect.DirectionalLight0.DiffuseColor = sphereData[0].color.ToVector3();
                ballEffect.DirectionalLight0.Direction = lightDirection;
                ballEffect.DirectionalLight0.SpecularColor = Vector3.One;

                ballEffect.AmbientLightColor = sphereData[0].color.ToVector3();

                sphere.Draw(ballEffect);
            }


            base.Draw(gameTime);
        }

        /// <summary>
        /// Generates a new random float between the min and max.
        /// </summary>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns></returns>
        private float NextFloat(float min, float max)
        {
            return (float)((new Random((int)DateTime.Now.Ticks & 0x0000FFFF).NextDouble()) * (max - min) + min);
        }

        /// <summary>
        /// Updates the position of the ball based on its speed.
        /// </summary>
        /// <param name="ball">The current ball</param>
        /// <param name="time">The current time elipased</param>
        /// <param name="position">The current ball's new position</param>
        private void UpdateBall(SphereData ball, float time, out Vector3 position)
        {
            position = ball.position + ball.velocity * time;
        }
    }
}
