(function ($) {

    $.cookieConsent = function (userConfig) {

        if (!userConfig) {
            userConfig = {};
        }

        var config = {
            mode: 'default', // Default, tab or popover
            persistence: 'heavy', // Light (hides after viewed once) or Heavy (requires user to interact/close)
            color: {
                main: '#29f', // Border & icon color
                bg: '#fff', // Background color
                popover: 'rgba(0,0,0,.3)', // Popover background color
                text: '#444' // Text color
            },
            font: '12px Tahoma, sans-serif', // Font size & family
            slideSpeed: 'fast', // Fast, Slow, or number e.g. 300 or 0 (no animation)
            width: 'auto', // Width of the banner
            maxWidth: '50%', // Responsiveness
            link: {
                cookies: "http://en.wikipedia.org/wiki/HTTP_cookie", // The cookies link
                policy: null // The cookie policy link
            }
        };

        if (localStorage.getItem('cookieConsent') === 'yes') {
            $('html').addClass('cookie-consent-given');
            $('#cookie-consent-wrapper').slideDown(config.slideSpeed);
            return;
        }

        config.content =
        { // Allows you to specify the text content of the plugin, using an aray & object based syntax (explained below)
            heading:
              ['strong', { content: 'This site uses ' },
                ['a', { href: config.link.cookies, content: 'cookies.', target: '_blank' }]
              ],
            text:
              // If config.link.policy evaluates to true, include a link.
              // Ternary operator is somewhat messy here.
              ['p', { content: 'By using this site you agree to our ' + (!!config.link.policy ? '' : 'cookie policy.') },
                (!!config.link.policy ? ['a', { href: config.link.policy, content: 'cookie policy.' }] : [])
              ]
        };

        // Build new DOM element from an object
        var buildElement = function buildElement(elemConfig) {

            var i, l;

            // First array element is the new DOM element tag
            var temp = document.createElement(elemConfig[0]);

            // Second is an object of attributes
            // 'content' attribute is inserted as element content
            if (elemConfig[1]) {
                var attr = elemConfig[1];
                for (i in attr) {
                    if (attr.hasOwnProperty(i)) {
                        if (i === 'content') {
                            $(temp).text(attr[i]);
                        } else if (i === 'css') {
                            $(temp).css(attr[i]);
                        } else {
                            temp.setAttribute(i, attr[i]);
                        }
                    }
                }
            }

            // Third or greater are child elements
            if (elemConfig.length > 2) {
                i = 2, l = elemConfig.length;
                for (; i < l; i++) {
                    // Allow a string to be passed in
                    if (typeof elemConfig[i] === "string") {
                        $(temp).html($(temp).html() + elemConfig[i]);
                    } else {
                        temp.appendChild(buildElement(elemConfig[i]));
                    }
                }
            }

            return temp;

        };

        // Extend our config file using the user's config
        $.extend(true, config, userConfig);

        // The cookieConsent element
        // Sytanx is:
        //  [element [string], attributes [object], childElements... [arrays]]
        // Special attributes:
        //  content - inserted as text content of the element
        //  css - object run through jQuery's css method
        var cookieElement =
        ['div', { id: "cookie-consent-wrapper" },
          ['div', { id: "cookie-consent" },
            ['p', {},
              config.content.heading
            ],
            config.content.text,
            ['a', { id: "cookie-close", href: '#', content: 'Close' }]
          ]
        ];

        // Get HTML Element
        var html_elem = $('html');

        // Build the element
        var elem = buildElement(cookieElement);

        if (config.persistence === "light") { // Light, hide after first view
            localStorage.setItem('cookieConsent', 'yes');
        }

        // Set up quit behaviour
        $(elem).on('click', '#cookie-close', function () {
            localStorage.setItem('cookieConsent', 'yes');
            $(elem).slideUp(config.slideSpeed, function () {
                elem.parentNode.removeChild(elem);
                $('html').addClass('cookie-consent-given');
            });
        });

        $(elem).appendTo('body');
    };

}(jQuery));

$(document).ready(function () {
    $.cookieConsent();
});