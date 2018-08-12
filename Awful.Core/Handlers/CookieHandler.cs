using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Awful.Parser.Handlers
{
    public class CookieSerializer
    {
        public static void Serialize(CookieCollection cookies, Uri address, Stream stream)
        {
            var serializer = new DataContractSerializer(typeof(IEnumerable<Cookie>));
            var cookieList = cookies.OfType<Cookie>();

            serializer.WriteObject(stream, cookieList);
        }

        public static CookieContainer Deserialize(Uri uri, Stream stream)
        {
            var container = new CookieContainer();
            var serializer = new DataContractSerializer(typeof(IEnumerable<Cookie>));
            var cookies = (IEnumerable<Cookie>)serializer.ReadObject(stream);
            var cookieCollection = new CookieCollection();

            foreach (var fixedCookie in cookies.Select(cookie => new Cookie(cookie.Name, cookie.Value, "/", ".somethingawful.com")))
            {
                cookieCollection.Add(fixedCookie);
            }
            container.Add(uri, cookieCollection);
            return container;
        }
    }

    public static class CookieManager
    {
        public static CookieContainer LoadCookie(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                var formatter = new BinaryFormatter();
                System.Console.WriteLine("Deserializing cookie container");
                return (CookieContainer)formatter.Deserialize(stream);
            }
        }

        public static bool SaveCookie(CookieContainer cookieContainer, string path)
        {
            using (FileStream stream = File.Create(path))
            {
                var formatter = new BinaryFormatter();
                System.Console.WriteLine("Serializing cookie container");
                formatter.Serialize(stream, cookieContainer);
            }
            return true;
        }
    }
}
