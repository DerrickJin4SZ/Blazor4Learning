// Use it for .NET 6+
export function BlazorDownloadFile(filename, contentType, content) {
    // Create the URL
    const file = new File([content], filename, { type: contentType });
    const exportUrl = URL.createObjectURL(file);

    // Create the <a> element and click on it
    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = filename;
    a.target = "_self";
    a.click();

    // We don't need to keep the object URL, let's release the memory
    // On older versions of Safari, it seems you need to comment this line...
    URL.revokeObjectURL(exportUrl);
}


export async function createCanvas(elementId) {
    var element = document.getElementById(elementId);

    // Convert the HTML element to a canvas using html2canvas library
    var imageData = null;
    await html2canvas(element).then(function (canvas) {
        // Convert the canvas to a data URL (Base64-encoded string)
        imageData = canvas.toDataURL("image/png");
        console.log('createCanvas - JS: image data type = ', typeof imageData);
        
    });
    return imageData;
}