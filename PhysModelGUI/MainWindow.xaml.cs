using PhysModelLibrary;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public static List<AnimatedBloodCompartment> animatedBloodCompartments = new List<AnimatedBloodCompartment>();
        public static AnimatedBloodCompartment leftAtrium;
        public static AnimatedBloodCompartment leftVentricle;

        DispatcherTimer dispatcherTimer = new DispatcherTimer(DispatcherPriority.Render);

        bool _graphPressuresEnabled = false;
        public bool GraphPressureEnabled { get { return _graphPressuresEnabled; } set { _graphPressuresEnabled = value; OnPropertyChanged(); } }

        bool _graphFlowsEnabled = false;
        public bool GraphFlowsEnabled { get { return _graphFlowsEnabled; } set { _graphFlowsEnabled = value; OnPropertyChanged(); } }

        bool _graphPVLoopsEnabled = false;
        public bool GraphPVLoopsEnabled { get { return _graphPVLoopsEnabled; } set { _graphPVLoopsEnabled = value; OnPropertyChanged(); } }

        string _modelName = "";
        public string ModelName { get { return _modelName; } set { _modelName = value; OnPropertyChanged(); } }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            PhysModelMain.Initialize();
            PhysModelMain.Start();

            ModelGraphic.BuildDiagram();

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            dispatcherTimer.Start();

            graphPressures.GraphMaxY = 100;
            graphPressures.GraphMaxX = 20;
            graphPressures.DataRefreshRate = 15;
            graphPressures.PixelPerDataPoint = 2;
            graphPressures.Graph1Enabled = true;
            graphPressures.Graph2Enabled = false;
            graphPressures.Legend2 = "white";
            graphPressures.IsSideScrolling = true;
            graphPressures.GraphicsClearanceRate = 15;
            
            graphPressures.RedrawGrid();

            ModelName = PhysModelMain.currentModel.Name;
           
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            // update the graphs
            if (GraphPressureEnabled)
                graphPressures.UpdateGraphData(0, PhysModelMain.modelInterface.ABPSignal);

            if (GraphFlowsEnabled) { }

            if (GraphPVLoopsEnabled) { }

            canvasDiagram.InvalidateVisual();
        }

        void CanvasDiagram_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            ModelGraphic.DrawMainDiagram(canvas, e.Info.Width, e.Info.Height);
        }

        void CanvasDiagramSkeleton_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            ModelGraphic.DrawDiagramSkeleton(canvas, e.Info.Width, e.Info.Height);
        }

    
    
    }
}
