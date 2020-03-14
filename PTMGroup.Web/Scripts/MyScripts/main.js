(function() {
  'use strict';
  window.addEventListener('load', function() {
      var forms = document.getElementsByClassName('needs-validation');
      var validation = Array.prototype.filter.call(forms, function(form) {
          form.addEventListener('submit', function(event) {
              if (form.checkValidity() === false) {
                  event.preventDefault();
                  event.stopPropagation();
              }
              form.classList.add('was-validated');
          }, false);
      });
  }, false);
  $('.popup').magnificPopup({
		type: 'inline',
		preloader: false,
		focus: '#name',
		// When elemened is focused, some mobile browsers in some cases zoom in
		// It looks not nice, so we disable it:
		callbacks: {
			beforeOpen: function() {
				if($(window).width() < 700) {
					this.st.focus = false;
				} else {
					this.st.focus = '#name';
				}
			}
		}
  });
  $('.popup-youtube, .popup-vimeo, .popup-gmaps').magnificPopup({
		disableOn: 700,
		type: 'iframe',
		mainClass: 'mfp-fade',
		removalDelay: 160,
		preloader: false,
		fixedContentPos: false
  });
  $("#customers .owl-carousel").owlCarousel({
      loop: true,
      autoplay: true,
      autoplayTimeout: 3000,
      autoplaySpeed: 1000,
      autoplayHoverPause: true,
      margin: 50,
      responsive: {
          0: {
              items: 4
          },
          576: {
              items: 5
          },
          768: {
              items: 6
          },
          992: {
              items: 7
          },
          1200: {
              items: 9
          }
      }
  });
})();