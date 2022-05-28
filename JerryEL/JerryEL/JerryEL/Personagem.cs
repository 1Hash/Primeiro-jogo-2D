using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace JerryEL
{

    public class Personagem : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum Direcoes { Esquerda, Direita }
        public enum Estados { Parado, Correndo, Colidindo, Coletando }

        SpriteBatch spriteBatch;
        Texture2D textura;
        Vector2 posicao;
        Vector2 velocidade = new Vector2(10, 10);
        Vector2 tamanho = new Vector2(64, 64);
        Vector2 frame = new Vector2(0, 0);
        Estados estado = Estados.Parado;
        Direcoes direcao = Direcoes.Direita;
        int pontos = 0;
        SpriteFont fontePontos;
        public Rectangle boundingBox = new Rectangle();
        TimeSpan ultimoUpdate = TimeSpan.Zero;
        SoundEffect somSteps;
        SoundEffectInstance somStepsInstance;

        public int getpontos()
        {
            return pontos;
        }

        public void setpontos(int pontos)
        {
            this.pontos = pontos;
        }

        public Personagem(Game game)
            : base(game)
        {
            posicao = new Vector2(400, 300);
        }

        public Personagem(Game game, Vector2 argposicao)
            : base(game)
        {
            posicao = argposicao;
        }

        public void LoadContent(Game arggame)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textura = arggame.Content.Load<Texture2D>(@"Textures\jerry");
            fontePontos = arggame.Content.Load<SpriteFont>(@"Fontes\fontePontos");
            somSteps = arggame.Content.Load<SoundEffect>(@"Audios\somsteps");
            somStepsInstance = somSteps.CreateInstance();
        }

        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            boundingBox = new Rectangle(
                (int)posicao.X, (int)posicao.Y,
                (int)tamanho.X, (int)tamanho.Y
            );


            if (gameTime.TotalGameTime > ultimoUpdate + TimeSpan.FromMilliseconds(50))
            {
                frame.X++;
                ultimoUpdate = gameTime.TotalGameTime;
            }

            if (estado == Estados.Parado)
            {
                if (frame.X > 0)
                    frame.X = 0;
                frame.Y = 0;
            }
            else if (estado == Estados.Correndo)
            {
                if (frame.X > 7)
                    frame.X = 0;
                frame.Y = 1;
            }
            else if (estado == Estados.Colidindo)
            {
                if (frame.X > 0)
                    frame.X = 0;
                frame.Y = 2;
            }
            else if (estado == Estados.Coletando)
            {
                if (frame.X > 0)
                    frame.X = 0;
                frame.Y = 3;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                fontePontos,
                "Pontos: " + pontos.ToString(),
                new Vector2(10, 10),
                Color.Tomato
                );
            spriteBatch.Draw(
               textura,
               new Rectangle(
                  (int)posicao.X, (int)posicao.Y,
                  (int)tamanho.X, (int)tamanho.Y),
               new Rectangle(
                  (int)(frame.X * tamanho.X), (int)(frame.Y * tamanho.Y),
                  (int)tamanho.X, (int)tamanho.Y),
               Color.White,
               0f,
               Vector2.Zero,
               direcao == Direcoes.Direita ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
               0
            );
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Mover(Direcoes argdirecao)
        {
            estado = Estados.Correndo;
            direcao = argdirecao;
            if (somStepsInstance.State == SoundState.Stopped)
                somStepsInstance.Play();

            if (argdirecao == Direcoes.Direita)
            {
                if (posicao.X < 1216)
                {
                    posicao.X += velocidade.X;
                }
                else
                {
                    Parar();
                }
            }
            else
            {
                if (posicao.X > 0)
                {
                    posicao.X -= velocidade.X;
                }
                else
                {
                    Parar();
                }
            }
        }

        public void Parar()
        {
            estado = Estados.Parado;
            if (somStepsInstance.State == SoundState.Playing)
                somStepsInstance.Stop();
        }

        public void Colidindo()
        {
            estado = Estados.Colidindo;
        }

        public void Coletando()
        {
            estado = Estados.Coletando;
        }

        public void GanhaPontoQueijo()
        {
            pontos+=5;
        }

        public void GanhaPontoBolo()
        {
            pontos+=3;
        }

        public void GanhaPontoCupCake()
        {
            pontos++;
        }
    }
}
