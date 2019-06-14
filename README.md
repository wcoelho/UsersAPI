# UsersAndRatingsAPI
API para Gerenciamento de Usuários e Avaliaçoes de Músicas, com integração ao Spotify para autenticação e captura de Token para acesso a algumas funcionalidades.  

## Inicialização
dotnet run  

## Testes 
Postman Collection - pasta 'Tests'  

## Sequência de ações para autenticação  
1 - Usuário acessa a solução.  
2 - É solicitado o acesso/login ao Spotify.  
3 - Os dados do usuário no Spotify são retornados à solução.  
    - Se o usuário já tem cadastro na solução (verificação feita a partir do email do cadastro do Spotify), serão retornados os dados do seu registro.  
    - Se o usuário não possui cadastro na solução, será retornado um resultado "vazio", ficando a cargo do front-end redirecionar para uma página de cadastro do usuário.  

## Serviços  
URI comum: https://localhost:5001/api/v1  ou http://localhost:5000/api/v1  
**Nota**: A porta das URLs presentes no arquivo 'Views/Home/Login_spotify.cshtml' devem ser alteradas para 6001 caso o API Gateway seja utilizado (https://github.com/wcoelho/APIGateway). 
   
**GET**    
- /usuarios  
=> Busca informações sobre a usuários a partir do banco de dados  
- /avaliacoes  
=> Busca avaliações cadastradas  
- /usuarios/\<id\>  
=> Busca usuário específico a partir do banco de dados  
- /avaliacoes/\<id\>  
=> Busca avaliação específica a partir do banco de dados  
    
**POST**  
- /usuarios 
=> Cadastra novo usuário. Payload (token opcional, já que expira):  
{  
	"name": "\<usuario\>",  
    "email": "\<email\>",  
    "spotifyToken": "\<token\>"  
}  
- /avaliacoes 
=> Cadastra nova avaliação. Payload:  
{  
      "musicId": \<id da musica avaliada\>,  
      "playlistId": \<id da playtlist avaliada\>,  
      "artistId": \<id do artista avaliado\>,  
      "userId": \<id do usuario\>,  
      "type": "\<tipo : musica, artista ou playlist\>",  
      "score": \<avaliação - 1 a 5\>,  
      "comment": "\<comentário\>",    
}     "id": \<id\>


**PUT**   
- /usuarios/\<id\>  
=> Atualiza cadastro de usuario. Payload:  
{  
	"name": "\<usuario\>",  
    "email": "\<email\>",  
    "spotifyToken": "\<token\>"  
    "id": \<id\>  
}  
    
**DELETE**   
- /usuarios/\<id\>  
=> Apaga cadastro de usuario.  
- /avaliacoes/\<id\>  
=> Apaga cadastro de avaliacao. 
