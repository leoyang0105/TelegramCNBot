using CNBot.Core.Configurations;
using CNBot.Core.Dtos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CNBot.Core.Clients
{
    public class TelegramHttpClient : ITelegramHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly TelegramUrlsConfig _settings;
        public TelegramHttpClient(
            IOptions<TelegramUrlsConfig> options,
            HttpClient httpClient)
        {
            _settings = options.Value;
            _httpClient = httpClient;
        }
        #region Chats
        public async Task<TGResponseDTO<TGChatDTO>> GetChat(string chatId)
        {
            var url = TelegramUrlsConfig.Chat.Get(_settings.ApiToken, chatId);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TGResponseDTO<TGChatDTO>>(responseBody);
        }
        public async Task<TGResponseDTO<int>> GetChatMembersCount(string chatId)
        {
            var url = TelegramUrlsConfig.Chat.GetMembersCount(_settings.ApiToken, chatId);
            var response = await _httpClient.GetAsync(url);

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TGResponseDTO<int>>(responseBody);
        }
        public async Task<TGResponseDTO<List<TGChatMemberDTO>>> GetChatAdministrators(string chatId)
        {
            var url = TelegramUrlsConfig.Chat.GetAdministrators(_settings.ApiToken, chatId);
            var response = await _httpClient.GetAsync(url);

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TGResponseDTO<List<TGChatMemberDTO>>>(responseBody);
        }
        #endregion
        #region Messages
        public async Task<TGResponseDTO<TGMessageDTO>> SendMessage(TGSendMessageDTO dto)
        {
            var url = TelegramUrlsConfig.Message.Send(_settings.ApiToken);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, ApplicationDefaults.DefaultContentType);
            var response = await _httpClient.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TGResponseDTO<TGMessageDTO>>(responseBody);
        }
        #endregion
    }
}
