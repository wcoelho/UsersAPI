    <div class="container">
      <div id="login" class="text-center">
        <h4>Precisamos que você faça login no Spotify para oferecermos uma experiência incrível!</h4>
           <br>
            <button id="login-button" class="btn btn-primary">Login</button>

      </div>
      <div id="loggedin">
        <div id="user-profile">
        </div>
        <div id="oauth">
        </div>
      </div>
    </div>

    <script id="user-profile-template" type="text/x-handlebars-template">
      <h1>Logado como {{display_name}}</h1>
      <div class="media">
        <div class="pull-left">
          <img class="media-object" width="150" src="{{images.0.url}}" />
        </div>
        <div class="media-body">
          <dl class="dl-horizontal">
            <dt>Nome</dt><dd class="clearfix">{{display_name}}</dd>
            <dt>Id</dt><dd>{{id}}</dd>
            <dt>Email</dt><dd>{{email}}</dd>
            <dt>URI do Spotify</dt><dd><a href="{{external_urls.spotify}}">{{external_urls.spotify}}</a></dd>
            <dt>Link</dt><dd><a href="{{href}}">{{href}}</a></dd>
            <dt>Imagem do Perfil</dt><dd class="clearfix"><a href="{{images.0.url}}">{{images.0.url}}</a></dd>
            <dt>País</dt><dd>{{country}}</dd>
          </dl>
          <p style="text-align:center">
            <button id="getin-button" class="btn btn-primary" onclick="getIn('{{email}}')">Entrar</button>
          </p>
        </div>
      </div>
    </script>

    <script id="oauth-template" type="text/x-handlebars-template">
      <h2>Informação do oAuth</h2>
      <dl class="dl-horizontal">
        <dt>Token de Acesso</dt><dd class="text-overflow">{{access_token}}</dd>
      </dl>
    </script>

    <script src="//cdnjs.cloudflare.com/ajax/libs/handlebars.js/2.0.0-alpha.1/handlebars.min.js"></script>
    <script src="https://code.jquery.com/jquery-1.10.1.min.js"></script>
    <script>
        function getIn(email) {
          var url = 'https://localhost:5001/api/v1/usuarios/';

          window.location = url+"?email=" + email + "&access_token="+ window.accessToken;              
        }
    </script>
    <script>
      (function() {

        var stateKey = 'spotify_auth_state';
        var accessToken = 'access_token';
        function getHashParams() {
          var hashParams = {};
          var e, r = /([^&;=]+)=?([^&;]*)/g,
              q = window.location.hash.substring(1);
          while ( e = r.exec(q)) {
             hashParams[e[1]] = decodeURIComponent(e[2]);
          }
          return hashParams;
        }

        function generateRandomString(length) {
          var text = '';
          var possible = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';

          for (var i = 0; i < length; i++) {
            text += possible.charAt(Math.floor(Math.random() * possible.length));
          }
          return text;
        };

        var userProfileSource = document.getElementById('user-profile-template').innerHTML,
            userProfileTemplate = Handlebars.compile(userProfileSource),
            userProfilePlaceholder = document.getElementById('user-profile');

            oauthSource = document.getElementById('oauth-template').innerHTML,
            oauthTemplate = Handlebars.compile(oauthSource),
            oauthPlaceholder = document.getElementById('oauth');

        var params = getHashParams();

        var access_token = params.access_token,            
            state = params.state,
            storedState = localStorage.getItem(stateKey);

        window.accessToken = access_token;

        localStorage.setItem(accessToken,access_token);

        if (access_token && (state == null || state !== storedState)) {
          alert('Houve um erro durante a autenticação');
        } else {
          localStorage.removeItem(stateKey);
          if (access_token) {
        
            $.ajax({
                url: 'https://api.spotify.com/v1/me',
                headers: {
                  'Authorization': 'Bearer ' + access_token
                },
                success: function(response) {
                  userProfilePlaceholder.innerHTML = userProfileTemplate(response);

                  $('#login').hide();
                  $('#loggedin').show();
                }
            });
          } else {
              $('#login').show();
              $('#loggedin').hide();
          }

          document.getElementById('login-button').addEventListener('click', function() {

            var client_id = '135ea3abc6e44233aedde94780926052'; // Your client id
            var redirect_uri = 'https://localhost:5001'; // Your redirect uri

            var state = generateRandomString(16);

            localStorage.setItem(stateKey, state);
            var scope = 'user-read-private user-read-email';

            var url = 'https://accounts.spotify.com/authorize';
            url += '?response_type=token';
            url += '&client_id=' + encodeURIComponent(client_id);
            url += '&scope=' + encodeURIComponent(scope);
            url += '&redirect_uri=' + encodeURIComponent(redirect_uri);
            url += '&state=' + encodeURIComponent(state);

            window.location = url;
          }, false);
        }        

      })();
    </script>
    
