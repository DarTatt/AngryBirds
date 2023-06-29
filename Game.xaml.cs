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
using System.Windows.Shapes;

namespace AngryBirds
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        //public RoutedPropertyChangedEventArgs<double> Value { get; private set; }
        private double sp;
        private double a;
        public Window1()
        {
            InitializeComponent();
            //var bird = new AB();
        }
        
        void SliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
             sp = speed.Value;
            //textBox1.Text = speed.Value.ToString();
            // return sp;
        }
        void SliderAngle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            a = angle.Value;
        }
        void Button_Click(object sender, RoutedEventArgs e)
            {
            //Console.Write(SliderSpeed_ValueChanged(speed,));
             
            var bird = new AB();
            bird.Notify += bird.message;
            bird.interval = 0.2;
            bird.Vo = Convert.ToDouble(sp);
            bird.angle = (Math.PI * Convert.ToDouble(a)) / 180;
            bird.M = 2;
            bird.wall_x = 15;
            bird.wall_y = 10;
            bird.coefficient = 0.1;
            bird.flight();
        }

    }


}
class AB
{
    public double interval;// шаг
    public double Vo;//начальная скорость
    public double angle;//угол
    public double M;//масса
                    //препятствие
    public double wall_x;//расстояние до препятствия
    public double wall_y;//высота препятствия
                         //public double wall_y2;//нижняя граница препятствия
                         //
    public double g = 9.8;
    public double coefficient;// коэффициент сопротивления воздуха
                              // event
    public delegate void AccountHandler(string message);
    public event AccountHandler Notify;

    public void message(string events)
    {
        Console.WriteLine(events);
    }

    public void flight()
    {
        double X = 0;//пройденный путь
        double time = 0;//время в определенный период полета
        double Vx = Vo * Math.Cos(angle);
        double Vy = Vo * Math.Sin(angle);
        double Y = 0;

        while (Y >= 0)
        {
            time += interval;
            Vx = Vx - interval * (coefficient * Vx / M);
            Vy = Vy - interval * (g + (coefficient * Vx / M));
            //X = X + interval * Vx;
            Y = Y + interval * Vy;
            if ((Y < wall_y) && (X < wall_x) && (wall_x < (X + interval * Vx)))
            {
                Notify?.Invoke($"Птичка врезалась в стену!");
                break;
            }
            X = X + interval * Vx;
            if (Y >= 0)
            {
                Console.Write($"X: {Math.Round(X, 4)} м  Y: {Math.Round(Y, 4)} м  t: {Math.Round(time, 4)} с \n");
            }
            else
            {
                Notify?.Invoke($"Птичка упала на землю!");
            }

        }
    }
}
