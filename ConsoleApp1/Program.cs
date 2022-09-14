using ConsoleApp1;
using Microsoft.Data.SqlClient;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
var modelFile = "CaseCategoryModel.zip";
if (!File.Exists(modelFile))
{
    TrainModel( modelFile);
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



static void TrainModel(string modelFile)
{
    // Create MLContext to be shared across the model creation workflow objects
    var context = new MLContext(seed: 0);
    DatabaseLoader loader = context.Data.CreateDatabaseLoader<CaseData>();
    string connectionString = @"Server=.;Database=TestMl;Trusted_Connection=True;MultipleActiveResultSets=true";

    string sqlCommand = "SELECT Id, Decision, Answer,Content, TypeOfCase FROM Cases";
    
    DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand);

    IDataView data = loader.Load(dbSource);
    // Loading the data
   
    var trainingDataView = data;
    
    // Common data process configuration with pipeline data transformations
    Console.WriteLine("Map raw input data columns to ML.NET data");
    var dataProcessPipeline = context.Transforms.Conversion.MapValueToKey("Label", nameof(CaseModel.Id))
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
    var type = Console.ReadLine();
    if (input == "стоп") break;
    TestModel(modelFile, input, type);
}



static void TestModel(string modelFile, string input, string type)
{
    var context = new MLContext();
    var model = context.Model.Load(modelFile, out _);
    var predictionEngine = context.Model.CreatePredictionEngine<CaseModel, OutputModel>(model);
    var prediction = predictionEngine.Predict(new CaseModel { Content = input, TypeOfCase = type });
    Console.WriteLine(new string('-', 60));
    Console.WriteLine($"Content: {input}");
    Console.WriteLine($"Prediction: {prediction.Id}");
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
