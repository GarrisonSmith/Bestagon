using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Bestagon
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private VertexBuffer _vertexBuffer;
		private BasicEffect _basicEffect;
		private Matrix _world;
		private Matrix _view;
		private Matrix _projection;
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();

			// Define the hexagon vertices
			VertexPositionColor[] vertices = CreateHexagons();

			// Set up the vertex buffer
			_vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
			_vertexBuffer.SetData(vertices);

			// Set up basic effect
			_basicEffect = new BasicEffect(GraphicsDevice)
			{
				VertexColorEnabled = true,
				LightingEnabled = false
			};

			// Set up matrices
			_world = Matrix.Identity;
			_view = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up);
			_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here

			// Rotate the cube
			float time = (float)gameTime.TotalGameTime.TotalSeconds;
			//_world = Matrix.CreateRotationY(time) * Matrix.CreateRotationX(time * 0.5f);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Set the vertex buffer
			GraphicsDevice.SetVertexBuffer(_vertexBuffer);

			// Draw each side of the cube
			_basicEffect.World = _world;
			_basicEffect.View = _view;
			_basicEffect.Projection = _projection;

			foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();

				// Draw front and back faces
				GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertexBuffer.VertexCount / 3);
			}

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}

		private VertexPositionColor[] CreateHexagons()
		{
			var vertices = new List<VertexPositionColor>();

			Vector3 center1 = new Vector3(0, 0, 0);
			vertices.AddRange(CreateHexagonalPrismVertices(center1, Color.Black));

			Vector3 center2 = new Vector3(0, 0, -0.5f);
			vertices.AddRange(CreateHexagonalPrismVertices(center2, Color.Gold));

			Vector3 center3 = new Vector3(0, 0, -1);
			vertices.AddRange(CreateHexagonalPrismVertices(center3, Color.Black));

			Vector3 center4 = new Vector3(0, 0, -1.5f);
			vertices.AddRange(CreateHexagonalPrismVertices(center4, Color.Gold));

			Vector3 center5 = new Vector3(0.75f*2, 0.433f*2, 0.5f);
			vertices.AddRange(CreateHexagonalPrismVertices(center5, Color.Gray));

			return vertices.ToArray();
		}

		private VertexPositionColor[] CreateHexagonalPrismVertices(Vector3 center, Color color)
		{
			// Hexagon vertices in the X-Y plane
			float radius = 1f;
			float halfHeight = 0.25f;

			// Generate top and bottom hexagon vertices
			Vector3[] top = new Vector3[6];
			Vector3[] bottom = new Vector3[6];
			for (int i = 0; i < 6; i++)
			{
				float angle = MathHelper.ToRadians(i * 60);
				float x = radius * MathF.Cos(angle);
				float y = radius * MathF.Sin(angle);
				top[i] = new Vector3(x, y, halfHeight) + center;
				bottom[i] = new Vector3(x, y, -halfHeight) + center;
			}

			// Build triangles for the hexagon prism
			var vertices = new List<VertexPositionColor>();

			// Top face
			for (int i = 1; i < 5; i++)
			{
				vertices.Add(new VertexPositionColor(top[0], color));
				vertices.Add(new VertexPositionColor(top[i], color));
				vertices.Add(new VertexPositionColor(top[i + 1], color));
			}

			// Bottom face
			for (int i = 1; i < 5; i++)
			{
				vertices.Add(new VertexPositionColor(bottom[0], color));
				vertices.Add(new VertexPositionColor(bottom[i + 1], color));
				vertices.Add(new VertexPositionColor(bottom[i], color));
			}

			// Side faces
			for (int i = 0; i < 6; i++)
			{
				int next = (i + 1) % 6;

				// First triangle
				vertices.Add(new VertexPositionColor(top[i], color));
				vertices.Add(new VertexPositionColor(bottom[i], color));
				vertices.Add(new VertexPositionColor(top[next], color));

				// Second triangle
				vertices.Add(new VertexPositionColor(bottom[i], color));
				vertices.Add(new VertexPositionColor(bottom[next], color));
				vertices.Add(new VertexPositionColor(top[next], color));
			}

			return vertices.ToArray();
		}
	}
}
