using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingDemo1
{
    public class Test
    {
        private ILogger<Test> _logger;

        public Test(ILogger<Test> logger)
        {
            _logger = logger;
        }

        public void Test1()
        {
            _logger.LogDebug("开始执行数据库同步");
            _logger.LogInformation("开始执行数据库同步");
            _logger.LogWarning("开始执行数据库同步");
            _logger.LogError("开始执行数据库同步");
        }
    }
}
