using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;

namespace ScreenShotTest
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class AnGame : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		private SpriteFont font;
		RenderTarget2D renderTarget;
		private Texture2D texture;
		private bool save;
		private const int tile = 8;
		private const Size size = Size.Med;

		int xx = 2;int qy = 5;
		int oay = 9; int oby = 13; int ocy = 17; int ody = 21;
		//int quiz = -1;
		string path = "";
		public AnGame()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 256 * (int)size;
			graphics.PreferredBackBufferHeight = 192 * (int)size;
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
			save = false;
			if (null != ConfigurationManager.AppSettings["save"])
			{
				save = Convert.ToBoolean(ConfigurationManager.AppSettings["save"]);
			}
			if (null != ConfigurationManager.AppSettings["quiz"])
			{
				//quiz = Convert.ToInt32(ConfigurationManager.AppSettings["quiz"]);
			}
			path = ConfigurationManager.AppSettings["path"];

			IsMouseVisible = true;
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

			font = Content.Load<SpriteFont>("Emulogic" + size.ToString().ToUpper());
			texture = Content.Load<Texture2D>("Simpsons" + size);

			PresentationParameters pp = GraphicsDevice.PresentationParameters;
			int width = pp.BackBufferWidth;
			int height = pp.BackBufferHeight;
			renderTarget = new RenderTarget2D(GraphicsDevice, width, height, 1, GraphicsDevice.DisplayMode.Format);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			Content.Unload();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			string inputFile = "inp/*.csv";
			string[] files = Directory.GetFiles(".", inputFile);

			save = true;
			if (save)
			{
				foreach (string file in files)
				{
					string filename = file.Replace(".\\inp\\", "");
					filename = filename.Replace(".csv", "");
					string outDir = "out/" + filename;
					if (!Directory.Exists(outDir))
					{
						Directory.CreateDirectory(outDir);
					}

					String[] lines = System.IO.File.ReadAllLines(file);
					//for (int idx = 0; idx < 2; idx++)
					for (int idx = 0; idx < lines.Length; idx++)
					{
						GraphicsDevice.SetRenderTarget(0, renderTarget);
						GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1, 0);

						string line = lines[idx];
						if (line.StartsWith(@"\\") || line.StartsWith(@"//") || line.StartsWith(@"##"))
						{
							continue;
						}

						Draw(line, idx);
						base.Draw(gameTime);

						GraphicsDevice.SetRenderTarget(0, null);
						Texture2D resolvedTexture = renderTarget.GetTexture();
						//resolvedTexture.Save("test.bmp", ImageFileFormat.Bmp);

						string bob = (idx + 1).ToString().PadLeft(3, '0');
						string outputFile = System.String.Format("{0}/{1}.png", outDir, bob);
						resolvedTexture.Save(outputFile, ImageFileFormat.Png);
					}
				}

				Exit();
			}
			else
			{
				foreach (string file in files)
				{
					string filename = file.Replace(".\\inp\\", "");
					filename = filename.Replace(".csv", "");
					string outDir = "out/" + filename;
					if (!Directory.Exists(outDir))
					{
						Directory.CreateDirectory(outDir);
					}

					String[] lines = System.IO.File.ReadAllLines(file);
					for (int idx = 0; idx < lines.Length; idx++)
					{
						string line = lines[idx];
						if(!line.StartsWith(@"\\") || !line.StartsWith(@"//") || !line.StartsWith(@"##"))
						{

						//if ((idx+1) == quiz)
						//{
						//	string line = lines[idx];
							Draw(line, 0);
						//	break;
						//}
						}
					}
				}

				

				base.Draw(gameTime);
			}
		}

		private void Draw(string line, int idx)
		{
			graphics.GraphicsDevice.Clear(Color.White);

			spriteBatch.Begin();

			spriteBatch.Draw(texture, Vector2.Zero, Color.White);

			//string line = "WHO OWNS THE KWIK E MART?;GIL;SANJAY;APU;ABE";
			string[] texts = line.Split(new char[] {';'});
			string answ = texts[0];
			string quiz = texts[1];
			string optA = texts[2];
			string optB = texts[3];
			string optC = texts[4];
			string optD = texts[5];

			//for (int i = 0; i < 24; i++)
			//{
			//    Draw("X", 0, i);
			//}
			//Draw("XXX", 0, 0);

			Draw("QUESTION #", 2, 3);
			Draw((idx+1).ToString(), 12, 3);
			//Draw(ques.ToString(), 12, 3);
			//Draw("========", 2, 4);
			//Draw("--------", 2, 4);

			Draw("SCORE", 22, 3);
			//Draw("-----", 22, 4);
			Draw("000", 28, 3);

			Draw("A.", xx, oay); Draw("B.", xx, oby); Draw("C.", xx, ocy); Draw("D.", xx, ody);

			Draw(quiz, xx + 0, qy);
			Draw(optA, xx + 2, oay);
			Draw(optB, xx + 2, oby);
			Draw(optC, xx + 2, ocy);
			Draw(optD, xx + 2, ody);

			// Draw answer
			if ("0" != answ)
			{
				if ("1" == answ)
				{
					Draw(">", xx-1, oay); 
				}
				if ("2" == answ)
				{
					Draw(">", xx - 1, oby);
				}
				if ("3" == answ)
				{
					Draw(">", xx - 1, ocy);
				}
				if ("4" == answ)
				{
					Draw(">", xx - 1, ody);
				}
			}

			spriteBatch.End();
		}

		private void Draw(string text, int x, int y)
		{
			string[] texts = text.Split(new char[] {'|'});
			int px = x*(int) size*tile;
			int py;

			for (int idx = 0; idx < texts.Length; idx++)
			{
				string data = texts[idx];
				data = data.Replace(@"\""", @"'");
				py = (y + idx) * (int)size * tile;
				spriteBatch.DrawString(font, data, new Vector2(px, py), Color.Black);
			}

			
		}

	}
}