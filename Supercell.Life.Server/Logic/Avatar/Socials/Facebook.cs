namespace Supercell.Life.Server.Logic.Avatar.Socials
{
    using global::Facebook;
    
    using Newtonsoft.Json;

    internal class Facebook
    {
        private const string ApplicationID      = "";
        private const string ApplicationSecret  = "";
        private const string ApplicationVersion = "2.8";

        [JsonProperty("FB_Identifier")] internal string Identifier;
        [JsonProperty("FB_Token")]      internal string Token;

        internal FacebookClient FBClient;
        internal LogicClientAvatar Avatar;

        /// <summary>
        /// Initializes a new instance of the <see cref="Facebook"/> class.
        /// </summary>
        internal Facebook()
        {
            // Facebook.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Facebook"/> class.
        /// </summary>
        internal Facebook(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;

            if (this.Filled)
            {
                this.Connect();
            }
            else
            {
                this.Identifier = string.Empty;
                this.Token      = string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Facebook"/> is connected.
        /// </summary>
        internal bool Connected
        {
            get
            {
                return this.Filled && this.FBClient != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Facebook"/> is filled.
        /// </summary>
        internal bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Token);
            }
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        internal void Connect()
        {
            this.FBClient = new FacebookClient(this.Token)
            {
                AppId     = Facebook.ApplicationID,
                AppSecret = Facebook.ApplicationSecret,
                Version   = Facebook.ApplicationVersion
            };
        }

        internal object Get(string path, bool includeIdentifier = true)
        {
            if (this.Connected)
            {
                return this.FBClient.Get("https://graph.facebook.com/v" + Facebook.ApplicationVersion + "/" + (includeIdentifier ? this.Identifier + '/' + path : path));
            }
            else
            {
                return null;
            }
        }
    }
}
