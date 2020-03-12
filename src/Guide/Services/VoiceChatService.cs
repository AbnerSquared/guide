﻿using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Guide.Services
{
    public class VoiceChatService
    {
        private readonly DiscordSocketClient _client;
        public VoiceChatService(DiscordSocketClient client)
        {
            _client = client;

            _client.UserVoiceStateUpdated += OnUserVoiceStateUpdated;
        }

        private async Task OnUserVoiceStateUpdated(SocketUser user, SocketVoiceState previous, SocketVoiceState current)
        {
            var guildUser = (user as SocketGuildUser);
            var voiceChannel = current.VoiceChannel ?? previous.VoiceChannel;

            if (voiceChannel.Guild.Id != Constants.TutorialGuildId)
                return;

            var role = voiceChannel.Guild.GetRole(Constants.VoiceChatRoleId);

            if (role == null)
                return;

            bool hasRole = guildUser.Roles.Any(x => x.Id == Constants.VoiceChatRoleId);

            if (current.VoiceChannel?.Id == Constants.VoiceChannelId)
            {
                if (!hasRole)
                {
                    await guildUser.AddRoleAsync(role);
                }
            }
            else if (previous.VoiceChannel?.Id == Constants.VoiceChannelId)
            {
                if (hasRole)
                {
                    await guildUser.RemoveRoleAsync(role);
                }
            }
        }
    }
}
