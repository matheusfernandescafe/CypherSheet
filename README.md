# CypherSheet PWA

Um aplicativo **PWA (Progressive Web App)** moderno e offline-first para gerenciar fichas de personagens do **Cypher System**. Desenvolvido com **Blazor WebAssembly** e **MudBlazor**.

## Funcionalidades

*   **Offline-First**: Funciona sem internet. Seus dados moram no seu dispositivo (IndexedDB).
*   **Mobile-First**: Interface desenhada para uso em celulares durante sessões de RPG.
*   **PWA Instalável**: Instale no iOS (Safari/Brave) ou Windows como um aplicativo nativo.
*   **Regras do Cypher System**:
    *   Pools de Might, Speed, Intellect com cálculo de Edge.
    *   Rastreamento automático de estados (Hale, Impaired, Debilitated).
    *   Gerenciamento de Recovery Rolls.

## Tecnologias

*   **.NET 10**
*   **Blazor WebAssembly** (Hosting Model)
*   **MudBlazor** (Component Library)
*   **IndexedDB** (Persistência Local via `TG.Blazor.IndexedDB`)
*   **Clean Architecture**:
    *   `Domain`: Regras de negócio puras.
    *   `Storage`: Implementação de persistência.
    *   `Client`: Interface do Usuário (MudBlazor).
    *   `Shared`: Contratos comuns.

## Como Rodar

Pré-requisitos: .NET SDK 10.0+ instalado.

1. Clone o repositório:
   ```bash
   git clone https://github.com/matheusfernandescafe/CypherSheet
   cd CypherSheet
   ```

2. Execute o projeto do Client:
   ```bash
   dotnet watch run --project CypherSheet.Client
   ```

3. Acesse no navegador:
   *   Geralmente: `http://localhost:5031` ou similar (verifique o output do terminal).

## Instalação PWA

*   **Desktop (Chrome/Edge/Brave)**: Clique no ícone de instalação na barra de endereço.
*   **iOS (Safari/Brave)**: Toque em "Compartilhar" -> "Adicionar à Tela de Início".
*   **Android (Chrome/Brave)**: Toque no menu (três pontos) -> "Instalar aplicativo".

## Estrutura do Projeto

```
/CypherSheet
 └─ src
     ├─ CypherSheet.Domain   # Entidades e Lógica (Character, Pool)
     ├─ CypherSheet.Storage  # Repositório IndexedDB
     ├─ CypherSheet.Client   # UI Blazor/MudBlazor e Configuração PWA
     └─ CypherSheet.Shared   # DTOs e Interfaces compartilhadas
```

## Notas
Este projeto foi criado para fins de estudo de arquitetura Blazor PWA e uso pessoal em mesas de RPG.
