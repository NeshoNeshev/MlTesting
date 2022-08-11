using ConsoleApp1;
using Microsoft.ML;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
var modelFile = "CaseCategoryModel.zip";
if (!File.Exists(modelFile))
{
    TrainModel(@"C:\Users\nnesh\source\repos\Web\MlTesting\ConsoleApp1\case-train-data.csv", modelFile);
}

//var testModelData = new List<string>
//{
//                                        "да си осиновя дете",
//                                        "Арест",
//                                        "Родителски права",
//                                        "откраднаха ми музиката",
//                                        " употреба на наркотици",
//                                        "използва музиката ми без позволение"
//                                    };



static void TrainModel(string dataFile, string modelFile)
{
    // Create MLContext to be shared across the model creation workflow objects
    var context = new MLContext(seed: 0);

    // Loading the data
    Console.WriteLine($"Loading the data ({dataFile})");
    var trainingDataView = context.Data.LoadFromTextFile<CaseModel>(dataFile, ',', true, true, true);

    // Common data process configuration with pipeline data transformations
    Console.WriteLine("Map raw input data columns to ML.NET data");
    var dataProcessPipeline = context.Transforms.Conversion.MapValueToKey("Label", nameof(CaseModel.Category))
        .Append(context.Transforms.Text.FeaturizeText("Features", nameof(CaseModel.Content)));

    // Create the selected training algorithm/trainer
    Console.WriteLine("Create and configure the selected training algorithm (trainer)");
    var trainer = context.MulticlassClassification.Trainers.SdcaMaximumEntropy(); // SDCA = Stochastic Dual Coordinate Ascent
                                                                                  //// Alternative: LightGbm (GBM = Gradient Boosting Machine)

    // Set the trainer/algorithm and map label to value (original readable state)
    var trainingPipeline = dataProcessPipeline.Append(trainer).Append(
        context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

    // Train the model fitting to the DataSet
    Console.WriteLine("Train the model fitting to the DataSet");
    var trainedModel = trainingPipeline.Fit(trainingDataView);

    // Save/persist the trained model to a .ZIP file
    Console.WriteLine($"Save the model to a file ({modelFile})");
    context.Model.Save(trainedModel, trainingDataView.Schema, modelFile);
}


while (true)
{
    Console.WriteLine("Въведете случай:");
    var input = Console.ReadLine();
    if (input == "стоп") break;
    TestModel(modelFile, input);
}



static void TestModel(string modelFile, string input)
{
    var context = new MLContext();
    var model = context.Model.Load(modelFile, out _);
    var predictionEngine = context.Model.CreatePredictionEngine<CaseModel, OutputModel>(model);
    var prediction = predictionEngine.Predict(new CaseModel { Content = input });
    Console.WriteLine(new string('-', 60));
    Console.WriteLine($"Content: {input}");
    Console.WriteLine($"Prediction: {prediction.Category}");
    Console.WriteLine($"Score: {prediction.Score.Max()}");
    //foreach (var testData in testModelData)
    //{
    //    var prediction = predictionEngine.Predict(new CaseModel { Content = input });
    //    Console.WriteLine(new string('-', 60));
    //    Console.WriteLine($"Content: {input}");
    //    Console.WriteLine($"Prediction: {prediction.Category}");
    //    Console.WriteLine($"Score: {prediction.Score.Max()}");
    //}
}
