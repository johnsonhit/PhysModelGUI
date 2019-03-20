using PhysModelLibrary;
using SkiaSharp;
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
using System.Windows.Threading;

namespace PhysModelGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<AnimatedBloodCompartment> animatedBloodCompartments = new List<AnimatedBloodCompartment>();
        public static AnimatedBloodCompartment leftAtrium;
        public static AnimatedBloodCompartment leftVentricle;

        DispatcherTimer dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);

        public MainWindow()
        {
            InitializeComponent();

            PhysModelMain.Initialize();
            PhysModelMain.Start();

            GUI.BuildDiagram();

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            canvasDiagram.InvalidateVisual();
        }

        void CanvasDiagram_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            GUI.DrawMainDiagram(canvas, e.Info.Width, e.Info.Height);
        }

        void CanvasDiagramSkeleton_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            GUI.DrawDiagramSkeleton(canvas, e.Info.Width, e.Info.Height);
        }

    
    
    }
}
