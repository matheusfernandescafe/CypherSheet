// GitHub Pages SPA: restore the path encoded by 404.html
(function () {
    var params = new URLSearchParams(window.location.search);
    var path = params.get('p');
    if (path) {
        var query = params.get('q') ? '?' + params.get('q') : '';
        var hash = params.get('h') ? '#' + params.get('h') : '';
        // Replace the current history entry with the real URL
        window.history.replaceState(
            null, null,
            window.location.pathname + path + query + hash
        );
    }
})();

// Service Worker registration
navigator.serviceWorker.register('service-worker.js', { updateViaCache: 'none' });

// Adiciona classe blazor-loaded quando o Blazor termina de carregar
// Isso permite que os estilos CSS transicionem da loading screen para a aplicação
(function () {
    // Aguarda o Blazor carregar completamente
    function waitForBlazorLoad() {
        // Verifica se o Blazor já carregou (presença de componentes Blazor) ou se houve erro
        const isErrorVisible = document.getElementById('blazor-error-ui') && 
                               window.getComputedStyle(document.getElementById('blazor-error-ui')).display !== 'none';
        
        if (document.querySelector('.mud-layout') || 
            document.querySelector('[data-blazor-component]') || 
            isErrorVisible) {
            // Adiciona as classes necessárias para transição
            document.documentElement.classList.add('blazor-loaded');
            document.body.classList.add('blazor-loaded');
            return;
        }
        
        // Se ainda não carregou, tenta novamente em 50ms
        setTimeout(waitForBlazorLoad, 50);
    }
    
    // Inicia a verificação após um pequeno delay para dar tempo do Blazor inicializar
    setTimeout(waitForBlazorLoad, 100);
})();