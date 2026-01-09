window.createIntersectionObserver = (dotNetHelper, element) => {
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                dotNetHelper.invokeMethodAsync('OnIntersection', true);
                observer.unobserve(entry.target);
            }
        });
    }, {
        rootMargin: '50px', // Começar a carregar 50px antes de entrar na viewport
        threshold: 0.1
    });

    observer.observe(element);
    
    return {
        disconnect: () => observer.disconnect()
    };
};