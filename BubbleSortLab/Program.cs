using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

namespace BubbleSortLab
{
    class Program
    {
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            var experiment = new XmlDocument();
            experiment.Load(@"experiments.xml");
            var arithmetic = experiment.SelectSingleNode(@"//experiments/experiment/nodes[@name='Arithmetic Progression']");
            var geometric = experiment.SelectSingleNode(@"//experiments/experiment/nodes[@name='Geometric Progression']");
            MakeExperiment(arithmetic);
            MakeExperiment(geometric);
            Console.ReadKey(true);
        }

        static void MakeExperiment(XmlNode experimentData)
        {
            var name = experimentData.Attributes.GetNamedItem("name").Value;
            if (name.Contains("Arithmetic"))
                ArithmeticProgressionExperiment(experimentData);
            if (name.Contains("Geometric"))
                GeometricProgressionExperiment(experimentData);
        }

        static void ArithmeticProgressionExperiment(XmlNode experimentData)
        {
            var name = experimentData.Attributes.GetNamedItem("name").Value;
            var rndMin = int.Parse(experimentData.Attributes.GetNamedItem("minElement").Value);
            var rndMax = int.Parse(experimentData.Attributes.GetNamedItem("maxElement").Value);
            var diff = int.Parse(experimentData.Attributes.GetNamedItem("diff").Value);
            var start = int.Parse(experimentData.Attributes.GetNamedItem("startLength").Value);
            var end = int.Parse(experimentData.Attributes.GetNamedItem("maxLength").Value);
            var repeat = int.Parse(experimentData.Attributes.GetNamedItem("repeat").Value);
            var data = new List<Tuple<int, int>>();
            for (int i = start; i < end; i += diff)
            {
                var sum = 0;
                for (int j = 0; j < repeat; j++)
                {
                    var array = CreateRandomArray(i, rndMin, rndMax);
                    var iterations = 0;
                    BubbleSort(array, out iterations);
                    sum += iterations;
                }
                data.Add(new Tuple<int, int>(i, sum / repeat));
                Console.WriteLine(sum / repeat);
            }
            SaveToCsv(name, data);
        }

        static void GeometricProgressionExperiment(XmlNode experimentData)
        {
            var name = experimentData.Attributes.GetNamedItem("name").Value;
            var rndMin = int.Parse(experimentData.Attributes.GetNamedItem("minElement").Value);
            var rndMax = int.Parse(experimentData.Attributes.GetNamedItem("maxElement").Value);
            var mult = double.Parse(experimentData.Attributes.GetNamedItem("Znamen").Value,
                                                System.Globalization.CultureInfo.InvariantCulture);
            var start = int.Parse(experimentData.Attributes.GetNamedItem("startLength").Value);
            var end = int.Parse(experimentData.Attributes.GetNamedItem("maxLength").Value);
            var repeat = int.Parse(experimentData.Attributes.GetNamedItem("repeat").Value);
            var data = new List<Tuple<int, int>>();
            for (double i = start; i < end; i *= mult)
            {
                var sum = 0;
                for (int j = 0; j < repeat; j++)
                {
                    var array = CreateRandomArray((int)i, rndMin, rndMax);
                    var iterations = 0;
                    BubbleSort(array, out iterations);
                    sum += iterations;
                }
                data.Add(new Tuple<int, int>((int)i, sum / repeat));
                Console.WriteLine(sum / repeat);
            }
            SaveToCsv(name, data);
        }

        static void SaveToCsv(string name, List<Tuple<int, int>> data)
        {
            var f = File.AppendText($"{name}.csv");
            for (int i = 0; i < data.Count; i++)
                f.WriteLine($"{data[i].Item1};{data[i].Item2}");
            f.Flush();
            f.Close();
            f.Dispose();
        }

        static string[] CreateRandomArray(int length, int min, int max)
        {
            var result = new string[length];
            for (int i = 0; i < length; i++) 
            {
                var strLength = rnd.Next(min, max);
                result[i] = new String(Enumerable.Range(0, strLength).Select((a) => (char)rnd.Next('a', 'z')).ToArray());
            }
            return result;
        }

        static string[] BubbleSort(string[] array, out int interations)
        {
            interations = 0;
            var result = array.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length - 1; j++)
                {
                    if (array[j].CompareTo(array[j + 1]) > 0)
                    {
                        var tmp = array[j];
                        array[j] = array[j + 1];
                        array[j] = tmp;
                    }
                    interations++;
                }
            }
            return result;
        }
    }
}
