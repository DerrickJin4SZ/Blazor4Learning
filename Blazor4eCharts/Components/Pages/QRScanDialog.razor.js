// QRScan.razor.js

let video;
let dotNetReference;
let scanTimeout;
export function scanQRCode(videoElementId, dotNetRef) {
    video = document.getElementById(videoElementId);
    video.setAttribute('autoplay', '');
    video.setAttribute('muted', '');
    video.setAttribute('playsinline', '')
    dotNetReference = dotNetRef;
    startCamera();
    // Set a timeout to stop scanning after 10 seconds
    /*
    scanTimeout = setTimeout(() => {
        stop();
        dotNetReference.invokeMethodAsync('NotifyCameraIdle');
    }, 100000); // 10 seconds
    */
}

function startCamera() {
    navigator.mediaDevices.getUserMedia({ video: { facingMode: "environment" } })
        .then(function (stream) {
            video.srcObject = stream;
            video.play();
            scan();
        })
        .catch(function (error) {
            console.error("Camera access error:", error);
            dotNetReference.invokeMethodAsync('NotifyCameraError', error.message || "No camera available");
        });
}

function scan() {
    if (video.readyState === video.HAVE_ENOUGH_DATA) {
        const canvas = document.createElement('canvas');
        canvas.width = video.videoWidth*4;
        canvas.height = video.videoHeight*4;
        const context = canvas.getContext('2d');
        context.drawImage(video, 0, 0, canvas.width, canvas.height);
        context.scale(2, 2);
        var imageData = context.getImageData(0, 0, canvas.width, canvas.height);
        var code = jsQR(imageData.data, imageData.width, imageData.height, {
            inversionAttempts: "dontInvert",
        });
        if (code && code.data && code.data.trim() !== '') {
            alert(code);
            clearTimeout(scanTimeout);
            dotNetReference.invokeMethodAsync('NotifyQRCodeRead', code.data)
                .then(stop);
        }
    }
    // Use setTimeout to control the frequency of scans
    setTimeout(() => {
        requestAnimationFrame(scan);
    }, 100); // Adjust the delay as needed
}

function stop() {
    if (video && video.srcObject) {
        video.srcObject.getTracks().forEach(track => track.stop());
    }
}

export function stopCamera() {
    stop();
}

export function restartCamera() {
    if (video) {
        startCamera();
    }
}
