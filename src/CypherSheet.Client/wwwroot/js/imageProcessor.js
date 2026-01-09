// Processador de imagens para CypherSheet
window.imageProcessor = {
    // Processa uma imagem (redimensiona e comprime)
    processImage: async function(base64Image, maxWidth, maxHeight, quality) {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.onload = function() {
                try {
                    // Validar dimensões da imagem
                    if (img.width <= 0 || img.height <= 0) {
                        reject(new Error('Dimensões da imagem inválidas'));
                        return;
                    }

                    // Validar se a imagem não é muito grande (proteção adicional)
                    if (img.width > 10000 || img.height > 10000) {
                        reject(new Error('Imagem muito grande para processar (máximo 10000x10000 pixels)'));
                        return;
                    }

                    const canvas = document.createElement('canvas');
                    const ctx = canvas.getContext('2d');
                    
                    if (!ctx) {
                        reject(new Error('Não foi possível criar contexto de canvas'));
                        return;
                    }
                    
                    // Calcular novas dimensões mantendo proporção
                    const { width, height } = calculateDimensions(img.width, img.height, maxWidth, maxHeight);
                    
                    canvas.width = width;
                    canvas.height = height;
                    
                    // Limpar canvas e configurar qualidade de renderização
                    ctx.clearRect(0, 0, width, height);
                    ctx.imageSmoothingEnabled = true;
                    ctx.imageSmoothingQuality = 'high';
                    
                    // Desenhar imagem redimensionada
                    ctx.drawImage(img, 0, 0, width, height);
                    
                    // Validar qualidade
                    if (quality < 0.1 || quality > 1.0) {
                        quality = 0.85; // Usar valor padrão se inválido
                    }
                    
                    // Converter para base64 com qualidade especificada
                    const processedBase64 = canvas.toDataURL('image/jpeg', quality);
                    
                    // Verificar se a conversão foi bem-sucedida
                    if (!processedBase64 || processedBase64 === 'data:,') {
                        reject(new Error('Falha ao processar imagem'));
                        return;
                    }
                    
                    // Remover prefixo data:image/jpeg;base64,
                    const base64Data = processedBase64.split(',')[1];
                    
                    if (!base64Data) {
                        reject(new Error('Dados da imagem processada inválidos'));
                        return;
                    }
                    
                    resolve(base64Data);
                } catch (error) {
                    reject(new Error(`Erro no processamento: ${error.message}`));
                }
            };
            
            img.onerror = function() {
                reject(new Error('Erro ao carregar imagem - arquivo pode estar corrompido'));
            };
            
            // Timeout para evitar travamento
            setTimeout(() => {
                reject(new Error('Timeout no processamento da imagem'));
            }, 30000); // 30 segundos
            
            img.src = 'data:image/jpeg;base64,' + base64Image;
        });
    },

    // Converte imagem para WebP se suportado
    convertToWebP: async function(base64Image, quality) {
        return new Promise((resolve, reject) => {
            if (!this.isWebPSupported()) {
                resolve(base64Image); // Retorna original se WebP não for suportado
                return;
            }
            
            const img = new Image();
            img.onload = function() {
                try {
                    // Validar dimensões
                    if (img.width <= 0 || img.height <= 0) {
                        reject(new Error('Dimensões da imagem inválidas para conversão WebP'));
                        return;
                    }

                    const canvas = document.createElement('canvas');
                    const ctx = canvas.getContext('2d');
                    
                    if (!ctx) {
                        reject(new Error('Não foi possível criar contexto para conversão WebP'));
                        return;
                    }
                    
                    canvas.width = img.width;
                    canvas.height = img.height;
                    
                    // Configurar qualidade de renderização
                    ctx.imageSmoothingEnabled = true;
                    ctx.imageSmoothingQuality = 'high';
                    
                    ctx.drawImage(img, 0, 0);
                    
                    // Validar qualidade
                    if (quality < 0.1 || quality > 1.0) {
                        quality = 0.85;
                    }
                    
                    // Converter para WebP
                    const webpBase64 = canvas.toDataURL('image/webp', quality);
                    
                    // Verificar se a conversão foi bem-sucedida
                    if (!webpBase64 || !webpBase64.startsWith('data:image/webp')) {
                        reject(new Error('Falha na conversão para WebP'));
                        return;
                    }
                    
                    // Remover prefixo data:image/webp;base64,
                    const base64Data = webpBase64.split(',')[1];
                    
                    if (!base64Data) {
                        reject(new Error('Dados WebP inválidos'));
                        return;
                    }
                    
                    resolve(base64Data);
                } catch (error) {
                    reject(new Error(`Erro na conversão WebP: ${error.message}`));
                }
            };
            
            img.onerror = function() {
                reject(new Error('Erro ao carregar imagem para conversão WebP'));
            };
            
            // Timeout para conversão WebP
            setTimeout(() => {
                reject(new Error('Timeout na conversão WebP'));
            }, 15000); // 15 segundos
            
            img.src = 'data:image/jpeg;base64,' + base64Image;
        });
    },

    // Verifica se o navegador suporta WebP
    isWebPSupported: function() {
        const canvas = document.createElement('canvas');
        canvas.width = 1;
        canvas.height = 1;
        
        try {
            const webpData = canvas.toDataURL('image/webp');
            return webpData.indexOf('data:image/webp') === 0;
        } catch {
            return false;
        }
    }
};

// Função auxiliar para calcular dimensões mantendo proporção
function calculateDimensions(originalWidth, originalHeight, maxWidth, maxHeight) {
    let width = originalWidth;
    let height = originalHeight;
    
    // Se a imagem já está dentro dos limites, não redimensionar
    if (width <= maxWidth && height <= maxHeight) {
        return { width, height };
    }
    
    // Calcular proporção
    const aspectRatio = width / height;
    
    // Redimensionar baseado na maior dimensão
    if (width > height) {
        width = maxWidth;
        height = width / aspectRatio;
        
        // Se altura ainda for maior que o máximo, ajustar pela altura
        if (height > maxHeight) {
            height = maxHeight;
            width = height * aspectRatio;
        }
    } else {
        height = maxHeight;
        width = height * aspectRatio;
        
        // Se largura ainda for maior que o máximo, ajustar pela largura
        if (width > maxWidth) {
            width = maxWidth;
            height = width / aspectRatio;
        }
    }
    
    return { 
        width: Math.round(width), 
        height: Math.round(height) 
    };
}