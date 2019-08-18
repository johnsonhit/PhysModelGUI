using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Controls.Legend;
using PhysModelLibrary;
using System.Threading.Tasks;

namespace PhysModelGUI
{
    /// <summary>
    /// Interaction logic for ScrollingGraph.xaml
    /// </summary>
    public partial class ScrollingGraph : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public double[] dataSource;

        public bool Data1Enabled { get; set; } = true;
        public bool Data2Enabled { get; set; } = false;
        public bool Data3Enabled { get; set; } = false;
        public bool Data4Enabled { get; set; } = false;

        public List<double> data1 = new List<double>();
        public List<double> data2 = new List<double>();
        public List<double> data3 = new List<double>();
        public List<double> data4 = new List<double>();

        Double[] AsyncData1;
        Double[] AsyncData2;
        Double[] AsyncData3;
        Double[] AsyncData4;



        private SolidColorBrush series1Color = new SolidColorBrush(Colors.DarkGreen);
        public SolidColorBrush Series1Color
        {
            get { return series1Color; }
            set { series1Color = value; OnPropertyChanged(); }
        }

        private string series1Legend = "ecg";
        public string Series1Legend
        {
            get { return series1Legend; }
            set { series1Legend = value; OnPropertyChanged(); }
        }

        private int series1StrokeThickness = 1;
        public int Series1StrokeThickness
        {
            get { return series1StrokeThickness; }
            set { series1StrokeThickness = value; OnPropertyChanged(); }
        }

        private SolidColorBrush series2Color = new SolidColorBrush(Colors.DarkGreen);
        public SolidColorBrush Series2Color
        {
            get { return series2Color; }
            set { series2Color = value; OnPropertyChanged(); }
        }

        private string series2Legend = "ecg";
        public string Series2Legend
        {
            get { return series2Legend; }
            set { series2Legend = value; OnPropertyChanged(); }
        }

        private int series2StrokeThickness = 1;
        public int Series2StrokeThickness
        {
            get { return series2StrokeThickness; }
            set { series2StrokeThickness = value; OnPropertyChanged(); }
        }

        private SolidColorBrush series3Color = new SolidColorBrush(Colors.DarkGreen);
        public SolidColorBrush Series3Color
        {
            get { return series3Color; }
            set { series3Color = value; OnPropertyChanged(); }
        }

        private string series3Legend = "ecg";
        public string Series3Legend
        {
            get { return series3Legend; }
            set { series3Legend = value; OnPropertyChanged(); }
        }

        private int series3StrokeThickness = 1;
        public int Series3StrokeThickness
        {
            get { return series3StrokeThickness; }
            set { series3StrokeThickness = value; OnPropertyChanged(); }
        }

        private SolidColorBrush series4Color = new SolidColorBrush(Colors.DarkGreen);
        public SolidColorBrush Series4Color
        {
            get { return series4Color; }
            set { series4Color = value; OnPropertyChanged(); }
        }

        private string series4Legend = "ecg";
        public string Series4Legend
        {
            get { return series4Legend; }
            set { series4Legend = value; OnPropertyChanged(); }
        }

        private int series4StrokeThickness = 1;
        public int Series4StrokeThickness
        {
            get { return series4StrokeThickness; }
            set { series4StrokeThickness = value; OnPropertyChanged(); }
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

        private double _gridYStep = 10;
        public double GridYStep
        {
            get => _gridYStep;
            set { _gridYStep = value; OnPropertyChanged(); }
        }

        private bool _showYLabels = true;
        public bool ShowYLabels
        {
            get => _showYLabels;
            set { _showYLabels = value; OnPropertyChanged(); }
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
        private double _gridXStep = 10;
        public double GridXStep
        {
            get => _gridXStep;
            set { _gridXStep = value; OnPropertyChanged(); }
        }
        private bool _showXLabels = true;
        public bool ShowXLabels
        {
            get => _showXLabels;
            set { _showXLabels = value; OnPropertyChanged(); }
        }

        public int BufferSize { get; set; } = 200;           // determine the buffer size off 200 datapoints

        public ScrollingGraph()
        {
            InitializeComponent();

            DataContext = this;

        }

        public void InitGraph(int buffer)
        {
            BufferSize = buffer;
        }
        public async Task DrawGraph()
        {
            if (Data1Enabled)
            {
                AsyncData1 = new Double[data1.Count];
                AsyncData1 = data1.ToArray();
                dataSeries1.ItemsSource = AsyncData1;
            }
           
            if (Data2Enabled)
            {
                AsyncData2 = new Double[data2.Count];
                AsyncData2 = data2.ToArray();
                dataSeries2.ItemsSource = AsyncData2;
            }
           
            if (Data3Enabled)
            {
                AsyncData3 = new Double[data3.Count];
                AsyncData3 = data3.ToArray();
                dataSeries3.ItemsSource = AsyncData3;
            }
            
            if (Data4Enabled)
            {
                AsyncData4 = new Double[data4.Count];
                AsyncData4 = data4.ToArray();
                dataSeries4.ItemsSource = AsyncData4;
            }
           

        }
        public void UpdateData(double d1, double d2 = 0, double d3 = 0, double d4 = 0)
        {
            if (Data1Enabled) data1.Add(d1);
            if (Data2Enabled) data2.Add(d2);
            if (Data3Enabled) data3.Add(d3);
            if (Data4Enabled) data4.Add(d4);

            if (data1.Count > BufferSize)
            {
                data1.RemoveAt(0);
            }

            if (data2.Count > BufferSize)
            {
                data2.RemoveAt(0);
            }

            if (data3.Count > BufferSize)
            {
                data3.RemoveAt(0);
            }

            if (data4.Count > BufferSize)
            {
                data4.RemoveAt(0);
            }


        }


    }
}

