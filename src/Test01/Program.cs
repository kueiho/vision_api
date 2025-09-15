// See https://aka.ms/new-console-template for more information
using Google.Cloud.Vision.V1;
using Test01.Services;

Console.WriteLine("Hello, World!");

//var service = new TextDetectionService();
//await service.DetectTextAsync("../../../1_wLOOeDMSw83XXxwDPi0sKg.jpg");

var service = new LabelDetectionService();
await service.DetectLabelsAsync("../../../IMG_1394.jpg");

Console.ReadLine();
