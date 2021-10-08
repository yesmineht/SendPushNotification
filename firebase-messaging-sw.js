importScripts('https://www.gstatic.com/firebasejs/5.8.5/firebase-app.js');

importScripts('https://www.gstatic.com/firebasejs/5.8.5/firebase-messaging.js');



firebase.initializeApp({

    'messagingSenderId': '543978709723'

});



const messaging = firebase.messaging();

messaging.setBackgroundMessageHandler(function (payload) {

    const notificationTitle = 'Background Message from html';

    const notificationOptions = {

        body: 'Background Message body.',

        icon: '/firebase-logo.png'



    };

    





});





//self.addEventListener('push', function (event) {

//    console.log('Received a push message', event);



//    var title = 'Yay a message.';

//    var body = 'We have received a push message.';

//    var icon = '/images/icon-192x192.png';

//    var tag = 'simple-push-demo-notification-tag';



//    event.waitUntil(

//        self.registration.showNotification(title, {

//            body: body,

//            icon: icon,

//            tag: tag

//        })

//    );

//});



self.addEventListener('push', function (event) {

     //Since there is no payload data with the first version

     //of push messages, we'll grab some data from

     //an API and use it to populate a notification

    event.waitUntil(

        fetch(SOME_API_ENDPOINT).then(function (response) {

            if (response.status !== 200) {

                // Either show a message to the user explaining the error

                // or enter a generic message and handle the

                // onnotificationclick event to direct the user to a web page

                console.log('Looks like there was a problem. Status Code: ' + response.status);

                throw new Error();

            }



            // Examine the text in the response

            return response.json().then(function (data) {

                if (data.error || !data.notification) {

                    console.error('The API returned an error.', data.error);

                    throw new Error();

                }



                var title = data.notification.title;

                var message = data.notification.message;

                var icon = data.notification.icon;

                var notificationTag = data.notification.tag;

                var click_action = data.notification.click_action;


                return self.registration.showNotification(title, {

                    body: message,

                    icon: icon,

                    tag: notificationTag,
                },


                    { click_action: click_action } 

                );

            });

        }).catch(function (err) {

            console.error('Unable to retrieve data', err);



            var title = 'An error occurred';

            var message = 'We were unable to get the information for this push message';

            var icon = URL_TO_DEFAULT_ICON;

            var notificationTag = 'notification-error';

            return self.registration.showNotification(title, {

                body: message,

                icon: icon,

                tag: notificationTag
            },

                { click_action: click_action }
            );

        })

    );

});

