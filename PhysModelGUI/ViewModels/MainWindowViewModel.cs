﻿using Microsoft.Win32;
using PhysModelGUI.Graphics;
using PhysModelLibrary;
using PhysModelLibrary.BaseClasses;
using PhysModelLibrary.Compartments;
using PhysModelLibrary.Connectors;
using PhysModelLibrary.Models;
using SkiaSharp;
using SkiaSharp.Views.WPF;
using System;
using System.Collections;
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

        public ObservableCollection<ChartDataClass> AsyncData1 { get; set; } = new ObservableCollection<ChartDataClass>();

        bool initialized = false;
        bool mainDiagramAnimationEnabled = true;

        bool apneaTestRunning = false;
        double apneaTestTime = 0;
        double apneaRecoveryTime = 0;
        bool restartedBreathing = false;
        bool messaged = false;
        double lowestAAO2Sat = 101;
        double lowestPOO2Sat = 101;
        double lowestHR = 300;
        private string apneaTestStatus = "Apnea Test (60s)";
        public string ApneaTestStatus
        {
            get { return apneaTestStatus; }
            set { apneaTestStatus = value; OnPropertyChanged(); }
        }
        private string apneaReport = "no results";
        public string ApneaReport
        {
            get { return apneaReport; }
            set { apneaReport = value; OnPropertyChanged(); }
        }


        bool circArrestTestRunning = false;
        double circArrestTestTime = 0;
        double circArrestRecoveryTime = 0;
        bool restartedCirc = false;


        private string circArrestTestStatus = "CircArrest Test (120s)";
        public string CircArrestTestStatus
        {
            get { return circArrestTestStatus; }
            set { circArrestTestStatus = value; OnPropertyChanged(); }
        }

        private string circArrestReport = "no results";
        public string CircArrestReport
        {
            get { return circArrestReport; }
            set { circArrestReport = value; OnPropertyChanged(); }
        }


        DispatcherTimer updateTimer = new DispatcherTimer(DispatcherPriority.Render);
        int slowUpdater = 0;
        int slowUpdater2 = 0;
        

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

        ParameterGraph2 PressureGraph { get; set; }
        ParameterGraph2 FlowGraph { get; set; }
        ParameterGraph2 PVLoopGraph { get; set; }

        ParameterGraph2 ElastanceGraphContainer { get; set; }
        ParameterGraph2 ResistanceGraph { get; set; }
        ParameterGraph2 PatMonitorGraph { get; set; }

        ScrollingGraph TestScrollingGraph { get; set; }


        TimeBasedGraph TestGraph { get; set; }

        LinearGraph TestCommonGraph { get; set; }

        TimeBasedGraph GraphScroller { get; set; }

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

        List<DataPoint> PVDataBlock1 = new List<DataPoint>();

        List<DataPoint> PVDataBlock2 = new List<DataPoint>();

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
        // autonomic nervous system model
        public double ThMAP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.ThMAP : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.ThMAP = value;
                    OnPropertyChanged();
                }
            }
        }
        public double OpMAP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.OpMAP : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.OpMAP = value;
                    OnPropertyChanged();
                }
            }
        }
        public double SaMAP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.SaMAP : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.SaMAP = value;
                    OnPropertyChanged();
                }
            }
        }

        public double ThPO2
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.ThPO2 : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.ThPO2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double OpPO2
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.OpPO2 : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.OpPO2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double SaPO2
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.SaPO2 : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.SaPO2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public double ThPH
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.ThPH : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.ThPH = value;
                    OnPropertyChanged();
                }
            }
        }
        public double OpPH
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.OpPH : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.OpPH = value;
                    OnPropertyChanged();
                }
            }
        }
        public double SaPH
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.SaPH : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.SaPH = value;
                    OnPropertyChanged();
                }
            }
        }

        public double ThPCO2
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.ThPCO2 : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.ThPCO2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double OpPCO2
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.OpPCO2 : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.OpPCO2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double SaPCO2
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.SaPCO2 : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.SaPCO2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public double GPHVE
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.GPHVe : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.GPHVe = value;
                    OnPropertyChanged();
                }
            }
        }
        public double GPHCont
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.GPHCont : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.GPHCont = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TcPHVE
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TcPHVe : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TcPHVe = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TcPHCont
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TcPHCont : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TcPHCont = value;
                    OnPropertyChanged();
                }
            }
        }

        public double GPO2VE
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.GPO2Ve : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.GPO2Ve = value;
                    OnPropertyChanged();
                }
            }
        }
        public double GPO2HP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.GPO2Hp : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.GPO2Hp = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TcPO2VE
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TcPO2Ve : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TcPO2Ve = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TcPO2HP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TcPO2Hp : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TcPO2Hp = value;
                    OnPropertyChanged();
                }
            }
        }
        public double GPCO2VE
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.GPCO2Ve : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.GPCO2Ve = value;
                    OnPropertyChanged();
                }
            }
        }
        public double GPCO2HP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.GPCO2Hp : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.GPCO2Hp = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TcPCO2VE
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TcPCO2Ve : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TcPCO2Ve = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TcPCO2HP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TcPCO2Hp : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TcPCO2Hp = value;
                    OnPropertyChanged();
                }
            }
        }
        public double GMAPHP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.GMAPHp : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.GMAPHp = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TcMAPHP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TcMAPHp : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TcMAPHp = value;
                    OnPropertyChanged();
                }
            }
        }
        public double GMAPCont
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.GMAPCont : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.GMAPCont = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TcMAPCont
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TcMAPCont : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TcMAPCont = value;
                    OnPropertyChanged();
                }
            }
        }
        public double GMAPSVR
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.GMAPRes : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.GMAPRes = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TcMAPSVR
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TcMAPRes : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TcMAPRes = value;
                    OnPropertyChanged();
                }
            }
        }
        public double GMAPVen
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.GMAPVen : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.GMAPVen = value;
                    OnPropertyChanged();
                }
            }
        }
        public double TcMAPVen
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.TcMAPVen : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.TcMAPVen = value;
                    OnPropertyChanged();
                }
            }
        }



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
                    PhysModelMain.modelInterface.SwitchVirtualVentilator(value);
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

        public double VATP
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.VATPBaseline : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.VATPBaseline = value;
                    OnPropertyChanged();
                }
            }
        }

        // oxygen
        public double VO2
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Vo2Baseline : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.Vo2Baseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Hemoglobin
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Hemoglobin : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustHemoglobinConcentration(value);
                    OnPropertyChanged();
                }
            }
        }
        public double DPG
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.DPG_blood : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustDPGConcentration(value);
                    OnPropertyChanged();
                }
            }
        }

        // acid base
        public double RespQ
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.RespQ : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.RespQ = value;
                    OnPropertyChanged();
                }
            }
        }
        public double NaPlasma
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Na_Plasma: 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustSodiumConcentration(value);
                    OnPropertyChanged();
                }
            }
        }
        public double KPlasma
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.K_Plasma : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustPotassiumConcentration(value);
                    OnPropertyChanged();
                }
            }
        }
        public double ClPlasma
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Cl_Plasma : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustChlorideConcentration(value);
                    OnPropertyChanged();
                }
            }
        }
        public double CaPlasma
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Ca_Plasma : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustCalciumConcentration(value);
                    OnPropertyChanged();
                }
            }
        }
        public double MgPlasma
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Mg_Plasma : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustMagnesiumConcentration(value);
                    OnPropertyChanged();
                }
            }
        }
        public double PhosPlasma
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Phos_Plasma : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustPhosphatesConcentration(value);
                    OnPropertyChanged();
                }
            }
        }
        public double AlbPlasma
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Alb_Plasma : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustAlbuminConcentration(value);
                    OnPropertyChanged();
                }
            }
        }
        public double UaPlasma
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Ua_Plasma : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustUnmeasuredAnionsConcentration(value);
                    OnPropertyChanged();
                }
            }
        }
        public double UcPlasma
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.Uc_Plasma : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.modelInterface.AdjustUnmeasuredCationsConcentration(value);
                    OnPropertyChanged();
                }
            }
        }

        public double LactateClearanceRate
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.LactateClearanceRate / PhysModelMain.currentModel.ModelingStepsize : 0;
            }
            set
            {
                if (PhysModelMain.modelInterface != null)
                {
                    PhysModelMain.currentModel.LactateClearanceRate = value * PhysModelMain.currentModel.ModelingStepsize;
                    OnPropertyChanged();
                }
            }
        }



        // lung and chestwall model

        public double Resp_UAR_Insp
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.OUT_NCA.resistance.RForwardBaseline : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.OUT_NCA.resistance.RForwardBaseline = value;
              
                    OnPropertyChanged();
                }
            }
        }
        public double Resp_UAR_Exp
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.OUT_NCA.resistance.RBackwardBaseline : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.OUT_NCA.resistance.RBackwardBaseline = value;
    
                    OnPropertyChanged();
                }
            }
        }
        public double Resp_LARR_Insp
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.NCA_ALR.resistance.RForwardBaseline : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.NCA_ALR.resistance.RForwardBaseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Resp_LARR_Exp
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.NCA_ALR.resistance.RBackwardBaseline : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.NCA_ALR.resistance.RBackwardBaseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Resp_LARL_Insp
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.NCA_ALL.resistance.RForwardBaseline : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.NCA_ALL.resistance.RForwardBaseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Resp_LARL_Exp
        {
            get
            {
                return PhysModelMain.currentModel != null ? PhysModelMain.currentModel.NCA_ALL.resistance.RBackwardBaseline : 0;
            }
            set
            {
                if (PhysModelMain.currentModel != null)
                {
                    PhysModelMain.currentModel.NCA_ALL.resistance.RBackwardBaseline = value;
                    OnPropertyChanged();
                }
            }
        }

        double _lungComplianceChange = 0;
        public double LungComplianceChange
        {
            get
            {
                return _lungComplianceChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {
                    _lungComplianceChange = value;
                    PhysModelMain.modelInterface.AdjustLungCompliance(value);
                  
                    OnPropertyChanged();
                } else
                {
                    _lungComplianceChange = 0;
                }
            }
        }

        double _airwayComplianceChange = 0;
        public double AirwayComplianceChange
        {
            get
            {
                return _airwayComplianceChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {
                    _airwayComplianceChange = value;
                    PhysModelMain.modelInterface.AdjustAirwayCompliance(value);

                    OnPropertyChanged();
                }
                else
                {
                    _airwayComplianceChange = 0;
                }
            }
        }
        double _chestwallComplianceChange = 0;
        public double ChestwallComplianceChange
        {
            get
            {
                return _chestwallComplianceChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {
                    _chestwallComplianceChange = value;
                    PhysModelMain.modelInterface.AdjustChestwallCompliance(value);

                    OnPropertyChanged();
                }
                else
                {
                    _chestwallComplianceChange = 0;
                }
            }
        }

        double _lungDiffCapacity = 0;
        public double LungDiffusionCapacity
        {
            get
            {
                return _lungDiffCapacity;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {
                    _lungDiffCapacity = value;
                    PhysModelMain.modelInterface.AdjustLungDiffusionCapacity(value);

                    OnPropertyChanged();
                }
                else
                {
                    _lungDiffCapacity = 0;
                }
            }
        }

        double _svrChange = 0;
        public double SystemicVascularResistanceChange
        {
            get
            {
                return _svrChange;
            }
            set
            {          
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _svrChange = value;
                    
                    PhysModelMain.modelInterface.AdjustSystemicVascularResistance(value);

                    OnPropertyChanged();
                }
                else
                {
                    _svrChange = 0;
                }
            }
        }

        double _pvrChange = 0;
        public double PulmonaryVascularResistanceChange
        {
            get
            {
                return _pvrChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _pvrChange = value;

                    PhysModelMain.modelInterface.AdjustPulmonaryVascularResistance(value);

                    OnPropertyChanged();
                }
                else
                {
                    _pvrChange = 0;
                }
            }
        }

        double _venPoolChange = 0;
        public double VenousPoolChange
        {
            get
            {
                return _venPoolChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _venPoolChange = value;

                    PhysModelMain.modelInterface.AdjustVenousPool(value);

                    OnPropertyChanged();
                }
                else
                {
                    _venPoolChange = 0;
                }
            }
        }

        double _heartDiastFunction = 0;
        public double HeartDiastolicFunctionChange
        {
            get
            {
                return _heartDiastFunction;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _heartDiastFunction = value;

                    PhysModelMain.modelInterface.AdjustHeartDiastolicFunction(value);

                    OnPropertyChanged();
                }
                else
                {
                    _heartDiastFunction = 0;
                }
            }
        }

        double _heartLeftDiastFunction = 0;
        public double HeartLeftDiastolicFunctionChange
        {
            get
            {
                return _heartLeftDiastFunction;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _heartLeftDiastFunction = value;

                    PhysModelMain.modelInterface.AdjustHeartLeftDiastolicFunction(value);

                    OnPropertyChanged();
                }
                else
                {
                    _heartLeftDiastFunction = 0;
                }
            }
        }
        double _heartRightDiastFunction = 0;
        public double HeartRightDiastolicFunctionChange
        {
            get
            {
                return _heartRightDiastFunction;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _heartRightDiastFunction = value;

                    PhysModelMain.modelInterface.AdjustHeartRightDiastolicFunction(value);

                    OnPropertyChanged();
                }
                else
                {
                    _heartRightDiastFunction = 0;
                }
            }
        }

        double _heartCont = 0;
        public double HeartContractilityChange
        {
            get
            {
                return _heartCont;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _heartCont = value;

                    PhysModelMain.modelInterface.AdjustHeartContractility(value);

                    OnPropertyChanged();
                }
                else
                {
                    _heartCont = 0;
                }
            }
        }

        double _heartLeftCont = 0;
        public double HeartLeftContractilityChange
        {
            get
            {
                return _heartLeftCont;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _heartLeftCont = value;

                    PhysModelMain.modelInterface.AdjustLeftHeartContractility(value);

                    OnPropertyChanged();
                }
                else
                {
                    _heartLeftCont = 0;
                }
            }
        }
        double _heartRightCont = 0;
        public double HeartRightContractilityChange
        {
            get
            {
                return _heartRightCont;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _heartRightCont = value;

                    PhysModelMain.modelInterface.AdjustRightHeartContractility(value);

                    OnPropertyChanged();
                }
                else
                {
                    _heartRightCont = 0;
                }
            }
        }

        double _avValveStenosisChange = 0;
        public double AVStenosisChange
        {
            get
            {
                return _avValveStenosisChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _avValveStenosisChange = value;

                    PhysModelMain.modelInterface.AdjustAVValveStenosis(value);

                    OnPropertyChanged();
                }
                else
                {
                    _avValveStenosisChange = 0;
                }
            }
        }

        double _avValveRegurgitationChange = 0;
        public double AVRegurgitationChange
        {
            get
            {
                return _avValveRegurgitationChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _avValveRegurgitationChange = value;

                    PhysModelMain.modelInterface.AdjustAVValveRegurgitation(value);

                    OnPropertyChanged();
                }
                else
                {
                    _avValveRegurgitationChange = 0;
                }
            }
        }

        double _pvValveStenosisChange = 0;
        public double PVStenosisChange
        {
            get
            {
                return _pvValveStenosisChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _pvValveStenosisChange = value;

                    PhysModelMain.modelInterface.AdjustPVValveStenosis(value);

                    OnPropertyChanged();
                }
                else
                {
                    _pvValveStenosisChange = 0;
                }
            }
        }

        double _pvValveRegurgitationChange = 0;
        public double PVRegurgitationChange
        {
            get
            {
                return _pvValveRegurgitationChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _pvValveRegurgitationChange = value;

                    PhysModelMain.modelInterface.AdjustPVValveRegurgitation(value);

                    OnPropertyChanged();
                }
                else
                {
                    _pvValveRegurgitationChange = 0;
                }
            }
        }

        double _mvValveStenosisChange = 0;
        public double MVStenosisChange
        {
            get
            {
                return _mvValveStenosisChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _mvValveStenosisChange = value;

                    PhysModelMain.modelInterface.AdjustMVValveStenosis(value);

                    OnPropertyChanged();
                }
                else
                {
                    _mvValveStenosisChange = 0;
                }
            }
        }

        double _mvValveRegurgitationChange = 0;
        public double MVRegurgitationChange
        {
            get
            {
                return _mvValveRegurgitationChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _mvValveRegurgitationChange = value;

                    PhysModelMain.modelInterface.AdjustMVValveRegurgitation(value);

                    OnPropertyChanged();
                }
                else
                {
                    _mvValveRegurgitationChange = 0;
                }
            }
        }

        double _tvValveStenosisChange = 0;
        public double TVStenosisChange
        {
            get
            {
                return _tvValveStenosisChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _tvValveStenosisChange = value;

                    PhysModelMain.modelInterface.AdjustTVValveStenosis(value);

                    OnPropertyChanged();
                }
                else
                {
                    _tvValveStenosisChange = 0;
                }
            }
        }

        double _tvValveRegurgitationChange = 0;
        public double TVRegurgitationChange
        {
            get
            {
                return _tvValveRegurgitationChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _tvValveRegurgitationChange = value;

                    PhysModelMain.modelInterface.AdjustTVValveRegurgitation(value);

                    OnPropertyChanged();
                }
                else
                {
                    _tvValveRegurgitationChange = 0;
                }
            }
        }

        double _pericardiumComplianceChange = 0;
        public double PericardiumComplianceChange
        {
            get
            {
                return _pericardiumComplianceChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {
                    _pericardiumComplianceChange = value;
                    PhysModelMain.modelInterface.AdjustPericardialCompliance(value);

                    OnPropertyChanged();
                }
                else
                {
                    _pericardiumComplianceChange = 0;
                }
            }
        }

        double _bloodVolumeChange = 0;
        public double BloodVolumeChange
        {
            get
            {
                return _bloodVolumeChange;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _bloodVolumeChange = value;

                    PhysModelMain.modelInterface.AdjustBloodVolume(value);

                    OnPropertyChanged();
                }
                else
                {
                    _bloodVolumeChange = 0;
                }
            }
        }

        double _pdaSize = 0;
        public double PDASize
        {
            get
            {
                return _pdaSize;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _pdaSize = value;

                    PhysModelMain.modelInterface.AdjustPDASize(value);

                    OnPropertyChanged();
                }
                else
                {
                    _pdaSize = 0;
                }
            }
        }

        double _ofoSize = 0;
        public double OFOSize
        {
            get
            {
                return _ofoSize;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _ofoSize = value;

                    PhysModelMain.modelInterface.AdjustOFOSize(value);

                    OnPropertyChanged();
                }
                else
                {
                    _ofoSize = 0;
                }
            }
        }
        double _vsdSize = 0;
        public double VSDSize
        {
            get
            {
                return _vsdSize;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _vsdSize = value;

                    PhysModelMain.modelInterface.AdjustVSDSize(value);

                    OnPropertyChanged();
                }
                else
                {
                    _vsdSize = 0;
                }
            }
        }

        double _lungShuntSize = 0;
        public double LUNGShuntSize
        {
            get
            {
                return _lungShuntSize;
            }
            set
            {
                if (PhysModelMain.modelInterface != null && !double.IsNaN(value))
                {

                    _lungShuntSize = value;

                    PhysModelMain.modelInterface.AdjustLungShuntSize(value);

                    OnPropertyChanged();
                }
                else
                {
                    _lungShuntSize = 0;
                }
            }
        }

        public ObservableCollection<Compartment> compartments { get; set; } = new ObservableCollection<Compartment>();
        public ObservableCollection<Connector> connectors { get; set; } = new ObservableCollection<Connector>();
        public ObservableCollection<ContainerCompartment> containers { get; set; } = new ObservableCollection<ContainerCompartment>();
        public ObservableCollection<Compartment> containedCompartments { get; set; } = new ObservableCollection<Compartment>();

        public ObservableCollection<GasExchangeBlock> gasexchangeUnits { get; set; } = new ObservableCollection<GasExchangeBlock>();
        public ObservableCollection<string> rhythmTypes { get; set; } = new ObservableCollection<string>();

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

        string _totalVolume = "99";
        public string TotalVolume { get { return _totalVolume; } set { _totalVolume = value; OnPropertyChanged(); } }

        string _myoflow = "99";
        public string Myoflow { get { return _myoflow; } set { _myoflow = value; OnPropertyChanged(); } }

        string _myoO2Index = "-";
        public string MyoO2Index { get { return _myoO2Index; } set { _myoO2Index = value; OnPropertyChanged(); } }

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

        string _vtmax = "-";
        public string VTMax { get { return _vtmax; } set { _vtmax = value; OnPropertyChanged(); } }

        string _vtref = "-";
        public string VTRef { get { return _vtref; } set { _vtref = value; OnPropertyChanged(); } }

        string _tidalvolume = "-";
        public string Tidalvolume { get { return _tidalvolume; } set { _tidalvolume = value; OnPropertyChanged(); } }

        string _tidalvolumeTarget = "-";
        public string TidalvolumeTarget { get { return _tidalvolumeTarget; } set { _tidalvolumeTarget = value; OnPropertyChanged(); } }

        string _minutevolume = "-";
        public string Minutevolume { get { return _minutevolume; } set { _minutevolume = value; OnPropertyChanged(); } }

        string _minutevolumeTarget = "-";
        public string MinutevolumeTarget { get { return _minutevolumeTarget; } set { _minutevolumeTarget = value; OnPropertyChanged(); } }

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

        private string lactateAA;

        public string LactateAA
        {
            get { return lactateAA; }
            set { lactateAA = value; OnPropertyChanged(); }
        }

        private string lactateLB;

        public string LactateLB
        {
            get { return lactateLB; }
            set { lactateLB = value; OnPropertyChanged(); }
        }
        private string lactateUB;

        public string LactateUB
        {
            get { return lactateUB; }
            set { lactateUB = value; OnPropertyChanged(); }
        }
        private string lactateBRAIN;

        public string LactateBRAIN
        {
            get { return lactateBRAIN; }
            set { lactateBRAIN = value; OnPropertyChanged(); }
        }
        private string lactateLIVER;

        public string LactateLIVER
        {
            get { return lactateLIVER; }
            set { lactateLIVER = value; OnPropertyChanged(); }
        }


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
        public RelayCommand SwitchVirtualVentilatorCommand { get; set; }
        public RelayCommand ChangeCompartmentCommand { get; set; }
        public RelayCommand ChangeConnectorCommand { get; set; }
        public RelayCommand ChangeContainerCommand { get; set; }
        public RelayCommand ChangeGexUnitCommand { get; set; }
        public RelayCommand ChangeRhythmCommand { get; set; }

        public RelayCommand SwitchToPaulCommand { get; set; }

        public RelayCommand ToggleArrestCommand { get; set; }
        public RelayCommand ToggleAutoPulseCommand { get; set; }

        public RelayCommand ApneaTestCommand { get; set; }
        public RelayCommand CircArrestTestCommand { get; set; }




        // editing of blood compartment
        Compartment selectedCompartment { get; set; }
        public double UVol
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.VolUBaseline : 0;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.VolUBaseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElBaseline
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.elastanceModel.ElBaseline : 0;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.elastanceModel.ElBaseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElContractionBaseline
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.elastanceModel.ElContractionBaseline : 0;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.elastanceModel.ElContractionBaseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElK1
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.elastanceModel.ElK1 : 0;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.elastanceModel.ElK1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElK2
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.elastanceModel.ElK2 : 0;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.elastanceModel.ElK2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElKMinVolume
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.elastanceModel.ElKMinVolume : 0;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.elastanceModel.ElKMinVolume = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElKMaxVolume
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.elastanceModel.ElKMaxVolume : 0;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.elastanceModel.ElKMaxVolume = value;
                    OnPropertyChanged();
                }
            }
        }
        public double FVO2
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.FVo2 : 0;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.FVo2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsEnabled
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.IsEnabled : false;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.IsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool HasFixedVolume
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.HasFixedVolume : false;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.HasFixedVolume = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool HasFixedComposition
        {
            get
            {
                return selectedCompartment != null ? selectedCompartment.FixedGasComposition : false;
            }
            set
            {
                if (selectedCompartment != null)
                {
                    selectedCompartment.FixedGasComposition = value;
                    OnPropertyChanged();
                }
            }
        }

        // editingh of blood connector
        Connector selectedConnector { get; set; }
        public double ResForward
        {
            get
            {
                return selectedConnector != null ? selectedConnector.resistance.RForwardBaseline : 0;
            }
            set
            {
                if (selectedConnector != null && !double.IsNaN(value) && value != 0)
                {
                    selectedConnector.resistance.RForwardBaseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ResBackward 
        {
            get
            {
                return selectedConnector != null ? selectedConnector.resistance.RBackwardBaseline : 0;
            }
            set
            {
                if (selectedConnector != null && !double.IsNaN(value) && value != 0)
                {
                    selectedConnector.resistance.RBackwardBaseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ResK1
        {
            get
            {
                return selectedConnector != null ? selectedConnector.resistance.RK1 : 0;
            }
            set
            {
                if (selectedConnector != null)
                {
                    selectedConnector.resistance.RK1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ResK2
        {
            get
            {
                return selectedConnector != null ? selectedConnector.resistance.RK2 : 0;
            }
            set
            {
                if (selectedConnector != null)
                {
                    selectedConnector.resistance.RK2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsCoupledRes
        {
            get
            {
                return selectedConnector != null ? selectedConnector.resistance.ResCoupled : false;
            }
            set
            {
                if (selectedConnector != null)
                {
                    selectedConnector.resistance.ResCoupled = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsEnabledRes
        {
            get
            {
                return selectedConnector != null ? selectedConnector.IsEnabled : false;
            }
            set
            {
                if (selectedConnector != null)
                {
                    selectedConnector.IsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool NoBackFlowRes
        {
            get
            {
                return selectedConnector != null ? selectedConnector.NoBackFlow : false;
            }
            set
            {
                if (selectedConnector != null)
                {
                    selectedConnector.NoBackFlow = value;
                    OnPropertyChanged();
                }
            }
        }

        // editing of container compartment
        ContainerCompartment selectedContainer { get; set; }
        public double UVolCont
        {
            get
            {
                return selectedContainer != null ? selectedContainer.VolUBaseline : 0;
            }
            set
            {
                if (selectedContainer != null)
                {
                    selectedContainer.VolUBaseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElBaselineCont
        {
            get
            {
                return selectedContainer != null ? selectedContainer.elastanceModel.ElBaseline : 0;
            }
            set
            {
                if (selectedContainer != null)
                {
                    selectedContainer.elastanceModel.ElBaseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElK1Cont
        {
            get
            {
                return selectedContainer != null ? selectedContainer.elastanceModel.ElK1 : 0;
            }
            set
            {
                if (selectedContainer != null)
                {
                    selectedContainer.elastanceModel.ElK1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElK2Cont
        {
            get
            {
                return selectedContainer != null ? selectedContainer.elastanceModel.ElK2 : 0;
            }
            set
            {
                if (selectedContainer != null)
                {
                    selectedContainer.elastanceModel.ElK2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElKMinVolumeCont
        {
            get
            {
                return selectedContainer != null ? selectedContainer.elastanceModel.ElKMinVolume : 0;
            }
            set
            {
                if (selectedContainer != null)
                {
                    selectedContainer.elastanceModel.ElKMinVolume = value;
                    OnPropertyChanged();
                }
            }
        }
        public double ElKMaxVolumeCont
        {
            get
            {
                return selectedContainer != null ? selectedContainer.elastanceModel.ElKMaxVolume : 0;
            }
            set
            {
                if (selectedContainer != null)
                {
                    selectedContainer.elastanceModel.ElKMaxVolume = value;
                    OnPropertyChanged();
                }
            }
        }

        GasExchangeBlock selectedGexUnit { get; set; }
        public string CompBloodGex
        {
            get
            {
                return selectedGexUnit != null ? selectedGexUnit.CompBlood.Description : "";
            }
            set
            {
                if (selectedGexUnit != null)
                {
                    selectedGexUnit.CompBlood.Description = value;
                    OnPropertyChanged();
                }
            }
        }
        public string CompGasGex
        {
            get
            {
                return selectedGexUnit != null ? selectedGexUnit.CompGas.Description : "";
            }
            set
            {
                if (selectedGexUnit != null)
                {
                    selectedGexUnit.CompGas.Description = value;
                    OnPropertyChanged();
                }
            }
        }
        public double DiffO2Gex
        {
            get
            {
                return selectedGexUnit != null ? selectedGexUnit.DiffCoO2Baseline : 0;
            }
            set
            {
                if (selectedGexUnit != null)
                {
                    selectedGexUnit.DiffCoO2Baseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double DiffCO2Gex
        {
            get
            {
                return selectedGexUnit != null ? selectedGexUnit.DiffCoCo2Baseline : 0;
            }
            set
            {
                if (selectedGexUnit != null)
                {
                    selectedGexUnit.DiffCoCo2Baseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double DiffN2Gex
        {
            get
            {
                return selectedGexUnit != null ? selectedGexUnit.DiffCoN2Baseline : 0;
            }
            set
            {
                if (selectedGexUnit != null)
                {
                    selectedGexUnit.DiffCoN2Baseline = value;
                    OnPropertyChanged();
                }
            }
        }
        public double DiffOtherGex
        {
            get
            {
                return selectedGexUnit != null ? selectedGexUnit.DiffCoOtherBaseline : 0;
            }
            set
            {
                if (selectedGexUnit != null)
                {
                    selectedGexUnit.DiffCoOtherBaseline = value;
                    OnPropertyChanged();
                }
            }
        }

        // command functions
        void ChangeSelectedCompartment(object p)
        {
            selectedCompartment = (Compartment)p;
            UVol = selectedCompartment.VolUBaseline;
            ElBaseline = selectedCompartment.elastanceModel.ElBaseline;
            ElContractionBaseline = selectedCompartment.elastanceModel.ElContractionBaseline;
            ElK1 = selectedCompartment.elastanceModel.ElK1;
            ElK2 = selectedCompartment.elastanceModel.ElK2;
            ElKMinVolume = selectedCompartment.elastanceModel.ElKMinVolume;
            ElKMaxVolume = selectedCompartment.elastanceModel.ElKMaxVolume;
            FVO2 = selectedCompartment.FVo2;
            IsEnabled = selectedCompartment.IsEnabled;
            HasFixedVolume = selectedCompartment.HasFixedVolume;
            HasFixedComposition = selectedCompartment.FixedGasComposition;
        }
        void ChangeConnector(object p)
        {
            selectedConnector = (Connector)p;
            ResForward = selectedConnector.resistance.RForwardBaseline;
            ResBackward = selectedConnector.resistance.RBackwardBaseline;
            ResK1 = selectedConnector.resistance.RK1;
            ResK2 = selectedConnector.resistance.RK2;
            IsCoupledRes = selectedConnector.resistance.ResCoupled;
            IsEnabledRes = selectedConnector.IsEnabled;
            NoBackFlowRes = selectedConnector.NoBackFlow;
        }
        void ChangeContainer(object p)
        {
            selectedContainer = (ContainerCompartment)p;
            UVolCont = selectedContainer.VolUBaseline;
            ElBaselineCont = selectedContainer.elastanceModel.ElBaseline;
            ElK1Cont = selectedContainer.elastanceModel.ElK1;
            ElK2Cont = selectedContainer.elastanceModel.ElK2;
            ElKMinVolumeCont = selectedContainer.elastanceModel.ElKMinVolume;
            ElKMaxVolumeCont = selectedContainer.elastanceModel.ElKMaxVolume;
            containedCompartments.Clear();
            foreach(Compartment c in selectedContainer.bloodCompartments)
            {
                containedCompartments.Add(c);
            }
            foreach (Compartment c in selectedContainer.gasCompartments)
            {
                containedCompartments.Add(c);
            }
        }
        void ChangeGexUnit(object p)
        {
            selectedGexUnit = (GasExchangeBlock)p;
            CompBloodGex = selectedGexUnit.CompBlood.Description;
            CompGasGex = selectedGexUnit.CompGas.Description;
            DiffO2Gex = selectedGexUnit.DiffCoO2Baseline;
            DiffCO2Gex = selectedGexUnit.DiffCoCo2Baseline;
            DiffN2Gex = selectedGexUnit.DiffCoN2Baseline;
            DiffOtherGex = selectedGexUnit.DiffCoOtherBaseline;
        }
        void ChangeRhythm(object p)
        {
            int selection = (int)p;
            PhysModelMain.ecg.ChangeRhythm(selection);
        }
        void NewModel(object p)
        {
            PhysModelMain.modelInterface.NewModel();
        }
        void LoadModel(object p)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML file| *.xml";
            openFileDialog1.Title = "Open Model";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                PhysModelMain.modelInterface.LoadModelState(openFileDialog1.FileName);

                ModelGraphic.BuildDiagram();

                SelectedIndexPressure = 0;
                SelectedIndexFlow = 0;
                SelectedIndexPVLoop = 0;
            }
         
        }
        void SaveModel(object p)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML file|*.xml";
            saveFileDialog1.Title = "Save Model";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                PhysModelMain.modelInterface.SaveModelState(saveFileDialog1.FileName);
            }

               
        }
        void Exit(object p) { }
        void ShowPDA(object p) { ModelGraphic.PDAView((bool)p); }
        void ShowOFO(object p) { ModelGraphic.OFOView((bool)p); }
        void ShowVSD(object p) { ModelGraphic.VSDView((bool)p); }
        void ShowLUNGSHUNT(object p) { ModelGraphic.LUNGSHUNTView((bool)p); }
        void ShowMYO(object p) { ModelGraphic.MYOView((bool)p); }
        void ClearLog(object p) { ModelLog.Clear(); }
        void SwitchVirtualVentilator(object p) { PhysModelMain.modelInterface.SwitchVirtualVentilator((bool)p); }

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
            SwitchVirtualVentilatorCommand = new RelayCommand(SwitchVirtualVentilator);
            ChangeCompartmentCommand = new RelayCommand(ChangeSelectedCompartment);
            ChangeConnectorCommand = new RelayCommand(ChangeConnector);
            ChangeContainerCommand = new RelayCommand(ChangeContainer);
            ChangeGexUnitCommand = new RelayCommand(ChangeGexUnit);
            ChangeRhythmCommand = new RelayCommand(ChangeRhythm);
            SwitchToPaulCommand = new RelayCommand(SwitchToPaul);
            ToggleArrestCommand = new RelayCommand(ToggleArrest);
            ToggleAutoPulseCommand = new RelayCommand(ToggleAutoPulse);
            ApneaTestCommand = new RelayCommand(ApneaTest);
            CircArrestTestCommand = new RelayCommand(CircArrestTest);
        }

        void CircArrestTest(object p)
        {
            circArrestTestRunning = true;
            CircArrestReport = "waiting for results." + System.Environment.NewLine;
            circArrestTestTime = 0;
            circArrestRecoveryTime = 0;
            restartedCirc = false;
            PhysModelMain.modelInterface.StopHeartOutput();
            
        }

        void CircArrestTestRoutine(double circArrestDuration, double hrRecovered)
        {
            if (circArrestTestRunning)
            {
                circArrestTestTime += 1;
                circArrestRecoveryTime += 1;

                if (circArrestTestTime > circArrestDuration)
                {
                    if (restartedCirc == false)
                    {
                        CircArrestTestStatus = "Circulation restarted. Waiting for recovery.";
                        PhysModelMain.modelInterface.RestartHeartOutput();
                        circArrestRecoveryTime = 0;
                        restartedCirc = true;
                    }
                   
                } else
                {
                    CircArrestTestStatus = "Circulation arrest test of " + circArrestDuration + "s. running now at : " + circArrestTestTime + "s.";
                }
            }
        }
        void ApneaTest(object p)
        {
            apneaTestRunning = true;
            ApneaReport = "waiting for results." + System.Environment.NewLine;
            restartedBreathing = false;
            apneaTestTime = 0;
            apneaRecoveryTime = 0;
            messaged = false;
            lowestAAO2Sat = 101;
            lowestPOO2Sat = 101;
            lowestHR = 300;
            PhysModelMain.modelInterface.SwitchSpontaneousBreathing(false);
 

        }
        void ApneaTestRoutine(double apneaDuration, double satLimit)
        {
            if (apneaTestRunning)
            {
                apneaTestTime += 1;
                apneaRecoveryTime += 1;

                

                double satAAO2 = PhysModelMain.currentModel.AA.SO2 * 100;
                double satPOO2 = PhysModelMain.modelInterface.PulseOximeterOutput;
                
                if (satAAO2 < lowestAAO2Sat)
                {
                    lowestAAO2Sat = satAAO2;
                }

                if (satPOO2 < lowestPOO2Sat)
                {
                    lowestPOO2Sat = satPOO2;
                }

                if (PhysModelMain.modelInterface.HeartRate < lowestHR)
                {
                    lowestHR = PhysModelMain.modelInterface.HeartRate;
                }

                if (apneaTestTime > apneaDuration)
                {
                    if (restartedBreathing == false)
                    {
                        PhysModelMain.modelInterface.SwitchSpontaneousBreathing(true);
                        apneaRecoveryTime = 0;
                        restartedBreathing = true;
                        messaged = false;
                        ApneaTestStatus = "Breathing restarted. Waiting for recovery.";
                    }
                    
                } else
                {
                    ApneaTestStatus = "Apnea test of " + apneaDuration + "s. running now at : " + apneaTestTime + "s.";
                }

                if (satAAO2 > satLimit && restartedBreathing && messaged == false)
                {
                    messaged = true;
                    ApneaReport = "";
                    ApneaReport += "Aorta SO2 >" + satLimit + "% in : " + apneaRecoveryTime + " seconds" + System.Environment.NewLine;
                    ApneaReport += "Aorta lowest SO2 : " + Math.Round(lowestAAO2Sat, 0) + "%" + System.Environment.NewLine;
                    ApneaReport += "----------------------------------------------------" + System.Environment.NewLine;
        
                }

                if (satPOO2 > satLimit && restartedBreathing)
                {
                    apneaTestRunning = false;
                    ApneaReport += "Pulse oximeter (left hand) SO2 >" + satLimit + "% in : " + apneaRecoveryTime + " s." + System.Environment.NewLine;
                    ApneaReport += "Pulse oximeter (left hand) lowest SO2 : " + Math.Round(lowestPOO2Sat, 0) + "%" + System.Environment.NewLine;
                    ApneaReport += "----------------------------------------------------" + System.Environment.NewLine;
                    ApneaReport += "Lowest heartrate : " + Math.Round(lowestHR, 0) + " bpm" + System.Environment.NewLine;
                    ApneaReport += "----------------------------------------------------" + System.Environment.NewLine;
                    ApneaReport += "Apnea test ended."; 
                    ApneaTestStatus = "Apnea Test (60s.)";
          
                }

               
            }
        
        }
        void ToggleArrest(object p)
        {
                PhysModelMain.modelInterface.CardiacArrest((bool)p);
        }
        void ToggleAutoPulse(object p)
        {
            PhysModelMain.modelInterface.AutoPulse((bool)p);
        }
        void SwitchToPaul(object p)
        {
            PhysModelMain.modelInterface.SwitchToPaul();
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

            ConstructComponentLists();

        }

        private void ConstructComponentLists()
        {
            foreach(BloodCompartment c in PhysModelMain.currentModel.bloodCompartments)
            {
                compartments.Add(c);
            }
            foreach (GasCompartment c in PhysModelMain.currentModel.gasCompartments)
            {
                compartments.Add(c);
            }
            foreach (Connector c in PhysModelMain.currentModel.bloodCompartmentConnectors)
            {
                connectors.Add(c);
            }
            foreach (Connector c in PhysModelMain.currentModel.gasCompartmentConnectors)
            {
                connectors.Add(c);
            }
            foreach (Connector c in PhysModelMain.currentModel.valveConnectors)
            {
                connectors.Add(c);
            }
            foreach(ContainerCompartment c in PhysModelMain.currentModel.containerCompartments)
            {
                containers.Add(c);
            }
            foreach(GasExchangeBlock c in PhysModelMain.currentModel.gasExchangeBlocks)
            {
                gasexchangeUnits.Add(c);
            }

            rhythmTypes.Add("SINUS");
            rhythmTypes.Add("PAC");
            rhythmTypes.Add("PVC");
            rhythmTypes.Add("AVBLOCK1");
            rhythmTypes.Add("AVBLOCK2a");
            rhythmTypes.Add("AVBLOCK2b");
            rhythmTypes.Add("AVBLOCK Complete");
            rhythmTypes.Add("VTOUTPUT");
            rhythmTypes.Add("VF");
            rhythmTypes.Add("LONGQT");
            rhythmTypes.Add("WPW");
            rhythmTypes.Add("SVT");

        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {

            TestScrollingGraph.DrawGraph();

            if (slowUpdater2 > 1000)
            {
                slowUpdater2 = 0;
                TestCommonGraph.DrawGraph();
               
            }
            slowUpdater2 += graphicsRefreshInterval;

            if (slowUpdater > 1000)
            {
              

                ApneaTestRoutine(60, 92);
                CircArrestTestRoutine(120, 120);

                //Console.WriteLine(PhysModelMain.currentModel.LEFTARM.dataCollector.PresMax);

                slowUpdater = 0;
                Heartrate = PhysModelMain.modelInterface.HeartRate.ToString();
                Spo2 = PhysModelMain.modelInterface.PulseOximeterOutput.ToString();
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
                MyoO2Index = Math.Round(PhysModelMain.modelInterface.Mii, 3).ToString();
                Myocardialdo2 = PhysModelMain.modelInterface.MyoO2Delivery.ToString();
                Braindo2 = Math.Round(PhysModelMain.modelInterface.BrainO2Delivery, 1).ToString();
                Kidneysflow = PhysModelMain.modelInterface.KidneysFlow.ToString();
                Liverflow = PhysModelMain.modelInterface.LiverFlow.ToString();
                Brainflow = PhysModelMain.modelInterface.BrainFlow.ToString();

                VTRef = Math.Round(PhysModelMain.currentModel.VERef,0).ToString();
                VTMax = Math.Round(PhysModelMain.currentModel.VEMax,0).ToString();
                Tidalvolume = PhysModelMain.modelInterface.TidalVolume.ToString();
                TidalvolumeTarget = PhysModelMain.modelInterface.TidalVolumeTarget.ToString();
                Minutevolume = PhysModelMain.modelInterface.MinuteVolume.ToString();
                MinutevolumeTarget = PhysModelMain.modelInterface.MinuteVolumeTarget.ToString();
                Alveolarvolume = PhysModelMain.modelInterface.AlveolarVolume;
                //TotalVolume = PhysModelMain.modelInterface.TotalBloodVolume().ToString();

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
                LactateAA = Math.Round(PhysModelMain.currentModel.AA.Lact, 1).ToString();
                LactateUB = Math.Round(PhysModelMain.currentModel.UB.Lact, 1).ToString();
                LactateLB = Math.Round(PhysModelMain.currentModel.LB.Lact, 1).ToString();
                LactateBRAIN = Math.Round(PhysModelMain.currentModel.BRAIN.Lact, 1).ToString();
                LactateLIVER = Math.Round(PhysModelMain.currentModel.LIVER.Lact, 1).ToString();

                Endtidalco2 = PhysModelMain.modelInterface.EndTidalCO2.ToString();

                UpdateTestGraph();
      
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

            if (GraphScroller != null)
                GraphScroller.UpdateData(PhysModelMain.modelInterface.LVPSignal, PhysModelMain.modelInterface.RVPSignal);

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

                    UpdateTestCommonGraph();
                    UpdateScrollingGraph();


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
        public void InitScrollingGraph(ScrollingGraph p)
        {
            TestScrollingGraph = p;
            TestScrollingGraph.InitGraph(250);
            TestScrollingGraph.ShowXLabels = false;
            TestScrollingGraph.Data1Enabled = true;
            TestScrollingGraph.Data2Enabled = true;
            TestScrollingGraph.Data3Enabled = false;

        }
        public void UpdateScrollingGraph()
        {
            if (TestScrollingGraph != null)
            {
                TestScrollingGraph.UpdateData(PhysModelMain.modelInterface.ECGSignal / 5, PhysModelMain.modelInterface.ABPSignal, 0, 0);
            }
        }
        public void InitCommomGraph(LinearGraph p)
        {
            TestCommonGraph = p;

            TestCommonGraph.InitGraph(4000);
            TestCommonGraph.ShowXLabels = true;

        }
        public void InitTestGraph(TimeBasedGraph p)
        {
            TestGraph = p;

            TestGraph.InitGraph(300, 400);
            TestGraph.GridXStep = 60;
            TestGraph.GridYStep = 25;
            TestGraph.MaxY = 200;
            TestGraph.ShowXLabels = true;
            TestGraph.ShowYLabels = true;

        }

        void UpdateTestCommonGraph()
        {
            if (TestCommonGraph != null)
            {
                PVDataBlock2 = PhysModelMain.currentModel.LV.dataCollector.PVDataBlock;
                lock(PVDataBlock2)
                {
                    TestCommonGraph.UpdateDataBlock(PVDataBlock2);
                }


                
       
            }
        }
        void UpdateTestGraph()
        {
            if (TestGraph != null)
            {

                TestGraph.UpdateData(PhysModelMain.modelInterface.HeartRate,PhysModelMain.modelInterface.PulseOximeterOutput, PhysModelMain.modelInterface.SystolicSystemicArterialPressure, PhysModelMain.modelInterface.DiastolicSystemicArterialPressure, PhysModelMain.modelInterface.RespiratoryRate);

            }
        }
        public void InitPressureGraph(ParameterGraph2 p)
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

                        selectedPres1Compartment = PhysModelMain.currentModel.AA;
                        selectedPres2Compartment = PhysModelMain.currentModel.LV;
                        selectedPres3Compartment = PhysModelMain.currentModel.PA;
                        selectedPres4Compartment = PhysModelMain.currentModel.RV;
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

                        selectedPres1Compartment = PhysModelMain.currentModel.AA;
                        selectedPres2Compartment = PhysModelMain.currentModel.LV;
                        selectedPres3Compartment = PhysModelMain.currentModel.LA;
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

                        selectedPres1Compartment = PhysModelMain.currentModel.PA;
                        selectedPres2Compartment = PhysModelMain.currentModel.RV;
                        selectedPres3Compartment = PhysModelMain.currentModel.RA;
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

                        PressureGraph.Legend1 = "TUBINGIN";
                        PressureGraph.Legend2 = "TUBINGOUT";
                        PressureGraph.Legend3 = "YPIECE";
                        PressureGraph.Legend4 = "ALL";
                        PressureGraph.Legend5 = "ALR";
                        PressureGraph.XAxisTitle = "time";

                        selectedPres1Compartment = PhysModelMain.currentModel.TUBINGIN;
                        selectedPres2Compartment = PhysModelMain.currentModel.TUBINGOUT;
                        selectedPres3Compartment = PhysModelMain.currentModel.YPIECE;
                        selectedPres4Compartment = PhysModelMain.currentModel.ALL;
                        selectedPres5Compartment = PhysModelMain.currentModel.ALR;

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
        public void InitFlowGraph(ParameterGraph2 p)
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
                        FlowGraph.GraphMaxY = 5;
                        FlowGraph.GraphMinY = -3;
                        FlowGraph.GraphMaxX = 20;

                        FlowGraph.Legend1 = "mv";
                        FlowGraph.Legend2 = "av";
                        FlowGraph.Legend3 = "";
                        FlowGraph.Legend4 = "";

                        selectedFlow1Connector = PhysModelMain.currentModel.LA_LV;
                        selectedFlow2Connector = PhysModelMain.currentModel.LV_AA;
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
                        FlowGraph.GraphMaxY = 5;
                        FlowGraph.GraphMinY = -3;
                        FlowGraph.GraphMaxX = 20;
                        FlowGraph.Legend1 = "tv";
                        FlowGraph.Legend2 = "pv";
                        FlowGraph.Legend3 = "";
                        FlowGraph.Legend4 = "";

                        selectedFlow1Connector = PhysModelMain.currentModel.RA_RV;
                        selectedFlow2Connector = PhysModelMain.currentModel.RV_PA;
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

                        selectedFlow1Connector = PhysModelMain.currentModel.PA_LL;
                        selectedFlow2Connector = PhysModelMain.currentModel.PA_LR;
                        selectedFlow3Connector = PhysModelMain.currentModel.PA_PV;
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

                        selectedFlow1Connector = PhysModelMain.currentModel.AA_BRAIN;
                        selectedFlow2Connector = PhysModelMain.currentModel.AA_UB;
                        selectedFlow3Connector = PhysModelMain.currentModel.AD_LB;
                        selectedFlow4Connector = PhysModelMain.currentModel.AD_KIDNEYS;
                        selectedFlow5Connector = PhysModelMain.currentModel.AD_LIVER;
                        break;
                    case 4: // foramen ovale
                        FlowGraph.GraphMaxY = 10;
                        FlowGraph.GraphMinY = -10;
                        FlowGraph.GraphMaxX = 20;
                        FlowGraph.Legend1 = "asd";
                        FlowGraph.Legend2 = "";
                        FlowGraph.Legend3 = "";
                        FlowGraph.Legend4 = "";

                        selectedFlow1Connector = PhysModelMain.currentModel.LA_RA;
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


                        selectedFlow1Connector = PhysModelMain.currentModel.LV_RV;
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

                        selectedFlow1Connector = PhysModelMain.currentModel.DA_PA;
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
                        FlowGraph.GraphMaxY = 10;
                        FlowGraph.GraphMinY = -10;
                        FlowGraph.GraphMaxX = 20;
                        FlowGraph.Legend1 = "NCA-ALL";
                        FlowGraph.Legend2 = "NCA-ALR";
                        FlowGraph.Legend3 = "";
                        FlowGraph.Legend4 = "";
                        FlowGraph.Legend5 = "";

                        selectedFlow1Connector = PhysModelMain.currentModel.NCA_ALL;
                        selectedFlow2Connector = PhysModelMain.currentModel.NCA_ALR;
                        selectedFlow3Connector = null;
                        selectedFlow4Connector = null ;
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

        public void InitPVLoopGraph(ParameterGraph2 p)
        {
            PVLoopGraph = p;

            PVLoopGraph.GraphMaxY = 20;
            PVLoopGraph.GraphMinY = -20;
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
                    PVDataBlock1 = PhysModelMain.ventilator.PVDataBlock;
                    lock (PVDataBlock1)
                    {
                        foreach (DataPoint p in PVDataBlock1)
                        {
                            PVLoopGraph.UpdateRealtimeGraphData(p.X - pvGraphScaleOffset, p.Y);
                            counter++;
                        }
                    }
                }
                else
                {
                    lock (PVDataBlock1)
                    {
                        foreach (DataPoint p in PVDataBlock1)
                        {
                            PVLoopGraph.UpdateRealtimeGraphData(p.Y, p.X - pvGraphScaleOffset);
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
                        selectedPV1Compartment = PhysModelMain.currentModel.LV;

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
                        selectedPV1Compartment = PhysModelMain.currentModel.LA;

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
                        selectedPV1Compartment = PhysModelMain.currentModel.RV;

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
                        selectedPV1Compartment = PhysModelMain.currentModel.RA;

                        break;
                    case 4: // NCA
                        PVLoopGraph.GraphMaxY = 20;
                        PVLoopGraph.GraphMinY = -20;
                        PVLoopGraph.GraphMaxX = 20;
                        PVLoopGraph.GridXAxisStep = 5;
                        PVLoopGraph.GraphMinX = -10;
                        PVLoopGraph.PointMode1 = SKPointMode.Lines;
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
                        PVLoopGraph.Legend1 = "airways";
                        selectedPV1Compartment = PhysModelMain.currentModel.NCA;
                        break;
                    case 5: // left lung
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
                        selectedPV1Compartment = PhysModelMain.currentModel.ALL;

                        break;
                    case 6: // right lung
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
                        selectedPV1Compartment = PhysModelMain.currentModel.ALR;

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

        


        public void InitPatMonitor(ParameterGraph2 p)
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
            PatMonitorGraph.GraphPaint5.Color = SKColors.DarkOrange;
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





    }

    public class ChartDataClass
    {
        public DateTime XValue { get; set; }
        public double YValue { get; set; }
    }

}
