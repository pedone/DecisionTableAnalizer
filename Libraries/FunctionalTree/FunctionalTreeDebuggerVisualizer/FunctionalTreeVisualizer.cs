using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Diagnostics;
using FunctionalTreeLibrary;

[assembly: DebuggerVisualizer(
    typeof(FunctionalTreeDebuggerVisualizer.FunctionalTreeVisualizer),
    typeof(VisualizerObjectSource),
    Target = typeof(FunctionalTreeLibrary.FunctionalTree),
    Description = "Functional Tree Visualizer")]
namespace FunctionalTreeDebuggerVisualizer
{

    public class FunctionalTreeVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, System.IO.Stream outgoingData)
        {
            
        }
    }

    public class FunctionalTreeVisualizer : DialogDebuggerVisualizer
    {

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            //FunctionalTreeVisualizerWindow visualizerWindow = new FunctionalTreeVisualizerWindow(objectProvider.GetObject() as FunctionalTree);
            FunctionalTreeVisualizerWindow visualizerWindow = new FunctionalTreeVisualizerWindow(null);
            visualizerWindow.ShowDialog();
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            var visualizerHost = new VisualizerDevelopmentHost(
                objectToVisualize,
                typeof(FunctionalTreeVisualizer),
                typeof(VisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }

    }

    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            FunctionalTreeVisualizer.TestShowVisualizer(null);
        }
    }

}
