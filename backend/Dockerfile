FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY GestaoColaboradores.API/GestaoColaboradores.API.csproj GestaoColaboradores.API/
RUN dotnet restore GestaoColaboradores.API/GestaoColaboradores.API.csproj

COPY . .
WORKDIR /src/GestaoColaboradores.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5000
ENTRYPOINT ["dotnet", "GestaoColaboradores.API.dll"]
