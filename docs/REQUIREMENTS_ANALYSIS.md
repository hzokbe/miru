# Miru — Requisitos Funcionais

## RF01 — Gestão de Usuários

- **RF01.1** — O sistema deve permitir que o usuário faça sign-up com nome de usuário, e-mail e senha.
- **RF01.2** — O sistema deve permitir que o usuário faça sign-in com e-mail e senha.
- **RF01.3** — O sistema deve diferenciar usuários por papel, como usuário comum e administrador.
- **RF01.4** — O sistema deve permitir que o usuário recupere a sua conta.

## RF02 — Catálogo de Animes

- **RF02.1** — O sistema deve permitir listar e buscar animes por título.
- **RF02.2** — O sistema deve exibir os detalhes de um anime: título, título japonês, sinopse, tipo, episódios, status,
  datas de exibição, nota, imagem.
- **RF02.3** — O sistema deve exibir os gêneros associados a um anime.
- **RF02.4** — O sistema deve exibir os estúdios responsáveis pela produção do anime.
- **RF02.5** — O sistema deve exibir os personagens vinculados a um anime.

## RF03 — Catálogo de Mangás

- **RF03.1** — O sistema deve permitir listar e buscar mangás por título.
- **RF03.2** — O sistema deve exibir os detalhes de um mangá: título, título japonês, sinopse, capítulos, volumes,
  status, datas de publicação, nota, imagem.
- **RF03.3** — O sistema deve exibir os personagens associados a um mangá.
- **RF03.4** — O sistema deve exibir os gêneros associados a um mangá.
- **RF03.5** — O sistema deve exibir os autores responsáveis pelo mangá.

## RF04 — Personagens, Estúdios e Autores

- **RF04.1** — O sistema deve exibir a página de detalhes de um personagem (nome, idade, gênero, papel, descrição,
  imagem).
- **RF04.2** — O sistema deve exibir a página de detalhes de um estúdio (nome, descrição, data de fundação, imagem).
- **RF04.3** — O sistema deve exibir a página de detalhes de um autor (nome, data de nascimento, imagem).

## RF05 — Listas Pessoais

- **RF05.1** — O sistema deve permitir que o usuário adicione um anime ou mangá à sua lista pessoal.
- **RF05.2** — Ao adicionar à lista, o sistema deve permitir definir o status (ex: assistindo, completo, pausado,
  abandonado, planejo assistir/ler).
- **RF05.3** — O sistema deve permitir registrar o progresso (episódios assistidos ou capítulos lidos).
- **RF05.4** — O sistema deve permitir registrar data de início e data de término do consumo.
- **RF05.5** — O sistema deve permitir que o usuário edite ou remova um item da sua lista.
- **RF05.6** — O sistema deve permitir que o usuário visualize sua lista completa de animes e mangás.

## RF06 — Avaliação (Rating)

- **RF06.1** — O sistema deve permitir que o usuário atribua uma nota a um anime ou mangá.
- **RF06.2** — O sistema deve permitir que o usuário edite ou remova sua avaliação.
- **RF06.3** — O sistema deve exibir a nota média de um anime/mangá com base nas avaliações dos usuários.

## RF07 — Favoritos

- **RF07.1** — O sistema deve permitir que o usuário marque um anime ou mangá como favorito, independentemente de estar
  na sua lista.
- **RF07.2** — O sistema deve permitir que o usuário desmarque um favorito.
- **RF07.3** — O sistema deve permitir que o usuário visualize sua lista de favoritos.

## RF08 — Busca e Navegação

- **RF08.1** — O sistema deve permitir buscar animes, mangás e personagens por nome/título.
- **RF08.2** — O sistema deve permitir filtrar animes/mangás por gênero.

## RF09 — Administração de Usuários

- **RF09.1** — O sistema deve permitir que um administrador visualize a lista de usuários cadastrados.
- **RF09.2** — O sistema deve permitir que um administrador altere o papel de um usuário.
- **RF09.3** — O sistema deve permitir que um administrador bloqueie/remova um usuário.

## RF10 — Administração de Animes

- **RF10.1** — O sistema deve permitir que um administrador cadastre, edite e remova animes.
- **RF10.2** — O sistema deve permitir que um administrador associe/desassocie gêneros a um anime.
- **RF10.3** — O sistema deve permitir que um administrador associe/desassocie estúdios a um anime.
- **RF10.4** — O sistema deve permitir que um administrador associe/desassocie personagens a um anime.

## RF11 — Administração de Mangás

- **RF11.1** — O sistema deve permitir que um administrador cadastre, edite e remova mangás.
- **RF11.2** — O sistema deve permitir que um administrador associe/desassocie gêneros a um mangá.
- **RF11.3** — O sistema deve permitir que um administrador associe/desassocie autores a um mangá.

## RF12 — Administração de Entidades Auxiliares

- **RF12.1** — O sistema deve permitir que um administrador cadastre, edite e remova gêneros.
- **RF12.2** — O sistema deve permitir que um administrador cadastre, edite e remova estúdios.
- **RF12.3** — O sistema deve permitir que um administrador cadastre, edite e remova autores.
- **RF12.4** — O sistema deve permitir que um administrador cadastre, edite e remova personagens.

---

# Miru — Requisitos Não Funcionais

## RNF01 — Segurança

- **RNF01.1** — O sistema deve armazenar as senhas dos usuários de forma segura, nunca em texto plano.
- **RNF01.2** — O sistema deve autenticar e autorizar requisições sensíveis conforme o papel do usuário.
- **RNF01.3** — O sistema deve trafegar dados via HTTPS em produção.
- **RNF01.4** — O sistema deve proteger os formulários contra ataques CSRF.

## RNF02 — Desempenho

- **RNF02.1** — O sistema deve responder a requisições de listagem e busca em tempo aceitável.
- **RNF02.2** — O sistema deve paginar listagens de grandes volumes de dados.

## RNF03 — Usabilidade

- **RNF03.1** — O sistema deve possuir interface responsiva, adaptável a diferentes tamanhos de tela.
- **RNF03.2** — O sistema deve exibir mensagens de erro claras e compreensíveis ao usuário.

## RNF04 — Confiabilidade e Disponibilidade

- **RNF04.1** — O sistema deve validar os dados de entrada antes de persistir no banco.
- **RNF04.2** — O sistema deve garantir integridade referencial entre as entidades.

## RNF05 — Manutenibilidade

- **RNF05.1** — O código deve seguir convenções de nomenclatura e organização consistentes ao longo do projeto.
