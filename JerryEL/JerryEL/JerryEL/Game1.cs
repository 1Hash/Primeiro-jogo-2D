using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace JerryEL
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bgcenario, bginicio, bgfim;
        Personagem personagem;
        List<Itens> itens = new List<Itens>();
        List<Itens> auxitens = new List<Itens>();
        List<Itens> vida = new List<Itens>();
        List<Itens> auxvida = new List<Itens>();
        SoundEffect somHit;
        SoundEffect somBomba;
        SoundEffect somFrutas;
        SoundEffectInstance somHitInstance;
        SoundEffectInstance somBombaInstance;
        SoundEffectInstance somFrutasInstance;
        SoundEffect somDerruba;
        SoundEffectInstance somDerrubaInstance;
        Song music = null;
        SpriteFont fontePontos, fontePontuacao;
        Random r = new Random();
        Estado estado = Estado.Inicio;

        int count = 0;
        bool cond = false;
        int contdificuldade = 90;
        int contchances = 10;
        int auxpontos = 0;

        bool condiniciajogo = false;
        
        bool condcongelar = false;
        int contcongelar = 0;

        bool condvida = true;

        bool condcoletar = false;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 760;
            Window.Title = "Jerry: The Thief Mouse";

            personagem = new Personagem(this, new Vector2(640, 650));

            for (int i = 0; i < 3; i++)
            {
                vida.Add(new Itens(this, new Vector2(r.Next(1216), 50)));
            }

            for (int i = 0; i < 6; i++)
            {
                itens.Add(new Itens(this, new Vector2(r.Next(1216), -70)));
            }
        }

        protected override void Initialize()
        {
            personagem.Initialize();

            for (int i = 0; i < vida.Count; i++)
            {
                vida[i].Initialize();
            }

            for (int i = 0; i < itens.Count; i++)
            {
                itens[i].Initialize();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            bgcenario = Content.Load<Texture2D>(@"Textures\bg");

            bginicio = Content.Load<Texture2D>(@"Textures\inicio");

            bgfim = Content.Load<Texture2D>(@"Textures\gameover");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            fontePontos = Content.Load<SpriteFont>(@"Fontes\fontePontos");
            fontePontuacao = Content.Load<SpriteFont>(@"Fontes\fontePontuacao");

            personagem.LoadContent(this);

            itens[0].LoadContent(this, Itens.Alimentos.Queijo);
            itens[1].LoadContent(this, Itens.Alimentos.Bolo);
            itens[2].LoadContent(this, Itens.Alimentos.Cupcake);
            itens[3].LoadContent(this, Itens.Alimentos.Bomba);
            itens[4].LoadContent(this, Itens.Alimentos.Cereja);
            itens[5].LoadContent(this, Itens.Alimentos.Morango);

            vida[0].LoadContent(this, Itens.Alimentos.Vida);
            vida[1].LoadContent(this, Itens.Alimentos.Vida);
            vida[2].LoadContent(this, Itens.Alimentos.Vida);

            somHit = Content.Load<SoundEffect>(@"Audios\somhit");
            somHitInstance = somHit.CreateInstance();

            somBomba = Content.Load<SoundEffect>(@"Audios\sombomba");
            somBombaInstance = somBomba.CreateInstance();

            somFrutas = Content.Load<SoundEffect>(@"Audios\somfrutas");
            somFrutasInstance = somFrutas.CreateInstance();

            somDerruba = Content.Load<SoundEffect>(@"Audios\somderruba");
            somDerrubaInstance = somDerruba.CreateInstance();

            music = Content.Load<Song>(@"Audios\music");

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            switch (estado)
            {
                case Estado.Inicio:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        estado = Estado.Jogo;
                        iniciaJogo();
                    }
                break;
                case Estado.Fim:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        estado = Estado.Inicio;
                    }
                    break;
            }

            if (condiniciajogo)
            {
                if (condvida)
                {
                    for (int i = 0, var = 1150; i < vida.Count; i++, var -= 60)
                    {
                        Itens v = new Itens(this, new Vector2(var, 25));

                        v.setsb(vida[i].getsb());
                        v.setvelocidade(vida[i].getvelocidade());
                        v.settextura(vida[i].gettextura());
                        v.setpos(new Vector2(var, 25));
                        auxvida.Add(v);
                    }

                    condvida = false;
                }

                if (cond)
                {
                    Itens i = new Itens(this, new Vector2(r.Next(1216), -70));

                    int pos = r.Next(6);

                    i.setsb(itens[pos].getsb());
                    i.setvelocidade(itens[pos].getvelocidade());
                    i.settextura(itens[pos].gettextura());
                    i.setpos(new Vector2(r.Next(1216), -70));
                    auxitens.Add(i);

                    cond = false;
                }
                else
                {
                    count++;
                }

                if (count > contdificuldade)
                {
                    cond = true;
                    count = 0;
                }

                for (int i = 0; i < auxitens.Count; i++)
                {
                    if (personagem.boundingBox.Intersects(auxitens[i].boundingBox))
                    {
                        if (auxitens[i].gettextura() == Content.Load<Texture2D>(@"Textures\queijo"))
                        {
                            condcoletar = true;
                            auxitens.Remove(auxitens[i]);

                            personagem.GanhaPontoQueijo();

                            if (contdificuldade >= 30)
                            {
                                contdificuldade -= 3;
                            }

                            somHit.Play();
                        }
                        else if (auxitens[i].gettextura() == Content.Load<Texture2D>(@"Textures\bolo"))
                        {
                            condcoletar = true;
                            auxitens.Remove(auxitens[i]);

                            personagem.GanhaPontoBolo();

                            if (contdificuldade >= 30)
                            {
                                contdificuldade -= 3;
                            }

                            somHit.Play();
                        }
                        else if (auxitens[i].gettextura() == Content.Load<Texture2D>(@"Textures\cupcake"))
                        {
                            condcoletar = true;
                            auxitens.Remove(auxitens[i]);

                            personagem.GanhaPontoCupCake();

                            if (contdificuldade >= 30)
                            {
                                contdificuldade -= 3;
                            }

                            somHit.Play();
                        }
                        else if (auxitens[i].gettextura() == Content.Load<Texture2D>(@"Textures\bomba"))
                        {
                            condcongelar = true;
                            auxitens.Remove(auxitens[i]);

                            if (auxvida.Count > 0)
                            {
                                auxvida.Remove(auxvida[auxvida.Count - 1]);
                                somBomba.Play();
                            }

                        }
                        else
                        {
                            condcongelar = true;
                            auxitens.Remove(auxitens[i]);

                            somFrutas.Play(0.7f, 0f, 0f);
                        }
                    }
                }

                if (condcongelar == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        personagem.Mover(Personagem.Direcoes.Direita);
                        condcoletar = false;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        personagem.Mover(Personagem.Direcoes.Esquerda);
                        condcoletar = false;
                    }
                    else if (condcoletar)
                    {
                        personagem.Coletando();
                    }
                    else
                    {
                        personagem.Parar();
                    }
                }
                else
                {
                    personagem.Colidindo();
                    contcongelar++;
                }

                if (contcongelar > 60)
                {
                    condcongelar = false;
                    contcongelar = 0;
                }

                if (contchances <= 0 || auxvida.Count <= 0)
                {
                    estado = Estado.Fim;
                    auxpontos = personagem.getpontos();
                    auxitens.Clear();
                    auxvida.Clear();
                    condcongelar = false;
                    personagem.Parar();
                    fimJogo();
                }

                personagem.Update(gameTime);

                for (int i = 0; i < auxitens.Count; i++)
                {
                    auxitens[i].Update(gameTime);

                    if (auxitens[i].boundingBox.Y >= 700)
                    {
                        if (auxitens[i].gettextura() == Content.Load<Texture2D>(@"Textures\queijo") || auxitens[i].gettextura() == Content.Load<Texture2D>(@"Textures\bolo") || auxitens[i].gettextura() == Content.Load<Texture2D>(@"Textures\cupcake"))
                        {
                            contchances--;
                            somDerruba.Play(0.7f, 0f, 0f);
                        }

                        auxitens.Remove(auxitens[i]);
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (estado)
            {
                case Estado.Inicio:
                    spriteBatch.Draw(bginicio, Vector2.Zero, Color.White);
                    break;

                case Estado.Jogo:
                    spriteBatch.Draw(bgcenario, Vector2.Zero, Color.White);
                    spriteBatch.DrawString(fontePontos, "Chances: " + contchances.ToString(), new Vector2(10, 60), Color.Tomato);
                    break;

                case Estado.Fim:
                    spriteBatch.Draw(bgfim, Vector2.Zero, Color.White);
                    spriteBatch.DrawString(fontePontuacao, auxpontos.ToString(), new Vector2(600, 400), Color.Yellow);
                    break;
            }
            
            spriteBatch.End();

            switch (estado)
            {
                case Estado.Jogo:
                    personagem.Draw(gameTime);
                    break;
            }

            for (int i = 0; i < auxvida.Count; i++)
            {
                auxvida[i].Draw(gameTime);
            }

            for (int i = 0; i < auxitens.Count; i++)
            {
                auxitens[i].Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public void iniciaJogo()
        {
            personagem.setpontos(0);
            contchances = 10;
            condiniciajogo = true;
            condvida = true;
            contdificuldade = 90;
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;
        }

        public void fimJogo()
        {
            MediaPlayer.Stop();
            condiniciajogo = false;
        }
    }
}
