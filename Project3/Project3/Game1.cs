using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Primitives;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
        TextureCube skyboxTexture;
        SpriteFont text;
        Texture2D paddles;
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
        BasicEffect indicaterEffect;
        Effect skyboxEffect;
        BoundingBox fieldBoundingBox;
        BoundingBox playerPaddle1;
        BoundingBox playerPaddle2;
        BoundingSphere ballBoundingSphere;
        SoundEffect wubba;
        SoundEffect rekt;
        Song background;
        

        Matrix world;
        Matrix view;
        Matrix projection;
        Vector3 cameraPosition = new Vector3(0, 0, 50f);
        float ballSpeed = 10f, paddleSpeed1 = 10f, pitch = MathHelper.PiOver2, yaw = 0;
        private int player1Score = 0, player2Score = 0;
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
            indicaterEffect = new BasicEffect(GraphicsDevice);
            paddleEffect = new BasicEffect(GraphicsDevice);
            
            field = new LineCube();

            cubeData = new CubeData[5];
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
            cubeData[1].position = new Vector3(0, 0, 19.8f);
            cubeData[1].color = new Color(NextFloat(0.5f, 1), 0, NextFloat(0.5f, 1));

            //paddles info player 2
            cubeData[2].xScale = 1;
            cubeData[2].yScale = 1;
            cubeData[2].zScale = 0.2f;
            cubeData[2].position = new Vector3(0, 0, -19.8f);
            cubeData[2].color = new Color(NextFloat(0.5f, 1), NextFloat(0.5f, 1), 0);

            //skybox
            cubeData[3].scale = 200;
            cubeData[3].position = new Vector3(0, 0, 0);

            //indication
            cubeData[4].xScale = 1;
            cubeData[4].yScale = 1;
            cubeData[4].zScale = 0.1f;
            cubeData[4].position = new Vector3(0, 0, -19.8f);
            cubeData[4].color = new Color(0, NextFloat(0.5f, 1), 0, 0.0f);

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            skyboxTexture = Content.Load<TextureCube>("Skybox/SkyBoxTexture");
            paddles = Content.Load<Texture2D>("Paddles/rick");
            skyboxEffect = Content.Load<Effect>("Skybox/skybox");
            text = Content.Load<SpriteFont>("text");
            wubba = Content.Load<SoundEffect>("Sounds/Rick");
            rekt = Content.Load<SoundEffect>("Sounds/Rekt");
            background = Content.Load<Song>("theme");
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.Play(background);
            MediaPlayer.IsRepeating = true;
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

            #region Checking if Ball hit something
            Vector3 p = Vector3.Zero;
            float deltaX = 0, deltaY = 0;
            //checks if the ball intersects with the bound box and inverts the apporiate velocity
            for (int i = 0; i < sphereData.Length; i++)
            {
                //creates the bounding sphere around the ball
                ballBoundingSphere = new BoundingSphere(sphereData[i].position, 1f);
                playerPaddle1 = new BoundingBox(new Vector3(-cubeData[1].xScale + cubeData[1].position.X, -cubeData[1].yScale + cubeData[1].position.Y, -cubeData[1].zScale + cubeData[1].position.Z), new Vector3(cubeData[1].xScale + cubeData[1].position.X, cubeData[1].yScale + cubeData[1].position.Y, cubeData[1].zScale + cubeData[1].position.Z));

                if (BallHitPaddle(sphereData[i], cubeData[1]))
                {
                    wubba.Play();

                    if (cubeData[1].position.X == sphereData[i].position.X && cubeData[1].position.Y == sphereData[i].position.Y)
                        sphereData[i].velocity *= new Vector3(1f, 1f, -1f);
                    else
                    {
                        deltaX = sphereData[i].position.X - cubeData[1].position.X;
                        deltaY = sphereData[i].position.Y - cubeData[1].position.Y;

                        sphereData[i].velocity.Normalize();

                        sphereData[i].velocity.X += deltaX;
                        sphereData[i].velocity.Y += deltaY;

                        sphereData[i].velocity.Normalize();

                        sphereData[i].velocity *= ballSpeed;
                        sphereData[i].velocity.Z = -ballSpeed;
                    }

                    Console.WriteLine(sphereData[i].velocity);
                }

                if (BallHitPaddle2(sphereData[i], cubeData[2]))
                {
                    rekt.Play();

                    if (cubeData[2].position.X == sphereData[i].position.X && cubeData[2].position.Y == sphereData[i].position.Y)
                        sphereData[i].velocity *= new Vector3(1f, 1f, -1f);
                    else
                    {
                        deltaX = sphereData[i].position.X - cubeData[2].position.X;
                        deltaY = sphereData[i].position.Y - cubeData[2].position.Y;

                        sphereData[i].velocity.Normalize();

                        sphereData[i].velocity.X += deltaX;
                        sphereData[i].velocity.Y += deltaY;

                        sphereData[i].velocity.Normalize();

                        sphereData[i].velocity *= ballSpeed;
                        sphereData[i].velocity.Z = -ballSpeed;
                    }

                    Console.WriteLine(sphereData[i].velocity + "Paddle#2");
                }

                //indicater update
                if (sphereData[i].position.Z >= 7 && sphereData[i].velocity.Z > 0)
                {
                    cubeData[4].position.X = sphereData[i].position.X;
                    cubeData[4].position.Y = sphereData[i].position.Y;
                    cubeData[4].position.Z = 20;
                }
                else if (sphereData[i].position.Z <= -7 && sphereData[i].velocity.Z < 0)
                {
                    cubeData[4].position.X = sphereData[i].position.X;
                    cubeData[4].position.Y = sphereData[i].position.Y;
                    cubeData[4].position.Z = -20;
                }

                //checks if the ball insterected with the bounding box
                if (ballBoundingSphere.Intersects(fieldBoundingBox))
                {
                    if (sphereData[i].position.X >= cubeData[0].xScale || sphereData[i].position.X <= -cubeData[0].xScale) sphereData[i].velocity *= new Vector3(-1f, 1f, 1f);
                    if (sphereData[i].position.Y >= cubeData[0].yScale || sphereData[i].position.Y <= -cubeData[0].yScale) sphereData[i].velocity *= new Vector3(1f, -1f, 1f);

                    if (sphereData[i].position.Z >= cubeData[0].zScale)
                    {
                        player2Score++;
                        sphereData[i].position = Vector3.Zero;
                        sphereData[i].velocity = new Vector3(0f, 0f, ballSpeed);
                        cubeData[1].position = new Vector3(0, 0, 19.8f);
                    }
                    else if (sphereData[i].position.Z <= -cubeData[0].zScale)
                    {
                        player1Score++;
                        sphereData[i].position = Vector3.Zero;
                        sphereData[i].velocity = new Vector3(0f, 0f, -ballSpeed);
                        cubeData[2].position = new Vector3(0, 0, -19.8f);

                    }
                }

                UpdateBall(sphereData[i], gameTime.ElapsedGameTime.Milliseconds / 1000f, out p);
                sphereData[i].position = p;
            }

            #endregion

            //gets the state of the keyboards
            KeyboardState state = Keyboard.GetState();

            #region Paddle1 Movement

            //paddle1 movement keeps paddle in bounding box
            if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Up) && cubeData[1].position.Y + cubeData[1].yScale < cubeData[0].position.Y + cubeData[0].yScale && cubeData[1].position.X - cubeData[1].xScale > cubeData[0].position.X - cubeData[0].xScale)
            {
                cubeData[1].position.Y += paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                cubeData[1].position.X -= paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Up) && cubeData[1].position.Y + cubeData[1].yScale < cubeData[0].position.Y + cubeData[0].yScale && cubeData[1].position.X + cubeData[1].xScale < cubeData[0].position.X + cubeData[0].xScale)
            {
                cubeData[1].position.Y += paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                cubeData[1].position.X += paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
            else if (state.IsKeyDown(Keys.Up) && cubeData[1].position.Y + cubeData[1].yScale < cubeData[0].position.Y + cubeData[0].yScale)
            {
                cubeData[1].position.Y += paddleSpeed1 * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Down) && cubeData[1].position.Y - cubeData[1].yScale > cubeData[0].position.Y - cubeData[0].yScale && cubeData[1].position.X - cubeData[1].xScale > cubeData[0].position.X - cubeData[0].xScale)
            {
                cubeData[1].position.Y -= paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                cubeData[1].position.X -= paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
            else if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Down) && cubeData[1].position.Y - cubeData[1].yScale > cubeData[0].position.Y - cubeData[0].yScale && cubeData[1].position.X + cubeData[1].xScale < cubeData[0].position.X + cubeData[0].xScale)
            {
                cubeData[1].position.Y -= paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                cubeData[1].position.X += paddleSpeed1 * (float)(Math.Sqrt(2) / 2) * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
            else if (state.IsKeyDown(Keys.Down) && cubeData[1].position.Y - cubeData[1].yScale > cubeData[0].position.Y - cubeData[0].yScale)
            {
                cubeData[1].position.Y -= paddleSpeed1 * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            if (state.IsKeyDown(Keys.Left) && cubeData[1].position.X - cubeData[1].xScale > cubeData[0].position.X - cubeData[0].xScale)
            {
                cubeData[1].position.X -= paddleSpeed1 * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            if (state.IsKeyDown(Keys.Right) && cubeData[1].position.X + cubeData[1].xScale < cubeData[0].position.X + cubeData[0].xScale)
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

            #region Paddle2 Movement
            if (sphereData[0].position.Z < 0)
            { 
            
                float paddle2X;
                float paddle2Y;
                
                paddle2X = cubeData[2].position.X - sphereData[0].position.X;
                paddle2Y = cubeData[2].position.Y - sphereData[0].position.Y;
                Vector3 Velocity = new Vector3(paddle2X, paddle2Y, 0);
                Velocity.Normalize();
                //5Velocity *= -1; 

                cubeData[2].position += (new Vector3(paddle2X, paddle2Y, 0)) * 0.5f * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            else if(sphereData[0].position.Z > 0)
            {
                float paddle2X;
                float paddle2Y;

                paddle2X = 0 - cubeData[2].position.X;
                paddle2Y = 0 - cubeData[2].position.Y;

                Vector3 Velocity = new Vector3(paddle2X, paddle2Y, 0);
                Velocity.Normalize();

                if (cubeData[2].position.X != 0 && cubeData[2].position.Y != 0)
                    cubeData[2].position += (new Vector3(paddle2X, paddle2Y, 0)) * 8f * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            #endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Color textColor = Color.DarkViolet;
            textColor.A = 128;

            GraphicsDevice.Clear(Color.Black);

            effect.VertexColorEnabled = true;

            RasterizerState orginal = GraphicsDevice.RasterizerState;
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            #region Renders Objects
            //renders skycube
            foreach (EffectTechnique technique in skyboxEffect.Techniques)
            {
                foreach (EffectPass pass in technique.Passes)
                {
                    pass.Apply();

                    skyboxEffect.Parameters["World"].SetValue(world * Matrix.CreateScale(cubeData[3].scale) * Matrix.CreateTranslation(cubeData[3].position));
                    skyboxEffect.Parameters["View"].SetValue(view);
                    skyboxEffect.Parameters["Projection"].SetValue(projection);
                    skyboxEffect.Parameters["SkyBoxTexture"].SetValue(skyboxTexture);
                    skyboxEffect.Parameters["CameraPosition"].SetValue(cameraPosition);

                    cube.Render(GraphicsDevice);//renders the skycube
                }
            }

            Vector3 lightDirection = new Vector3(1f, 1f, -1f);

            foreach (EffectPass pass in paddleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                paddleEffect.World = world * Matrix.CreateScale(cubeData[1].xScale, cubeData[1].yScale, cubeData[1].zScale) * Matrix.CreateTranslation(cubeData[1].position);
                paddleEffect.View = view;
                paddleEffect.Projection = projection;
                paddleEffect.Texture = paddles;
                paddleEffect.TextureEnabled = true;

                cube.Render(GraphicsDevice);//renders paddle 1
            }

            foreach (EffectPass pass in paddleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                paddleEffect.World = world * Matrix.CreateScale(cubeData[2].xScale, cubeData[2].yScale, cubeData[2].zScale) * Matrix.CreateTranslation(cubeData[2].position);
                paddleEffect.View = view;
                paddleEffect.Projection = projection;
                paddleEffect.Texture = paddles;
                paddleEffect.TextureEnabled = true;

                cube.Render(GraphicsDevice);//renders paddle2
            }

            foreach (EffectPass pass in indicaterEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                indicaterEffect.World = world * Matrix.CreateScale(cubeData[4].xScale, cubeData[4].yScale, cubeData[4].zScale) * Matrix.CreateTranslation(cubeData[4].position);
                indicaterEffect.View = view;
                indicaterEffect.Projection = projection;

                indicaterEffect.TextureEnabled = false;
                indicaterEffect.LightingEnabled = true;
                indicaterEffect.DirectionalLight0.DiffuseColor = cubeData[4].color.ToVector3();
                indicaterEffect.Alpha = 0.2f;
                indicaterEffect.DirectionalLight0.Direction = lightDirection;
                indicaterEffect.AmbientLightColor = cubeData[4].color.ToVector3();

                cube.Render(GraphicsDevice);//renders indicates
            }

            GraphicsDevice.RasterizerState = orginal;

            //renders bounding box
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                effect.World = world * Matrix.CreateScale(cubeData[0].xScale, cubeData[0].yScale, cubeData[0].zScale) * Matrix.CreateTranslation(cubeData[0].position);
                effect.View = view;
                effect.Projection = projection;

                field.Render(GraphicsDevice);
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

            #endregion

            spriteBatch.Begin();

            spriteBatch.DrawString(text, String.Format("Human Score: {0}", player1Score), new Vector2(0, 0), textColor);
            spriteBatch.DrawString(text, String.Format("Computer Score: {0}", player2Score), new Vector2(590, 0), textColor);

            spriteBatch.End();


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

        /// <summary>
        /// Checks if the ball is hitting the paddles surface
        /// </summary>
        /// <param name="ball">Ball to check against paddle</param>
        /// <param name="paddle">Paddle you are checking</param>
        /// <returns></returns>
        private bool BallHitPaddle(SphereData ball, CubeData paddle)
        {
            return ((ball.position.Z + 1 >= paddle.position.Z - paddle.zScale && ball.position.X >= paddle.position.X - paddle.xScale && ball.position.X <= paddle.position.X + paddle.xScale && ball.position.Y >= paddle.position.Y - paddle.yScale && ball.position.Y <= paddle.position.Y + paddle.yScale));
        }

        private bool BallHitPaddle2(SphereData ball, CubeData paddle)
        {
            return ((ball.position.Z - 1 <= paddle.position.Z + paddle.zScale && ball.position.X >= paddle.position.X - paddle.xScale && ball.position.X <= paddle.position.X + paddle.xScale && ball.position.Y >= paddle.position.Y - paddle.yScale && ball.position.Y <= paddle.position.Y + paddle.yScale));
        }
    }
}
