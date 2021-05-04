# build
FROM mcr.microsoft.com/dotnet/sdk:5.0.202-alpine3.13-amd64 AS builder
WORKDIR /app
COPY . ./
RUN dotnet publish -c Release -o /out

# run
FROM mcr.microsoft.com/dotnet/runtime:5.0.5-alpine3.13-amd64
WORKDIR /app
COPY --from=builder /out .

# Healthcheck
RUN apk --no-cache add curl
HEALTHCHECK --timeout=3s --interval=10s \
    CMD curl -s --fail http://127.0.0.1:8080/ || exit 1

# User
USER 1000:1000

ENTRYPOINT ["dotnet", "TestApp1.dll"]