FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PayCoreFinalProject.RabbitMQ.Consumer/PayCoreFinalProject.RabbitMQ.Consumer.csproj", "PayCoreFinalProject.RabbitMQ.Consumer/"]
RUN dotnet restore "PayCoreFinalProject.RabbitMQ.Consumer/PayCoreFinalProject.RabbitMQ.Consumer.csproj"
COPY . .
WORKDIR "/src/PayCoreFinalProject.RabbitMQ.Consumer"
RUN dotnet build "PayCoreFinalProject.RabbitMQ.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PayCoreFinalProject.RabbitMQ.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PayCoreFinalProject.RabbitMQ.Consumer.dll"]
