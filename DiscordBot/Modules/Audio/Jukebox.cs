using Discord;
using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace DiscordBot.Modules.Audio
{
	public sealed class Jukebox : IDisposable
	{
		private readonly JukeboxConfiguration _Configuration;
		private readonly ConcurrentDictionary<ulong, JukeboxConnection> _Connections;
		private Timer _Timer;

		public Jukebox(JukeboxConfiguration configuration)
		{
			_Configuration = configuration;
			_Connections = new ConcurrentDictionary<ulong, JukeboxConnection>();
			_Timer = new Timer(OnTimerTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(1.0));
		}

		private static Process CreateFfmpeg()
		{
			return Process.Start(new ProcessStartInfo
			{
				FileName = "ffmpeg",//won't run on linux if this is set to ffmpeg.exe - fix this you noob
				Arguments = $"-hide_banner -loglevel panic -i - -ac 2 -f s16le -ar 48000 -",
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardInput = true,
				RedirectStandardOutput = true
			});
		}

		private void OnTimerTick(object state)
		{
			foreach (JukeboxConnection connection in _Connections.Values)
			{
				if (connection.CurrentRequest == null && connection.Queue.TryDequeue(out JukeboxRequest request))
				{
					HandleRequest(connection, request);
				}
			}
		}

		private static async Task EnsureConnectedToUsersChannel(JukeboxConnection connection, IVoiceState voiceState)
		{
			if (connection.AudioClient != null && connection.CurrentChannelId != voiceState.VoiceChannel.Id)
			{
				await connection.AudioClient.StopAsync();
				connection.AudioClient.Dispose();
				connection.AudioClient = null;
				connection.CurrentChannelId = null;
			}

			if (connection.AudioClient == null)
			{
				connection.AudioClient = await voiceState.VoiceChannel.ConnectAsync();
				connection.CurrentChannelId = voiceState.VoiceChannel.Id;
			}
		}

		private async void HandleRequest(JukeboxConnection connection, JukeboxRequest request)
		{
			connection.CurrentRequest = request;

			IVoiceState voiceState = (IVoiceState)request.User;
			YoutubeClient youtube = new();
			// Try to look up the video's manifest from YouTube
			StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(request.VideoId);

			if (streamManifest != null)
			{
				// Get a reference to the audio-only stream from YouTube for the specified video
				IStreamInfo streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();

				if (streamInfo != null)
				{
					// Ensure that the bot is connected to the requesting user's channel
					await EnsureConnectedToUsersChannel(connection, voiceState);

					// Create a new stream object to send audio to the Discord channel
					using AudioOutStream audioOutStream = connection.AudioClient.CreatePCMStream(AudioApplication.Music);
					// Start ffmpeg.exe and prepare to start capturing the output
					using Process ffmpeg = CreateFfmpeg();
					Task ffmpegOutputTask = ffmpeg.StandardOutput.BaseStream.CopyToAsync(audioOutStream);

					// Transfer the audio data from YouTube to the ffmpeg input stream
					await youtube.Videos.Streams.CopyToAsync(streamInfo, ffmpeg.StandardInput.BaseStream);
					ffmpeg.StandardInput.BaseStream.Close();

					// Wait until all output has been captured from ffmpeg
					ffmpegOutputTask.Wait();

					// By this point the song has finished playing
					connection.CurrentRequest = null;
				}
			}
		}

		public bool QueueSong(ulong serverId, SocketUser socketUser, string videoId)
		{
			JukeboxConnection connection = _Connections.GetOrAdd(serverId, new JukeboxConnection { ServerId = serverId });

			if (connection.Queue.Count >= _Configuration.QueueSizeMax)
			{
				return false;
			}

			connection.Queue.Enqueue(new JukeboxRequest
			{
				User = socketUser,
				VideoId = videoId
			});
			return true;
		}

		public async Task<bool> QueuePlaylist(ulong serverId, SocketUser socketUser, string playlistId)
		{
			JukeboxConnection connection = _Connections.GetOrAdd(serverId, new JukeboxConnection { ServerId = serverId });

			if (connection.Queue.Count >= _Configuration.QueueSizeMax)
			{
				return false;
			}

			YoutubeClient youtube = new();
			IReadOnlyList<Video> playlistVideos = await youtube.Playlists.GetVideosAsync(playlistId);

			if (playlistVideos.Count == 0)
			{
				return false;
			}

			foreach (Video video in playlistVideos)
			{
				if (connection.Queue.Count >= _Configuration.QueueSizeMax)
				{
					break;
				}

				connection.Queue.Enqueue(new JukeboxRequest
				{
					User = socketUser,
					VideoId = video.Id
				});
			}

			return true;
		}

		public void Dispose()
		{
			if (_Timer != null)
			{
				_Timer.Dispose();
				_Timer = null;
			}
		}
	}
}