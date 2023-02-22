#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AkvelonTest/AkvelonTest.csproj", "AkvelonTest/"]
COPY ["AkvelonTest.Data/AkvelonTest.Data.csproj", "AkvelonTest.Data/"]
RUN dotnet restore "AkvelonTest/AkvelonTest.csproj"
COPY . .
WORKDIR "/src/AkvelonTest"
RUN dotnet build "AkvelonTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AkvelonTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AkvelonTest.dll"]