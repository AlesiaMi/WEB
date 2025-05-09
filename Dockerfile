FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY WEB/WEB/ ./WEB
RUN ls -R /src/WEB
RUN dotnet restore "WEB/WEB.csproj"
COPY . .  
RUN dotnet publish "WEB/WEB.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WEB.dll"]
