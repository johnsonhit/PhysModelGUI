using PhysModelLibrary;
using PhysModelLibrary.Compartments;
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

        string _heartrate = "140";
        public string Heartrate { get { return _heartrate; } set { _heartrate = value; OnPropertyChanged(); } }

        string _spo2 = "99";
        public string Spo2 { get { return _spo2; } set { _spo2 = value; OnPropertyChanged(); } }

        string _abp = "99";
        public string Abp { get { return _abp; } set { _abp = value; OnPropertyChanged(); } }

        string _pap = "99";
        public string Pap { get { return _pap; } set { _pap = value; OnPropertyChanged(); } }

        string _cvp = "99";
        public string Cvp { get { return _cvp; } set { _cvp = value; OnPropertyChanged(); } }

        string _resprate = "99";
        public string Resprate { get { return _resprate; } set { _resprate = value; OnPropertyChanged(); } }

        string _temp = "99";
        public string Temp { get { return _temp; } set { _temp = value; OnPropertyChanged(); } }

        string _lvo = "99";
        public string Lvo { get { return _lvo; } set { _lvo = value; OnPropertyChanged(); } }

        string _rvo = "99";
        public string Rvo { get { return _rvo; } set { _rvo = value; OnPropertyChanged(); } }

        string _ivcflow = "99";
        public string Ivcflow { get { return _ivcflow; } set { _ivcflow = value; OnPropertyChanged(); } }

        string _svcflow = "99";
        public string Svcflow { get { return _svcflow; } set { _svcflow = value; OnPropertyChanged(); } }

        string _lvstroke = "99";
        public string Lvstroke { get { return _lvstroke; } set { _lvstroke = value; OnPropertyChanged(); } }

        string _rvstroke = "99";
        public string Rvstroke { get { return _rvstroke; } set { _rvstroke = value; OnPropertyChanged(); } }

        string _rapressures = "99";
        public string Rapressures { get { return _rapressures; } set { _rapressures = value; OnPropertyChanged(); } }

        string _lapressures = "99";
        public string Lapressures { get { return _lapressures; } set { _lapressures = value; OnPropertyChanged(); } }

        string _rvpressures = "99";
        public string Rvpressures { get { return _rvpressures; } set { _rvpressures = value; OnPropertyChanged(); } }

        string _lvpressures = "99";
        public string Lvpressures { get { return _lvpressures; } set { _lvpressures = value; OnPropertyChanged(); } }

        string _ravolumes = "99";
        public string Ravolumes { get { return _ravolumes; } set { _ravolumes = value; OnPropertyChanged(); } }

        string _lavolumes = "99";
        public string Lavolumes { get { return _lavolumes; } set { _lavolumes = value; OnPropertyChanged(); } }

        string _rvvolumes = "99";
        public string Rvvolumes { get { return _rvvolumes; } set { _rvvolumes = value; OnPropertyChanged(); } }

        string _lvvolumes = "99";
        public string Lvvolumes { get { return _lvvolumes; } set { _lvvolumes = value; OnPropertyChanged(); } }

        string _myoflow = "99";
        public string Myoflow { get { return _myoflow; } set { _myoflow = value; OnPropertyChanged(); } }

        string _pdaflow = "99";
        public string Pdaflow { get { return _pdaflow; } set { _pdaflow = value; OnPropertyChanged(); } }

        string _brainflow = "-";
        public string Brainflow { get { return _brainflow; } set { _brainflow = value; OnPropertyChanged(); } }

        string _kidneysflow = "-";
        public string Kidneysflow { get { return _kidneysflow; } set { _kidneysflow = value; OnPropertyChanged(); } }

        string _liverflow = "-";
        public string Liverflow { get { return _liverflow; } set { _liverflow = value; OnPropertyChanged(); } }

        string _intestinesflow = "-";
        public string Intestinesflow { get { return _intestinesflow; } set { _intestinesflow = value; OnPropertyChanged(); } }

        string _globaldo2 = "-";
        public string Globaldo2 { get { return _globaldo2; } set { _globaldo2 = value; OnPropertyChanged(); } }

        string _myocardialdo2 = "-";
        public string Myocardialdo2 { get { return _myocardialdo2; } set { _myocardialdo2 = value; OnPropertyChanged(); } }

        string _braindo2 = "-";
        public string Braindo2 { get { return _braindo2; } set { _braindo2 = value; OnPropertyChanged(); } }


        int slowUpdater = 0;
        int graphicsRefreshInterval = 15;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
  
            PhysModelMain.Initialize();
            PhysModelMain.modelInterface.PropertyChanged += ModelInterface_PropertyChanged;
            PhysModelMain.Start();

            ModelGraphic.BuildDiagram();

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, graphicsRefreshInterval);
            dispatcherTimer.Start();

            graphPressures.GraphMaxY = 100;
            graphPressures.GraphMaxX = 20;
            graphPressures.DataRefreshRate = 15;
            graphPressures.PixelPerDataPoint = 2;
            graphPressures.Graph1Enabled = true;
            graphPressures.Graph2Enabled = false;
            graphPressures.Legend2 = "white";
            graphPressures.IsSideScrolling = true;
            graphPressures.GraphicsClearanceRate = graphicsRefreshInterval;
            
            ModelName = PhysModelMain.currentModel.Name;

           
            graphElastance.DataRefreshRate = 15;
            graphElastance.PixelPerDataPoint = 1;
            graphElastance.Graph1Enabled = true;
            graphElastance.IsSideScrolling = false;
            graphElastance.GraphicsClearanceRate = graphicsRefreshInterval;
            graphElastance.RealTimeDrawing = false;

            // populate controls
            PopulateLists();
        }

        private void ModelInterface_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { 
            switch (e.PropertyName)
            {
                case "ModelUpdated":
                    if (GraphPressureEnabled)
                        graphPressures.UpdateRealtimeGraphData(0, PhysModelMain.modelInterface.ABPSignal);
                    break;
                case "StatusMessage":
                    lstBox.Items.Add(PhysModelMain.modelInterface.StatusMessage);
                    break;
            }

        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            // update the graph

            if (slowUpdater > 1000)
            {
                slowUpdater = 0;
                Heartrate = PhysModelMain.modelInterface.HeartRate.ToString();
                Spo2 = PhysModelMain.modelInterface.ArterialSO2Pre.ToString();
                Abp = PhysModelMain.modelInterface.ArterialBloodPressure;
                Pap = PhysModelMain.modelInterface.PulmonaryArteryPressure.ToString();
                Cvp = PhysModelMain.modelInterface.CentralVenousPressure.ToString();
                Resprate = PhysModelMain.modelInterface.RespiratoryRate.ToString();
                Temp = PhysModelMain.modelInterface.PatientTemperature.ToString();
                Lvo = PhysModelMain.modelInterface.LeftVentricularOutput.ToString();
                Rvo = PhysModelMain.modelInterface.RightVentricularOutput.ToString();
                Ivcflow = PhysModelMain.modelInterface.InferiorVenaCavaFlow.ToString();
                Svcflow = PhysModelMain.modelInterface.SuperiorVenaCavaFlow.ToString();
                Myoflow = PhysModelMain.modelInterface.CoronaryFlow.ToString();
                Lvstroke = PhysModelMain.modelInterface.StrokeVolumeLeftVentricle.ToString();
                Rvstroke = PhysModelMain.modelInterface.StrokeVolumeRightVentricle.ToString();
                Rapressures = PhysModelMain.modelInterface.RightAtrialPressures;
                Lapressures = PhysModelMain.modelInterface.LeftAtrialPressures;
                Rvpressures = PhysModelMain.modelInterface.RightVentricularPressures;
                Lvpressures = PhysModelMain.modelInterface.LeftVentricularPressures;
                Ravolumes = PhysModelMain.modelInterface.RightAtrialVolumes;
                Lavolumes = PhysModelMain.modelInterface.LeftAtrialVolumes;
                Rvvolumes = PhysModelMain.modelInterface.RightVentricularVolumes;
                Lvvolumes = PhysModelMain.modelInterface.LeftVentricularVolumes;
                Pdaflow = Math.Round(PhysModelMain.modelInterface.PDAFlow, 1).ToString();
                Myocardialdo2 = Math.Round(PhysModelMain.modelInterface.MyoO2Delivery, 1).ToString();
                Braindo2 = Math.Round(PhysModelMain.modelInterface.BrainO2Delivery, 1).ToString();

            }
            slowUpdater += graphicsRefreshInterval;
 

            //canvasDiagram.InvalidateVisual();

            if (GraphPressureEnabled)
                //graphPressures.DrawGraphOnScreen();
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

        void CalculateElastanceCurve(string _compartmentName)
        {
            BloodCompartment testComp = new BloodCompartment();
            BloodCompartment selectedCompartment = PhysModelMain.FindBloodCompartmentByName(_compartmentName);

            if (selectedCompartment != null)
            {
                testComp.VolCurrent = selectedCompartment.VolCurrent;
                testComp.VolU = selectedCompartment.VolU;
                testComp.VolUBaseline = selectedCompartment.VolUBaseline;
                testComp.VolUBaselineChange = selectedCompartment.VolUBaselineChange;
                testComp.elastanceModel.ElBaseline = selectedCompartment.elastanceModel.ElBaseline;
                testComp.elastanceModel.ElBaselineChange = selectedCompartment.elastanceModel.ElBaselineChange;
                testComp.elastanceModel.ElContractionBaseline = selectedCompartment.elastanceModel.ElContractionBaseline;
                testComp.elastanceModel.ElContractionBaselineChange = selectedCompartment.elastanceModel.ElContractionBaselineChange;
                testComp.elastanceModel.ElK1 = selectedCompartment.elastanceModel.ElK1;
                testComp.elastanceModel.ElK2 = selectedCompartment.elastanceModel.ElK2;
                testComp.elastanceModel.ElKMaxVolume = selectedCompartment.elastanceModel.ElKMaxVolume;
                testComp.elastanceModel.ElKMinVolume = selectedCompartment.elastanceModel.ElKMinVolume;

                if (testComp.elastanceModel.ElContractionBaseline > 0)
                {
                    graphElastance.Graph2Enabled = true;
                } else
                {
                    graphElastance.Graph2Enabled = false;
                }

                double testVolume = testComp.VolCurrent * 2;

                if (testVolume < 10) testVolume = 10;

                graphElastance.ClearStaticData();
                for (double i = 0; i <= testVolume; i += 0.1)
                {
                    testComp.elastanceModel.ContractionActivation = 0;
                    testComp.VolCurrent = i;
                    testComp.UpdateCompartment();
                    double pres1 = testComp.PresCurrent;

                    testComp.elastanceModel.ContractionActivation = 1;
                    testComp.VolCurrent = i;
                    testComp.UpdateCompartment();
                    double pres2 = testComp.PresCurrent;

                    graphElastance.UpdateStaticData(i, pres1, i, pres2);
  
                }
                graphElastance.DrawStaticData();
          
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CalculateElastanceCurve(cmbSelectCompartmentSettings.SelectedItem.ToString());
        }

        void PopulateLists()
        {
            cmbSelectCompartmentSettings.Items.Clear();
            foreach(BloodCompartment c in PhysModelMain.currentModel.bloodCompartments)
            {
                cmbSelectCompartmentSettings.Items.Add(c.Name);
            }

            if (cmbSelectCompartmentSettings.Items.Count > 0)
            {
                cmbSelectCompartmentSettings.SelectedIndex = 0;
            }
        }
    }
}
