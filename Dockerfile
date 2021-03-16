# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy and build app and libraries
COPY . .
RUN dotnet restore
RUN dotnet build -c release --no-restore

FROM build AS publish
RUN dotnet publish -c release --no-build -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /source
RUN apt-get update && apt-get install -y curl build-essential \
	&& curl -LO https://download.libsodium.org/libsodium/releases/libsodium-1.0.18.tar.gz \
	&& curl -LO https://ftp.osuosl.org/pub/xiph/releases/opus/opus-1.3.1.tar.gz \
	&& tar -xf libsodium-1.0.18.tar.gz && tar -xf opus-1.3.1.tar.gz \
	&& cd /source/opus-1.3.1 && ./configure && make && make install \
	&& cd /source/libsodium-1.0.18 && ./configure && make && make install \
	&& apt-get install -y libsodium23 libopus0 ffmpeg
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DiscordBot.dll"]
