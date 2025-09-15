using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test01.Services
{
    internal class TextDetectionService
    {
        public async Task DetectTextAsync(string imagePath)
        {
            // 初始化 Vision API 客戶端
            var client = ImageAnnotatorClient.Create();

            // 從檔案載入圖像
            var image = Image.FromFile(imagePath);

            // 執行文字檢測
            var response = await client.DetectTextAsync(image);

            // 輸出結果
            Console.WriteLine($"圖像 '{Path.GetFileName(imagePath)}' 的文字檢測結果:");
            if (response.Any())
            {
                // 輸出所有檢測到的文字
                foreach (var annotation in response)
                {
                    Console.WriteLine($"  {annotation.Description}");
                }

                // 輸出完整的文字塊 (如果有)
                Console.WriteLine("\n完整文字塊:");
                Console.WriteLine(response.FirstOrDefault()?.Description);
            }
            else
            {
                Console.WriteLine("未檢測到文字。");
            }
        }
    }
}