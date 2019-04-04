using PhysModelLibrary;
using PhysModelLibrary.BaseClasses;
using SkiaSharp;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PhysModelGUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        bool initialized = false;
        bool mainDiagramAnimationEnabled = true;

        DispatcherTimer updateTimer = new DispatcherTimer(DispatcherPriority.Render);
        int slowUpdater = 0;

        List<string> _modelLog = new List<string>();
        List<string> ModelLog { get { return _modelLog; } set { _modelLog = value; OnPropertyChanged(); } }

        int _selectedIndexPressure = 0;
        public int SelectedIndexPressure { get { return _selectedIndexPressure; } set { _selectedIndexPressure = value; SelectPressureGraph(value); OnPropertyChanged(); } }

        int _selectedIndexFlow= 0;
        public int SelectedIndexFlow { get { return _selectedIndexFlow; } set { _selectedIndexFlow = value; SelectFlowGraph(value); OnPropertyChanged(); } }

        int _selectedIndexPVLoop = 0;
        public int SelectedIndexPVLoop { get { return _selectedIndexPVLoop; } set { _selectedIndexPVLoop = value; SelectPVloopGraph(value); OnPropertyChanged(); } }

        SKElement MainDiagram { get; set; }
        SKElement MainDiagramSkeleton { get; set; }

        ParameterGraph PressureGraph { get; set; }
        ParameterGraph FlowGraph { get; set; }
        ParameterGraph PVLoopGraph { get; set; }
        ParameterGraph ElastanceGraph { get; set; }
        ParameterGraph PatMonitorGraph { get; set; }
        ParameterGraph TrendsGraph { get; set; }
       
        bool _patMonitorGraphEnabled = false;
        public bool PatMonitorGraphEnabled { get { return _patMonitorGraphEnabled; } set { _patMonitorGraphEnabled = value; OnPropertyChanged(); } }

        bool _pressureGraphEnabled = true;
        public bool PressureGraphEnabled { get { return _pressureGraphEnabled; } set { _pressureGraphEnabled = value; OnPropertyChanged(); } }

        bool _flowGraphEnabled = false;
        public bool FlowGraphEnabled { get { return _flowGraphEnabled; } set { _flowGraphEnabled = value; OnPropertyChanged(); } }

        bool _pvLoopGraphEnabled = false;
        public bool PVLoopGraphEnabled { get { return _pvLoopGraphEnabled; } set { _pvLoopGraphEnabled = value; OnPropertyChanged(); } }

        double pressureGraphScaleOffset = 0;
        double pvGraphScaleOffset = 0;
        int graphicsRefreshInterval = 15;

        int selectedPressureID = 0;
        int selectedFlowID = 0;
        int selectedPVID = 0;
        int trendGraphSelection = 1;

        Compartment selectedPres1Compartment;
        Compartment selectedPres2Compartment;
        Compartment selectedPres3Compartment;
        Compartment selectedPres4Compartment;
        Compartment selectedPres5Compartment;

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

        Compartment selectedPV1Compartment;
        Compartment selectedPV2Compartment;
        Compartment selectedPV3Compartment;
        Compartment selectedPV4Compartment;
        Compartment selectedPV5Compartment;
        List<PVPoint> PVDataBlock1 = new List<PVPoint>();

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

        Connector selectedFlow1Connector;
        Connector selectedFlow2Connector;
        Connector selectedFlow3Connector;
        Connector selectedFlow4Connector;
        Connector selectedFlow5Connector;

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



        #region "Model dependent variables"
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
        #endregion

        // commands
        public RelayCommand NewModelCommand { get; set; }
        public RelayCommand LoadModelCommand { get; set; }
        public RelayCommand SaveModelCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ShowPDACommand { get; set; }
        public RelayCommand ShowOFOCommand { get; set; }
        public RelayCommand ShowVSDCommand { get; set; }
        public RelayCommand ShowLUNGSHUNTCommand { get; set; }
        public RelayCommand ShowMYOCommand { get; set; }
        public RelayCommand ClearLogCommand { get; set; }

        // command functions
        void NewModel(object p)         { }
        void LoadModel(object p)        { }
        void SaveModel(object p)        { }
        void Exit(object p)             { }
        void ShowPDA(object p)          { ModelGraphic.PDAView((bool)p); }
        void ShowOFO(object p)          { ModelGraphic.OFOView((bool)p); }
        void ShowVSD(object p)          { ModelGraphic.VSDView((bool)p); }
        void ShowLUNGSHUNT(object p)    { ModelGraphic.LUNGSHUNTView((bool)p); }
        void ShowMYO(object p)          { ModelGraphic.MYOView((bool)p); }
        void ClearLog(object p)         { ModelLog.Clear(); }

        private void SetCommands()
        {
            NewModelCommand = new RelayCommand(NewModel);
            LoadModelCommand = new RelayCommand(LoadModel);
            SaveModelCommand = new RelayCommand(SaveModel);
            ExitCommand = new RelayCommand(Exit);

            ShowPDACommand = new RelayCommand(ShowPDA);
            ShowOFOCommand = new RelayCommand(ShowOFO);
            ShowVSDCommand = new RelayCommand(ShowVSD);
            ShowLUNGSHUNTCommand = new RelayCommand(ShowLUNGSHUNT);
            ShowMYOCommand = new RelayCommand(ShowMYO);
            ClearLogCommand = new RelayCommand(ClearLog);
        }

   

        public MainWindowViewModel()
        {
            // 
            SetCommands();

            // get the physiological model initialized and setup a routine to get the data from it
            PhysModelMain.Initialize();
            PhysModelMain.modelInterface.PropertyChanged += ModelInterface_PropertyChanged;
            PhysModelMain.Start();

            ModelGraphic.BuildDiagram();

            updateTimer.Tick += UpdateTimer_Tick; ;
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, graphicsRefreshInterval);
            updateTimer.Start();

            ModelName = PhysModelMain.currentModel.Name;

            initialized = true;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
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
                if (TrendsGraph != null)
                    TrendsGraph.DrawGraphOnScreen();
            }
            slowUpdater += graphicsRefreshInterval;

            if (mainDiagramAnimationEnabled && MainDiagram != null)
                MainDiagram.InvalidateVisual();

            if (PressureGraphEnabled && PressureGraph != null)
                PressureGraph.DrawGraphOnScreen();

            if (FlowGraphEnabled && FlowGraph != null)
                FlowGraph.DrawGraphOnScreen();

            if (PatMonitorGraphEnabled && PatMonitorGraph != null)
                PatMonitorGraph.DrawGraphOnScreen();

            if (PVLoopGraphEnabled && PVLoopGraph != null)
                PVLoopGraph.DrawGraphOnScreen();
        }

        public void InitDiagram(SKElement _main, SKElement _skeleton)
        {
            MainDiagram = _main;
            MainDiagramSkeleton = _skeleton;
            MainDiagram.PaintSurface += MainDiagram_PaintSurface;
            MainDiagramSkeleton.PaintSurface += MainDiagramSkeleton_PaintSurface;
            MainDiagram.InvalidateVisual();
            MainDiagramSkeleton.InvalidateVisual();
        }

        private void MainDiagramSkeleton_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            ModelGraphic.DrawDiagramSkeleton(canvas, e.Info.Width, e.Info.Height);
        }

        private void MainDiagram_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            ModelGraphic.DrawMainDiagram(canvas, e.Info.Width, e.Info.Height);
        }

        public void InitPressureGraph(ParameterGraph p)
        {
            PressureGraph = p;

            PressureGraph.GraphMaxY = 100;
            PressureGraph.GraphMinY = 0;
            PressureGraph.GraphMaxX = 20;
            PressureGraph.DataRefreshRate = 15;
            PressureGraph.PixelPerDataPoint = 2;
            PressureGraph.Graph1Enabled = true;
            PressureGraph.Graph2Enabled = true;
            PressureGraph.Graph3Enabled = true;
            PressureGraph.Graph4Enabled = true;
            PressureGraph.IsSideScrolling = true;
            PressureGraph.GraphicsClearanceRate = graphicsRefreshInterval;
            SelectedIndexPressure = 0;


        }
        void UpdatePressureGraph()
        {
            if (PressureGraphEnabled && PressureGraph != null)
            {
                double param1 = 0;
                double param2 = 0;
                double param3 = 0;
                double param4 = 0;
                double param5 = 0;

                if (selectedPres1Compartment == null || Graph1PressureDisabled)
                {
                    PressureGraph.Graph1Enabled = false;
                }
                else
                {
                    param1 = selectedPres1Compartment.PresCurrent - pressureGraphScaleOffset;
                    PressureGraph.Graph1Enabled = true;
                }

                if (selectedPres2Compartment == null || Graph2PressureDisabled)
                {
                    PressureGraph.Graph2Enabled = false;
                }
                else
                {
                    param2 = selectedPres2Compartment.PresCurrent - pressureGraphScaleOffset;
                    PressureGraph.Graph2Enabled = true;
                }

                if (selectedPres3Compartment == null || Graph3PressureDisabled)
                {
                    PressureGraph.Graph3Enabled = false;
                }
                else
                {
                    param3 = selectedPres3Compartment.PresCurrent - pressureGraphScaleOffset;
                    PressureGraph.Graph3Enabled = true;
                }

                if (selectedPres4Compartment == null || Graph4PressureDisabled)
                {
                    PressureGraph.Graph4Enabled = false;
                }
                else
                {
                    param4 = selectedPres4Compartment.PresCurrent - pressureGraphScaleOffset;
                    PressureGraph.Graph4Enabled = true;
                }

                if (selectedPres5Compartment == null || Graph5PressureDisabled)
                {
                    PressureGraph.Graph5Enabled = false;
                }
                else
                {
                    param5 = selectedPres5Compartment.PresCurrent - pressureGraphScaleOffset;
                    PressureGraph.Graph5Enabled = true;
                }

                PressureGraph.UpdateRealtimeGraphData(0, param1, 0, param2, 0, param3, 0, param4, 0, param5);

            }
        }
        void SelectPressureGraph(int selection)
        {
            if (initialized && PressureGraph != null)
            {
                selectedPressureID = selection;
                switch (selection)
                {
                    case 0: // heart
                        PressureGraph.GraphMaxY = 100;
                        PressureGraph.GraphMinY = 0;
                        PressureGraph.GraphMaxX = 20;
                        PressureGraph.Legend1 = "AA";
                        PressureGraph.Legend2 = "LV";
                        PressureGraph.Legend3 = "PA";
                        PressureGraph.Legend4 = "RV";
                        PressureGraph.XAxisTitle = "time";

                        pressureGraphScaleOffset = 0;

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("AA");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LV");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("PA");
                        selectedPres4Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("RV");
                        selectedPres5Compartment = null;

                        Graph1PressureDisabled = false;
                        Graph2PressureDisabled = false;
                        Graph3PressureDisabled = false;
                        Graph4PressureDisabled = false;
                        Graph5PressureDisabled = true;

                        PressureGraph.Graph1Enabled = true;
                        PressureGraph.Graph2Enabled = true;
                        PressureGraph.Graph3Enabled = true;
                        PressureGraph.Graph4Enabled = true;
                        PressureGraph.Graph5Enabled = false;

                        break;
                    case 1: // left heart
                        PressureGraph.GraphMaxY = 100;
                        PressureGraph.GraphMinY = 0;
                        PressureGraph.GraphMaxX = 20;
                        pressureGraphScaleOffset = 0;

                        PressureGraph.Legend1 = "AA";
                        PressureGraph.Legend2 = "LV";
                        PressureGraph.Legend3 = "LA";

                        PressureGraph.XAxisTitle = "time";

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("AA");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LV");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LA");
                        selectedPres4Compartment = null;
                        selectedPres5Compartment = null;

                        Graph1PressureDisabled = false;
                        Graph2PressureDisabled = false;
                        Graph3PressureDisabled = false;
                        Graph4PressureDisabled = true;
                        Graph5PressureDisabled = true;

                        PressureGraph.Graph1Enabled = true;
                        PressureGraph.Graph2Enabled = true;
                        PressureGraph.Graph3Enabled = true;
                        PressureGraph.Graph4Enabled = false;
                        PressureGraph.Graph5Enabled = false;

                        break;
                    case 2: // right heart
                        PressureGraph.GraphMaxY = 100;
                        PressureGraph.GraphMinY = 0;
                        PressureGraph.GraphMaxX = 20;
                        pressureGraphScaleOffset = 0;

                        PressureGraph.Legend1 = "PA";
                        PressureGraph.Legend2 = "RV";
                        PressureGraph.Legend3 = "RA";

                        PressureGraph.XAxisTitle = "time";

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("PA");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("RV");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("RA");
                        selectedPres4Compartment = null;
                        selectedPres5Compartment = null;

                        Graph1PressureDisabled = false;
                        Graph2PressureDisabled = false;
                        Graph3PressureDisabled = false;
                        Graph4PressureDisabled = true;
                        Graph5PressureDisabled = true;

                        PressureGraph.Graph1Enabled = true;
                        PressureGraph.Graph2Enabled = true;
                        PressureGraph.Graph3Enabled = true;
                        PressureGraph.Graph4Enabled = false;
                        PressureGraph.Graph5Enabled = false;

                        break;
                    case 3: // lungs
                        PressureGraph.GraphMaxY = 15;
                        PressureGraph.GraphMinY = -15;
                        PressureGraph.GraphMaxX = 20;
                        pressureGraphScaleOffset = PhysModelMain.currentModel.Patm;

                        PressureGraph.Legend1 = "OUT";
                        PressureGraph.Legend2 = "NCA";
                        PressureGraph.Legend3 = "ALL";
                        PressureGraph.Legend4 = "ALR";
                        PressureGraph.XAxisTitle = "time";

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("OUT");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("NCA");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALL");
                        selectedPres4Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALR");
                        selectedPres5Compartment = null;

                        Graph1PressureDisabled = false;
                        Graph2PressureDisabled = false;
                        Graph3PressureDisabled = false;
                        Graph4PressureDisabled = false;
                        Graph5PressureDisabled = true;

                        PressureGraph.Graph1Enabled = true;
                        PressureGraph.Graph2Enabled = true;
                        PressureGraph.Graph3Enabled = true;
                        PressureGraph.Graph4Enabled = true;
                        PressureGraph.Graph5Enabled = false;

                        break;
                }

                PressureGraph.DataRefreshRate = 15;
                PressureGraph.PixelPerDataPoint = 2;
                PressureGraph.IsSideScrolling = true;
                PressureGraph.GraphicsClearanceRate = graphicsRefreshInterval;
                PressureGraph.HideXAxisLabels = true;
                PressureGraph.HideLegends = false;
                PressureGraph.DrawGridOnScreen();
            }

        }

        public void InitFlowGraph(ParameterGraph p)
        {
            FlowGraph = p;

            FlowGraph.GraphMaxY = 200;
            FlowGraph.GraphMinY = -50;
            FlowGraph.GraphMaxX = 20;
            FlowGraph.DataRefreshRate = 15;
            FlowGraph.PixelPerDataPoint = 2;
            FlowGraph.Graph1Enabled = true;
            FlowGraph.Graph2Enabled = true;
            FlowGraph.Graph3Enabled = true;
            FlowGraph.Graph4Enabled = true;
            FlowGraph.IsSideScrolling = true;
            FlowGraph.GraphicsClearanceRate = graphicsRefreshInterval;
            FlowGraph.DrawGridOnScreen();
            SelectedIndexFlow = 0;

        }
        void UpdateFlowGraph()
        {
            if (FlowGraphEnabled && FlowGraph != null)
            {
                double param1 = 0;
                double param2 = 0;
                double param3 = 0;
                double param4 = 0;
                double param5 = 0;

                if (selectedFlow1Connector == null || Graph1FlowDisabled)
                {
                    FlowGraph.Graph1Enabled = false;
                }
                else
                {
                    param1 = selectedFlow1Connector.RealFlow;
                    FlowGraph.Graph1Enabled = true;
                }

                if (selectedFlow2Connector == null || Graph2FlowDisabled)
                {
                    FlowGraph.Graph2Enabled = false;
                }
                else
                {
                    param2 = selectedFlow2Connector.RealFlow;
                    FlowGraph.Graph2Enabled = true;
                }

                if (selectedFlow3Connector == null || Graph3FlowDisabled)
                {
                    FlowGraph.Graph3Enabled = false;
                }
                else
                {
                    param3 = selectedFlow3Connector.RealFlow;
                    FlowGraph.Graph3Enabled = true;
                }

                if (selectedFlow4Connector == null || Graph4FlowDisabled)
                {
                    FlowGraph.Graph4Enabled = false;
                }
                else
                {
                    param4 = selectedFlow4Connector.RealFlow;
                    FlowGraph.Graph4Enabled = true;
                }

                if (selectedFlow5Connector == null || Graph5FlowDisabled)
                {
                    FlowGraph.Graph5Enabled = false;
                }
                else
                {
                    param5 = selectedFlow5Connector.RealFlow;
                    FlowGraph.Graph5Enabled = true;
                }

                FlowGraph.UpdateRealtimeGraphData(0, param1, 0, param2, 0, param3, 0, param4, 0, param5);
            }
        }
        void SelectFlowGraph(int selection)
        {

        }

        public void InitPVLoopGraph(ParameterGraph p)
        {
            PVLoopGraph = p;

            PVLoopGraph.GraphMaxY = 100;
            PVLoopGraph.GraphMinY = 0;
            PVLoopGraph.GraphMaxX = 20;
            PVLoopGraph.GridXAxisStep = 5;
            PVLoopGraph.GraphMinX = 0;
            PVLoopGraph.DataRefreshRate = 15;
            PVLoopGraph.PixelPerDataPoint = 2;
            PVLoopGraph.Graph1Enabled = true;
            PVLoopGraph.Graph2Enabled = false;
            PVLoopGraph.Graph3Enabled = false;
            PVLoopGraph.Graph4Enabled = false;
            PVLoopGraph.Graph5Enabled = false;
            PVLoopGraph.IsSideScrolling = false;
            PVLoopGraph.HideLegends = false;
            PVLoopGraph.HideXAxisLabels = false;
            PVLoopGraph.XAxisTitle = "volume";
            PVLoopGraph.PointMode1 = SKPointMode.Points;
            PVLoopGraph.GraphicsClearanceRate = 5000;
            PVLoopGraph.DrawGridOnScreen();
            SelectedIndexPVLoop = 0;

        }
        void UpdatePVLoops()
        {
            if (PVLoopGraphEnabled && PVLoopGraph != null)
            {

                if (selectedPV1Compartment == null || Graph1PVDisabled)
                {
                    PVLoopGraph.Graph1Enabled = false;
                }
                else
                {
                    PVLoopGraph.Graph1Enabled = true;
                    PVDataBlock1 = selectedPV1Compartment.dataCollector.PVDataBlock;
                }


                int counter = 0;
                if (pvGraphScaleOffset > 0)  // then it is a lung compartment and the pv loop is the other way around!
                {
                    lock (PVDataBlock1)
                    {
                        foreach (PVPoint p in PVDataBlock1)
                        {
                            PVLoopGraph.UpdateRealtimeGraphData(p.pressure1 - pvGraphScaleOffset, p.volume1);
                            counter++;
                        }
                    }
                }
                else
                {
                    lock (PVDataBlock1)
                    {
                        foreach (PVPoint p in PVDataBlock1)
                        {
                            PVLoopGraph.UpdateRealtimeGraphData(p.volume1, p.pressure1 - pvGraphScaleOffset);
                            counter++;
                        }
                    }
                }
            }
        }
        void SelectPVloopGraph(int selection)
        {

        }

        public void InitPatMonitor(ParameterGraph p)
        {
            PatMonitorGraph = p;

            PatMonitorGraph.GraphMaxY = 100;
            PatMonitorGraph.GraphMaxX = 20;
            PatMonitorGraph.GraphXOffset = 10;
            PatMonitorGraph.DataRefreshRate = 15;
            PatMonitorGraph.PixelPerDataPoint = 2;
            PatMonitorGraph.Graph1Enabled = true;
            PatMonitorGraph.Graph2Enabled = true;
            PatMonitorGraph.Graph3Enabled = true;
            PatMonitorGraph.Graph4Enabled = true;
            PatMonitorGraph.Graph5Enabled = true;
            PatMonitorGraph.IsSideScrolling = true;
            PatMonitorGraph.XAxisTitle = "";
            PatMonitorGraph.HideYAxisLabels = true;
            PatMonitorGraph.HideXAxisLabels = true;
            PatMonitorGraph.NoGrid = true;
            PatMonitorGraph.GraphPaint1.Color = SKColors.LimeGreen;
            PatMonitorGraph.GraphPaint2.Color = SKColors.Fuchsia;
            PatMonitorGraph.GraphPaint3.Color = SKColors.Red;
            PatMonitorGraph.GraphPaint4.Color = SKColors.White;
            PatMonitorGraph.GraphicsClearanceRate = graphicsRefreshInterval;


        }
        void UpdateMonitorGraph()
        {
            if (PatMonitorGraphEnabled && PatMonitorGraph != null)
            {
                double param1 = PhysModelMain.modelInterface.ECGSignal / 6 + 100;
                double param2 = (PhysModelMain.modelInterface.SPO2POSTSignal - PhysModelMain.currentModel.DA.dataCollector.PresMin) / 4 + 70;
                double param3 = (PhysModelMain.modelInterface.ABPSignal - PhysModelMain.currentModel.AA.dataCollector.PresMin) / 4 + 45;
                double param4 = PhysModelMain.modelInterface.RESPVolumeSignal / 4 + 15;
                double param5 = PhysModelMain.modelInterface.ETCO2Signal / 4 - 10;

                PatMonitorGraph.UpdateRealtimeGraphData(0, param1, 0, param2, 0, param3, 0, param4, 0, param5);
            }

        }

        public void InitTrendsGraph(ParameterGraph p)
        {
            TrendsGraph = p;

            TrendsGraph.GraphMaxY = 150;
            TrendsGraph.GraphMaxX = 20;
            TrendsGraph.DataRefreshRate = 15;
            TrendsGraph.PixelPerDataPoint = 2;
            TrendsGraph.Graph1Enabled = true;
            TrendsGraph.Graph2Enabled = true;
            TrendsGraph.Graph3Enabled = true;
            TrendsGraph.Graph4Enabled = true;
            TrendsGraph.Graph5Enabled = true;
            TrendsGraph.IsSideScrolling = true;
            TrendsGraph.XAxisTitle = "time";
            TrendsGraph.HideYAxisLabels = false;
            TrendsGraph.HideXAxisLabels = false;
            TrendsGraph.NoGrid = false;
            TrendsGraph.Legend1 = "heartrate";
            TrendsGraph.Legend2 = "spo2";
            TrendsGraph.Legend3 = "systole";
            TrendsGraph.Legend4 = "diastole";
            TrendsGraph.Legend5 = "resp rate";
            TrendsGraph.HideLegends = false;
            TrendsGraph.GraphPaint1.Color = SKColors.DarkGreen;
            TrendsGraph.GraphPaint2.Color = SKColors.Fuchsia;
            TrendsGraph.GraphPaint3.Color = SKColors.DarkRed;
            TrendsGraph.GraphPaint4.Color = SKColors.DarkRed;
            TrendsGraph.GraphPaint5.Color = SKColors.Black;
            TrendsGraph.GraphicsClearanceRate = 5000;
            TrendsGraph.BackgroundColor = SKColors.White;
            TrendsGraph.GridLineAxisPaint.Color = SKColors.Black;
            TrendsGraph.GridLinePaint.Color = SKColors.Black;
            TrendsGraph.LegendTextPaint.Color = SKColors.Black;
            TrendsGraph.GridAxisLabelsPaint.Color = SKColors.Black;

        }
        void UpdateTrendGraph1()
        {
            if (TrendsGraph != null)
                TrendsGraph.UpdateRealtimeGraphData(0, PhysModelMain.modelInterface.HeartRate, 0, PhysModelMain.modelInterface.ArterialSO2Pre, 0, PhysModelMain.modelInterface.SystolicSystemicArterialPressure, 0, PhysModelMain.modelInterface.DiastolicSystemicArterialPressure, 0, PhysModelMain.modelInterface.RespiratoryRate);

        }

        public void InitElastanceGraph(ParameterGraph p)
        {
            ElastanceGraph = p;

            ElastanceGraph.DataRefreshRate = 15;
            ElastanceGraph.PixelPerDataPoint = 1;
            ElastanceGraph.Graph1Enabled = true;
            ElastanceGraph.IsSideScrolling = false;
            ElastanceGraph.GraphicsClearanceRate = graphicsRefreshInterval;
            ElastanceGraph.RealTimeDrawing = false;

        }

 

        private void ModelInterface_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
                    // add the status message to the model log
                    ModelLog.Add(PhysModelMain.modelInterface.StatusMessage);
                    break;
            }
        }

        void CalculateElastanceCurve(string _compartmentName)
        {
            //BloodCompartment testComp = new BloodCompartment();
            //BloodCompartment selectedCompartment = PhysModelMain.FindBloodCompartmentByName(_compartmentName);

            //if (selectedCompartment != null)
            //{
            //    testComp.VolCurrent = selectedCompartment.VolCurrent;
            //    testComp.VolU = selectedCompartment.VolU;
            //    testComp.VolUBaseline = selectedCompartment.VolUBaseline;
            //    testComp.VolUBaselineChange = selectedCompartment.VolUBaselineChange;
            //    testComp.elastanceModel.ElBaseline = selectedCompartment.elastanceModel.ElBaseline;
            //    testComp.elastanceModel.ElBaselineChange = selectedCompartment.elastanceModel.ElBaselineChange;
            //    testComp.elastanceModel.ElContractionBaseline = selectedCompartment.elastanceModel.ElContractionBaseline;
            //    testComp.elastanceModel.ElContractionBaselineChange = selectedCompartment.elastanceModel.ElContractionBaselineChange;
            //    testComp.elastanceModel.ElK1 = selectedCompartment.elastanceModel.ElK1;
            //    testComp.elastanceModel.ElK2 = selectedCompartment.elastanceModel.ElK2;
            //    testComp.elastanceModel.ElKMaxVolume = selectedCompartment.elastanceModel.ElKMaxVolume;
            //    testComp.elastanceModel.ElKMinVolume = selectedCompartment.elastanceModel.ElKMinVolume;

            //    if (testComp.elastanceModel.ElContractionBaseline > 0)
            //    {
            //        graphElastance.Graph2Enabled = true;
            //    }
            //    else
            //    {
            //        graphElastance.Graph2Enabled = false;
            //    }

            //    double testVolume = testComp.VolCurrent * 2;

            //    if (testVolume < 10) testVolume = 10;

            //    graphElastance.ClearStaticData();
            //    for (double i = 0; i <= testVolume; i += 0.1)
            //    {
            //        testComp.elastanceModel.ContractionActivation = 0;
            //        testComp.VolCurrent = i;
            //        testComp.UpdateCompartment();
            //        double pres1 = testComp.PresCurrent;

            //        testComp.elastanceModel.ContractionActivation = 1;
            //        testComp.VolCurrent = i;
            //        testComp.UpdateCompartment();
            //        double pres2 = testComp.PresCurrent;

            //        graphElastance.UpdateStaticData(i, pres1, i, pres2);

            //    }
            //    graphElastance.DrawStaticData();

            //}

        }



    }
}
