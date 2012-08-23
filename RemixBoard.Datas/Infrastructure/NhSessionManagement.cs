using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate;
using NHibernate.Cfg;

namespace RemixBoard.Datas.Infrastructure
{
    public class NhSessionManagement
    {
        public static ISessionFactory SessionFactory { get; private set; }

        private static Configuration Configuration { get; set; }

        private static bool IsConfigurationFileValid {
            get {
                var ass = Assembly.GetCallingAssembly();
                if (ass.Location == null)
                    return false;
                var configInfo = new FileInfo(SerializedConfiguration);
                var assInfo = new FileInfo(ass.Location);
                var configFileInfo = new FileInfo(ConfigFile);
                if (configInfo.LastWriteTime < assInfo.LastWriteTime)
                    return false;
                if (configInfo.LastWriteTime < configFileInfo.LastWriteTime)
                    return false;
                return true;
            }
        }

        public static ISession Session
        {
            get { return session ?? (session = SessionFactory.OpenSession()); }
            set { session = value; }
        }

        public static IStatelessSession StatelessSession
        {
            get { return statelessSession ?? (statelessSession = SessionFactory.OpenStatelessSession()); }
        }

        public static void Initialize() {
            Configuration = LoadConfigurationFromFile();
            if (Configuration == null) {
                Configuration = new Configuration().Configure(ConfigFile);
                SaveConfigurationToFile(Configuration);
            }

            SessionFactory = Configuration.BuildSessionFactory();
        }

        private static void SaveConfigurationToFile(Configuration configuration) {
            using (var file = File.Open(SerializedConfiguration, FileMode.Create)) {
                var bf = new BinaryFormatter();
                bf.Serialize(file, configuration);
            }
        }

        private static Configuration LoadConfigurationFromFile() {
            if (IsConfigurationFileValid == false)
                return null;
            try {
                using (var file = File.Open(SerializedConfiguration, FileMode.Open)) {
                    var bf = new BinaryFormatter();
                    return bf.Deserialize(file) as Configuration;
                }
            }
            catch (Exception) {
                return null;
            }
        }

        private const string SerializedConfiguration = "configurtion.serialized";
        private const string ConfigFile = "hibernate.cfg.xml";
        private static ISession session;
        private static IStatelessSession statelessSession;
    }
}