#region File Header

// //---------------------------------------------------------------------------------------------------------------------//
// // User Name: 
// // File Name :BouncingBall.cs
// // Date :2019 / 04 / 23 / 11:47
// // File Data: 2018 / 10 / 18
// //---------------------------------------------------------------------------------------------------------------------//

#endregion

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;


namespace IliumManager.Components
{
    public partial class BouncingBall : UserControl
    {
        // Some drawing parameters.
        private const int BallWidth = 20;
        private const int BallHeight = 20;

        private const int GateWidth = 1;
        private const int GateHeight = 200;

        private readonly Random RandomNumber = new Random(255);

        private Brush BallColor = Brushes.DodgerBlue;

        private const int BallAmount = 5;

        // Velocity.
        private int[] BallsVx = new int[BallAmount];
        private int[] BallsVy = new int[BallAmount];
        // Position.
        private int[] BallsX = new int[BallAmount];
        private int[] BallsY = new int[BallAmount];

        private Brush[] BallColors = new Brush[BallAmount];

        private Rectangle Gate;
        private Rectangle[] Balls = new Rectangle[BallAmount];

        //----------------------------------------------------------------------------------------------------------------//
        public BouncingBall()
        {
            this.InitializeComponent();
            this.Init();
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     Init Form
        /// </summary>
        private void Init()
        {
            this.tmrBall.Enabled = false;
            this.Visible = false;
            // Pick a random start position and velocity.
            var rnd = new Random();

            for (int i = 0; i < BallAmount; i++)
            {
                this.BallColors[i] = Brushes.DodgerBlue;
                this.BallsVx[i] = rnd.Next(3, 6);
                this.BallsVy[i] = rnd.Next(3, 12);
                this.BallsX[i] = rnd.Next(0, this.ClientSize.Width - BallWidth);
                this.BallsY[i] = rnd.Next(0, this.ClientSize.Height - BallHeight);
                this.Balls[i] = new Rectangle(Int32.Parse(this.BallsX[i].ToString()), Int32.Parse(this.BallsY[i].ToString()), BallWidth, BallHeight);
            }

            // Use double buffering to reduce flicker.
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);
            this.UpdateStyles();
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     Timer Tick Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TmrBall_Tick(object sender, EventArgs e)
        {
            this.Tick();
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     Paint event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BouncingBall_Paint(object sender, PaintEventArgs e)
        {
            Gate = new Rectangle((this.ClientSize.Width - GateWidth) / 2, (this.ClientSize.Height - GateHeight) / 2, GateWidth, GateHeight);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(this.BackColor);
            //e.Graphics.FillEllipse(this.BallColor, this.BallX, this.BallY, BallWidth, BallHeight);
            //e.Graphics.DrawEllipse(Pens.Black, this.BallX, this.BallY, BallWidth, BallHeight);

            e.Graphics.FillRectangle(this.BallColor, Gate);
            e.Graphics.DrawRectangle(Pens.Black, Gate);

            for (int i = 0; i < BallAmount; i++)
            {
                e.Graphics.FillEllipse(this.BallColors[i], Balls[i]);
                e.Graphics.DrawEllipse(Pens.Black, Balls[i]);
            }
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     To Process Ticks
        /// </summary>
        private void Tick()
        {

            for (int i = 0; i < BallAmount; i++)
            {
                //Ball's new X position
                this.Balls[i].X += this.BallsVx[i];
                //Collision with X axis walls
                if (this.Balls[i].X < 0)
                {
                    this.BallsVx[i] = -this.BallsVx[i];
                    Boing(i);
                }
                else if (this.Balls[i].X + BallWidth > this.ClientSize.Width)
                {
                    this.BallsVx[i] = -this.BallsVx[i];
                    Boing(i);
                }

                //Ball's new Y position
                this.Balls[i].Y += this.BallsVy[i];
                //Collision with Y axis walls
                if (this.Balls[i].Y < 0)
                {
                    this.BallsVy[i] = -this.BallsVy[i];
                    Boing(i);
                }
                else if (this.Balls[i].Y + BallHeight > this.ClientSize.Height)
                {
                    this.BallsVy[i] = -this.BallsVy[i];
                    Boing(i);
                }

                //Collision with Gate
                if (this.Balls[i].IntersectsWith(Gate))
                {
                    this.BallsVx[i] = -this.BallsVx[i];

                    Boing(i);
                }
            }


            this.Refresh();
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     Play the boing sound file resource.
        /// </summary>
        private void Boing(int i)
        {
            using (var player = new SoundPlayer(BounceBall.Properties.Resources.boing))
            {
                player.Play();
                //return;
                //player.LoadAsync();
                var color = Color.FromArgb((byte) this.RandomNumber.Next(), (byte) this.RandomNumber.Next(), (byte)this.RandomNumber.Next());
                this.BallColors[i] = new SolidBrush(color);
            }
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     Method Sets the Ball In Motion...
        /// </summary>
        /// <param name="startStop"></param>
        public void StartStop(bool startStop)
        {
            this.tmrBall.Enabled = startStop;
            this.Visible = startStop;
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Updates the Speed of the Ball
        /// </summary>
        /// <param name="speedValueIn"></param>
        public void BounceBallSpeed(int speedValueIn)
        {
            this.tmrBall.Interval = speedValueIn;
        }
    }
}
//------------------------------------------...ooo000 END OF FILE 000ooo...-------------------------------------------------//