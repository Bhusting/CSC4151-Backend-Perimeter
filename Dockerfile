#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CSC4151-Backend-Perimeter/CSC4151-Backend-Perimeter.csproj", "CSC4151-Backend-Perimeter/"]
RUN dotnet restore "CSC4151-Backend-Perimeter/CSC4151-Backend-Perimeter.csproj"
COPY . .
WORKDIR "/src/CSC4151-Backend-Perimeter"
RUN dotnet build "CSC4151-Backend-Perimeter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CSC4151-Backend-Perimeter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSC4151-Backend-Perimeter.dll"]