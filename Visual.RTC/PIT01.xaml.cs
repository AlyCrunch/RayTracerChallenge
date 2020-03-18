﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using media = System.Windows.Media;
using System.Windows.Shapes;
using RayTracerChallenge.Features;
using System.Text.RegularExpressions;
using ctr = System.Windows.Controls;
using RayTracerChallenge.Helpers;

namespace Visual.RTC
{
    public partial class PIT01 : Window
    {
        int inc = 1;
        readonly Random rnd = new Random();
        public PIT01()
        {
            InitializeComponent();
            CalculatePath();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ctr.Button).Name == "PlusButton")
                inc++;
            else
                inc = (inc > 1) ? inc - 1 : 1;

            CalculatePath();
        }

        private void CalculatePath()
        {
            DrawCanvas.Children.Clear();

            var proj = new Projectile<float>(
                    PointType<float>.Point(0, 1, 0),
                    PointType<float>.Vector(1, 1, 0).Normalizing() * inc);
            var env = new Environment<float>(
                    PointType<float>.Vector(0, -0.1f, 0),
                    PointType<float>.Vector(-0.01f, 0, 0));

            foreach (var coord in Projectile<float>.GetTick(env, proj))
                DrawCircle(coord.X, coord.Y);
        }

        private void DrawCircle(float X, float Y)
        {
            var r = (byte)rnd.Next(255);
            var g = (byte)rnd.Next(255);
            var b = (byte)rnd.Next(255);

            media.SolidColorBrush brush = new media.SolidColorBrush
            {
                Color = media.Color.FromRgb(r, g, b)
            };

            Ellipse circle = new Ellipse
            {
                Height = 10,
                Width = 10,
                Fill = brush
            };


            ctr.Canvas.SetBottom(circle, Y * 10);
            ctr.Canvas.SetLeft(circle, X * 10);
            DrawCanvas.Children.Add(circle);
        }
        
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

}