using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BailianAIConsole
{
    class Program
    {
        // ===== 配置区 =====
        private static readonly string API_KEY = "sk-754220b265014b9d853f754cff0bac74"; // 替换为你的实际API Key
        private static readonly string API_URL = "https://dashscope.aliyuncs.com/compatible-mode/v1";
        private static readonly string MODEL = "qwen3-vl-plus"; // 图文模型
        // ==================

        static async Task Main(string[] args)
        {
            Console.WriteLine("===== 阿里云百炼 AI 图文生成演示 =====");
            Console.Write("请输入问题（支持图文问答，示例：这张图片里有什么？）：");
            string prompt = Console.ReadLine() ?? "这张图片里有什么？";

            // 示例图片URL（可替换为你的图片地址，支持公网URL或Base64）
            string imageUrl = "https://img.alicdn.com/imgextra/i1/O1CN01gDEY8M1W114Hi3XcN_!!6000000002727-0-tps-1024-406.jpg";

            try
            {
                Console.WriteLine("\n===== AI 响应结果 =====");
                // 调用流式接口（更符合图文模型的交互方式）
                await CallBailianAIImageStream(prompt, imageUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 调用百炼图文模型（qwen3-vl-plus）+ 流式响应（兼容OpenAI接口）
        /// </summary>
        /// <param name="prompt">用户问题</param>
        /// <param name="imageUrl">图片URL（公网可访问）</param>
        private static async Task CallBailianAIImageStream(string prompt, string imageUrl)
        {
            using var httpClient = new HttpClient();
            // 1. 设置认证头（兼容模式仅需API Key）
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // 设置超时（图文模型响应稍慢）
            httpClient.Timeout = TimeSpan.FromMinutes(5);

            // 2. 构建兼容OpenAI的请求体（图文混合）
            var requestBody = new
            {
                model = MODEL,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = new object[] // 图文混合内容：text + image_url
                        {
                            new { type = "text", text = prompt },
                            new
                            {
                                type = "image_url",
                                image_url = new { url = imageUrl } // 图片URL
                            }
                        }
                    }
                },
                stream = true, // 开启流式响应
                temperature = 0.7,
                max_tokens = 2000 // 图文模型建议增大token上限
            };

            // 序列化请求体
            var jsonOptions = new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody, jsonOptions),
                Encoding.UTF8,
                "application/json"
            );

            // 3. 发送POST请求（兼容模式的chat/completions端点）
            using var response = await httpClient.PostAsync($"{API_URL}/chat/completions", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API 请求失败 [{response.StatusCode}]: {errorContent}");
            }

            // 4. 处理流式响应（逐行读取SSE格式）
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new System.IO.StreamReader(stream);

            string fullResponse = "";
            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                // 解析SSE格式（data: {...} 或 data: [DONE]）
                if (line.StartsWith("data: "))
                {
                    string jsonData = line.Substring(6);
                    if (jsonData == "[DONE]") break; // 流结束

                    try
                    {
                        // 反序列化流式响应片段
                        var chunk = JsonSerializer.Deserialize<OpenAIStreamChunk>(jsonData);
                        if (chunk?.choices != null && chunk.choices.Count > 0)
                        {
                            var delta = chunk.choices[0].delta;
                            if (!string.IsNullOrEmpty(delta.content))
                            {
                                // 实时输出响应内容
                                Console.Write(delta.content);
                                fullResponse += delta.content;
                            }
                        }
                    }
                    catch (JsonException)
                    {
                        // 忽略解析失败的片段（如心跳包）
                        continue;
                    }
                }
            }

            Console.WriteLine($"\n\n===== 响应完成 =====");
        }

        /// <summary>
        /// 非流式调用（纯文本场景）
        /// </summary>
        private static async Task<string> CallBailianAI(string prompt)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new
            {
                model = MODEL,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                stream = false, // 关闭流式
                temperature = 0.7,
                max_tokens = 500
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync($"{API_URL}/chat/completions", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<OpenAINonStreamResponse>(responseBody);
                return apiResponse?.choices?[0]?.message?.content ?? "未提取到响应内容";
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API 请求失败 [{response.StatusCode}]: {errorContent}");
            }
        }
    }

    #region 响应模型（兼容OpenAI格式）
    // 流式响应片段模型
    public class OpenAIStreamChunk
    {
        public string id { get; set; }
        public string @object { get; set; }
        public long created { get; set; }
        public string model { get; set; }
        public List<OpenAIStreamChoice> choices { get; set; }
    }

    public class OpenAIStreamChoice
    {
        public OpenAIDelta delta { get; set; }
        public int index { get; set; }
        public string finish_reason { get; set; }
    }

    public class OpenAIDelta
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    // 非流式响应模型
    public class OpenAINonStreamResponse
    {
        public string id { get; set; }
        public string @object { get; set; }
        public long created { get; set; }
        public string model { get; set; }
        public List<OpenAINonStreamChoice> choices { get; set; }
        public Usage usage { get; set; }
    }

    public class OpenAINonStreamChoice
    {
        public OpenAIMessage message { get; set; }
        public int index { get; set; }
        public string finish_reason { get; set; }
    }

    public class OpenAIMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }
    #endregion
}
