using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test01.Services
{
    internal class LabelDetectionService
    {
        public async Task DetectLabelsAsync(string imagePath)
        {
            // 初始化 Vision API 客戶端
            var client = ImageAnnotatorClient.Create();

            // 從檔案載入圖像
            var image = Image.FromFile(imagePath);

            // 執行標籤檢測
            var response = await client.DetectLabelsAsync(image);

            // 輸出結果
            Console.WriteLine($"圖像 '{Path.GetFileName(imagePath)}' 的標籤檢測結果:");

            if (response.Any())
            {
                foreach (var annotation in response)
                {
                    Console.WriteLine($"  {annotation.Description} (分數: {annotation.Score:P2})");
                }
            }
            else
            {
                Console.WriteLine("未檢測到標籤。");
            }
        }
    }
}