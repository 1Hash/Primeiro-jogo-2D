using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JerryEL
{

    public class Itens : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum Direcoes { Esquerda, Direita }
        public enum Alimentos { Queijo, Bolo, Cupcake, Bomba, Cereja, Morango, Vida }

        SpriteBatch spriteBatch;
        Texture2D textura;
        Vector2 posicao;
        Vector2 velocidade = new Vector2(5, 1);
        public Rectangle boundingBox = new Rectangle();


        public Itens(Game game, Vector2 argposicao)
            : base(game)
        {
            posicao = argposicao;
        }

        public void LoadContent(Game arggame, Alimentos tipoalimentos)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (tipoalimentos == Alimentos.Queijo)
            {
                textura = arggame.Content.Load<Texture2D>(@"Textures\queijo");
            }
            else if (tipoalimentos == Alimentos.Bolo)
            {
                textura = arggame.Content.Load<Texture2D>(@"Textures\bolo");
            }
            else if (tipoalimentos == Alimentos.Cupcake)
            {
                textura = arggame.Content.Load<Texture2D>(@"Textures\cupcake");
            }
            else if (tipoalimentos == Alimentos.Bomba)
            {
                textura = arggame.Content.Load<Texture2D>(@"Textures\bomba");
            }
            else if (tipoalimentos == Alimentos.Cereja)
            {
                textura = arggame.Content.Load<Texture2D>(@"Textures\cereja");
            }
            else if (tipoalimentos == Alimentos.Morango)
            {
                textura = arggame.Content.Load<Texture2D>(@"Textures\morango");
            }
            else if (tipoalimentos == Alimentos.Vida)
            {
                textura = arggame.Content.Load<Texture2D>(@"Textures\vida");
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            posicao.Y += 5;

            boundingBox = new Rectangle(
                (int)posicao.X, (int)posicao.Y,
                textura.Width, textura.Height
            );

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                textura,
                new Rectangle((int)posicao.X, (int)posicao.Y, textura.Width, textura.Height),
                Color.White
            );
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Mover(Direcoes argdirecao)
        {
            switch (argdirecao)
            {
                case Direcoes.Direita: posicao.X += velocidade.X; break;
                case Direcoes.Esquerda: posicao.X -= velocidade.X; break;
            }
        }

    
        public Texture2D gettextura()
        {
            return textura;
        }

        public void settextura(Texture2D t)
        {
            textura = t;
        }

        public Vector2 getvelocidade()
        {
            return velocidade;
        }

        public void setvelocidade(Vector2 v)
        {
            velocidade = v;
        }

        public SpriteBatch getsb()
        {
            return spriteBatch;
        }

        public void setsb(SpriteBatch sb)
        {
            spriteBatch = sb;
        }

        public Vector2 getpos()
        {
            return posicao;
        }

        public void setpos(Vector2 v)
        {
            posicao = v;
        }

    }
}
