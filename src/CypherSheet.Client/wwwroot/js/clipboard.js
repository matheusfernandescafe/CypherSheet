// Fallback function for copying to clipboard in browsers that don't support navigator.clipboard
window.copyToClipboardFallback = function (text) {
    // Create a temporary textarea element
    const textarea = document.createElement('textarea');
    textarea.value = text;
    textarea.style.position = 'fixed';
    textarea.style.opacity = '0';
    document.body.appendChild(textarea);
    
    // Select and copy the text
    textarea.select();
    textarea.setSelectionRange(0, 99999); // For mobile devices
    
    try {
        document.execCommand('copy');
    } finally {
        document.body.removeChild(textarea);
    }
};