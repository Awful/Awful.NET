using Mazui.Core.Models.Forums;
using Mazui.Core.Models.Threads;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace Mazui.Notifications
{
    public class NotifyStatusTile
    {
        public static async Task<bool> CreateSecondaryForumLinkTile(Forum forumEntity)
        {
            var tileId = forumEntity.ForumId;
            var pinned = SecondaryTile.Exists(tileId.ToString());
            if (pinned)
                return true;

            Uri square150X150Logo = new Uri("ms-appx:///Assets/Logo.png");

            var tile = new SecondaryTile(tileId.ToString())
            {
                DisplayName = forumEntity.Name,
                Arguments = JsonConvert.SerializeObject(forumEntity),
                VisualElements = { Square150x150Logo = square150X150Logo, ShowNameOnSquare150x150Logo = true },
            };
            return await tile.RequestCreateAsync();
        }

        public static void CreateBookmarkLiveTile(Thread forumThread)
        {
            var bindingContent = new TileBindingContentAdaptive()
            {
                Children =
                {
                    new AdaptiveText()
                    {
                        Text = forumThread.Name,
                        HintStyle = AdaptiveTextStyle.Body
                    },
                    new AdaptiveText()
                    {
                        Text = string.Format("Unread Posts: {0}", forumThread.RepliesSinceLastOpened),
                        HintWrap = true,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                    new AdaptiveText()
                    {
                        Text = string.Format("Killed By: {0}", forumThread.KilledBy),
                        HintWrap = true,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }
                }
            };
            var binding = new TileBinding()
            {
                Branding = TileBranding.NameAndLogo,
                Content = bindingContent
            };
            var content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = binding,
                    TileWide = binding,
                    TileLarge = binding
                }
            };
            var tileXml = content.GetXml();
            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public static void CreateToastNotification(Thread forumThread)
        {
            string replyText = forumThread.RepliesSinceLastOpened > 1 ? " has {0} replies." : " has {0} reply.";
			var notification = new ToastNotificationArgs()
			{
				Type = ToastType.Toast,
				ThreadId = forumThread.ThreadId,
				PageNumber = forumThread.CurrentPage,
				IsThreadBookmark = forumThread.IsBookmark
			};
            ToastContent content = new ToastContent()
            {
                Launch = JsonConvert.SerializeObject(notification),
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                            {
                                new AdaptiveText()
                                {
                                Text = string.Format("\"{0}\"", forumThread.Name)
                                },
                                new AdaptiveText()
                                {
                                    Text = string.Format(replyText, forumThread.RepliesSinceLastOpened)
                                },
                                new AdaptiveImage()
                                {
                                    Source = forumThread.ImageIconLocation
                                }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = "/Assets/StoreLogo.png",
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }
                    }

                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Open Thread", JsonConvert.SerializeObject(notification))
                        {
                            ActivationType = ToastActivationType.Foreground
                        },
                        new ToastButton("Sleep", JsonConvert.SerializeObject(new ToastNotificationArgs() { Type = ToastType.Sleep }))
                        {
                            ActivationType = ToastActivationType.Background
                        }
                    }
                },
                Audio = new ToastAudio()
                {
                    Src = new Uri("ms-winsoundevent:Notification.Reminder")
                }
            };

            XmlDocument doc = content.GetXml();

            var toastNotification = new ToastNotification(doc);
            var nameProperty = toastNotification.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "Tag");
            nameProperty?.SetValue(toastNotification, forumThread.ThreadId.ToString());
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        public static void CreateToastNotification(string header, string text)
        {
            XmlDocument notificationXml =
    ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            XmlNodeList toastElements = notificationXml.GetElementsByTagName("text");
            toastElements[0].AppendChild(
                notificationXml.CreateTextNode(header));
            toastElements[1].AppendChild(
                 notificationXml.CreateTextNode(text));
            XmlNodeList imageElement = notificationXml.GetElementsByTagName("image");
            string imageName = string.Empty;
            if (string.IsNullOrEmpty(imageName))
            {
                imageName = @"Assets/Logo.scale-100.png";
            }
            imageElement[0].Attributes[1].NodeValue = imageName;
            IXmlNode toastNode = notificationXml.SelectSingleNode("/toast");
			var notification = new ToastNotificationArgs()
			{
				Type = ToastType.Ignore
			};
			var xmlElement = (XmlElement)toastNode;
            xmlElement?.SetAttribute("launch", JsonConvert.SerializeObject(notification));
            var toastNotification = new ToastNotification(notificationXml)
            {
                ExpirationTime = DateTimeOffset.UtcNow.AddSeconds(30)
            };
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }
    }
}
