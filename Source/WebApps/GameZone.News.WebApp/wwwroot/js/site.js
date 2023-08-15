(function(){
    'use strict';
    var $responsive = document.querySelectorAll('.w3-responsive');
    var $table = document.querySelectorAll('.w3-table');
    var $tds = document.querySelectorAll('td');
    var $m5 = document.querySelectorAll('.m5');
    var $m3 = document.querySelectorAll('.m3');
    var $m7 = document.querySelectorAll('.m7');
    var $mm2 = document.querySelectorAll('.mm2');
    //var $btnprint = document.querySelector("#js-btn-print");
    //$btnprint.addEventListener("click", function(){
    //    window.print();
    //});
    window.addEventListener('beforeprint', function(){
        
        for(var i = 0; i <  $responsive.length; i++){
            $responsive[i].style.overflowX = "initial";
        }
        for(var i = 0; i < $tds.length; i++){                   
            $tds[i].classList.remove("w3-hideTable-small");
            $tds[i].classList.remove("color-white1");
            $tds[i].classList.remove("color-white2");
            $tds[i].classList.remove("color-white3");
            $tds[i].classList.remove("color-white4");
            $tds[i].classList.remove("color-white5");
            $tds[i].classList.remove("color-grey1");
            $tds[i].classList.remove("color-grey2");
            $tds[i].classList.remove("color-grey3");
            $tds[i].classList.remove("color-grey4");
            
        }
        for(var i = 0; i < $table.length; i++){                   
            $table[i].classList.remove("w3-striped-mobile");
            $table[i].classList.remove("w3-table-mobile");
            $table[i].classList.remove("w3-table-line");
            $table[i].classList.add("w3-striped");
         }
         for(var i = 0; i <  $m5.length; i++){
            $m5[i].classList.add("s4");
        }
        for(var i = 0; i <  $m3.length; i++){
            $m3[i].classList.add("s3");
        }
        for(var i = 0; i <  $m7.length; i++){
            $m7[i].classList.remove("s4");
            $m7[i].classList.add("s3");
        }
        for(var i = 0; i <  $mm2.length; i++){            
            $mm2[i].classList.add("s4");
        }
    });
    window.addEventListener('afterprint', function(){
        document.location.reload(true);
    });
    
})();