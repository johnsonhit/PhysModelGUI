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

        int selectedPressureID = 0;
        int selectedFlowID = 0;
        int selectedPVID = 0;
        int trendGraphSelection = 1;

        bool _viewOFOEnabled = false;
        public bool ViewOFOEnabled { get { return _viewOFOEnabled; } set { _viewOFOEnabled = value; OnPropertyChanged(); } }

        bool _viewVSDEnabled = false;
        public bool ViewVSDEnabled { get { return _viewVSDEnabled; } set { _viewVSDEnabled = value; OnPropertyChanged(); } }

        bool _viewPDAEnabled = false;
        public bool ViewPDAEnabled { get { return _viewPDAEnabled; } set { _viewPDAEnabled = value; OnPropertyChanged(); } }

        bool _viewSHUNTEnabled = false;
        public bool ViewSHUNTEnabled { get { return _viewSHUNTEnabled; } set { _viewSHUNTEnabled = value; OnPropertyChanged(); } }

        bool _viewMYOEnabled = false;
        public bool ViewMYOEnabled { get { return _viewMYOEnabled; } set { _viewMYOEnabled = value; OnPropertyChanged(); } }

        bool _graphPatMonitorEnabled = false;
        public bool GraphPatMonitorEnabled { get { return _graphPatMonitorEnabled; } set { _graphPatMonitorEnabled = value; OnPropertyChanged(); } }

        bool _graphPressuresEnabled = false;
        public bool GraphPressureEnabled { get { return _graphPressuresEnabled; } set { _graphPressuresEnabled = value; OnPropertyChanged(); } }

        bool _graphFlowsEnabled = false;
        public bool GraphFlowsEnabled { get { return _graphFlowsEnabled; } set { _graphFlowsEnabled = value; OnPropertyChanged(); } }

        bool _graphPVLoopsEnabled = false;
        public bool GraphPVLoopsEnabled { get { return _graphPVLoopsEnabled; } set { _graphPVLoopsEnabled = value; OnPropertyChanged(); } }

        bool _graph1PressureDisabled = false;
        public bool Graph1PressureDisabled { get { return _graph1PressureDisabled; } set { _graph1PressureDisabled = value; OnPropertyChanged(); } }

        bool _graph2PressureDisabled = false;
        public bool Graph2PressureDisabled { get { return _graph2PressureDisabled; } set { _graph2PressureDisabled = value; OnPropertyChanged(); } }

        bool _graph3PressureDisabled = false;
        public bool Graph3PressureDisabled { get { return _graph3PressureDisabled; } set { _graph3PressureDisabled = value; OnPropertyChanged(); } }

        bool _graph4PressureDisabled = false;
        public bool Graph4PressureDisabled { get { return _graph4PressureDisabled; } set { _graph4PressureDisabled = value; OnPropertyChanged(); } }

        bool _graph5PressureDisabled = false;
        public bool Graph5PressureDisabled { get { return _graph5PressureDisabled; } set { _graph5PressureDisabled = value; OnPropertyChanged(); } }

        bool _graph1FlowDisabled = false;
        public bool Graph1FlowDisabled { get { return _graph1FlowDisabled; } set { _graph1FlowDisabled = value; OnPropertyChanged(); } }

        bool _graph2FlowDisabled = false;
        public bool Graph2FlowDisabled { get { return _graph2FlowDisabled; } set { _graph2FlowDisabled = value; OnPropertyChanged(); } }

        bool _graph3FlowDisabled = false;
        public bool Graph3FlowDisabled { get { return _graph3FlowDisabled; } set { _graph3FlowDisabled = value; OnPropertyChanged(); } }

        bool _graph4FlowDisabled = false;
        public bool Graph4FlowDisabled { get { return _graph4FlowDisabled; } set { _graph4FlowDisabled = value; OnPropertyChanged(); } }

        bool _graph5FlowDisabled = false;
        public bool Graph5FlowDisabled { get { return _graph5FlowDisabled; } set { _graph5FlowDisabled = value; OnPropertyChanged(); } }

        bool _graph1PVDisabled = false;
        public bool Graph1PVDisabled { get { return _graph1PVDisabled; } set { _graph1PVDisabled = value; OnPropertyChanged(); } }

        bool _graph2PVDisabled = false;
        public bool Graph2PVDisabled { get { return _graph2PVDisabled; } set { _graph2PVDisabled = value; OnPropertyChanged(); } }

        bool _graph3PVDisabled = false;
        public bool Graph3PVDisabled { get { return _graph3PVDisabled; } set { _graph3PVDisabled = value; OnPropertyChanged(); } }

        bool _graph4PVDisabled = false;
        public bool Graph4PVDisabled { get { return _graph4PVDisabled; } set { _graph4PVDisabled = value; OnPropertyChanged(); } }

        bool _graph5PVDisabled = false;
        public bool Graph5PVDisabled { get { return _graph5PVDisabled; } set { _graph5PVDisabled = value; OnPropertyChanged(); } }

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

        Compartment selectedPV1Compartment;
        Compartment selectedPV2Compartment;
        Compartment selectedPV3Compartment;
        Compartment selectedPV4Compartment;
        Compartment selectedPV5Compartment;

        Connector selectedConnector1;
        Connector selectedConnector2;
        Connector selectedConnector3;
        Connector selectedConnector4;
        Connector selectedConnector5;



        List<PVPoint> PVDataBlock1 = new List<PVPoint>();

        bool initialized = false;

        double pressureGraphScaleOffset = 0;
        double pvGraphScaleOffset = 0;


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

            graphPVLoops.GraphMaxY = 100;
            graphPVLoops.GraphMinY = 0;
            graphPVLoops.GraphMaxX = 20;
            graphPVLoops.GridXAxisStep = 5;
            graphPVLoops.GraphMinX = 0;
            graphPVLoops.DataRefreshRate = 15;
            graphPVLoops.PixelPerDataPoint = 2;
            graphPVLoops.Graph1Enabled = true;
            graphPVLoops.Graph2Enabled = false;
            graphPVLoops.Graph3Enabled = false;
            graphPVLoops.Graph4Enabled = false;
            graphPVLoops.Graph5Enabled = false;
            graphPVLoops.IsSideScrolling = false;
            graphPVLoops.HideLegends = false;
            graphPressures.HideXAxisLabels = false;
            graphPVLoops.XAxisTitle = "volume";
            graphPVLoops.PointMode1 = SKPointMode.Points;
            graphPVLoops.GraphicsClearanceRate = 5000;
            graphPVLoops.DrawGridOnScreen();


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


            ConfigureTrendGraph1();
            ConfigureTrendGraph2();
            ConfigureTrendGraph3();

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

            selectedPV1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LV");

            initialized = true;

            cmbFlowGraphSelector.SelectedIndex = 0;
            cmbPressureGraphSelector.SelectedIndex = 0;
            cmbPVLoopsGraphSelector.SelectedIndex = 0;

            SelectTrendGraph(1);

        }

        public void SelectTrendGraph(int id)
        {
            trendGraphSelection = id;
            switch (id)
            {
                case 1:
                    graphTrendMonitor1.Visibility = Visibility.Visible;
                    graphTrendMonitor2.Visibility = Visibility.Collapsed;
                    graphTrendMonitor3.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    graphTrendMonitor1.Visibility = Visibility.Collapsed;
                    graphTrendMonitor2.Visibility = Visibility.Visible;
                    graphTrendMonitor3.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    graphTrendMonitor1.Visibility = Visibility.Collapsed;
                    graphTrendMonitor2.Visibility = Visibility.Collapsed;
                    graphTrendMonitor3.Visibility = Visibility.Visible;
                    break;

            }
        }

        void ConfigureTrendGraph1()
        {
            graphTrendMonitor1.GraphMaxY = 150;
            graphTrendMonitor1.GraphMaxX = 20;
            graphTrendMonitor1.DataRefreshRate = 15;
            graphTrendMonitor1.PixelPerDataPoint = 2;
            graphTrendMonitor1.Graph1Enabled = true;
            graphTrendMonitor1.Graph2Enabled = true;
            graphTrendMonitor1.Graph3Enabled = true;
            graphTrendMonitor1.Graph4Enabled = true;
            graphTrendMonitor1.Graph5Enabled = true;
            graphTrendMonitor1.IsSideScrolling = true;
            graphTrendMonitor1.XAxisTitle = "time";
            graphTrendMonitor1.HideYAxisLabels = false;
            graphTrendMonitor1.HideXAxisLabels = false;
            graphTrendMonitor1.NoGrid = false;
            graphTrendMonitor1.Legend1 = "heartrate";
            graphTrendMonitor1.Legend2 = "spo2";
            graphTrendMonitor1.Legend3 = "systole";
            graphTrendMonitor1.Legend4 = "diastole";
            graphTrendMonitor1.Legend5 = "resp rate";
            graphTrendMonitor1.HideLegends = false;
            graphTrendMonitor1.GraphPaint1.Color = SKColors.DarkGreen;
            graphTrendMonitor1.GraphPaint2.Color = SKColors.Fuchsia;
            graphTrendMonitor1.GraphPaint3.Color = SKColors.DarkRed;
            graphTrendMonitor1.GraphPaint4.Color = SKColors.DarkRed;
            graphTrendMonitor1.GraphPaint5.Color = SKColors.Black;
            graphTrendMonitor1.GraphicsClearanceRate = 5000;
            graphTrendMonitor1.BackgroundColor = SKColors.White;
            graphTrendMonitor1.GridLineAxisPaint.Color = SKColors.Black;
            graphTrendMonitor1.GridLinePaint.Color = SKColors.Black;
            graphTrendMonitor1.LegendTextPaint.Color = SKColors.Black;
            graphTrendMonitor1.GridAxisLabelsPaint.Color = SKColors.Black;
        }

        void ConfigureTrendGraph2()
        {
            graphTrendMonitor2.GraphMaxY = 150;
            graphTrendMonitor2.GraphMaxX = 20;
            graphTrendMonitor2.DataRefreshRate = 15;
            graphTrendMonitor2.PixelPerDataPoint = 2;
            graphTrendMonitor2.Graph1Enabled = true;
            graphTrendMonitor2.Graph2Enabled = true;
            graphTrendMonitor2.Graph3Enabled = true;
            graphTrendMonitor2.Graph4Enabled = true;
            graphTrendMonitor2.Graph5Enabled = true;
            graphTrendMonitor2.IsSideScrolling = true;
            graphTrendMonitor2.XAxisTitle = "trend graph 2";
            graphTrendMonitor2.HideYAxisLabels = false;
            graphTrendMonitor2.HideXAxisLabels = false;
            graphTrendMonitor2.NoGrid = false;
            graphTrendMonitor2.Legend1 = "heartrate";
            graphTrendMonitor2.Legend2 = "spo2";
            graphTrendMonitor2.Legend3 = "systole";
            graphTrendMonitor2.Legend4 = "diastole";
            graphTrendMonitor2.Legend5 = "resp rate";
            graphTrendMonitor2.HideLegends = false;
            graphTrendMonitor2.GraphPaint1.Color = SKColors.DarkGreen;
            graphTrendMonitor2.GraphPaint2.Color = SKColors.Fuchsia;
            graphTrendMonitor2.GraphPaint3.Color = SKColors.DarkRed;
            graphTrendMonitor2.GraphPaint4.Color = SKColors.DarkRed;
            graphTrendMonitor2.GraphPaint5.Color = SKColors.Black;
            graphTrendMonitor2.GraphicsClearanceRate = 5000;
            graphTrendMonitor2.BackgroundColor = SKColors.White;
            graphTrendMonitor2.GridLineAxisPaint.Color = SKColors.Black;
            graphTrendMonitor2.GridLinePaint.Color = SKColors.Black;
            graphTrendMonitor2.LegendTextPaint.Color = SKColors.Black;
            graphTrendMonitor2.GridAxisLabelsPaint.Color = SKColors.Black;
        }

        void ConfigureTrendGraph3()
        {
            graphTrendMonitor3.GraphMaxY = 25;
            graphTrendMonitor3.GraphMinY = -15;
            graphTrendMonitor3.GraphMaxX = 20;
            graphTrendMonitor3.DataRefreshRate = 15;
            graphTrendMonitor3.PixelPerDataPoint = 2;
            graphTrendMonitor3.Graph1Enabled = true;
            graphTrendMonitor3.Graph2Enabled = true;
            graphTrendMonitor3.Graph3Enabled = true;
            graphTrendMonitor3.Graph4Enabled = true;
            graphTrendMonitor3.Graph5Enabled = true;
            graphTrendMonitor3.IsSideScrolling = true;
            graphTrendMonitor3.XAxisTitle = "time";
            graphTrendMonitor3.HideYAxisLabels = false;
            graphTrendMonitor3.HideXAxisLabels = false;
            graphTrendMonitor3.NoGrid = false;
            graphTrendMonitor3.Legend1 = "pH";
            graphTrendMonitor3.Legend2 = "po2";
            graphTrendMonitor3.Legend3 = "pco2";
            graphTrendMonitor3.Legend4 = "hco3";
            graphTrendMonitor3.Legend5 = "be";
            graphTrendMonitor3.HideLegends = false;
            graphTrendMonitor3.GraphPaint1.Color = SKColors.DarkGreen;
            graphTrendMonitor3.GraphPaint2.Color = SKColors.Fuchsia;
            graphTrendMonitor3.GraphPaint3.Color = SKColors.DarkRed;
            graphTrendMonitor3.GraphPaint4.Color = SKColors.DarkRed;
            graphTrendMonitor3.GraphPaint5.Color = SKColors.Black;
            graphTrendMonitor3.GraphicsClearanceRate = 5000;
            graphTrendMonitor3.BackgroundColor = SKColors.White;
            graphTrendMonitor3.GridLineAxisPaint.Color = SKColors.Black;
            graphTrendMonitor3.GridLinePaint.Color = SKColors.Black;
            graphTrendMonitor3.LegendTextPaint.Color = SKColors.Black;
            graphTrendMonitor3.GridAxisLabelsPaint.Color = SKColors.Black;
        }

        void ModelInterface_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { 
            switch (e.PropertyName)
            {
                case "ModelUpdated":
                    UpdatePVLoops();
                    UpdatePressureGraph();
                    UpdateFlowGraph();
                    UpdateMonitorGraph();
                 
                    break;
                case "StatusMessage":
                    lstBox.Items.Add(PhysModelMain.modelInterface.StatusMessage);
                    break;
            }

        }

        void UpdateTrendGraph1()
        {
            graphTrendMonitor1.UpdateRealtimeGraphData(0, PhysModelMain.modelInterface.HeartRate, 0, PhysModelMain.modelInterface.ArterialSO2Pre, 0, PhysModelMain.modelInterface.SystolicSystemicArterialPressure, 0, PhysModelMain.modelInterface.DiastolicSystemicArterialPressure, 0, PhysModelMain.modelInterface.RespiratoryRate);
       
        }

        void UpdateTrendGraph2()
        {
            //graphTrendMonitor2.UpdateRealtimeGraphData(0, 20, 0, 30, 0, 40, 0, 50, 0, 60);
        }

        void UpdateTrendGraph3()
        {
            //graphTrendMonitor3.UpdateRealtimeGraphData(0, PhysModelMain.modelInterface.ArterialPH, 0, PhysModelMain.modelInterface.ArterialPO2, 0, PhysModelMain.modelInterface.ArterialPCO2, 0, PhysModelMain.modelInterface.ArterialHCO3, 0, PhysModelMain.modelInterface.ArterialBE);
        }

        void UpdatePVLoops()
        {
        
            if (GraphPVLoopsEnabled)
            {

                if (selectedPV1Compartment == null || Graph1PVDisabled)
                {
                    graphPVLoops.Graph1Enabled = false;
                } else
                {
                    graphPVLoops.Graph1Enabled = true;
                    PVDataBlock1 = selectedPV1Compartment.dataCollector.PVDataBlock;
                }


                int counter = 0;
                if (pvGraphScaleOffset > 0)  // then it is a lung compartment and the pv loop is the other way around!
                {
                    lock (PVDataBlock1)
                    {
                        foreach (PVPoint p in PVDataBlock1)
                        {
                            graphPVLoops.UpdateRealtimeGraphData(p.pressure1 - pvGraphScaleOffset, p.volume1);
                            counter++;
                        }
                    }
                } else
                {
                    lock (PVDataBlock1)
                    {
                        foreach (PVPoint p in PVDataBlock1)
                        {
                            graphPVLoops.UpdateRealtimeGraphData(p.volume1, p.pressure1 - pvGraphScaleOffset);
                            counter++;
                        }
                    }
                }
              




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

                if (selectedConnector1 == null || Graph1FlowDisabled)
                {
                    graphFlows.Graph1Enabled = false;
                } else
                {
                    param1 = selectedConnector1.RealFlow;
                    graphFlows.Graph1Enabled = true;
                }

                if (selectedConnector2 == null || Graph2FlowDisabled)
                {
                    graphFlows.Graph2Enabled = false;
                }
                else
                {
                    param2 = selectedConnector2.RealFlow;
                    graphFlows.Graph2Enabled = true;
                }

                if (selectedConnector3 == null || Graph3FlowDisabled)
                {
                    graphFlows.Graph3Enabled = false;
                }
                else
                {
                    param3 = selectedConnector3.RealFlow;
                    graphFlows.Graph3Enabled = true;
                }

                if (selectedConnector4 == null || Graph4FlowDisabled)
                {
                    graphFlows.Graph4Enabled = false;
                }
                else
                {
                    param4 = selectedConnector4.RealFlow;
                    graphFlows.Graph4Enabled = true;
                }

                if (selectedConnector5 == null || Graph5FlowDisabled)
                {
                    graphFlows.Graph5Enabled = false;
                }
                else
                {
                    param5 = selectedConnector5.RealFlow;
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

                if (selectedPres1Compartment == null || Graph1PressureDisabled)
                {
                    graphPressures.Graph1Enabled = false;
                } else
                {
                    param1 = selectedPres1Compartment.PresCurrent - pressureGraphScaleOffset;
                    graphPressures.Graph1Enabled = true;
                }

                if (selectedPres2Compartment == null || Graph2PressureDisabled)
                {
                    graphPressures.Graph2Enabled = false;
                }
                else
                {
                    param2 = selectedPres2Compartment.PresCurrent - pressureGraphScaleOffset;
                    graphPressures.Graph2Enabled = true;
                }

                if (selectedPres3Compartment == null || Graph3PressureDisabled)
                {
                    graphPressures.Graph3Enabled = false;
                }
                else
                {
                    param3 = selectedPres3Compartment.PresCurrent - pressureGraphScaleOffset;
                    graphPressures.Graph3Enabled = true;
                }

                if (selectedPres4Compartment == null || Graph4PressureDisabled)
                {
                    graphPressures.Graph4Enabled = false;
                }
                else
                {
                    param4 = selectedPres4Compartment.PresCurrent - pressureGraphScaleOffset;
                    graphPressures.Graph4Enabled = true;
                }

                if (selectedPres5Compartment == null || Graph5PressureDisabled)
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

        void DispatcherTimer_Tick(object sender, EventArgs e)
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

                UpdateTrendGraph1();
                UpdateTrendGraph2();
                UpdateTrendGraph3();

                switch (trendGraphSelection)
                {
                    case 1:
                        graphTrendMonitor1.DrawGraphOnScreen();
                        break;
                    case 2:
                        graphTrendMonitor2.DrawGraphOnScreen();
                        break;
                    case 3:
                        graphTrendMonitor3.DrawGraphOnScreen();
                        break;
                }
            }
            slowUpdater += graphicsRefreshInterval;
 
            canvasDiagram.InvalidateVisual();

         

            if (GraphPressureEnabled)
                graphPressures.DrawGraphOnScreen();

            if (GraphFlowsEnabled)
                graphFlows.DrawGraphOnScreen();

            if (GraphPatMonitorEnabled)
                graphPatMonitor.DrawGraphOnScreen();

            if (GraphPVLoopsEnabled)
                graphPVLoops.DrawGraphOnScreen();
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

        void SelectPressureGraph(int selection)
        {
            if (initialized)
            {
                selectedPressureID = selection;
                switch (selection)
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

                        Graph1PressureDisabled = false;
                        Graph2PressureDisabled = false;
                        Graph3PressureDisabled = false;
                        Graph4PressureDisabled = false;
                        Graph5PressureDisabled = true;

                        graphPressures.Graph1Enabled = true;
                        graphPressures.Graph2Enabled = true;
                        graphPressures.Graph3Enabled = true;
                        graphPressures.Graph4Enabled = true;
                        graphPressures.Graph5Enabled = false;

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

                        Graph1PressureDisabled = false;
                        Graph2PressureDisabled = false;
                        Graph3PressureDisabled = false;
                        Graph4PressureDisabled = true;
                        Graph5PressureDisabled = true;

                        graphPressures.Graph1Enabled = true;
                        graphPressures.Graph2Enabled = true;
                        graphPressures.Graph3Enabled = true;
                        graphPressures.Graph4Enabled = false;
                        graphPressures.Graph5Enabled = false;

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

                        Graph1PressureDisabled = false;
                        Graph2PressureDisabled = false;
                        Graph3PressureDisabled = false;
                        Graph4PressureDisabled = true;
                        Graph5PressureDisabled = true;

                        graphPressures.Graph1Enabled = true;
                        graphPressures.Graph2Enabled = true;
                        graphPressures.Graph3Enabled = true;
                        graphPressures.Graph4Enabled = false;
                        graphPressures.Graph5Enabled = false;

                        break;
                    case 3: // lungs
                        graphPressures.GraphMaxY = 15;
                        graphPressures.GraphMinY = -15;
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

                        Graph1PressureDisabled = false;
                        Graph2PressureDisabled = false;
                        Graph3PressureDisabled = false;
                        Graph4PressureDisabled = false;
                        Graph5PressureDisabled = true;

                        graphPressures.Graph1Enabled = true;
                        graphPressures.Graph2Enabled = true;
                        graphPressures.Graph3Enabled = true;
                        graphPressures.Graph4Enabled = true;
                        graphPressures.Graph5Enabled = false;

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
        private void CmbPressureGraphSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialized)
            {
                SelectPressureGraph(cmbPressureGraphSelector.SelectedIndex);
            }
        }

        void SelectFlowGraph(int selection)
        {
            if (initialized)
            {
                selectedFlowID = selection;
                switch (selection)
                {
                    case 0: // left heart
                        graphFlows.GraphMaxY = 200;
                        graphFlows.GraphMinY = -50;
                        graphFlows.GraphMaxX = 20;

                        graphFlows.Legend1 = "mv";
                        graphFlows.Legend2 = "av";
                        graphFlows.Legend3 = "";
                        graphFlows.Legend4 = "";

                        selectedConnector1 = (Connector)PhysModelMain.FindValveByName("LA_LV");
                        selectedConnector2 = (Connector)PhysModelMain.FindValveByName("LV_AA");
                        selectedConnector3 = null;
                        selectedConnector4 = null;
                        selectedConnector5 = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = false;
                        Graph3FlowDisabled = true;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;

                        graphFlows.Graph1Enabled = true;
                        graphFlows.Graph2Enabled = true;
                        graphFlows.Graph3Enabled = false;
                        graphFlows.Graph4Enabled = false;
                        graphFlows.Graph5Enabled = false;


                        break;
                    case 1: // right heart
                        graphFlows.GraphMaxY = 200;
                        graphFlows.GraphMinY = -50;
                        graphFlows.GraphMaxX = 20;
                        graphFlows.Legend1 = "tv";
                        graphFlows.Legend2 = "pv";
                        graphFlows.Legend3 = "";
                        graphFlows.Legend4 = "";

                        selectedConnector1 = (Connector)PhysModelMain.FindValveByName("RA_RV");
                        selectedConnector2 = (Connector)PhysModelMain.FindValveByName("RV_PA");
                        selectedConnector3 = null;
                        selectedConnector4 = null;
                        selectedConnector5 = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = false;
                        Graph3FlowDisabled = true;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;

                        graphFlows.Graph1Enabled = true;
                        graphFlows.Graph2Enabled = true;
                        graphFlows.Graph3Enabled = false;
                        graphFlows.Graph4Enabled = false;
                        graphFlows.Graph5Enabled = false;
                        break;
                    case 2: // pulmonary artery
                        graphFlows.GraphMaxY = 51;
                        graphFlows.GraphMinY = -50;
                        graphFlows.GraphMaxX = 20;
                        graphFlows.Legend1 = "lung l";
                        graphFlows.Legend2 = "lung r";
                        graphFlows.Legend3 = "lung shunt";
                        graphFlows.Legend4 = "";

                        selectedConnector1 = (Connector)PhysModelMain.FindBloodConnectorByName("PA_LL");
                        selectedConnector2 = (Connector)PhysModelMain.FindBloodConnectorByName("PA_LR");
                        selectedConnector3 = (Connector)PhysModelMain.FindBloodConnectorByName("PA_PV");
                        selectedConnector4 = null;
                        selectedConnector5 = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = false;
                        Graph3FlowDisabled = false;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;

                        graphFlows.Graph1Enabled = true;
                        graphFlows.Graph2Enabled = true;
                        graphFlows.Graph3Enabled = true;
                        graphFlows.Graph4Enabled = false;
                        graphFlows.Graph5Enabled = false;
                        break;
                    case 3: // aorta
                        graphFlows.GraphMaxY = 10;
                        graphFlows.GraphMinY = -10;
                        graphFlows.GraphMaxX = 20;
                        graphFlows.Legend1 = "brain";
                        graphFlows.Legend2 = "ub";
                        graphFlows.Legend3 = "lb";
                        graphFlows.Legend4 = "kidneys";
                        graphFlows.Legend5 = "liver";

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = false;
                        Graph3FlowDisabled = false;
                        Graph4FlowDisabled = false;
                        Graph5FlowDisabled = false;

                        graphFlows.Graph1Enabled = true;
                        graphFlows.Graph2Enabled = true;
                        graphFlows.Graph3Enabled = true;
                        graphFlows.Graph4Enabled = true;
                        graphFlows.Graph5Enabled = true;

                        selectedConnector1 = (Connector)PhysModelMain.FindBloodConnectorByName("AA_BRAIN");
                        selectedConnector2 = (Connector)PhysModelMain.FindBloodConnectorByName("AA_UB");
                        selectedConnector3 = (Connector)PhysModelMain.FindBloodConnectorByName("AD_LB");
                        selectedConnector4 = (Connector)PhysModelMain.FindBloodConnectorByName("AD_KIDNEYS");
                        selectedConnector5 = (Connector)PhysModelMain.FindBloodConnectorByName("AD_LIVER");
                        break;
                    case 4: //  foramen ovale
                        graphFlows.GraphMaxY = 50;
                        graphFlows.GraphMinY = -50;
                        graphFlows.GraphMaxX = 20;
                        graphFlows.Legend1 = "asd";
                        graphFlows.Legend2 = "";
                        graphFlows.Legend3 = "";
                        graphFlows.Legend4 = "";

                        selectedConnector1 = (Connector)PhysModelMain.FindBloodConnectorByName("LA_RA");
                        selectedConnector2 = null;
                        selectedConnector3 = null;
                        selectedConnector4 = null;
                        selectedConnector5 = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = true;
                        Graph3FlowDisabled = true;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;

                        graphFlows.Graph1Enabled = true;
                        graphFlows.Graph2Enabled = false;
                        graphFlows.Graph3Enabled = false;
                        graphFlows.Graph4Enabled = false;
                        graphFlows.Graph5Enabled = false;
                        break;
                    case 5: // vsd
                        graphFlows.GraphMaxY = 50;
                        graphFlows.GraphMinY = -50;
                        graphFlows.GraphMaxX = 20;
                        graphFlows.Legend1 = "vsd";
                        graphFlows.Legend2 = "";
                        graphFlows.Legend3 = "";
                        graphFlows.Legend4 = "";


                        selectedConnector1 = (Connector)PhysModelMain.FindBloodConnectorByName("LV_RV");
                        selectedConnector2 = null;
                        selectedConnector3 = null;
                        selectedConnector4 = null;
                        selectedConnector5 = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = true;
                        Graph3FlowDisabled = true;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;

                        graphFlows.Graph1Enabled = true;
                        graphFlows.Graph2Enabled = false;
                        graphFlows.Graph3Enabled = false;
                        graphFlows.Graph4Enabled = false;
                        graphFlows.Graph5Enabled = false;
                        break;
                    case 6: // ductus
                        graphFlows.GraphMaxY = 50;
                        graphFlows.GraphMinY = -50;
                        graphFlows.GraphMaxX = 20;
                        graphFlows.Legend1 = "ductus";
                        graphFlows.Legend2 = "";
                        graphFlows.Legend3 = "";
                        graphFlows.Legend4 = "";

                        selectedConnector1 = (Connector)PhysModelMain.FindBloodConnectorByName("DA_PA");
                        selectedConnector2 = null;
                        selectedConnector3 = null;
                        selectedConnector4 = null;
                        selectedConnector5 = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = true;
                        Graph3FlowDisabled = true;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;
                        graphFlows.Graph1Enabled = true;
                        graphFlows.Graph2Enabled = false;
                        graphFlows.Graph3Enabled = false;
                        graphFlows.Graph4Enabled = false;
                        graphFlows.Graph5Enabled = false;
                        break;
                    case 7: // lungs
                        graphFlows.GraphMaxY = 100;
                        graphFlows.GraphMinY = -100;
                        graphFlows.GraphMaxX = 20;
                        graphFlows.Legend1 = "nca";
                        graphFlows.Legend2 = "all";
                        graphFlows.Legend3 = "alr";
                        graphFlows.Legend4 = "";

                        selectedConnector1 = (Connector)PhysModelMain.FindGasConnectorByName("OUT_NCA");
                        selectedConnector2 = (Connector)PhysModelMain.FindGasConnectorByName("NCA_ALL");
                        selectedConnector3 = (Connector)PhysModelMain.FindGasConnectorByName("NCA_ALR");
                        selectedConnector4 = null;
                        selectedConnector5 = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = false;
                        Graph3FlowDisabled = false;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;
                        graphFlows.Graph1Enabled = true;
                        graphFlows.Graph2Enabled = true;
                        graphFlows.Graph3Enabled = true;
                        graphFlows.Graph4Enabled = false;
                        graphFlows.Graph5Enabled = false;
                        break;

                }

                graphFlows.DataRefreshRate = 15;
                graphFlows.PixelPerDataPoint = 2;
                graphFlows.IsSideScrolling = true;
                graphFlows.GraphicsClearanceRate = graphicsRefreshInterval;
                graphFlows.HideLegends = false;
                graphFlows.XAxisTitle = "time";

                graphFlows.DrawGridOnScreen();
            }
  
        }
        private void CmbFlowGraphSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialized)
            {
                SelectFlowGraph(cmbFlowGraphSelector.SelectedIndex);
            }
        }

        void SelectPVLoop(int selection)
        {
            if (initialized)
            {
                selectedPVID = selection;
                switch (selection)
                {
                    case 0: // lv
                        graphPVLoops.GraphMaxY = 100;
                        graphPVLoops.GraphMinY = 0;
                        graphPVLoops.GraphMaxX = 20;
                        graphPVLoops.GridXAxisStep = 5;
                        graphPVLoops.GraphMinX = 0;
                        graphPVLoops.Graph1Enabled = true;
                        graphPVLoops.Graph2Enabled = false;
                        graphPVLoops.Graph3Enabled = false;
                        graphPVLoops.Graph4Enabled = false;
                        graphPVLoops.Graph5Enabled = false;
                        graphPVLoops.XAxisTitle = "volume";
                        pvGraphScaleOffset = 0;
                        graphPVLoops.Legend1 = "left ventricle";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LV");

                        break;
                    case 1: // la
                        graphPVLoops.GraphMaxY = 100;
                        graphPVLoops.GraphMinY = 0;
                        graphPVLoops.GraphMaxX = 20;
                        graphPVLoops.GridXAxisStep = 5;
                        graphPVLoops.GraphMinX = 0;
                        graphPVLoops.Graph1Enabled = true;
                        graphPVLoops.Graph2Enabled = false;
                        graphPVLoops.Graph3Enabled = false;
                        graphPVLoops.Graph4Enabled = false;
                        graphPVLoops.Graph5Enabled = false;
                        graphPVLoops.XAxisTitle = "volume";
                        pvGraphScaleOffset = 0;
                        graphPVLoops.Legend1 = "left atrium";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LA");

                        break;
                    case 2: // rv
                        graphPVLoops.GraphMaxY = 100;
                        graphPVLoops.GraphMinY = 0;
                        graphPVLoops.GraphMaxX = 20;
                        graphPVLoops.GridXAxisStep = 5;
                        graphPVLoops.GraphMinX = 0;
                        Graph1PVDisabled = false;
                        Graph2PVDisabled = true;
                        Graph3PVDisabled = true;
                        Graph4PVDisabled = true;
                        Graph5PVDisabled = true;
                        graphPVLoops.Graph1Enabled = true;
                        graphPVLoops.Graph2Enabled = false;
                        graphPVLoops.Graph3Enabled = false;
                        graphPVLoops.Graph4Enabled = false;
                        graphPVLoops.Graph5Enabled = false;
                        graphPVLoops.XAxisTitle = "volume";
                        pvGraphScaleOffset = 0;
                        graphPVLoops.Legend1 = "right ventricle";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("RV");

                        break;
                    case 3: // ra
                        graphPVLoops.GraphMaxY = 100;
                        graphPVLoops.GraphMinY = 0;
                        graphPVLoops.GraphMaxX = 20;
                        graphPVLoops.GridXAxisStep = 5;
                        graphPVLoops.GraphMinX = 0;
                        Graph1PVDisabled = false;
                        Graph2PVDisabled = true;
                        Graph3PVDisabled = true;
                        Graph4PVDisabled = true;
                        Graph5PVDisabled = true;
                        graphPVLoops.Graph1Enabled = true;
                        graphPVLoops.Graph2Enabled = false;
                        graphPVLoops.Graph3Enabled = false;
                        graphPVLoops.Graph4Enabled = false;
                        graphPVLoops.Graph5Enabled = false;
                        graphPVLoops.XAxisTitle = "volume";
                        pvGraphScaleOffset = 0;
                        graphPVLoops.Legend1 = "right atrium";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("RA");

                        break;
                    case 4: // left lung
                        graphPVLoops.GraphMaxY = 60;
                        graphPVLoops.GraphMinY = 25;
                        graphPVLoops.GraphMaxX = 20;
                        graphPVLoops.GridXAxisStep = 5;
                        graphPVLoops.GraphMinX = -10;
                        Graph1PVDisabled = false;
                        Graph2PVDisabled = true;
                        Graph3PVDisabled = true;
                        Graph4PVDisabled = true;
                        Graph5PVDisabled = true;
                        graphPVLoops.Graph1Enabled = true;
                        graphPVLoops.Graph2Enabled = false;
                        graphPVLoops.Graph3Enabled = false;
                        graphPVLoops.Graph4Enabled = false;
                        graphPVLoops.Graph5Enabled = false;
                        graphPVLoops.XAxisTitle = "pressure";
                        pvGraphScaleOffset = PhysModelMain.currentModel.Patm;
                        graphPVLoops.Legend1 = "left lung";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALL");

                        break;
                    case 5: // right lung
                        graphPVLoops.GraphMaxY = 60;
                        graphPVLoops.GraphMinY = 25;
                        graphPVLoops.GraphMaxX = 20;
                        graphPVLoops.GraphMinX = 0;
                        graphPVLoops.GridXAxisStep = 5;
                        graphPVLoops.GraphMinX = -10;
                        Graph1PVDisabled = false;
                        Graph2PVDisabled = true;
                        Graph3PVDisabled = true;
                        Graph4PVDisabled = true;
                        Graph5PVDisabled = true;
                        graphPVLoops.Graph1Enabled = true;
                        graphPVLoops.Graph2Enabled = false;
                        graphPVLoops.Graph3Enabled = false;
                        graphPVLoops.Graph4Enabled = false;
                        graphPVLoops.Graph5Enabled = false;
                        graphPVLoops.XAxisTitle = "pressure";
                        pvGraphScaleOffset = PhysModelMain.currentModel.Patm;
                        graphPVLoops.Legend1 = "right lung";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALR");

                        break;

                }

                graphPVLoops.DataRefreshRate = 15;
                graphPVLoops.PixelPerDataPoint = 2;

                graphPVLoops.IsSideScrolling = false;
                graphPVLoops.HideLegends = false;
                graphPressures.HideXAxisLabels = false;

                graphPVLoops.PointMode1 = SKPointMode.Points;
                graphPVLoops.GraphicsClearanceRate = 5000;
                graphPVLoops.DrawGridOnScreen();
            }
     

        }
        private void CmbPVLoopsGraphSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialized)
            {
                SelectPVLoop(cmbPVLoopsGraphSelector.SelectedIndex);
            }
        }

        private void MenuPDA_Click(object sender, RoutedEventArgs e)
        {
            ViewPDAEnabled = !ViewPDAEnabled;
            ModelGraphic.PDAView(ViewPDAEnabled);
        }

        private void MenuOFO_Click(object sender, RoutedEventArgs e)
        {
            ViewOFOEnabled = !ViewOFOEnabled;
            ModelGraphic.OFOView(ViewOFOEnabled);
        }

        private void MenuVSD_Click(object sender, RoutedEventArgs e)
        {
            ViewVSDEnabled = !ViewVSDEnabled;
            ModelGraphic.VSDView(ViewVSDEnabled);
        }

        private void MenuLUNGSHUNT_Click(object sender, RoutedEventArgs e)
        {
            ViewSHUNTEnabled = !ViewSHUNTEnabled;
            ModelGraphic.LUNGSHUNTView(ViewSHUNTEnabled);
        }

        private void MenuMYO_Click(object sender, RoutedEventArgs e)
        {
            ViewMYOEnabled = !ViewMYOEnabled;
            ModelGraphic.MYOView(ViewMYOEnabled);
        }

        private void SliPDASize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (initialized)
            {
             
                PhysModelMain.modelInterface.AdjustPDASize(sliPDASize.Value);
            }
        }
    }
}
