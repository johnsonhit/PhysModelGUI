﻿using System;
using System.Collections.Generic;
using PhysModelLibrary;
using PhysModelLibrary.Compartments;
using PhysModelLibrary.Connectors;
using SkiaSharp;

namespace PhysModelGUI
{
    public class AnimatedBloodConnector
    {

        public List<BloodCompartmentConnector> connectors = new List<BloodCompartmentConnector>();
        public BloodCompartment sizeCompartment;

        SKRect mainRect = new SKRect(0, 0, 0, 0);

        SKColor colorFrom;
        SKColor colorTo;


        SKPaint circleOut = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.Orange,
            StrokeWidth = 5,
        };


        SKPaint paint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.AliceBlue,
            StrokeWidth = 10
        };

        SKPaint textPaint = new SKPaint
        {
            Typeface = SKTypeface.FromFamilyName("Arial Bold"),
            FakeBoldText = true,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            Color = SKColors.White,
            IsStroke = false,
            TextSize = 16f


        };
        public SKPoint locationOrigen= new SKPoint(0, 0);
        public SKPoint locationTarget = new SKPoint(0, 0);
        public SKPoint location1 = new SKPoint(0, 0);
        public SKPoint locationPump = new SKPoint(0, 0);

        public float scaleRelative = 18;
        float scale = 1;
        public float Degrees { get; set; } = 0;
        public float RadiusXOffset { get; set; } = 1;
        public float RadiusYOffset { get; set; } = 1;

        float currentAngle = 0;
        public float StartAngle { get; set; } = 0;
        public float EndAngle { get; set; } = 0;
        public float Direction { get; set; } = 1;
        public float Speed { get; set; } = 0.05f;

        public float XOffset { get; set; } = 0;
        public float YOffset { get; set; } = 0;

        public bool NoLoss { get; set; } = true;
        public string Name { get; set; } = "";
        public bool IsVisible  = true;


        public void AddConnector(BloodCompartmentConnector c)
        {
            connectors.Add(c);
        }


        public void DrawConnector(SKCanvas canvas, float _radX, float _radY)
        {
            float totalFlow = 0;
            float totalSpO2 = 0;
            float totalSpO2To = 0;
            float totalSpO2From = 0;
            float currentVolume = 0;
            float radius = 0;

            scale = _radX * scaleRelative;
            radius = _radX / 2.5f;

            if (_radX > _radY)
            {
                scale = _radY * scaleRelative;
                radius = _radY / 2.5f;
            }

            // calculate the total volume and average spO2 if lumping is the case
            foreach (BloodCompartmentConnector c in connectors)
            {
                totalFlow += (float)c.CurrentFlow * Speed;
                if (totalFlow >= 0)
                {
                    totalSpO2 += (float)c.Comp1.TO2;
                    if (NoLoss)
                    {
                        totalSpO2From += (float)c.Comp1.TO2;
                        totalSpO2To += (float)c.Comp1.TO2;
                    }
                    else
                    {
                        totalSpO2From += (float)c.Comp1.TO2;
                        totalSpO2To += (float)c.Comp2.TO2;
                    }
                }
                else
                {
                    totalSpO2 += (float)c.Comp2.TO2;
                    if (NoLoss)
                    {
                        totalSpO2From += (float)c.Comp2.TO2;
                        totalSpO2To += (float)c.Comp2.TO2;
                    }
                    else
                    {
                        totalSpO2From += (float)c.Comp2.TO2;
                        totalSpO2To += (float)c.Comp1.TO2;
                    }

                }
   
            }

            colorTo = AnimatedElementHelper.CalculateBloodColor(totalSpO2To / connectors.Count);
            colorFrom = AnimatedElementHelper.CalculateBloodColor(totalSpO2From / connectors.Count);

            currentAngle += totalFlow * Direction;
            if (Math.Abs(currentAngle) > Math.Abs(StartAngle - EndAngle))
            {
                currentAngle = 0;
            }

            if (sizeCompartment != null)
            {
                currentVolume = (float)sizeCompartment.VolCurrent;
                circleOut.StrokeWidth = AnimatedElementHelper.RadiusCalculator(currentVolume, scale);
            }
            else
            {
               circleOut.StrokeWidth = 22;
            }


            // calculate position
            locationOrigen = AnimatedElementHelper.GetPosition(StartAngle, radius, RadiusXOffset, RadiusYOffset);
            locationTarget = AnimatedElementHelper.GetPosition(EndAngle, radius, RadiusXOffset, RadiusYOffset);

            float left = (float)Math.Sin(270 * 0.0174532925) * RadiusXOffset * radius + XOffset;
            float right = (float)Math.Sin(90 * 0.0174532925) * RadiusXOffset * radius + XOffset;
            float top = (float)Math.Cos(180 * 0.0174532925) * RadiusYOffset * radius + YOffset;
            float bottom = (float)Math.Cos(0 * 0.0174532925) * RadiusYOffset * radius + YOffset;

            mainRect.Left = left;
            mainRect.Right = right;
            mainRect.Top = top;
            mainRect.Bottom = bottom;


            circleOut.Shader = SKShader.CreateSweepGradient(
                new SKPoint(0f, 0f),
                new SKColor[] { colorFrom, colorTo },
                new float[] { StartAngle / 360f, EndAngle / 360f }
            );

            using (SKPath path = new SKPath())
            {
                path.AddArc(mainRect, StartAngle, Math.Abs(StartAngle - EndAngle));
                canvas.DrawPath(path, circleOut);
            }

            location1 = AnimatedElementHelper.GetPosition(StartAngle + currentAngle, radius, RadiusXOffset,RadiusYOffset);
            canvas.DrawCircle(location1.X + XOffset, location1.Y + YOffset, 10, paint);

          
      


        }






    }
}
