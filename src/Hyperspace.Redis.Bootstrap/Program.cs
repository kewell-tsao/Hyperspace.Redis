using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Hyperspace.Redis.Bootstrap
{
    public class Program
    {
        public int Main(string[] args)
        {
            var redis64Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @".dnx\packages\redis-64\");
            if (!Directory.Exists(redis64Path))
            {
                Console.Error.WriteLine($"Redis path {redis64Path} is not exists.");
                Console.ReadLine();
                return -1;
            }
            var versionName = Directory.EnumerateDirectories(redis64Path).Select(p => new DirectoryInfo(p).Name).OrderBy(n =>
              {
                  Version version;
                  return Version.TryParse(n, out version) ? version : null;
              }).LastOrDefault();
            if (string.IsNullOrEmpty(versionName))
            {
                Console.Error.WriteLine($"Redis path {redis64Path} has not any version.");
                Console.ReadLine();
                return -1;
            }
            var redisServerPath = Path.Combine(redis64Path, versionName, "redis-server.exe");
            Console.WriteLine($"Redis bin path is {redisServerPath}.");

            if (Process.GetProcessesByName("redis-server")
                       .Any(p => string.Equals(p.MainModule.FileName, redisServerPath)))
            {
                Console.Error.WriteLine("Redis is has been running.");
                Console.ReadLine();
                return -1;
            }

            var redisConfigPath = args.FirstOrDefault();
            if (string.IsNullOrEmpty(redisConfigPath))
                redisConfigPath = Path.Combine(redis64Path, versionName, "redis.windows.conf");
            else
                redisConfigPath = Path.GetFullPath(redisConfigPath);

            if (File.Exists(redisConfigPath))
            {
                Console.WriteLine($"Redis config path is {redisConfigPath}.");
            }
            else
            {
                Console.Error.WriteLine($"Redis config path {redisConfigPath} is not exists.");
                Console.ReadLine();
                return -1;
            }
            try
            {
                using (Process.Start(redisServerPath, $"\"{redisConfigPath}\"")) { }
                Console.WriteLine("Redis is start.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Console.ReadLine();
                return -1;
            }
        }
    }
}
