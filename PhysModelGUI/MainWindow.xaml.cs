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
            
            //graphPressures.RedrawGrid();

            ModelName = PhysModelMain.currentModel.Name;

           
            graphElastance.DataRefreshRate = 15;
            graphElastance.PixelPerDataPoint = 1;
            graphElastance.Graph1Enabled = true;
            graphElastance.IsSideScrolling = false;
            graphElastance.GraphicsClearanceRate = 15;
            graphElastance.RealTimeDrawing = false;


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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PhysModelLibrary.Compartments.BloodCompartment testComp = new PhysModelLibrary.Compartments.BloodCompartment();

            testComp.VolU = PhysModelLibrary.PhysModelMain.currentModel.AA.VolU;
            testComp.elastanceModel.ElBaseline = PhysModelLibrary.PhysModelMain.currentModel.AA.elastanceModel.ElBaseline;
            testComp.elastanceModel.ElK1 = PhysModelLibrary.PhysModelMain.currentModel.AA.elastanceModel.ElK1;
            testComp.elastanceModel.ElK2 = PhysModelLibrary.PhysModelMain.currentModel.AA.elastanceModel.ElK2;
            testComp.elastanceModel.ElKMaxVolume = PhysModelLibrary.PhysModelMain.currentModel.AA.elastanceModel.ElKMaxVolume;
            testComp.elastanceModel.ElKMinVolume = PhysModelLibrary.PhysModelMain.currentModel.AA.elastanceModel.ElKMinVolume;
    
            testComp.elastanceModel.ElK1 = 0.1;
            testComp.elastanceModel.ElK2 = 0.003;
            testComp.elastanceModel.ElKMaxVolume = 15;
            testComp.elastanceModel.ElKMinVolume = 8.5;

            // y = pressure
            graphElastance.GraphMinY = -75;
            graphElastance.GraphMaxY = 200;
            graphElastance.GridYAxisStep = 50;

            // x = volume
            graphElastance.GraphMinX = 0;
            graphElastance.GraphMaxX = 20;
            graphElastance.GridXAxisStep = 5;

            graphElastance.RedrawGrid();

            // do loop for grid

            graphElastance.ClearRawData();

            for (double i = 0; i <= 20; i += 0.1)
            {
                testComp.VolCurrent = i;
                testComp.UpdateCompartment();
                graphElastance.UpdateRawData(i, testComp.PresCurrent);

            }

            
            graphElastance.DrawRawData();
        }

    }
}
