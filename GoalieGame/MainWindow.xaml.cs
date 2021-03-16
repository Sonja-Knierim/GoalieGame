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
            _leftGameTimer.Tick += PositionLeftGoalie;
            _leftGameTimer.Start();
        }
        // Find direction of the ball and move it
        private void PositionBall(object sender, EventArgs e)
        {
            var x = Canvas.GetLeft(Ball);
            var y = Canvas.GetTop(Ball);

            // Move the ball
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

            // Set the next moving x-direction of the ball.
            // Count the hit of the left and right wall.
            if (Canvas.GetLeft(Ball) >= Playground.ActualWidth - Ball.ActualWidth)
            {
                goRight = false;
                counterFail_1 += 1;
                OutLabel_1.Content = $"{counterFail_1} Fail for Goalie 2";
                score.Content = $"Score {counterFail_1} : {counterFail_2}";
            }
            else if (Canvas.GetLeft(Ball) <= 0)
            {
                goRight = true;
                counterFail_2 += 1;
                OutLabel_2.Content = $"{counterFail_2} Fail for Goalie 1";
                score.Content = $"Score {counterFail_1} : {counterFail_2}";
            }

            //Goalie shoot the ball and increase points for the goalie
            if (goRight == false && 
                Canvas.GetTop(Ball) >= Canvas.GetTop(GoalieLeft) && 
               (Canvas.GetTop(Ball) + Ball.Height) <= (Canvas.GetTop(GoalieLeft) + GoalieLeft.Height) &&
               Canvas.GetLeft(Ball) > Canvas.GetLeft(GoalieLeft) && 
               Canvas.GetLeft(Ball) <= (Canvas.GetLeft(GoalieLeft) + GoalieLeft.Width))
            {
                goRight = true;
                goalie_1 += 1;
                goalieLabel_1.Content = $"{goalie_1} Point for Goalie 1";
            }
            else if (goRight == true && 
                 Canvas.GetTop(Ball) >= Canvas.GetTop(GoalieRight) && 
                (Canvas.GetTop(Ball) + Ball.Height) <= (Canvas.GetTop(GoalieRight) + GoalieRight.Height) &&
                (Canvas.GetLeft(Ball) + Ball.Width) < (Canvas.GetLeft(GoalieRight) + GoalieRight.Width) && 
                (Canvas.GetLeft(Ball) + Ball.Width) >= Canvas.GetLeft(GoalieRight))
            {
                goRight = false;
                goalie_2 += 1;
                goalieLabel_2.Content = $"{goalie_2} Point for Goalie 2";
            }


            // Set the next y-moving direction of the ball
            if (Canvas.GetTop(Ball) >= Playground.ActualHeight - Ball.ActualHeight)
            {
                goTop = false;
            }
            else if (Canvas.GetTop(Ball) <= 0)
            {
                goTop = true;
            }
        }

        //Start and Stop the game
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

        private void PositionLeftGoalie(object sender, EventArgs e)
        {
            //position left Goalie
            if (goUpLeft == true && Canvas.GetTop(GoalieLeft) >= GoalieLeft.ActualHeight / 5)
            {
                Canvas.SetTop(GoalieLeft, Canvas.GetTop(GoalieLeft) - playerSpeed2);
            }
            if (goDownLeft == true && Canvas.GetTop(GoalieLeft) < Playground.ActualHeight - GoalieLeft.ActualHeight)
            {
                Canvas.SetTop(GoalieLeft, Canvas.GetTop(GoalieLeft) + playerSpeed2);
            }
            //position right Goalie
            if (goUpRight == true && Canvas.GetTop(GoalieRight) >= GoalieRight.ActualHeight / 5)
            {
                Canvas.SetTop(GoalieRight, Canvas.GetTop(GoalieRight) - playerSpeed2);
            }
            if (goDownRight == true && Canvas.GetTop(GoalieRight) < Playground.ActualHeight - GoalieRight.ActualHeight)
            {
                Canvas.SetTop(GoalieRight, Canvas.GetTop(GoalieRight) + playerSpeed2);
            }
        }
        private void Goalie_KeyDown(object sender, KeyEventArgs e)
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
        private void Goalie_KeyUp(object sender, KeyEventArgs e)
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
