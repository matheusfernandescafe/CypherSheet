# GitHub Actions Workflows

Este diretório contém o workflow do GitHub Actions para deploy do CypherSheet PWA no GitHub Pages.

## Workflow

### Deploy to GitHub Pages (`deploy.yml`)
- **Trigger**: Push para `main`, Pull Request, ou execução manual
- **Função**: Build, test e deploy automático para GitHub Pages
- **URL**: `https://[username].github.io/[repository-name]/`
- **Configuração necessária**: Habilitar GitHub Pages no repositório
- **Base Href**: Automaticamente ajustado para o nome do repositório

## Configuração

### Habilitar GitHub Pages
1. Vá para **Settings** → **Pages** no seu repositório
2. Em **Source**, selecione **GitHub Actions**
3. O workflow será executado automaticamente no próximo push para `main`

## Configuração do Base Href

O workflow automaticamente ajusta o `base href` no `index.html` para corresponder ao nome do repositório:

- **Repositório**: `usuario/CypherSheet` → **Base href**: `/CypherSheet/`
- **URL final**: `https://usuario.github.io/CypherSheet/`

Isso garante que todos os recursos (CSS, JS, imagens) sejam carregados corretamente no GitHub Pages.

## Estrutura de Deploy

O workflow faz build, test e deploy da aplicação Blazor WebAssembly para GitHub Pages.

### O que o workflow faz:
1. **Build**: Compila a aplicação em modo Release
2. **Test**: Executa todos os testes do projeto
3. **Publish**: Gera os arquivos estáticos otimizados
4. **Deploy**: Publica no GitHub Pages com base href correto

### Características do Build:
- **.NET 10**: Versão mais recente do framework
- **Release Configuration**: Otimizado para produção
- **PWA Ready**: Inclui Service Worker e Web App Manifest
- **Offline-First**: Funciona sem conexão com internet
- **Base Href**: Automaticamente ajustado para GitHub Pages

## Personalização

Para personalizar o workflow:

1. **Branch Protection**: Modifique os triggers conforme necessário
2. **Testes**: Adicione projetos de teste conforme necessário
3. **Build Configuration**: Ajuste configurações de build se necessário

## Monitoramento

- Verifique a aba "Actions" do GitHub para acompanhar builds e deploys
- Logs detalhados estão disponíveis para debugging
- O deploy só acontece quando o push é feito na branch `main`