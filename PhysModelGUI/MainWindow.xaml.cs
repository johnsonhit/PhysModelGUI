using PhysModelGUI.ViewModels;
using PhysModelGUI.Graphics;
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
    public partial class MainWindow : Window
    {

        MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            // declare and assign the viewmodel
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            // send the graphs to the viewmodel
            //viewModel.InitDiagram(canvasDiagram, canvasDiagramSkeleton);
            viewModel.InitPressureGraph(graphPressures);
            viewModel.InitFlowGraph(graphFlows);
            viewModel.InitPVLoopGraph(graphPVLoops);
            viewModel.InitPatMonitor(graphPatMonitor);
            viewModel.InitTestGraph(testGraph);
            viewModel.InitCommomGraph(commonGraph);
        }

    }

   
}
