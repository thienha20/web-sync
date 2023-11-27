using System.Text;
namespace web_sync.Services
{
    public class FileLogService
    {
        public async Task<Boolean> writeFile(string objectName, string content)
        {
            try
            {
                string path = "Storage/Logs/" + objectName + ".log";
                await File.WriteAllTextAsync(path, content, Encoding.UTF8);
                return true;
            } catch { return false; }
        }

        public async Task<string> readFile(string objectName)
        {
            try
            {
                string path = "Storage/Logs/" + objectName + ".log";
                if (!File.Exists(path))
                {
                    return "";
                }
                string content = await File.ReadAllTextAsync(path, Encoding.UTF8);
                return content;
            }
            catch { return ""; }
        }

        public async Task<Boolean> appendFile(string objectName, string content)
        {
            try
            {
                string path = "Storage/Logs/" + objectName + ".log";
                if (!File.Exists(path))
                {
                    return await this.writeFile(objectName, content);
                }
                await File.AppendAllTextAsync(path, content, Encoding.UTF8);
                return true;
            }
            catch { return false; }
        }

        public Boolean deleteFile(string objectName)
        {
            try
            {
                string path = "Storage/Logs/" + objectName + ".log";
                if (!File.Exists(path))
                {
                    return false;
                }
                File.Delete(path);
                return true;
            }
            catch { return false; }
        }
    }
}
