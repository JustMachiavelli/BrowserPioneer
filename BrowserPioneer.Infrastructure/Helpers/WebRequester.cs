using HtmlAgilityPack;

namespace CrawlPioneer.Infrastructure.Helpers
{
    public abstract class WebRequester : IDisposable
    {
        /// <summary>
        /// 网页html解析
        /// </summary>
        protected HtmlDocument document = new HtmlDocument();

        /// <summary>
        /// 获取当前html解析
        /// </summary>
        public HtmlDocument Document => document;

        public HtmlNode SelectSingleNode(string xpath)
        {
            return document.DocumentNode.SelectSingleNode(xpath);
        }

        public HtmlNodeCollection SelectNodes(string xpath)
        {
            return document.DocumentNode.SelectNodes(xpath);
        }

        /// <summary>
        /// 通过selenium操作浏览器访问指定网址
        /// </summary>
        /// <param name="url"></param>
        public abstract Task VisitUrl(string url);

        public abstract Task<string> GetHtml(string url);

        /// <summary>
        /// 资源是否已释放
        /// </summary>
        private bool _hasDisposed = false;

        /// <summary>
        /// 释放非托管资源
        /// </summary>
        protected abstract void DisposeUnmanaged();

        /// <summary>
        /// 释放所有资源
        /// </summary>
        /// <param name="needDisposeManaged">是否释放托管的资源</param>
        protected virtual void Dispose(bool needDisposeManaged)
        {
            if (_hasDisposed)
            {
                return;
            }

            if (needDisposeManaged)
            {
                //回收托管资源
            }

            //回收非托管资源，设置为null，等待CLR调用析构函数的时候回收
            DisposeUnmanaged();

            //已回收
            _hasDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~WebRequester()
        {
            Dispose(false);
        }

    }
}
