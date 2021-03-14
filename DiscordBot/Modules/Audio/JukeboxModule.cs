using Discord.Commands;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;

namespace DiscordBot.Modules.Audio
{
	[Name("Jukebox")]
	[Summary("Plays audio from a YouTube video")]
	public sealed class JukeboxModule : ModuleBase<SocketCommandContext>
	{
		private readonly Jukebox _Jukebox;

		public JukeboxModule(Jukebox jukebox)
		{
			_Jukebox = jukebox;
		}

		[Command("play")]
		[Summary("Adds a YouTube video or playlist to the queue")]
		public async Task Play([Summary("The url of the YouTube video or playlist")] string url)
		{
			NameValueCollection queryString = HttpUtility.ParseQueryString(new Uri(url).Query);
			string videoId = queryString["v"];
			string playlistId = queryString["list"];

			if (string.IsNullOrWhiteSpace(videoId) && string.IsNullOrWhiteSpace(playlistId))
			{
				await ReplyAsync("Unable to extract an id from the specified URL. Please specify a valid URL.");
				return;
			}

			bool isPlaylist = string.IsNullOrWhiteSpace(videoId);
			bool isQueued = isPlaylist ?
				await _Jukebox.QueuePlaylist(Context.Guild.Id, Context.User, playlistId) :
				_Jukebox.QueueSong(Context.Guild.Id, Context.User, videoId);

			await ReplyAsync(isQueued ? $"{(isPlaylist ? "Playlist" : "Song")} queued." : "The queue is full!");
		}
	}
}