function testNetStatic() {
    DotNet.invokeMethodAsync("Peliculas.Client", "IncrementCountStatic")
        .then(result => {
            console.log('Conteo desde JS: ' + result);
        })
}