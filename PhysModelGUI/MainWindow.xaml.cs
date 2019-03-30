using PhysModelLibrary;
using PhysModelLibrary.BaseClasses;
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

        bool _graphPatMonitorEnabled = true;
        public bool GraphPatMonitorEnabled { get { return _graphPatMonitorEnabled; } set { _graphPatMonitorEnabled = value; OnPropertyChanged(); } }


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

        string _tidalvolume = "-";
        public string Tidalvolume { get { return _tidalvolume; } set { _tidalvolume = value; OnPropertyChanged(); } }

        string _minutevolume = "-";
        public string Minutevolume { get { return _minutevolume; } set { _minutevolume = value; OnPropertyChanged(); } }

        string _alveolarvolume = "-";
        public string Alveolarvolume { get { return _alveolarvolume; } set { _alveolarvolume = value; OnPropertyChanged(); } }

        string _appliedpressure = "-";
        public string Appliedpressure { get { return _appliedpressure; } set { _appliedpressure = value; OnPropertyChanged(); } }

        string _airwaypressure = "-";
        public string Airwaypressure { get { return _airwaypressure; } set { _airwaypressure = value; OnPropertyChanged(); } }

        string _alvleftpressure = "-";
        public string Alvleftpressure { get { return _alvleftpressure; } set { _alvleftpressure = value; OnPropertyChanged(); } }

        string _alvrightpressure = "-";
        public string Alvrightpressure { get { return _alvrightpressure; } set { _alvrightpressure = value; OnPropertyChanged(); } }

        string _ph = "-";
        public string Ph { get { return _ph; } set { _ph = value; OnPropertyChanged(); } }

        string _pao2 = "-";
        public string Pao2 { get { return _pao2; } set { _pao2 = value; OnPropertyChanged(); } }
        string _paco2 = "-";
        public string Paco2 { get { return _paco2; } set { _paco2 = value; OnPropertyChanged(); } }
        string _hco3 = "-";
        public string Hco3 { get { return _hco3; } set { _hco3 = value; OnPropertyChanged(); } }
        string _be = "-";
        public string Be { get { return _be; } set { _be = value; OnPropertyChanged(); } }
        string _po2alv = "-";
        public string Po2alv { get { return _po2alv; } set { _po2alv = value; OnPropertyChanged(); } }
        string _pco2alv = "-";
        public string Pco2alv { get { return _pco2alv; } set { _pco2alv = value; OnPropertyChanged(); } }
        string _endtidalco2 = "-";
        public string Endtidalco2 { get { return _endtidalco2; } set { _endtidalco2 = value; OnPropertyChanged(); } }

        int slowUpdater = 0;
        int graphicsRefreshInterval = 15;


        Compartment selectedPres1Compartment;
        Compartment selectedPres2Compartment;
        Compartment selectedPres3Compartment;
        Compartment selectedPres4Compartment;
        Compartment selectedPres5Compartment;

        Connector selectedConnector1;
        Connector selectedConnector2;
        Connector selectedConnector3;
        Connector selectedConnector4;
        Connector selectedConnector5;





        bool initialized = false;

        double pressureGraphScaleOffset = 0;

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
            graphPressures.GraphMinY = 0;
            graphPressures.GraphMaxX = 20;
            graphPressures.DataRefreshRate = 15;
            graphPressures.PixelPerDataPoint = 2;
            graphPressures.Graph1Enabled = true;
            graphPressures.Graph2Enabled = true;
            graphPressures.Graph3Enabled = true;
            graphPressures.Graph4Enabled = true;
            graphPressures.IsSideScrolling = true;
            graphPressures.GraphicsClearanceRate = graphicsRefreshInterval;

            graphFlows.GraphMaxY = 200;
            graphFlows.GraphMinY = -50;
            graphFlows.GraphMaxX = 20;
            graphFlows.DataRefreshRate = 15;
            graphFlows.PixelPerDataPoint = 2;
            graphFlows.Graph1Enabled = true;
            graphFlows.Graph2Enabled = true;
            graphFlows.Graph3Enabled = true;
            graphFlows.Graph4Enabled = true;
            graphFlows.IsSideScrolling = true;
            graphFlows.GraphicsClearanceRate = graphicsRefreshInterval;
            graphFlows.DrawGridOnScreen();

            graphPatMonitor.GraphMaxY = 100;
            graphPatMonitor.GraphMaxX = 20;
            graphPatMonitor.GraphXOffset = 10;
            graphPatMonitor.DataRefreshRate = 15;
            graphPatMonitor.PixelPerDataPoint = 2;
            graphPatMonitor.Graph1Enabled = true;
            graphPatMonitor.Graph2Enabled = true;
            graphPatMonitor.Graph3Enabled = true;
            graphPatMonitor.Graph4Enabled = true;
            graphPatMonitor.Graph5Enabled = true;
            graphPatMonitor.IsSideScrolling = true;
            graphPatMonitor.XAxisTitle = "";
            graphPatMonitor.HideYAxisLabels = true;
            graphPatMonitor.HideXAxisLabels = true;
            graphPatMonitor.NoGrid = true;
            graphPatMonitor.GraphPaint1.Color = SKColors.LimeGreen;
            graphPatMonitor.GraphPaint2.Color = SKColors.Fuchsia;
            graphPatMonitor.GraphPaint3.Color = SKColors.Red;
            graphPatMonitor.GraphPaint4.Color = SKColors.White;
            graphPatMonitor.GraphicsClearanceRate = graphicsRefreshInterval;


            ModelName = PhysModelMain.currentModel.Name;
            
            graphElastance.DataRefreshRate = 15;
            graphElastance.PixelPerDataPoint = 1;
            graphElastance.Graph1Enabled = true;
            graphElastance.IsSideScrolling = false;
            graphElastance.GraphicsClearanceRate = graphicsRefreshInterval;
            graphElastance.RealTimeDrawing = false;

            // populate controls
            PopulateLists();

            // select defsault compartments
            selectedPres1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("AA");
            selectedPres2Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LV");
            selectedPres3Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LA");

            selectedConnector1 = (Connector)PhysModelMain.FindValveByName("LA_LV");
            selectedConnector2 = (Connector)PhysModelMain.FindValveByName("LV_AA");

            initialized = true;
        }

        private void ModelInterface_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { 
            switch (e.PropertyName)
            {
                case "ModelUpdated":
                    UpdatePressureGraph();
                    UpdateFlowGraph();
                    UpdateMonitorGraph();
                    break;
                case "StatusMessage":
                    lstBox.Items.Add(PhysModelMain.modelInterface.StatusMessage);
                    break;
            }

        }
        void UpdateFlowGraph()
        {
            if (GraphFlowsEnabled)
            {
                double param1 = 0;
                double param2 = 0;
                double param3 = 0;
                double param4 = 0;
                double param5 = 0;

                if (selectedConnector1 == null)
                {
                    graphFlows.Graph1Enabled = false;
                } else
                {
                    param1 = selectedConnector1.CurrentFlow;
                    graphFlows.Graph1Enabled = true;
                }

                if (selectedConnector2 == null)
                {
                    graphFlows.Graph2Enabled = false;
                }
                else
                {
                    param2 = selectedConnector2.CurrentFlow;
                    graphFlows.Graph2Enabled = true;
                }

                if (selectedConnector3 == null)
                {
                    graphFlows.Graph3Enabled = false;
                }
                else
                {
                    param3 = selectedConnector3.CurrentFlow;
                    graphFlows.Graph3Enabled = true;
                }

                if (selectedConnector4 == null)
                {
                    graphFlows.Graph4Enabled = false;
                }
                else
                {
                    param4 = selectedConnector4.CurrentFlow;
                    graphFlows.Graph4Enabled = true;
                }

                if (selectedConnector5 == null)
                {
                    graphFlows.Graph5Enabled = false;
                }
                else
                {
                    param5 = selectedConnector5.CurrentFlow;
                    graphFlows.Graph5Enabled = true;
                }

                graphFlows.UpdateRealtimeGraphData(0, param1, 0, param2, 0, param3, 0, param4, 0 , param5);
            }
        }

        void UpdatePressureGraph()
        {
            if (GraphPressureEnabled)
            {
                double param1 = 0;
                double param2 = 0;
                double param3 = 0;
                double param4 = 0;
                double param5 = 0;

                if (selectedPres1Compartment == null)
                {
                    graphPressures.Graph1Enabled = false;
                } else
                {
                    param1 = selectedPres1Compartment.PresCurrent - pressureGraphScaleOffset;
                    graphPressures.Graph1Enabled = true;
                }

                if (selectedPres2Compartment == null)
                {
                    graphPressures.Graph2Enabled = false;
                }
                else
                {
                    param2 = selectedPres2Compartment.PresCurrent - pressureGraphScaleOffset;
                    graphPressures.Graph2Enabled = true;
                }

                if (selectedPres3Compartment == null)
                {
                    graphPressures.Graph3Enabled = false;
                }
                else
                {
                    param3 = selectedPres3Compartment.PresCurrent - pressureGraphScaleOffset;
                    graphPressures.Graph3Enabled = true;
                }

                if (selectedPres4Compartment == null)
                {
                    graphPressures.Graph4Enabled = false;
                }
                else
                {
                    param4 = selectedPres4Compartment.PresCurrent - pressureGraphScaleOffset;
                    graphPressures.Graph4Enabled = true;
                }

                if (selectedPres5Compartment == null)
                {
                    graphPressures.Graph5Enabled = false;
                }
                else
                {
                    param5 = selectedPres5Compartment.PresCurrent - pressureGraphScaleOffset;
                    graphPressures.Graph5Enabled = true;
                }

                graphPressures.UpdateRealtimeGraphData(0, param1, 0, param2, 0, param3, 0, param4, 0, param5);

            }
        }

        void UpdateMonitorGraph()
        {
            if (GraphPatMonitorEnabled)
            {
                double param1 = PhysModelMain.modelInterface.ECGSignal / 6 + 100;
                double param2 = (PhysModelMain.modelInterface.SPO2POSTSignal - PhysModelMain.currentModel.DA.dataCollector.PresMin) / 4 + 70;
                double param3 = (PhysModelMain.modelInterface.ABPSignal - PhysModelMain.currentModel.AA.dataCollector.PresMin) / 4 + 45;
                double param4 = PhysModelMain.modelInterface.RESPVolumeSignal / 4 + 15;
                double param5 = PhysModelMain.modelInterface.ETCO2Signal / 4 - 10;
           

                graphPatMonitor.UpdateRealtimeGraphData(0, param1, 0, param2, 0, param3, 0, param4, 0, param5);

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
                Kidneysflow = PhysModelMain.modelInterface.KidneysFlow.ToString();
                Liverflow = PhysModelMain.modelInterface.LiverFlow.ToString();
                Brainflow = PhysModelMain.modelInterface.BrainFlow.ToString();

                Tidalvolume = PhysModelMain.modelInterface.TidalVolume.ToString();
                Minutevolume = PhysModelMain.modelInterface.MinuteVolume.ToString();
                Alveolarvolume = PhysModelMain.modelInterface.AlveolarVolume;

                Appliedpressure = PhysModelMain.modelInterface.AppliedAirwayPressure;
                Airwaypressure = PhysModelMain.modelInterface.AirwayPressure;
                Alvleftpressure = PhysModelMain.modelInterface.AlveolarLeftPressure;
                Alvrightpressure = PhysModelMain.modelInterface.AlveolarRightPressure;

                Ph = PhysModelMain.modelInterface.ArterialPH.ToString();
                Pao2 = PhysModelMain.modelInterface.ArterialPO2.ToString();
                Paco2 = PhysModelMain.modelInterface.ArterialPCO2.ToString();
                Hco3 = PhysModelMain.modelInterface.ArterialHCO3.ToString();
                Be = PhysModelMain.modelInterface.ArterialBE.ToString();
                
                Po2alv = PhysModelMain.modelInterface.AlveolarPO2;
                Pco2alv = PhysModelMain.modelInterface.AlveolarPCO2;
                Endtidalco2 = PhysModelMain.modelInterface.EndTidalCO2.ToString();

            }
            slowUpdater += graphicsRefreshInterval;
 
            //canvasDiagram.InvalidateVisual();

            if (GraphPressureEnabled)
                graphPressures.DrawGraphOnScreen();

            if (GraphFlowsEnabled)
                graphFlows.DrawGraphOnScreen();

            if (GraphPatMonitorEnabled)
                graphPatMonitor.DrawGraphOnScreen();
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

        private void CmbPressureGraphSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialized)
            {
                switch (cmbPressureGraphSelector.SelectedIndex)
                {
                    case 0: // heart
                        graphPressures.GraphMaxY = 100;
                        graphPressures.GraphMinY = 0;
                        graphPressures.GraphMaxX = 20;
                        graphPressures.Legend1 = "AA";
                        graphPressures.Legend2 = "LV";
                        graphPressures.Legend3 = "PA";
                        graphPressures.Legend4 = "RV";
                        graphPressures.XAxisTitle = "time";

                        pressureGraphScaleOffset = 0;

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("AA");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LV");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("PA");
                        selectedPres4Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("RV");

                        break;
                    case 1: // left heart
                        graphPressures.GraphMaxY = 100;
                        graphPressures.GraphMinY = 0;
                        graphPressures.GraphMaxX = 20;
                        pressureGraphScaleOffset = 0;

                        graphPressures.Legend1 = "AA";
                        graphPressures.Legend2 = "LV";
                        graphPressures.Legend3 = "LA";
 
                        graphPressures.XAxisTitle = "time";

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("AA");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LV");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LA");
                        selectedPres4Compartment = null;
                        break;
                    case 2: // right heart
                        graphPressures.GraphMaxY = 100;
                        graphPressures.GraphMinY = 0;
                        graphPressures.GraphMaxX = 20;
                        pressureGraphScaleOffset = 0;

                        graphPressures.Legend1 = "PA";
                        graphPressures.Legend2 = "RV";
                        graphPressures.Legend3 = "RA";

                        graphPressures.XAxisTitle = "time";

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("PA");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("RV");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("RA");
                        selectedPres4Compartment = null;
                        break;
                    case 3: // lungs
                        graphPressures.GraphMaxY = 40;
                        graphPressures.GraphMinY = -20;
                        graphPressures.GraphMaxX = 20;
                        pressureGraphScaleOffset = PhysModelMain.currentModel.Patm;

                        graphPressures.Legend1 = "OUT";
                        graphPressures.Legend2 = "NCA";
                        graphPressures.Legend3 = "ALL";
                        graphPressures.Legend4 = "ALR";
                        graphPressures.XAxisTitle = "time";

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("OUT");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("NCA");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALL");
                        selectedPres4Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALR");
                        break;
                    case 4: // left lung
                        graphPressures.GraphMaxY = 40;
                        graphPressures.GraphMinY = -20;
                        graphPressures.GraphMaxX = 20;
                        pressureGraphScaleOffset = PhysModelMain.currentModel.Patm;

                        graphPressures.Legend1 = "OUT";
                        graphPressures.Legend2 = "NCA";
                        graphPressures.Legend3 = "ALL";
                        graphPressures.Legend4 = "ALR";
                        graphPressures.XAxisTitle = "time";

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("OUT");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("NCA");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALL");
                        selectedPres4Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALR");

                        break;
                    case 5: // right lung
                        graphPressures.GraphMaxY = 40;
                        graphPressures.GraphMinY = -20;
                        graphPressures.GraphMaxX = 20;
                        pressureGraphScaleOffset = PhysModelMain.currentModel.Patm;

                        graphPressures.Legend1 = "OUT";
                        graphPressures.Legend2 = "NCA";
                        graphPressures.Legend3 = "ALL";
                        graphPressures.Legend4 = "ALR";
                        graphPressures.XAxisTitle = "time";

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("OUT");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("NCA");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALL");
                        selectedPres4Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALR");

                        break;

                }

                graphPressures.DataRefreshRate = 15;
                graphPressures.PixelPerDataPoint = 2;
                graphPressures.IsSideScrolling = true;
                graphPressures.GraphicsClearanceRate = graphicsRefreshInterval;
                graphPressures.HideXAxisLabels = true;
                graphPressures.HideLegends = false;
                graphPressures.DrawGridOnScreen();

               

            }
        }

   
    }
}
