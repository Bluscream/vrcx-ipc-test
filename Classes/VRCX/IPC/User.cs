namespace IPCTest.Classes.IPC {
    internal class User {
        public string Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string CurrentAvatarImageUrl { get; set; }
        public string CurrentAvatarThumbnailImageUrl { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
        public string Location { get; set; }
        public string WorldId { get; set; }
        public string InstanceId { get; set; }
        public bool IsFriend { get; set; }
        public string LastLocation { get; set; }
        public DateTime LastOnline { get; set; }
        public int JoinCount { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public int FriendsCount { get; set; }
        public int AvatarCount { get; set; }
        public int OnlineFriendsCount { get; set; }
        public int ActiveFriendsCount { get; set; }
        public int OfflineFriendsCount { get; set; }
        public bool Blocked { get; set; }
        public int TrustLevel { get; set; }
        public List<string> Tags { get; set; }
        public string DeveloperType { get; set; }
        public bool Moderator { get; set; }
        public DateTime DateJoined { get; set; }
        public bool AllowAvatarCopying { get; set; }
        public bool IsBanned { get; set; }
        public bool IsNuisance { get; set; }
        public string FriendKey { get; set; }
        public string CurrentInstance { get; set; }
    }

}
