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

# Corrige o nome do arquivo blazor.webassembly.js no index.html
RUN cd /app/publish/wwwroot && \
    echo "=== Listando arquivos em _framework ===" && \
    ls -la _framework/ | grep blazor && \
    echo "=== Conteúdo do index.html antes ===" && \
    grep "blazor.webassembly" index.html && \
    BLAZOR_FILE=$(find _framework/ -name "blazor.webassembly*.js" -type f | head -1 | xargs basename) && \
    echo "Arquivo encontrado: $BLAZOR_FILE" && \
    sed -i "s|blazor.webassembly.js|$BLAZOR_FILE|g" index.html && \
    echo "=== Conteúdo do index.html depois ===" && \
    grep "blazor.webassembly" index.html

# =========================
# Stage 2 - Static web server
# =========================
FROM nginx:alpine

# Remove config padrão
RUN rm /etc/nginx/conf.d/default.conf

# Copia config customizada
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copia o site para a raiz (Docker padrão)
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

EXPOSE 80