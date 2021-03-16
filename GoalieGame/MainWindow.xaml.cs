using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Annotations;
using System.Windows.Threading;

namespace BallSpiel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _animationsTimer = new DispatcherTimer();
        private readonly DispatcherTimer _leftGameTimer = new DispatcherTimer();

        private bool goRight = true;
        private bool goTop = true;

        private int counterFail_1 = 0;
        private int counterFail_2 = 0;
        private int goalie_1 = 0;
        private int goalie_2 = 0;

        private int playerSpeed = 5;
        private int playerSpeed2 = 20;

        public MainWindow()
        {
            InitializeComponent();
            _animationsTimer.Interval = TimeSpan.FromMilliseconds(50);
            _animationsTimer.Tick += PositionBall;
            _leftGameTimer.Interval = TimeSpan.FromMilliseconds(20);
            _leftGameTimer.Tick += PositionLeftRacket;
            _leftGameTimer.Start();
        }
        // Finds Direction of the ball and moves it
        private void PositionBall(object sender, EventArgs e)
        {
            var x = Canvas.GetLeft(Ball);
            var y = Canvas.GetTop(Ball);

            // Moves the ball
            if (goRight)
            {
                Canvas.SetLeft(Ball, x + playerSpeed);
            }
            else
            {
                Canvas.SetLeft(Ball, x - playerSpeed);
            }

            if (goTop)
            {
                Canvas.SetTop(Ball, y + playerSpeed);
            }
            else
            {
                Canvas.SetTop(Ball, y - playerSpeed);
            }

            // Sets the next moving x-direction of the ball.
            // Counts the hit of the left and right wall.
            if (Canvas.GetLeft(Ball) >= Playground.ActualWidth - Ball.ActualWidth)
            {
                goRight = false;
                counterFail_1 += 1;
                OutLabel_1.Content = $"{counterFail_1} Fail for Goalie 2";
            }
            else if (Canvas.GetLeft(Ball) <= 0)
            {
                goRight = true;
                counterFail_2 += 1;
                OutLabel_2.Content = $"{counterFail_2} Fail for Goalie 1";
            }

            //Goalie shoot the ball and increase points for the goalie
            if (Canvas.GetTop(Ball) >= Canvas.GetTop(RacketLeft) && (Canvas.GetTop(Ball) + Ball.Height) <= (Canvas.GetTop(RacketLeft) + RacketLeft.Height) &&
               Canvas.GetLeft(Ball) > Canvas.GetLeft(RacketLeft) && Canvas.GetLeft(Ball) <= (Canvas.GetLeft(RacketLeft) + RacketLeft.Width))
            {
                goRight = true;
                goalie_1 += 1;
                goalieLabel_1.Content = $"{goalie_1} for Goalie 1";
            }
            else if (Canvas.GetTop(Ball) >= Canvas.GetTop(RacketRight) && (Canvas.GetTop(Ball) + Ball.Height) <= (Canvas.GetTop(RacketRight) + RacketRight.Height) &&
                (Canvas.GetLeft(Ball) + Ball.Width) < (Canvas.GetLeft(RacketRight) + RacketRight.Width) && (Canvas.GetLeft(Ball) + Ball.Width) >= Canvas.GetLeft(RacketRight))
            {
                goRight = false;
                goalie_2 += 1;
                goalieLabel_2.Content = $"{goalie_2} for Goalie 2";
            }


            // Sets the next y-moving direction of the ball
            if (Canvas.GetTop(Ball) >= Playground.ActualHeight - Ball.ActualHeight)
            {
                goTop = false;
            }
            else if (Canvas.GetTop(Ball) <= 0)
            {
                goTop = true;
            }
        }

        //Starts and Stops the game
        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_animationsTimer.IsEnabled)
            {
                _animationsTimer.Stop();
            }
            else
            {
                _animationsTimer.Start();
            }
        }

        //Move Goalie
        private bool goUpLeft = false;
        private bool goDownLeft = false;
        private bool goUpRight = false;
        private bool goDownRight = false;

        private void PositionLeftRacket(object sender, EventArgs e)
        {
            //position left racket
            if (goUpLeft == true && Canvas.GetTop(RacketLeft) >= RacketLeft.ActualHeight / 5)
            {
                Canvas.SetTop(RacketLeft, Canvas.GetTop(RacketLeft) - playerSpeed2);
            }
            if (goDownLeft == true && Canvas.GetTop(RacketLeft) < Playground.ActualHeight - RacketLeft.ActualHeight)
            {
                Canvas.SetTop(RacketLeft, Canvas.GetTop(RacketLeft) + playerSpeed2);
            }
            //position right racket
            if (goUpRight == true && Canvas.GetTop(RacketRight) >= RacketRight.ActualHeight / 5)
            {
                Canvas.SetTop(RacketRight, Canvas.GetTop(RacketRight) - playerSpeed2);
            }
            if (goDownRight == true && Canvas.GetTop(RacketRight) < Playground.ActualHeight - RacketRight.ActualHeight)
            {
                Canvas.SetTop(RacketRight, Canvas.GetTop(RacketRight) + playerSpeed2);
            }
        }
        private void Racket_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                goUpLeft = true;
            }
            if (e.Key == Key.D)
            {
                goDownLeft = true;
            }
            if (e.Key == Key.Up)
            {
                goUpRight = true;
            }
            if (e.Key == Key.Down)
            {
                goDownRight = true;
            }
        }
        private void Racket_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                goUpLeft = false;
            }
            if (e.Key == Key.D)
            {
                goDownLeft = false;
            }
            if (e.Key == Key.Up)
            {
                goUpRight = false;
            }
            if (e.Key == Key.Down)
            {
                goDownRight = false;
            }
        }
    }
}
