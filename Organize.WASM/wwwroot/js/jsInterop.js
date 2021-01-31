window.addEventListener('resize', () => {
    // Call the static method OnResize in the Organize assembly
    //console.log('Static resize from js');
    DotNet.invokeMethodAsync("Organize.WASM", "OnResize");
});

window.blazorDimension = {
    getWidth: () => window.innerWidth
};

window.blazorResize = {
    registerReferenceForResizeEvent: (dotnetReference) => {
        console.log(blazorResize.assignments);
        window.addEventListener("resize", () => {
            //console.log('Handle resize from JS');
            dotnetReference.invokeMethodAsync("HandleResize", window.innerWidth, window.innerHeight);
        });
    }
};