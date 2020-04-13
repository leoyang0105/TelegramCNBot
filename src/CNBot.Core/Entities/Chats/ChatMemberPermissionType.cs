using System;

namespace CNBot.Core.Entities.Chats
{
    [Flags]
    public enum ChatMemberPermissionType
    {
        none = 0,
        CaneEdited = 1,
        CanChangeInfo = 2,
        CanDeleteMessages = 4,
        CanInviteUsers = 8,
        CanRestrictMembers = 16,
        CanPinMessages = 32,
        CanPromoteMembers = 64
    }
}
