function testNetStatic() {
    DotNet.invokeMethodAsync("Peliculas.Client", "IncrementCountStatic")
        .then(result => {
            console.log('Conteo desde JS: ' + result);
        })
}

function testNetInstancia(dotNetHelper) {
    dotNetHelper.invokeMethodAsync("IncrementCount");        
}

function timerInactivo(dotNetHelper){
    var timer;
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer(){
        clearTimeout(timer);
        timer = setTimeout(logout, 60*1000) //3 segundos
    }

    function logout() {
        dotNetHelper.invokeMethodAsync("Logout");
    }
}