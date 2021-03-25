FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Frontend/WebSite/Frontend.WebSite.csproj", "Frontend/WebSite/"]
RUN dotnet restore "Frontend\WebSite\Frontend.WebSite.csproj"
COPY . .
WORKDIR "/src/Frontend/WebSite"
RUN dotnet build "Frontend.WebSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Frontend.WebSite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frontend.WebSite.dll"]
