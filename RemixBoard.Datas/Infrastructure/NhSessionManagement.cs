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
                var configInfo = new FileInfo(serializedConfiguration);
                var assInfo = new FileInfo(ass.Location);
                var configFileInfo = new FileInfo(configFile);
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
                Configuration = new Configuration().Configure(configFile);
                SaveConfigurationToFile(Configuration);
            }

            SessionFactory = Configuration.BuildSessionFactory();
        }

        private static void SaveConfigurationToFile(Configuration configuration) {
            using (var file = File.Open(serializedConfiguration, FileMode.Create)) {
                var bf = new BinaryFormatter();
                bf.Serialize(file, configuration);
            }
        }

        private static Configuration LoadConfigurationFromFile() {
            if (IsConfigurationFileValid == false)
                return null;
            try {
                using (var file = File.Open(serializedConfiguration, FileMode.Open)) {
                    var bf = new BinaryFormatter();
                    return bf.Deserialize(file) as Configuration;
                }
            }
            catch (Exception) {
                return null;
            }
        }

        private const string serializedConfiguration = "configuration.serialized";
        private const string configFile = "hibernate.cfg.xml";
        private static ISession session;
        private static IStatelessSession statelessSession;
    }
}