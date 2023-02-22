#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AkvelonTest.ImageTransformer/AkvelonTest.ImageTransformer.csproj", "AkvelonTest.ImageTransformer/"]
COPY ["AkvelonTest.Data/AkvelonTest.Data.csproj", "AkvelonTest.Data/"]
RUN dotnet restore "AkvelonTest.ImageTransformer/AkvelonTest.ImageTransformer.csproj"
COPY . .
WORKDIR "/src/AkvelonTest.ImageTransformer"
RUN dotnet build "AkvelonTest.ImageTransformer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AkvelonTest.ImageTransformer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AkvelonTest.ImageTransformer.dll"]