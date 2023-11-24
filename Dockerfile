FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY *.csproj .
RUN dotnet restore
COPY Properties/. .
COPY *.cs ./
COPY *.json ./
RUN dotnet publish --no-restore -o /app


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./cloud-run-asp-net-http2"]
