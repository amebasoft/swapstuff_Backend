/** 16. Bottom Footer
 **************************************************************** **/
if (!jQuery("body").hasClass("boxed")) {

    jQuery(window).load(function () {

        function _bottomFooter() {
            jQuery("#footer").removeClass("bottom");

            var _h = parseInt(jQuery(document).height()),
                _wh = parseInt(jQuery("#wrapper").height());


            if (_h > _wh) {

                jQuery("#footer").addClass("bottom");


            } else {

                jQuery("#footer").removeClass("bottom");

            }

        } _bottomFooter();


        // On Resize
        jQuery(window).resize(function () {

            if (window.afterResize) {
                clearTimeout(window.afterResize);
            }

            window.afterResize = setTimeout(function () {

                /**
                    After Resize Code
                    .................
                **/	_bottomFooter();

            }, 500);

        });

    });
}
