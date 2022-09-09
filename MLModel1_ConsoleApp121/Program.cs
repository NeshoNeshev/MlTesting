using Mosaik.Core;
using System.Text;
using Microsoft.Extensions.Logging;
using MLModel1_ConsoleApp121;

//Catalyst.Models.English.Register();



Console.OutputEncoding = Encoding.UTF8;ApplicationLogging.SetLoggerFactory(LoggerFactory.Create(lb => lb.AddConsole()));

//// В момента Catalyst поддържа 3 различни типа модели за разпознаване на именувани обекти (NER):
//// - Подобно на Gazetteer (т.е. [Spotter](https://github.com/curiosity-ai/catalyst/blob/master/Catalyst/src/Models/EntityRecognition/Spotter.cs))
//// - Подобно на Regex(т.е. [PatternSpotter](https://github.com/curiosity-ai/catalyst/blob/master/Catalyst/src/Models/EntityRecognition/PatternSpotter.cs))
//// - Perceptron (т.е. [AveragePerceptronEntityRecognizer](https://github.com/curiosity-ai/catalyst/blob/master/Catalyst/src/Models/EntityRecognition/AveragePerceptronEntityRecognizer.cs))
EntityRecognition.SpotterSample();

await EntityRecognition.AveragePerceptronEntityRecognizerAndPatternSpotterSample();




