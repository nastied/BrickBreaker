using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BrickBreaker
{
    public partial class Form1 : Form
    {
        int score = 0;
        Random rnd = new Random();
        string[] colors = { "blue", "green", "pink", "red", "yellow" };

        int ballX = 4, ballY = 4;
        int cnt = 4;

        bool gameOver = false;

        bool goLeft = false;
        bool goRight = false;
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void RestartGame()
        {
            foreach (Control p in this.Controls)
            {
                if ((p is PictureBox) && p.Tag?.ToString() != "no")
                {
                    this.Controls.Remove(p);
                }
            }

            int x = 15;
            int y = 74;
            for (int i = 1; i <= 4; i++)
            {
                x = 15;

                for(int j = 1; j <= 6; j++)
                {
                    PictureBox pbx = new PictureBox();
                    pbx.Location = new Point(x, y);
                    pbx.Tag = "block";
                    pbx.SizeMode = PictureBoxSizeMode.AutoSize;
                    this.Controls.Add(pbx);

                    x += 56;
                }

                y += 31;
            }

            cnt = 4;
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\img\" + cnt + ".png";
            healthbar.Image = Image.FromFile(path);

            player.Left = (this.ClientSize.Width - player.Width) / 2;
            player.Top = this.ClientSize.Height - player.Height - 20;

            ball.Left = player.Left + (player.Width / 2) - (ball.Width / 2);
            ball.Top = player.Top - ball.Height - 5;

            ballX = 4;
            ballY = -4;

            gameOver = false;
            score = 0;
            lblscore.Text = "Score: " + score;

            foreach (Control p in this.Controls)
            {
                if (p is PictureBox && p.Tag?.ToString() == "block")
                {
                    int nr = rnd.Next(0, 5);

                    string path1 = AppDomain.CurrentDomain.BaseDirectory + @"\img\" + colors[nr] + ".png";
                    ((PictureBox)p).Image = Image.FromFile(path1);
                }       
            }
            timer1.Start();
        }

        private void BallMovement()
        {
            ball.Left += ballX;
            ball.Top += ballY;
            if (ball.Left > this.ClientSize.Width - ball.Width || ball.Left < 5)
            {
                ballX = -ballX;
            }

            
            if (ball.Top < 4)
            {
                ballY = -ballY;
            }
            if (ball.Bounds.IntersectsWith(player.Bounds) && ballY > 0)
            {
                ballY = -ballY;
            }

            if (ball.Top > this.ClientSize.Height - ball.Height)
            {
                if (cnt > 0)
                {
                    cnt--;
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"\img\" + cnt + ".png";
                    healthbar.Image = Image.FromFile(path);

                    ballY = -ballY;
                }
                else
                {
                    gameOver = true;
                }
            }
        }

        private void Game()
        {
            Control blockToRemove = null;

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox &&  c.Tag?.ToString() == "block")
                {
                    if (ball.Bounds.IntersectsWith(c.Bounds))
                    {
                        blockToRemove = c;
                        ballY = -ballY;
                        score++;
                        lblscore.Text = "Score: " + score.ToString();
                        break;
                    }
                }
            }

            if (blockToRemove != null)
            {
                this.Controls.Remove(blockToRemove);
                blockToRemove.Dispose();
            }
        }

        private void GameOver()
        {
            if (gameOver)
            {
                timer1.Stop();
                MessageBox.Show("GAME OVER!\n Press OK to restart.");
                RestartGame();
                //this.Close();
            }
        }

        private void Win()
        {
            if (score == 24)
            {
                timer1.Stop();
                if (cnt == 4)
                {
                    MessageBox.Show("PERFECT SCORE! CONGRATS!\n Press OK to restart.");
                    RestartGame();
                }
                else
                {
                    MessageBox.Show("CONGRATS! YOU WON!\n Press OK to restart.");
                    RestartGame();
                }
                //this.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (goLeft && player.Left > 5)
            {
                player.Left -= 8;
                if (player.Left < 5)
                {
                    player.Left = 5;
                }
            }

            if (goRight && player.Left < this.ClientSize.Width - player.Width)
            {
                player.Left += 8;
                if (player.Left > this.ClientSize.Width - player.Width)
                {
                    player.Left = this.ClientSize.Width - player.Width;
                }
            }

            BallMovement();
            Game();
            GameOver();
            Win();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) goLeft = true;
            if (e.KeyCode == Keys.Right) goRight = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) goLeft = false;
            if (e.KeyCode == Keys.Right) goRight = false;
        }
    }
}
