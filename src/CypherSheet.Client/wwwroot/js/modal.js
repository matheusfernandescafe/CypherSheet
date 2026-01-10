// Funções para suporte ao modal de imagem do personagem

// Adicionar listener para tecla ESC
window.addEscapeKeyListener = function(dotNetRef) {
    try {
        const handleEscapeKey = function(event) {
            if (event.key === 'Escape' || event.keyCode === 27) {
                event.preventDefault();
                event.stopPropagation();
                
                try {
                    dotNetRef.invokeMethodAsync('HandleEscapeKey');
                } catch (error) {
                    console.error('Erro ao chamar HandleEscapeKey:', error);
                }
            }
        };

        // Adicionar listener
        document.addEventListener('keydown', handleEscapeKey);
        
        // Retornar função para remover o listener
        return {
            dispose: function() {
                try {
                    document.removeEventListener('keydown', handleEscapeKey);
                } catch (error) {
                    console.error('Erro ao remover listener de ESC:', error);
                }
            }
        };
        
    } catch (error) {
        console.error('Erro ao adicionar listener de ESC:', error);
        return {
            dispose: function() {}
        };
    }
};

// Remover listener para tecla ESC
window.removeEscapeKeyListener = function(listenerRef) {
    try {
        if (listenerRef && typeof listenerRef.dispose === 'function') {
            listenerRef.dispose();
        }
    } catch (error) {
        console.error('Erro ao remover listener de ESC:', error);
    }
};

// Função para prevenir scroll da página quando modal estiver aberto
window.preventBodyScroll = function(prevent) {
    try {
        if (prevent) {
            document.body.style.overflow = 'hidden';
            document.documentElement.style.overflow = 'hidden';
        } else {
            document.body.style.overflow = '';
            document.documentElement.style.overflow = '';
        }
    } catch (error) {
        console.error('Erro ao controlar scroll do body:', error);
    }
};

// Função para focar no botão de fechar quando modal abrir (acessibilidade)
window.focusCloseButton = function(selector) {
    try {
        setTimeout(() => {
            const closeButton = document.querySelector(selector || '.close-button');
            if (closeButton && typeof closeButton.focus === 'function') {
                closeButton.focus();
            }
        }, 100); // Pequeno delay para garantir que o elemento foi renderizado
    } catch (error) {
        console.error('Erro ao focar no botão de fechar:', error);
    }
};

// Função para detectar clique fora do modal (se necessário no futuro)
window.addClickOutsideListener = function(dotNetRef, modalSelector) {
    try {
        const handleClickOutside = function(event) {
            const modal = document.querySelector(modalSelector || '.mud-dialog');
            if (modal && !modal.contains(event.target)) {
                event.preventDefault();
                event.stopPropagation();
                
                try {
                    dotNetRef.invokeMethodAsync('HandleClickOutside');
                } catch (error) {
                    console.error('Erro ao chamar HandleClickOutside:', error);
                }
            }
        };

        // Adicionar listener com delay para evitar fechamento imediato
        setTimeout(() => {
            document.addEventListener('click', handleClickOutside);
        }, 100);
        
        // Retornar função para remover o listener
        return {
            dispose: function() {
                try {
                    document.removeEventListener('click', handleClickOutside);
                } catch (error) {
                    console.error('Erro ao remover listener de clique fora:', error);
                }
            }
        };
        
    } catch (error) {
        console.error('Erro ao adicionar listener de clique fora:', error);
        return {
            dispose: function() {}
        };
    }
};