using System;
using System.IO;
using System.Linq;
using System.Threading;
using log4net;
using Parcs;

namespace QuickSort
{
    public class MainQuickSortModule : MainModule
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainQuickSortModule));
        private static CommandLineOptions _options;
        
        public static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            _options = new CommandLineOptions();

            if (args != null)
            {
                if (!CommandLine.Parser.Default.ParseArguments(args, _options))
                {
                    throw new ArgumentException(string.Format(@"Cannot parse the arguments. Possible usages:{0}",
                        _options.GetUsage()));
                }
            }

            new MainQuickSortModule().RunModule(_options);
            Console.ReadKey();
        }

        public override void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            int[] initialArray;
            try
            {
                initialArray = CustomArray.LoadFromFile(_options.InputFile);
            }
            catch (FileNotFoundException ex)
            {
                Log.Error("File with a given fileName not found, stopping the application...", ex);
                return;
            }
            
            var possibleValues = new[]{ 1, 2, 4, 8, 16, 32 };
            var pointsNum = _options.PointsNum;
            if (!possibleValues.Contains(pointsNum))
            {
                Log.ErrorFormat("Cannot start module with given number of points. Possible usages: {0}", string.Join(" ", possibleValues));
                return;
            }
            Log.InfoFormat("Starting Quick Sort Module on {0} points", pointsNum);
            
            const string moduleName = "QuickSort.QuickSortModule";
            
            var points = new IPoint[pointsNum];
            var channels = new IChannel[pointsNum];
            for (var i = 0; i < pointsNum; ++i)
            {
                points[i] = info.CreatePoint();
                channels[i] = points[i].CreateChannel();
                points[i].ExecuteClass(moduleName);
            }

            var time = DateTime.Now;
            Log.Info("Waiting for a result...");

            var arrayPieces = CustomArray.Split(initialArray, pointsNum).ToArray();
            for (var i = 0; i < arrayPieces.Length; i++)
            {
                channels[i].WriteObject(arrayPieces[i]);
            }
            LogSendingTime(time);
            
            var pieces = channels.Select(c => new Lazy<CustomArray>(c.ReadObject<CustomArray>)).ToArray();
            var resArr = pieces.Aggregate(Array.Empty<int>(), (current, piece) => current.Concat(piece.Value.Values).ToArray())
                .OrderBy(v => v).ToArray();
            
            LogResultFoundTime(time);
            CustomArray.SaveArray(resArr, _options.OutputFile);
        }
        
        private static void LogSendingTime(DateTime time)
        {
            Log.InfoFormat("Sending finished: time = {0}", Math.Round((DateTime.Now - time).TotalSeconds, 3));
        }
        
        private static void LogResultFoundTime(DateTime time)
        {
            Log.InfoFormat(
                "Result found: time = {0}, saving the result to the file {1}",
                Math.Round((DateTime.Now - time).TotalSeconds, 3),
                _options.OutputFile);
        }
    }
}