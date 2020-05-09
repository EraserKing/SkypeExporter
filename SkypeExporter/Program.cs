using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace SkypeExporter
{
    class Program
    {
        private static string ReplaceInvalidCharInPath(string originalPath) => string.Join("_", originalPath.Split(Path.GetInvalidFileNameChars()));

        static void Main(string[] args)
        {
            string baseFolder = Environment.CurrentDirectory;
            SkypeLog skypeLog = JsonConvert.DeserializeObject<SkypeLog>(File.ReadAllText("messages.json"));

            string userFolder = Path.Combine(baseFolder, ReplaceInvalidCharInPath(skypeLog.userId));
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            foreach(Conversation conversation in skypeLog.conversations.Where(x => x.displayName != null))
            {
                string conversationPath = Path.Combine(userFolder, ReplaceInvalidCharInPath(conversation.displayName) + ".txt");
                File.WriteAllLines(conversationPath, conversation.MessageList.OrderBy(x => x.id).Select(
                    x => $"[{x.originalarrivaltime}] {x.displayName ?? "Me"}: {x.content}"));
            }
        }
    }


    public class SkypeLog
    {
        public string userId { get; set; }
        public string exportDate { get; set; }
        public Conversation[] conversations { get; set; }
    }

    public class Conversation
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public float version { get; set; }
        public Properties properties { get; set; }
        public Threadproperties threadProperties { get; set; }
        public object messages { get; set; }
        public Messagelist[] MessageList { get; set; }
    }

    public class Properties
    {
        public bool conversationblocked { get; set; }
        public DateTime? lastimreceivedtime { get; set; }
        public string consumptionhorizon { get; set; }
        public string conversationstatus { get; set; }
    }

    public class Threadproperties
    {
        public int membercount { get; set; }
        public string members { get; set; }
        public string topic { get; set; }
    }

    public class Messagelist
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public DateTime originalarrivaltime { get; set; }
        public string messagetype { get; set; }
        public float version { get; set; }
        public string content { get; set; }
        public string conversationid { get; set; }
        public string from { get; set; }
        public Properties1 properties { get; set; }
        public string[] amsreferences { get; set; }
    }

    public class Properties1
    {
        public string albumId { get; set; }
        public Emotion[] emotions { get; set; }
        public string isserversidegenerated { get; set; }
        public string edittime { get; set; }
        public string urlpreviews { get; set; }
        public string deletetime { get; set; }
        public string forwardMetadata { get; set; }
        public string calllog { get; set; }
    }

    public class Emotion
    {
        public string key { get; set; }
        public User[] users { get; set; }
    }

    public class User
    {
        public string mri { get; set; }
        public long time { get; set; }
        public string value { get; set; }
    }

}
