using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    public partial class Form1 : Form
    {

        int ballXSpeed = 4;
        int ballYSpeed = 4;
        int speed = 2; 

        //velocidade random pra bola e para o IA 
        Random rand = new Random();

        bool goDown, goUp;
        int IA_speed_change = 50; 
        int playerScore = 0;
        int IAScore = 0;

        int playerSpeed = 8;

        int[] i = { 5, 6, 8, 9 };
        int[] j = { 10, 9, 8, 11, 12 };





        public Form1()
        {
            InitializeComponent();
        }
       
        // metodo mais importante 
        private void GameTimerEvent(object sender, EventArgs e)
        {
            Ball.Top -= ballYSpeed; 
            Ball.Left -= ballXSpeed;

            this.Text = "Player Score : " + playerScore + " -- IA Score: " + IAScore; //Placar no canto superior do Form

            if (Ball.Top < 0 || Ball.Bottom > this.ClientSize.Height)
            {
                ballYSpeed = -ballYSpeed;

            }
            if(Ball.Left < -2)
            {
                Ball.Left = 300;
                ballXSpeed = -ballXSpeed;
                IAScore++; 
                // Se a bola tocar a parede da esquerda no quadrante -2 , será contato ponto pra IA 
                // o 300 é a posição central do mapa, assim que a bola tocar a margem esquerda, ela ira resetar a posição para o meio reiniciando o jogo
            }
            if (Ball.Right > this.ClientSize.Width + 2)
            {
                Ball.Left = 300;
                ballXSpeed = -ballXSpeed;
                playerScore++;
                //pontuação do jogador
            }
            //movimento do IA 
            if (IA.Top <= 1)
            {
                IA.Top = 0;
            }
            else if ( IA.Bottom >= this.ClientSize.Height)
            {
                IA.Top = this.ClientSize.Height - IA.Height;
            }

            if (Ball.Top < IA.Top + (IA.Height / 2 ) && Ball.Left > 300)
            {
                IA.Top -= speed; //Movimento do IA de acordo com a posição da bola 
            }
            if ( Ball.Top > IA.Top + (IA.Height / 2 ) && Ball.Left > 300)
            {
                IA.Top += speed; //Movimento inverso,caso a bola esteja movendo para baixo , estando abaixo da raquete da IA
                                    // > 300 significa que ja passou do meio campo e esta na area da IA, fazendo com que a raquete se mova
            }

            IA_speed_change -= 1;

            if (IA_speed_change < 0)
            {
                speed = i[rand.Next(i.Length)];
                IA_speed_change = 50;
            }

            if ( goDown && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += playerSpeed;
            }
            if ( goUp && player.Top > 0)
            {

                player.Top -= playerSpeed;  
            }

            CheckCollision(Ball, player, player.Right + 5);
            CheckCollision(Ball, IA, IA.Left - 35);


            //As validações abaixo definem quando o jogo devera acabar, pegando o metodo GameOver
            if(IAScore > 5)
            {
                GameOver("Voce Perdeu");

            }else if (playerScore > 5)
            {
                GameOver("Voce Venceu");
            }

            
        }

        //Pressiona tecla para baixo
        private void KeyIsDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Down) // Keys é uma lib que reconhece a tecla pressionada pelo usuario
            {
                goDown = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                goUp = true;

            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) { goDown  = false; }
                else if (e.KeyCode == Keys.Up) { goUp = false; }

        }

        //Valida se havera colisão entre os objetos ( no caso a bola (pickOne) com as raquetes(pickTwo) e as laterais(offSet) ) 
        private void CheckCollision(PictureBox PicOne, PictureBox PicTwo, int offSet) 
        {
            if (PicOne.Bounds.IntersectsWith(PicTwo.Bounds)) //se a bola intercecção com raquete
            {
                PicOne.Left = offSet;

                int x = j[rand.Next(j.Length)];
                int y = j[rand.Next(j.Length)];

                if (ballXSpeed < 0) // menor que zero é quando a bola estiver se movendo a esquerda
                {
                    ballXSpeed = x;
                }
                else  //quando a bola estiver indo em direção a direita , maior que zero 
                {
                    ballXSpeed = -x;
                    //ao setar valores positivos e negativos de acordo com a posição da bola, ela irá sempre para a direção oposta de acordo
                }
                if (ballYSpeed < 0)
                {
                    ballYSpeed = -y;
                }
                else { ballYSpeed = y;}
                //Mesma logica, porem na vertical, quando a bola se chocar com a parede de cima, ira para a direção oposta, mediante a posição dela no campo ( <> do que zero)



            }
        
        
        }


        private void GameOver (string message)
        {
            gameTimer.Stop();
            MessageBox.Show(message, "Cabo: ");
            IAScore = 0; 
            playerScore = 0;
            ballXSpeed = ballYSpeed = 4;
            IA_speed_change = 50;
            gameTimer.Start();

        }

    }
}
