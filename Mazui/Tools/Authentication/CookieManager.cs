using Mazui.Core.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Mazui.Tools.Authentication
{
    public class CookieManager
    {
        public static async Task<bool> SaveCookie(string filename, CookieContainer rcookie, Uri uri)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var sampleFile = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            try
            {
                using (StorageStreamTransaction transaction = await sampleFile.OpenTransactedWriteAsync())
                {
                    CookieSerializer.Serialize(rcookie.GetCookies(uri), uri, transaction.Stream.AsStream());
                    await transaction.CommitAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<CookieContainer> LoadCookie(string filename)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile;
            try
            {
                sampleFile = await localFolder.GetFileAsync(filename);
            }
            catch
            {
                return null;
            }

            using (var stream = await sampleFile.OpenStreamForReadAsync())
            {
                return CookieSerializer.Deserialize(new Uri(EndPoints.CookieDomainUrl), stream);
            }
        }
    }

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
}
