// site.js
(function () {

  /*
  var main = $("#main");
  main.on("mouseenter", function () {
    main.css("background-color", "#888");
  });
  main.on("mouseleave", function () {
    main.css("background-color", "");
  });

  $(".menu li a").on("click", function () {
    alert($(this).text());
    return false;
  });
  */

  var $sidebarAndWrapper = $("#sidebar, #wrapper");

  $("#menuToggle").on("click", function () {
    $sidebarAndWrapper.toggleClass("hide-sidebar");
    if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
      $(this).text("Show Menu");
    } else {
      $(this).text("Hide Menu");
    }
  });

})();