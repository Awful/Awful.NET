using Mazui.Services;
using Mazui.Tools.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Mazui.Tools.Web
{
    public class AddImage
    {
        public static async Task AddImageViaImgur(TextBox replyText)
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".gif");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null) return;
            if (!file.ContentType.Contains("image"))
            {
                return;
            }
            var stream = await file.OpenAsync(FileAccessMode.Read);
            if (SettingsService.Instance.ImgurSignedIn)
            {
                var imgurManager = new ImgurManager();
                var image = await imgurManager.UploadImage(stream, SettingsService.Instance.ImgurUsername);
                if (image == null)
                {
                    var msgDlg = new MessageDialog("Something went wrong with the upload. :-(.");
                    await msgDlg.ShowAsync();
                    return;
                }

                // We have got an image up on Imgur! Time to get it into the reply box!
                string imgLink = $"[TIMG]{image.Link}[/TIMG]";
                replyText.Text = replyText.Text.Insert(replyText.Text.Length, imgLink);
            }
            else
            {
                // TODO: Remove this once we make sure that the new Imgur library works right.
                var result = await UploadManager.UploadImgur(stream);
                if (result == null)
                {
                    var msgDlg = new MessageDialog("Something went wrong with the upload. :-(.");
                    await msgDlg.ShowAsync();
                    return;
                }

                // We have got an image up on Imgur! Time to get it into the reply box!

                string imgLink = $"[TIMG]{result.data.link}[/TIMG]";
                replyText.Text = replyText.Text.Insert(replyText.Text.Length, imgLink);
            }
            stream.Dispose();
        }
    }
}
