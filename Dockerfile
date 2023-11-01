#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/core/aspnet:6.0 AS base
RUN apt-get update \ 
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*
RUN cd /usr/lib && ln -s libgdiplus.so gdiplus.dll
WORKDIR /app
EXPOSE 5011
ENV ASPNETCORE_URLS=http://*:5011

FROM mcr.microsoft.com/dotnet/core/sdk:6.0-buster AS build
WORKDIR /
COPY . .
WORKDIR "/"
RUN dotnet restore "./keyclock_Authentication.csproj"

RUN dotnet build "keyclock_Authentication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "keyclock_Authentication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoginMS.dll"]