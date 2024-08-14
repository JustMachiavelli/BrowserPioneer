using HappreeTool.Surfers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace CrawlPioneer.Infrastructure.Helpers
{
    public class SeleniumRequester(ILogger<SeleniumRequester> logger, IConfiguration configuration) : WebRequester
    {
        private readonly ILogger _logger = logger;
        private readonly IWebDriver _webDriver = WebDriverFactory.AttachToExistingChromeInstance
            (
                configuration["WebRequesters:Selenium:DirverExePath"]!, configuration["WebRequesters:Selenium:DebuggerAddress"]!
            );

        /// <summary>
        /// 获取当前Url
        /// </summary>
        /// <returns></returns>
        public string CurrentUrl => _webDriver.Url;

        /// <summary>
        /// 访问网址，解析到document
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public override async Task VisitUrl(string url)
        {
            _logger.LogInformation("webdriver尝试打开网址：{url}", url);
            try
            {
                // 尝试打开网页
                await _webDriver.Navigate().GoToUrlAsync(url);
                WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(20));
                wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                document.LoadHtml(_webDriver.PageSource);
            }
            catch (WebDriverTimeoutException ex)
            {
                _logger.LogError("webdriver打开网页 {url} 超时：{msg}", url, ex.Message);
                throw new WebDriverTimeoutException($"webdriver打开网页 {url} 超时！");
            }
            catch (WebDriverException ex)
            {
                _logger.LogError("webdriver打开网页 {url} 失败：{msg}", url, ex.Message);
                throw new WebDriverException($"webdriver打开网页 {url} 失败！");
            }
            catch (Exception ex)
            {
                _logger.LogError("webdriver打开网页 {url} 发生未知错误：{msg}", url, ex.Message);
                throw new Exception($"webdriver打开网页 {url} 发生未知错误！");
            }
        }

        /// <summary>
        /// 访问网址，解析到document
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public override async Task<string> GetHtml(string url)
        {
            _logger.LogInformation("webdriver尝试打开网址：{url}", url);
            try
            {
                // 尝试打开网页
                await _webDriver.Navigate().GoToUrlAsync(url);
                WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(20));
                //wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("interactive"));
                return _webDriver.PageSource;
            }
            catch (WebDriverTimeoutException ex)
            {
                _logger.LogError("webdriver打开网页 {url} 超时：{msg}", url, ex.Message);
                throw new WebDriverTimeoutException($"webdriver打开网页 {url} 超时！");
            }
            catch (WebDriverException ex)
            {
                _logger.LogError("webdriver打开网页 {url} 失败：{msg}", url, ex.Message);
                throw new WebDriverException($"webdriver打开网页 {url} 失败！");
            }
            catch (Exception ex)
            {
                _logger.LogError("webdriver打开网页 {url} 发生未知错误：{msg}", url, ex.Message);
                throw new Exception($"webdriver打开网页 {url} 发生未知错误！");
            }
        }

        public async Task DownloadImageAsync(string pageUrl, string imageXPath, string? obstacleXPath, byte downCount, string savePath)
        {
            _logger.LogInformation("【使用浏览器下载图片】图片所在网页【{webUrl}】，保存到【{savePath}】", pageUrl, savePath);

            //访问原图片的页面
            await VisitUrl(pageUrl);

            //移除遮盖干扰元素
            if (obstacleXPath != null)
            {
                RemoveObstacle(obstacleXPath);
            }

            //找到图片元素
            ClickContent(imageXPath);

            Thread.Sleep(500); // 等待右键菜单显示

            // 模拟按下八次“下箭头”键来选择“图片另存为”选项
            for (int i = 0; i < downCount; i++)
            {
                InputSimulator.SendKey(InputSimulator.VK_DOWN);
                Thread.Sleep(100); // 每次按键后稍微等待
            }


            // 模拟按下回车键选择“图片另存为”选项
            InputSimulator.SendKey(InputSimulator.VK_RETURN);

            Thread.Sleep(800); // 等待另存为窗口弹出

            // 模拟输入文件保存路径（假设保存路径为1.jpg）
            foreach (char ch in savePath)
            {
                InputSimulator.SendChar(ch);
                Thread.Sleep(10); // 等待每个字符输入
            }

            InputSimulator.SendKey(InputSimulator.VK_RETURN); // 模拟按下回车键保存文件

            Thread.Sleep(1000); // 等待文件保存完成
            _logger.LogInformation("【使用浏览器下载图片】_end");
        }

        protected override void DisposeUnmanaged()
        {
            // 释放 Selenium 资源
            _webDriver?.Quit();
            _webDriver?.Dispose();
        }

        /// <summary>
        /// 点击页面上的按钮
        /// </summary>
        /// <param name="url"></param>
        /// <param name="by"></param>
        public void ClickButton(string xPath)
        {
            IWebElement? agreeButton = _webDriver.FindElement(By.XPath(xPath));
            agreeButton?.Click();
        }

        /// <summary>
        /// 点击页面上的按钮
        /// </summary>
        /// <param name="url"></param>
        /// <param name="by"></param>
        public void ClickContent(string xPath)
        {
            Actions action = new Actions(_webDriver);
            IWebElement imageElement = _webDriver.FindElement(By.XPath(xPath));
            action.ContextClick(imageElement).Perform();
        }

        public void RemoveObstacle(string xPath)
        {
            var overlayElement = _webDriver.FindElement(By.XPath(xPath));
            ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].remove();", overlayElement);
        }

    }
}
