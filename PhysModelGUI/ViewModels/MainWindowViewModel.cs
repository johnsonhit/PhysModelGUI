using PhysModelLibrary;
using PhysModelLibrary.BaseClasses;
using SkiaSharp;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        ObservableCollection<string> _modelLog = new ObservableCollection<string>();
        public ObservableCollection<string> ModelLog { get { return _modelLog; } set { _modelLog = value; OnPropertyChanged(); } }

        int _selectedIndexPressure = 0;
        public int SelectedIndexPressure { get { return _selectedIndexPressure; } set { _selectedIndexPressure = value; SelectPressureGraph(value); OnPropertyChanged(); } }

        int _selectedIndexFlow = 0;
        public int SelectedIndexFlow { get { return _selectedIndexFlow; } set { _selectedIndexFlow = value; SelectFlowGraph(value); OnPropertyChanged(); } }

        int _selectedIndexPVLoop = 0;
        public int SelectedIndexPVLoop { get { return _selectedIndexPVLoop; } set { _selectedIndexPVLoop = value; SelectPVLoopGraph(value); OnPropertyChanged(); } }

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

        bool _pressureGraphEnabled = false;
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

        #region "independent model parameters setters"
        // Breathing model
        public bool SpontaneousBreathing
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.SpontBreathingEnabled : true;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.SpontBreathingEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public double VERef
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.VERef : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.VERef = value;
                    OnPropertyChanged();
                }
            }
        }
        public double VEMax
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.VEMax : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.VEMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public double VtRrRatio
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.VtRrRatio : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.VtRrRatio = value;
                    OnPropertyChanged();
                }
            }
        }
        public double BreathDuration
        { get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.BreathDuration : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.BreathDuration = value;
                    OnPropertyChanged();
                }
            }
        }
        public double BreathDurationRand
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.BreathDurationRand : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.BreathDurationRand = value;
                    OnPropertyChanged();
                }
            }
        }
        public double BreathDepthRand
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.BreathDepthRand : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.BreathDepthRand = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ApnoeOccurence
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.ApnoeOccurence : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.ApnoeOccurence = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ApnoeDuration
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.ApnoeDuration : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.ApnoeDuration = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ApnoeDurationRand
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.ApnoeDurationRand : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.ApnoeDurationRand = value;
                    OnPropertyChanged();
                }
            }
        }
        // ecg model
        public double PQTime
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.PQTime : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.PQTime = value;
                    OnPropertyChanged();
                }
            }
        }
        public double QRSTime
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.QRSTime : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.QRSTime = value;
                    OnPropertyChanged();
                }
            }
        }
        public double QTTime
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.QTTime : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.QTTime = value;
                    OnPropertyChanged();
                }
            }
        }
        public double VentEscapeRate
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.VentEscapeRate : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.VentEscapeRate = value;
                    OnPropertyChanged();
                }
            }
        }
        public double WandPacemakerRate
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.WandPacemakerRate : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.WandPacemakerRate = value;
                    OnPropertyChanged();
                }
            }
        }
        public double PWaveAmp
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.PWaveAmp : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.PWaveAmp = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TWaveAmp
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TWaveAmp : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TWaveAmp = value;
                    OnPropertyChanged();
                }
            }
        }
        public int NoiseLevel
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.NoiseLevel : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.NoiseLevel = value;
                    OnPropertyChanged();
                }
            }
        }
        public int RhythmType
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.RhythmType : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.RhythmType = value;
                    OnPropertyChanged();
                }
            }
        }
        // mechanical ventilator 
        public bool VirtualVentilatorEnabled
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.VirtualVentilatorEnabled : true;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.VirtualVentilatorEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool Vent_VolumeControlled
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Vent_VolumeControlled : true;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.Vent_VolumeControlled = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Vent_PIP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Vent_PIP : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.Vent_PIP = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Vent_PEEP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Vent_PEEP : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.Vent_PEEP = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Vent_InspFlow
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Vent_InspFlow : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.Vent_InspFlow = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Vent_ExpFlow
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Vent_ExpFlow : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.Vent_ExpFlow = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Vent_TargetTidalVolume
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Vent_TargetTidalVolume : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.Vent_TargetTidalVolume = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Vent_TIn
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Vent_TIn : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.Vent_TIn = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Vent_TOut
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Vent_TOut : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.Vent_TOut = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Vent_TriggerVolume
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Vent_TriggerVolume : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.Vent_TriggerVolume = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region "dependent model variable getters"
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
        void NewModel(object p) { }
        void LoadModel(object p) { }
        void SaveModel(object p) { }
        void Exit(object p) { }
        void ShowPDA(object p) { ModelGraphic.PDAView((bool)p); }
        void ShowOFO(object p) { ModelGraphic.OFOView((bool)p); }
        void ShowVSD(object p) { ModelGraphic.VSDView((bool)p); }
        void ShowLUNGSHUNT(object p) { ModelGraphic.LUNGSHUNTView((bool)p); }
        void ShowMYO(object p) { ModelGraphic.MYOView((bool)p); }
        void ClearLog(object p) { ModelLog.Clear(); }

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
                //Temp = PhysModelMain.modelInterface.PatientTemperature.ToString();
                Temp = PhysModelMain.currentModel.VENTIN.PresCurrent.ToString();
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
                        PressureGraph.GraphMaxY = 20;
                        PressureGraph.GraphMinY = -5;
                        PressureGraph.GraphMaxX = 20;
                        pressureGraphScaleOffset = PhysModelMain.currentModel.Patm;

                        PressureGraph.Legend1 = "VENTIN";
                        PressureGraph.Legend2 = "VENTOUT";
                        PressureGraph.Legend3 = "TUBINGIN";
                        PressureGraph.Legend4 = "NCA2";
                        PressureGraph.Legend5 = "TUBINGOUT";
                        PressureGraph.XAxisTitle = "time";

                        selectedPres1Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("VENTIN");
                        selectedPres2Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("VENTOUT");
                        selectedPres3Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("TUBINGIN");
                        selectedPres4Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("NCA2");
                        selectedPres5Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("TUBINGOUT");

                        Graph1PressureDisabled = false;
                        Graph2PressureDisabled = false;
                        Graph3PressureDisabled = false;
                        Graph4PressureDisabled = false;
                        Graph5PressureDisabled = false;

                        PressureGraph.Graph1Enabled = true;
                        PressureGraph.Graph2Enabled = true;
                        PressureGraph.Graph3Enabled = true;
                        PressureGraph.Graph4Enabled = true;
                        PressureGraph.Graph5Enabled = true;

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
                    param1 = selectedFlow1Connector.RealFlow * 60 / 1000;
                    FlowGraph.Graph1Enabled = true;
                }

                if (selectedFlow2Connector == null || Graph2FlowDisabled)
                {
                    FlowGraph.Graph2Enabled = false;
                }
                else
                {
                    param2 = selectedFlow2Connector.RealFlow * 60 / 1000;
                    FlowGraph.Graph2Enabled = true;
                }

                if (selectedFlow3Connector == null || Graph3FlowDisabled)
                {
                    FlowGraph.Graph3Enabled = false;
                }
                else
                {
                    param3 = selectedFlow3Connector.RealFlow * 60 / 1000;
                    FlowGraph.Graph3Enabled = true;
                }

                if (selectedFlow4Connector == null || Graph4FlowDisabled)
                {
                    FlowGraph.Graph4Enabled = false;
                }
                else
                {
                    param4 = selectedFlow4Connector.RealFlow * 60 / 1000;
                    FlowGraph.Graph4Enabled = true;
                }

                if (selectedFlow5Connector == null || Graph5FlowDisabled)
                {
                    FlowGraph.Graph5Enabled = false;
                }
                else
                {
                    param5 = selectedFlow5Connector.RealFlow * 60 / 1000;
                    FlowGraph.Graph5Enabled = true;
                }

                FlowGraph.UpdateRealtimeGraphData(0, param1, 0, param2, 0, param3, 0, param4, 0, param5);
            }
        }
        void SelectFlowGraph(int selection)
        {
            if (initialized && FlowGraph != null)
            {
                selectedFlowID = selection;
                switch (selection)
                {
                    case 0: // left heart
                        FlowGraph.GraphMaxY = 200;
                        FlowGraph.GraphMinY = -50;
                        FlowGraph.GraphMaxX = 20;

                        FlowGraph.Legend1 = "mv";
                        FlowGraph.Legend2 = "av";
                        FlowGraph.Legend3 = "";
                        FlowGraph.Legend4 = "";

                        selectedFlow1Connector = (Connector)PhysModelMain.FindValveByName("LA_LV");
                        selectedFlow2Connector = (Connector)PhysModelMain.FindValveByName("LV_AA");
                        selectedFlow3Connector = null;
                        selectedFlow4Connector = null;
                        selectedFlow5Connector = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = false;
                        Graph3FlowDisabled = true;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;

                        FlowGraph.Graph1Enabled = true;
                        FlowGraph.Graph2Enabled = true;
                        FlowGraph.Graph3Enabled = false;
                        FlowGraph.Graph4Enabled = false;
                        FlowGraph.Graph5Enabled = false;

                        break;
                    case 1: // right heart
                        FlowGraph.GraphMaxY = 200;
                        FlowGraph.GraphMinY = -50;
                        FlowGraph.GraphMaxX = 20;
                        FlowGraph.Legend1 = "tv";
                        FlowGraph.Legend2 = "pv";
                        FlowGraph.Legend3 = "";
                        FlowGraph.Legend4 = "";

                        selectedFlow1Connector = (Connector)PhysModelMain.FindValveByName("RA_RV");
                        selectedFlow2Connector = (Connector)PhysModelMain.FindValveByName("RV_PA");
                        selectedFlow3Connector = null;
                        selectedFlow4Connector = null;
                        selectedFlow5Connector = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = false;
                        Graph3FlowDisabled = true;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;

                        FlowGraph.Graph1Enabled = true;
                        FlowGraph.Graph2Enabled = true;
                        FlowGraph.Graph3Enabled = false;
                        FlowGraph.Graph4Enabled = false;
                        FlowGraph.Graph5Enabled = false;
                        break;
                    case 2: // pulmonary artery
                        FlowGraph.GraphMaxY = 51;
                        FlowGraph.GraphMinY = -50;
                        FlowGraph.GraphMaxX = 20;
                        FlowGraph.Legend1 = "lung l";
                        FlowGraph.Legend2 = "lung r";
                        FlowGraph.Legend3 = "lung shunt";
                        FlowGraph.Legend4 = "";

                        selectedFlow1Connector = (Connector)PhysModelMain.FindBloodConnectorByName("PA_LL");
                        selectedFlow2Connector = (Connector)PhysModelMain.FindBloodConnectorByName("PA_LR");
                        selectedFlow3Connector = (Connector)PhysModelMain.FindBloodConnectorByName("PA_PV");
                        selectedFlow4Connector = null;
                        selectedFlow5Connector = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = false;
                        Graph3FlowDisabled = false;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;

                        FlowGraph.Graph1Enabled = true;
                        FlowGraph.Graph2Enabled = true;
                        FlowGraph.Graph3Enabled = true;
                        FlowGraph.Graph4Enabled = false;
                        FlowGraph.Graph5Enabled = false;
                        break;
                    case 3: // aorta
                        FlowGraph.GraphMaxY = 10;
                        FlowGraph.GraphMinY = -10;
                        FlowGraph.GraphMaxX = 20;
                        FlowGraph.Legend1 = "brain";
                        FlowGraph.Legend2 = "ub";
                        FlowGraph.Legend3 = "lb";
                        FlowGraph.Legend4 = "kidneys";
                        FlowGraph.Legend5 = "liver";

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = false;
                        Graph3FlowDisabled = false;
                        Graph4FlowDisabled = false;
                        Graph5FlowDisabled = false;

                        FlowGraph.Graph1Enabled = true;
                        FlowGraph.Graph2Enabled = true;
                        FlowGraph.Graph3Enabled = true;
                        FlowGraph.Graph4Enabled = true;
                        FlowGraph.Graph5Enabled = true;

                        selectedFlow1Connector = (Connector)PhysModelMain.FindBloodConnectorByName("AA_BRAIN");
                        selectedFlow2Connector = (Connector)PhysModelMain.FindBloodConnectorByName("AA_UB");
                        selectedFlow3Connector = (Connector)PhysModelMain.FindBloodConnectorByName("AD_LB");
                        selectedFlow4Connector = (Connector)PhysModelMain.FindBloodConnectorByName("AD_KIDNEYS");
                        selectedFlow5Connector = (Connector)PhysModelMain.FindBloodConnectorByName("AD_LIVER");
                        break;
                    case 4: // foramen ovale
                        FlowGraph.GraphMaxY = 10;
                        FlowGraph.GraphMinY = -10;
                        FlowGraph.GraphMaxX = 20;
                        FlowGraph.Legend1 = "asd";
                        FlowGraph.Legend2 = "";
                        FlowGraph.Legend3 = "";
                        FlowGraph.Legend4 = "";

                        selectedFlow1Connector = (Connector)PhysModelMain.FindBloodConnectorByName("LA_RA");
                        selectedFlow2Connector = null;
                        selectedFlow3Connector = null;
                        selectedFlow4Connector = null;
                        selectedFlow5Connector = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = true;
                        Graph3FlowDisabled = true;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;

                        FlowGraph.Graph1Enabled = true;
                        FlowGraph.Graph2Enabled = false;
                        FlowGraph.Graph3Enabled = false;
                        FlowGraph.Graph4Enabled = false;
                        FlowGraph.Graph5Enabled = false;
                        break;
                    case 5: // vsd
                        FlowGraph.GraphMaxY = 10;
                        FlowGraph.GraphMinY = -10;
                        FlowGraph.GraphMaxX = 20;
                        FlowGraph.Legend1 = "vsd";
                        FlowGraph.Legend2 = "";
                        FlowGraph.Legend3 = "";
                        FlowGraph.Legend4 = "";


                        selectedFlow1Connector = (Connector)PhysModelMain.FindBloodConnectorByName("LV_RV");
                        selectedFlow2Connector = null;
                        selectedFlow3Connector = null;
                        selectedFlow4Connector = null;
                        selectedFlow5Connector = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = true;
                        Graph3FlowDisabled = true;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;

                        FlowGraph.Graph1Enabled = true;
                        FlowGraph.Graph2Enabled = false;
                        FlowGraph.Graph3Enabled = false;
                        FlowGraph.Graph4Enabled = false;
                        FlowGraph.Graph5Enabled = false;
                        break;

                    case 6: // ductus
                        FlowGraph.GraphMaxY = 10;
                        FlowGraph.GraphMinY = -10;
                        FlowGraph.GraphMaxX = 20;
                        FlowGraph.Legend1 = "ductus";
                        FlowGraph.Legend2 = "";
                        FlowGraph.Legend3 = "";
                        FlowGraph.Legend4 = "";

                        selectedFlow1Connector = (Connector)PhysModelMain.FindBloodConnectorByName("DA_PA");
                        selectedFlow2Connector = null;
                        selectedFlow3Connector = null;
                        selectedFlow4Connector = null;
                        selectedFlow5Connector = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = true;
                        Graph3FlowDisabled = true;
                        Graph4FlowDisabled = true;
                        Graph5FlowDisabled = true;
                        FlowGraph.Graph1Enabled = true;
                        FlowGraph.Graph2Enabled = false;
                        FlowGraph.Graph3Enabled = false;
                        FlowGraph.Graph4Enabled = false;
                        FlowGraph.Graph5Enabled = false;
                        break;
                    case 7: // lungs
                        FlowGraph.GraphMaxY = 20;
                        FlowGraph.GraphMinY = -20;
                        FlowGraph.GraphMaxX = 20;
                        FlowGraph.Legend1 = "vin-tin";
                        FlowGraph.Legend2 = "tin-test";
                        FlowGraph.Legend3 = "test-tout";
                        FlowGraph.Legend4 = "tout-vout";

                        selectedFlow1Connector = (Connector)PhysModelMain.FindGasConnectorByName("VENTIN_TUBINGIN");
                        selectedFlow2Connector = (Connector)PhysModelMain.FindGasConnectorByName("TUBINGIN_TESTLUNG");
                        selectedFlow3Connector = (Connector)PhysModelMain.FindGasConnectorByName("TESTLUNG_TUBINGOUT");
                        selectedFlow4Connector = (Connector)PhysModelMain.FindGasConnectorByName("TUBINGOUT_VENTOUT"); ;
                        selectedFlow5Connector = null;

                        Graph1FlowDisabled = false;
                        Graph2FlowDisabled = false;
                        Graph3FlowDisabled = false;
                        Graph4FlowDisabled = false;
                        Graph5FlowDisabled = true;
                        FlowGraph.Graph1Enabled = true;
                        FlowGraph.Graph2Enabled = true;
                        FlowGraph.Graph3Enabled = true;
                        FlowGraph.Graph4Enabled = true;
                        FlowGraph.Graph5Enabled = false;
                        break;

                }

                FlowGraph.DataRefreshRate = 15;
                FlowGraph.PixelPerDataPoint = 2;
                FlowGraph.IsSideScrolling = true;
                FlowGraph.GraphicsClearanceRate = graphicsRefreshInterval;
                FlowGraph.HideLegends = false;
                FlowGraph.XAxisTitle = "time";
                FlowGraph.DrawGridOnScreen();
            }
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
        void SelectPVLoopGraph(int selection)
        {
            if (initialized)
            {
                selectedPVID = selection;
                switch (selection)
                {
                    case 0: // lv
                        PVLoopGraph.GraphMaxY = 100;
                        PVLoopGraph.GraphMinY = 0;
                        PVLoopGraph.GraphMaxX = 20;
                        PVLoopGraph.GridXAxisStep = 5;
                        PVLoopGraph.GraphMinX = 0;
                        PVLoopGraph.Graph1Enabled = true;
                        PVLoopGraph.Graph2Enabled = false;
                        PVLoopGraph.Graph3Enabled = false;
                        PVLoopGraph.Graph4Enabled = false;
                        PVLoopGraph.Graph5Enabled = false;
                        PVLoopGraph.XAxisTitle = "volume";
                        pvGraphScaleOffset = 0;
                        PVLoopGraph.Legend1 = "left ventricle";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LV");

                        break;
                    case 1: // la
                        PVLoopGraph.GraphMaxY = 100;
                        PVLoopGraph.GraphMinY = 0;
                        PVLoopGraph.GraphMaxX = 20;
                        PVLoopGraph.GridXAxisStep = 5;
                        PVLoopGraph.GraphMinX = 0;
                        PVLoopGraph.Graph1Enabled = true;
                        PVLoopGraph.Graph2Enabled = false;
                        PVLoopGraph.Graph3Enabled = false;
                        PVLoopGraph.Graph4Enabled = false;
                        PVLoopGraph.Graph5Enabled = false;
                        PVLoopGraph.XAxisTitle = "volume";
                        pvGraphScaleOffset = 0;
                        PVLoopGraph.Legend1 = "left atrium";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("LA");

                        break;
                    case 2: // rv
                        PVLoopGraph.GraphMaxY = 100;
                        PVLoopGraph.GraphMinY = 0;
                        PVLoopGraph.GraphMaxX = 20;
                        PVLoopGraph.GridXAxisStep = 5;
                        PVLoopGraph.GraphMinX = 0;
                        Graph1PVDisabled = false;
                        Graph2PVDisabled = true;
                        Graph3PVDisabled = true;
                        Graph4PVDisabled = true;
                        Graph5PVDisabled = true;
                        PVLoopGraph.Graph1Enabled = true;
                        PVLoopGraph.Graph2Enabled = false;
                        PVLoopGraph.Graph3Enabled = false;
                        PVLoopGraph.Graph4Enabled = false;
                        PVLoopGraph.Graph5Enabled = false;
                        PVLoopGraph.XAxisTitle = "volume";
                        pvGraphScaleOffset = 0;
                        PVLoopGraph.Legend1 = "right ventricle";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("RV");

                        break;
                    case 3: // ra
                        PVLoopGraph.GraphMaxY = 100;
                        PVLoopGraph.GraphMinY = 0;
                        PVLoopGraph.GraphMaxX = 20;
                        PVLoopGraph.GridXAxisStep = 5;
                        PVLoopGraph.GraphMinX = 0;
                        Graph1PVDisabled = false;
                        Graph2PVDisabled = true;
                        Graph3PVDisabled = true;
                        Graph4PVDisabled = true;
                        Graph5PVDisabled = true;
                        PVLoopGraph.Graph1Enabled = true;
                        PVLoopGraph.Graph2Enabled = false;
                        PVLoopGraph.Graph3Enabled = false;
                        PVLoopGraph.Graph4Enabled = false;
                        PVLoopGraph.Graph5Enabled = false;
                        PVLoopGraph.XAxisTitle = "volume";
                        pvGraphScaleOffset = 0;
                        PVLoopGraph.Legend1 = "right atrium";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindBloodCompartmentByName("RA");

                        break;
                    case 4: // left lung
                        PVLoopGraph.GraphMaxY = 60;
                        PVLoopGraph.GraphMinY = 25;
                        PVLoopGraph.GraphMaxX = 20;
                        PVLoopGraph.GridXAxisStep = 5;
                        PVLoopGraph.GraphMinX = -10;
                        Graph1PVDisabled = false;
                        Graph2PVDisabled = true;
                        Graph3PVDisabled = true;
                        Graph4PVDisabled = true;
                        Graph5PVDisabled = true;
                        PVLoopGraph.Graph1Enabled = true;
                        PVLoopGraph.Graph2Enabled = false;
                        PVLoopGraph.Graph3Enabled = false;
                        PVLoopGraph.Graph4Enabled = false;
                        PVLoopGraph.Graph5Enabled = false;
                        PVLoopGraph.XAxisTitle = "pressure";
                        pvGraphScaleOffset = PhysModelMain.currentModel.Patm;
                        PVLoopGraph.Legend1 = "left lung";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALL");

                        break;
                    case 5: // right lung
                        PVLoopGraph.GraphMaxY = 60;
                        PVLoopGraph.GraphMinY = 25;
                        PVLoopGraph.GraphMaxX = 20;
                        PVLoopGraph.GraphMinX = 0;
                        PVLoopGraph.GridXAxisStep = 5;
                        PVLoopGraph.GraphMinX = -10;
                        Graph1PVDisabled = false;
                        Graph2PVDisabled = true;
                        Graph3PVDisabled = true;
                        Graph4PVDisabled = true;
                        Graph5PVDisabled = true;
                        PVLoopGraph.Graph1Enabled = true;
                        PVLoopGraph.Graph2Enabled = false;
                        PVLoopGraph.Graph3Enabled = false;
                        PVLoopGraph.Graph4Enabled = false;
                        PVLoopGraph.Graph5Enabled = false;
                        PVLoopGraph.XAxisTitle = "pressure";
                        pvGraphScaleOffset = PhysModelMain.currentModel.Patm;
                        PVLoopGraph.Legend1 = "right lung";
                        selectedPV1Compartment = (Compartment)PhysModelMain.FindGasCompartmentByName("ALR");

                        break;

                }

                PVLoopGraph.DataRefreshRate = 15;
                PVLoopGraph.PixelPerDataPoint = 2;
                PVLoopGraph.IsSideScrolling = false;
                PVLoopGraph.HideLegends = false;
                PVLoopGraph.HideXAxisLabels = false;
                PVLoopGraph.PointMode1 = SKPointMode.Points;
                PVLoopGraph.GraphicsClearanceRate = 5000;
                PVLoopGraph.DrawGridOnScreen();
            }
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

 
        void UpdateBreathingModel()
        {

        }


    }
}
