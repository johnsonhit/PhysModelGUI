using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using Telerik.Windows.Controls.Legend;

namespace PhysModelGUI.Graphics
{
    /// <summary>
    /// Interaction logic for CommonGraph.xaml
    /// </summary>
    public partial class CommonGraph : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ChartDataClass> AsyncData1 { get; set; } = new ObservableCollection<ChartDataClass>();

        public ObservableCollection<ChartDataClass> DefData1 { get; set; } = new ObservableCollection<ChartDataClass>();
        public ObservableCollection<ChartDataClass> AsyncData2 { get; set; } = new ObservableCollection<ChartDataClass>();
        public ObservableCollection<ChartDataClass> AsyncData3 { get; set; } = new ObservableCollection<ChartDataClass>();
        public ObservableCollection<ChartDataClass> AsyncData4 { get; set; } = new ObservableCollection<ChartDataClass>();
        public ObservableCollection<ChartDataClass> AsyncData5 { get; set; } = new ObservableCollection<ChartDataClass>();

        public ChartDataClass currentData1 = new ChartDataClass();
        public ChartDataClass currentData2 = new ChartDataClass();
        public ChartDataClass currentData3 = new ChartDataClass();
        public ChartDataClass currentData4 = new ChartDataClass();
        public ChartDataClass currentData5 = new ChartDataClass();

        private SolidColorBrush series1Color = new SolidColorBrush(Colors.DarkGreen);
        public SolidColorBrush Series1Color
        {
            get { return series1Color; }
            set { series1Color = value; OnPropertyChanged(); }
        }

        private SolidColorBrush series2Color = new SolidColorBrush(Colors.Fuchsia);
        public SolidColorBrush Series2Color
        {
            get { return series2Color; }
            set { series2Color = value; OnPropertyChanged(); }
        }

        private SolidColorBrush series3Color = new SolidColorBrush(Colors.Red);
        public SolidColorBrush Series3Color
        {
            get { return series3Color; }
            set { series3Color = value; OnPropertyChanged(); }
        }

        private SolidColorBrush series4Color = new SolidColorBrush(Colors.Red);
        public SolidColorBrush Series4Color
        {
            get { return series4Color; }
            set { series4Color = value; OnPropertyChanged(); }
        }

        private SolidColorBrush series5Color = new SolidColorBrush(Colors.Black);
        public SolidColorBrush Series5Color
        {
            get { return series5Color; }
            set { series5Color = value; OnPropertyChanged(); }
        }

        private string series1Legend = "heartrate";
        public string Series1Legend
        {
            get { return series1Legend; }
            set { series1Legend = value; OnPropertyChanged(); }
        }

        private string series2Legend = "o2 sat";
        public string Series2Legend
        {
            get { return series2Legend; }
            set { series2Legend = value; OnPropertyChanged(); }
        }

        private string series3Legend = "systole";
        public string Series3Legend
        {
            get { return series3Legend; }
            set { series3Legend = value; OnPropertyChanged(); }
        }

        private string series4Legend = "diastole";
        public string Series4Legend
        {
            get { return series4Legend; }
            set { series4Legend = value; OnPropertyChanged(); }
        }

        private string series5Legend = "resp rate";
        public string Series5Legend
        {
            get { return series5Legend; }
            set { series5Legend = value; OnPropertyChanged(); }
        }

        private int series1StrokeThickness = 1;
        public int Series1StrokeThickness
        {
            get { return series1StrokeThickness; }
            set { series1StrokeThickness = value; OnPropertyChanged(); }
        }

        private int series2StrokeThickness = 1;
        public int Series2StrokeThickness
        {
            get { return series2StrokeThickness; }
            set { series2StrokeThickness = value; OnPropertyChanged(); }
        }

        private int series3StrokeThickness = 1;
        public int Series3StrokeThickness
        {
            get { return series3StrokeThickness; }
            set { series3StrokeThickness = value; OnPropertyChanged(); }
        }

        private int series4StrokeThickness = 1;
        public int Series4StrokeThickness
        {
            get { return series4StrokeThickness; }
            set { series4StrokeThickness = value; OnPropertyChanged(); }
        }

        private int series5StrokeThickness = 1;
        public int Series5StrokeThickness
        {
            get { return series5StrokeThickness; }
            set { series5StrokeThickness = value; OnPropertyChanged(); }
        }

        private double _maxY = 250;
        public double MaxY
        {
            get => _maxY;
            set { _maxY = value; OnPropertyChanged(); }
        }

        private double _minY = 0;
        public double MinY
        {
            get => _minY;
            set { _minY = value; OnPropertyChanged(); }
        }

        private double _gridXStep = 10;
        public double GridXStep
        {
            get => _gridXStep;
            set { _gridXStep = value; OnPropertyChanged(); }
        }
        private double _gridYStep = 10;
        public double GridYStep
        {
            get => _gridYStep;
            set { _gridYStep = value; OnPropertyChanged(); }
        }

        private double _minX = 0;
        public double MinX
        {
            get => _minX;
            set { _minX = value; OnPropertyChanged(); }
        }

        private double _maxX = 300;
        public double MaxX
        {
            get => _maxX;
            set { _maxX = value; OnPropertyChanged(); }
        }

 
        public int BufferSize { get; set; } = 20;           // determine the buffer size in seconds

        private bool _showXLabels = false;
        public bool ShowXLabels
        {
            get => _showXLabels;
            set { _showXLabels = value; OnPropertyChanged(); }
        }

        private bool _showYLabels = true;
        public bool ShowYLabels
        {
            get => _showYLabels;
            set { _showYLabels = value; OnPropertyChanged(); }
        }

        int counter = 0;
        private bool firstRunFlag = true;
        private double runDuration = 0;

        public CommonGraph()
        {
            InitializeComponent();

            DataContext = this;

            InitGraph(120, 180);

        }

        public void InitGraph(double max_x, int buffer)
        {

            BufferSize = buffer;

            MinX = 0;
            MaxX = max_x;

            runDuration = 0;
            firstRunFlag = true;

            // reset the datasources
            AsyncData1 = new ObservableCollection<ChartDataClass>();

            // attach to the graph
            dataSeries1.ItemsSource = AsyncData1;


        }

        public void UpdateData(double data1, double data2)
        {
            //if (counter >= BufferSize )
            //{
            //    counter = 0;
            //}

            //AsyncData1[counter].XValue = data1 + counter * 10;
            //AsyncData1[counter].YValue = data2;

            //DefData1 = new ObservableCollection<ChartDataClass>();
            //DefData1 = AsyncData1;

            //dataSeries1.ItemsSource = DefData1;

            //Console.WriteLine(counter);
            //counter++;


            //update the data
            if (currentData1.Enabled)
            {
                currentData1.XValue = data1;
                currentData1.YValue = data2;
                lock (((ICollection)AsyncData1).SyncRoot)
                {
                    AsyncData1.Add(currentData1);
                }

            }


            // if the buffer is exceed start removing entries so save memory
            if (AsyncData1.Count > BufferSize)
            {
                if (AsyncData1.Count > 0) AsyncData1.RemoveAt(0);

            }

        }


        public class ChartDataClass
        {
            public DateTime TimeValue { get; set; }
            public double XValue { get; set; }
            public double YValue { get; set; }
            public bool Enabled { get; set; } = true;
        }



    }
}
