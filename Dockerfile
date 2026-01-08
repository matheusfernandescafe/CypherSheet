# =========================
# Stage 1 - Build Blazor WASM
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia tudo
COPY . .

# Publica o projeto Blazor
RUN dotnet publish src/CypherSheet.Client/CypherSheet.Client.csproj \
    -c Release \
    -o /app/publish

# =========================
# Stage 2 - Static web server
# =========================
FROM nginx:alpine

# Remove config padrão
RUN rm /etc/nginx/conf.d/default.conf

# Copia config customizada
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copia o site para simular estrutura do GitHub Pages
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html/CypherSheet

EXPOSE 80