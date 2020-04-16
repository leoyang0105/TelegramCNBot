using CNBot.Core.Entities.Chats;
using CNBot.Core.Paging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static CNBot.Core.Dtos.TGCallbackQueryDTO;

namespace CNBot.Core.Dtos
{
    public class TGSendMessageDTO
    {
        [JsonProperty("chat_id")]
        public long ChatId { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("parse_mode")]
        public string ParseMode { get; set; }
        [JsonProperty("reply_to_message_id")]
        public long? ReplyToMessageId { get; set; }
        [JsonProperty("reply_markup")]
        public object ReplyMarkup { get; set; }

        [JsonProperty("disable_web_page_preview")]
        public bool DisableWebPagePreview { get; set; }
        public static TGEditMessageTextDTO BuildChatListEditMessage(TGSendMessageDTO message, long messageId)
        {
            return new TGEditMessageTextDTO
            {
                ChatId = message.ChatId,
                DisableWebPagePreview = true,
                MessageId = messageId,
                ParseMode = nameof(MessageParseModelType.Html),
                ReplyMarkup = message.ReplyMarkup,
                Text = message.Text
            };
        }
        public static TGSendMessageDTO BuildChatListMessage(IPagedResult<Chat> paged, long chatId, long messageId)
        {
            var dto = new TGSendMessageDTO
            {
                ChatId = chatId,
                DisableWebPagePreview = true,
                ParseMode = nameof(MessageParseModelType.Html)
            };
            if (paged.Data == null || !paged.Data.Any())
            {
                dto.Text = "暂无数据，群组数据将持续完善";
            }
            else
            {
                foreach (var item in paged.Data)
                {
                    if (item.ChatType == ChatType.Channel)
                    {
                        dto.Text += $"📢|👤{item.MembersCount}| <a href=\"https://t.me/{item.UserName}\">{item.Title}</a>\n";
                    }
                    else if (item.ChatType == ChatType.SuperGroup)
                    {
                        dto.Text += $"👥|👤{item.MembersCount}| <a href=\"https://t.me/{item.UserName}\">{item.Title}</a>\n";
                    }
                    else if (item.ChatType == ChatType.Group)
                    {
                        dto.Text += $"🔒|👤{item.MembersCount}| <a href=\"{item.InviteLink}\">{item.Title}</a>\n";
                    }
                }

                var pager = new TGInlineKeyboardMarkup
                {
                    InlineKeyboard = new[]
                    {
                    new List<TGInlineKeyboardMarkup.InlineKeyboardButton>()
                    {
                        new TGInlineKeyboardMarkup.InlineKeyboardButton
                        {
                            Text = paged.PageIndex == 1 ? "首页" : "上一页",
                            CallbackData = JsonConvert.SerializeObject(new TGCallbackQueryDataDTO
                            {
                                PageIndex = paged.PageIndex == 1 ? 0 : paged.PageIndex - 1,
                                MessageId = messageId
                            })
                        },
                          new TGInlineKeyboardMarkup.InlineKeyboardButton
                          {
                              Text = $"第{paged.PageIndex}页,共{paged.TotalPages}页",
                              CallbackData = JsonConvert.SerializeObject(new TGCallbackQueryDataDTO
                            {
                                PageIndex = 0,
                                MessageId = messageId
                            })
                          },
                        new TGInlineKeyboardMarkup.InlineKeyboardButton
                        {
                            Text = paged.PageIndex == paged.TotalPages ? "尾页" : "下一页",
                            CallbackData = JsonConvert.SerializeObject(new TGCallbackQueryDataDTO
                            {
                                PageIndex = paged.PageIndex == paged.TotalPages ? 0 : paged.PageIndex + 1,
                                MessageId = messageId
                            })
                        }
                    }
    }
                };
                dto.ReplyMarkup = pager;
            }
            return dto;
        }
        public static TGSendMessageDTO BuildChatCategoriesMessage(long chatId, List<string> categories)
        {
            var replyMarkup = new TGReplyKeyboardMarkup
            {
                OneTimeKeyboard = true,
                Keyboard = new List<List<TGReplyKeyboardMarkup.KeyboardButton>>
                    {
                        new List<TGReplyKeyboardMarkup.KeyboardButton>()
                        {
                            new TGReplyKeyboardMarkup.KeyboardButton
                            {
                               Text = "全部群组 | 排行榜"
                            }
                        }
                    }
            };
            if (categories != null && categories.Any())
            {
                while (categories.Any())
                {
                    var cells = categories.Take(3).ToList();
                    var menus = new List<TGReplyKeyboardMarkup.KeyboardButton>();
                    cells.ForEach(text =>
                    {
                        menus.Add(new TGReplyKeyboardMarkup.KeyboardButton
                        {
                            Text = text
                        });
                        categories.Remove(text);
                    });
                    replyMarkup.Keyboard.Add(menus);
                }
            }
            return new TGSendMessageDTO
            {
                ChatId = chatId,
                DisableWebPagePreview = true,
                Text = "请在下方菜单中选择分类",
                ReplyMarkup = replyMarkup
            };
        }
        public static TGSendMessageDTO BuildOnlyUseInPrivateMessage(long chatId, long? replyTo = 0)
        {
            return new TGSendMessageDTO
            {
                ChatId = chatId,
                DisableWebPagePreview = true,
                Text = "群组中无法使用命令，请在私聊中使用",
                ReplyToMessageId = replyTo
            };
        }
        public static TGSendMessageDTO BuildUnrecognizedMessage(long chatId, long? replyTo = 0)
        {
            return new TGSendMessageDTO
            {
                ChatId = chatId,
                DisableWebPagePreview = true,
                Text = "无法识别该命令或文字，要查看帮助请输入 /help",
                ReplyToMessageId = replyTo
            };
        }
        public static TGSendMessageDTO BuildHelpMessage(long chatId)
        {
            var message = new TGSendMessageDTO
            {
                ChatId = chatId,
                DisableWebPagePreview = true,
                ReplyMarkup = new { remove_keyboard = true }
            };
            message.Text = "/list 列出已收录群组分类 \n" +
                            "/join 收录群组或频道 \n" +
                            "/search 收录群组或者频道 \n" +
                            "/mylist 列出你是创建人的群组或频道 \n" +
                            "/update 更改群组或频道分类 \n" +
                            "/remove 移除群组或频道 \n" +
                            "/reset 重置命令状态";
            return message;
        }
        public static TGSendMessageDTO BuildResetMessage(long chatId)
        {
            return new TGSendMessageDTO
            {
                ChatId = chatId,
                Text = "命令状态已重置",
                ReplyMarkup = new { remove_keyboard = true }
            };
        }
        public static TGSendMessageDTO BuildChatJoinMessage(long chatId)
        {
            return new TGSendMessageDTO
            {
                ChatId = chatId,
                Text = "【收录群组】请输入群组或频道用户名(可直接分享或粘贴群组链接)",
                ReplyMarkup = new { remove_keyboard = true }
            };
        }
        public static TGSendMessageDTO BuildChatRemoveMessage(long chatId)
        {
            return new TGSendMessageDTO
            {
                ChatId = chatId,
                Text = "【移除收录】请输入群组或频道用户名(可直接分享或粘贴群组链接)",
                ReplyMarkup = new { remove_keyboard = true }
            };
        }
        public static TGSendMessageDTO BuildChatSearchMessage(long chatId)
        {
            return new TGSendMessageDTO
            {
                ChatId = chatId,
                Text = "【搜索群组】请输入群组名称关键字",
                ReplyMarkup = new { remove_keyboard = true }
            };
        }
        public static TGSendMessageDTO BuildChatUpdateMessage(long chatId)
        {
            return new TGSendMessageDTO
            {
                ChatId = chatId,
                Text = "【更新群组】请输入群组或频道用户名(可直接分享或粘贴群组链接)",
                ReplyMarkup = new { remove_keyboard = true }
            };
        }
    }
    public class TGReplyKeyboardMarkup
    {

        [JsonProperty("keyboard")]
        public List<List<KeyboardButton>> Keyboard { get; set; }
        [JsonProperty("resize_keyboard")]
        public bool ResizeKeyboard { get; set; }
        [JsonProperty("one_time_keyboard")]
        public bool OneTimeKeyboard { get; set; }
        [JsonProperty("selective")]
        public bool Selective { get; set; }
        public class KeyboardButton
        {
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("request_contact")]
            public bool RequestContact { get; set; }
            [JsonProperty("request_location")]
            public bool RequestLocation { get; set; }
            [JsonProperty("request_poll")]
            public object RequestPoll { get; set; }
        }
    }
    public class TGInlineKeyboardMarkup
    {
        [JsonProperty("inline_keyboard")]
        public List<InlineKeyboardButton>[] InlineKeyboard { get; set; }
        public class InlineKeyboardButton
        {
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("url")]
            public string Url { get; set; }
            [JsonProperty("login_url")]
            public string LoginUrl { get; set; }
            [JsonProperty("callback_data")]
            public string CallbackData { get; set; }
            [JsonProperty("switch_inline_query")]
            public string WwitchInlineQuery { get; set; }
            [JsonProperty("switch_inline_query_current_chat")]
            public string WwitchInlineQueryCurrentChat { get; set; }
            [JsonProperty("pay")]
            public bool Pay { get; set; }
        }
    }
    public enum MessageParseModelType
    {
        Markdown,
        Html
    }
}
