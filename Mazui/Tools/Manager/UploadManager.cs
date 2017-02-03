using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Newtonsoft.Json;

namespace Mazui.Tools.Manager
{
    public class UploadManager
    {
        public static async Task<ImgurEntity> UploadImgur(IRandomAccessStream fileStream)
        {
            try
            {
                var imageData = new byte[fileStream.Size];
                for (int i = 0; i < imageData.Length; i++)
                {
                    imageData[i] = (byte)fileStream.AsStreamForRead().ReadByte();
                }
                var theAuthClient = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.imgur.com/3/image");
                request.Headers.Authorization = new AuthenticationHeaderValue("Client-ID", "e5c018ac1f4c157");
                var form = new MultipartFormDataContent();
                var t = new StreamContent(fileStream.AsStream());
                // TODO: See if this is the correct way to use imgur's v3 api. I can't see why we would still need to convert images to base64.
                string base64Img = Convert.ToBase64String(imageData);
                t.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                form.Add(new StringContent(base64Img), @"image");
                form.Add(new StringContent("file"), "type");
                request.Content = form;
                HttpResponseMessage response = await theAuthClient.SendAsync(request);
                string responseString = await response.Content.ReadAsStringAsync();
                if (responseString == null) return null;
                var imgurEntity = JsonConvert.DeserializeObject<ImgurEntity>(responseString);
                return imgurEntity;
            }
            catch (WebException)
            {
            }
            catch (IOException)
            {
                return null;
            }
            return null;
        }

        public class ImgurEntity
        {
            public Data data { get; set; }
            public bool success { get; set; }
            public int status { get; set; }

            public class Data
            {
                public string id { get; set; }
                public object title { get; set; }
                public object description { get; set; }
                public int datetime { get; set; }
                public string type { get; set; }
                public bool animated { get; set; }
                public int width { get; set; }
                public int height { get; set; }
                public int size { get; set; }
                public int views { get; set; }
                public int bandwidth { get; set; }
                public bool favorite { get; set; }
                public object nsfw { get; set; }
                public object section { get; set; }
                public string deletehash { get; set; }
                public string link { get; set; }
            }
        }
    }
}
