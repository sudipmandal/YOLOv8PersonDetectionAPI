#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["YoloPersonDetectionAPI.csproj", "."]
RUN dotnet restore "./YoloPersonDetectionAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "YoloPersonDetectionAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YoloPersonDetectionAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

ENV SENSITIVITY # (default 0.5) Set the person detection confidence of api,valid values between 0.1-1 where 0.1 = low and 1 = high

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YoloPersonDetectionAPI.dll"]
