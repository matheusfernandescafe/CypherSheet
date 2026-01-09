// Intersection Observer para lazy loading de imagens
window.createIntersectionObserver = function(dotNetRef, element) {
    try {
        // Verificar se o Intersection Observer é suportado
        if (!('IntersectionObserver' in window)) {
            console.warn('IntersectionObserver não suportado, carregando imagem imediatamente');
            // Fallback: chamar callback imediatamente
            dotNetRef.invokeMethodAsync('OnIntersection', true);
            return {
                disconnect: function() {},
                observe: function() {},
                unobserve: function() {}
            };
        }

        const options = {
            root: null,
            rootMargin: '50px', // Carregar imagem 50px antes de aparecer
            threshold: 0.1
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                try {
                    if (entry.isIntersecting) {
                        dotNetRef.invokeMethodAsync('OnIntersection', true);
                        observer.unobserve(entry.target);
                    }
                } catch (error) {
                    console.error('Erro no callback do IntersectionObserver:', error);
                    // Fallback: tentar chamar o callback mesmo com erro
                    try {
                        dotNetRef.invokeMethodAsync('OnIntersection', true);
                    } catch (fallbackError) {
                        console.error('Erro no fallback do IntersectionObserver:', fallbackError);
                    }
                }
            });
        }, options);

        // Verificar se o elemento é válido
        if (!element) {
            console.warn('Elemento não encontrado para IntersectionObserver, carregando imediatamente');
            dotNetRef.invokeMethodAsync('OnIntersection', true);
            return {
                disconnect: function() {},
                observe: function() {},
                unobserve: function() {}
            };
        }

        observer.observe(element);

        // Timeout de segurança - se não intersectar em 10 segundos, carregar mesmo assim
        setTimeout(() => {
            try {
                dotNetRef.invokeMethodAsync('OnIntersection', true);
            } catch (error) {
                console.error('Erro no timeout do IntersectionObserver:', error);
            }
        }, 10000);

        return observer;

    } catch (error) {
        console.error('Erro ao criar IntersectionObserver:', error);
        
        // Fallback completo: carregar imagem imediatamente
        try {
            dotNetRef.invokeMethodAsync('OnIntersection', true);
        } catch (fallbackError) {
            console.error('Erro no fallback completo:', fallbackError);
        }

        return {
            disconnect: function() {},
            observe: function() {},
            unobserve: function() {}
        };
    }
};

// Função auxiliar para verificar se um elemento está visível
window.isElementVisible = function(element) {
    try {
        if (!element) return false;
        
        const rect = element.getBoundingClientRect();
        const windowHeight = window.innerHeight || document.documentElement.clientHeight;
        const windowWidth = window.innerWidth || document.documentElement.clientWidth;
        
        return (
            rect.top < windowHeight &&
            rect.bottom > 0 &&
            rect.left < windowWidth &&
            rect.right > 0
        );
    } catch (error) {
        console.error('Erro ao verificar visibilidade do elemento:', error);
        return true; // Assumir visível em caso de erro
    }
};