<% Response.StatusCode = 500 %>
<!DOCTYPE html>
<html>
<head>
    <title>500</title>
    <meta charset="utf-8" />
    <style>
        body,
        html {
            background: lightgrey;
            position: relative;
            margin: 0;
            width: 100%;
            height: 100%;
            text-align: center;
        }

        #main {
            position: absolute;
            top: 50%;
            left: 50%;
            -webkit-transform: translate(-50%, -50%);
            transform: translate(-50%, -50%);
            color: grey;
            font-family: sans-serif;
            max-width: 640px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -o-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

        .message h1 {
            font-size: 160px;
            margin-bottom: 0px;
        }

        .message h3 {
            font-weight: normal;
        }

        .button {
            text-align: center;
        }

            .button > a {
                display: block;
                color: grey;
                text-decoration: none;
                padding: 15px;
                border: solid 1px grey;
                border-radius: 6px;
                -webkit-transition: 0.4s ease-in-out all;
                transition: 0.4s ease-in-out all;
            }

            .button:hover {
                background: darkgrey;
            }
    </style>
</head>
<body>
    <div id="main">
        <div class="message">
            <h1>Błąd :(</h1>
            <h3>Wystąpił nieoczekiwany błąd. Spróbuj ponownie wykonać czynność</h3>
        </div>
        <div class="button">
            <a href="/" title="home" target="_blank">Strona główna</a>
        </div>
    </div>
</body>
</html>
