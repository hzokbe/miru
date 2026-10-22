# Miru — Modelagem de Dados

Este documento descreve as entidades do domínio, seus atributos e os relacionamentos entre elas. Os exemplos de valores
usam o anime/mangá **Sousou no Frieren** como referência.

## Entidades

### User

Representa um usuário cadastrado na plataforma.

| Atributo        | Descrição                                   | Exemplo                                                        |
|-----------------|---------------------------------------------|----------------------------------------------------------------|
| `id` (PK)       | Identificador único (UUID)                  | `97624ed9-05f3-4b25-bed2-ae65c646b804`                         |
| `username`      | Nome de exibição do usuário                 | `hzokbe`                                                       |
| `email`         | E-mail utilizado no login                   | `hzokbe@gmail.com`                                             |
| `password_hash` | Senha armazenada de forma criptografada     | `$2a$12$MbnsPxGgFsv3BsKVqQNjvuDERwrVQLiNxKgoSZOrYLPWnZvjW8Ada` |
| `role`          | Papel do usuário (ex: comum, administrador) | `user`                                                         |

### Anime

Representa um título de anime do catálogo.

| Atributo         | Descrição                                                 | Exemplo                                                                                                            |
|------------------|-----------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------|
| `id` (PK)        | Identificador único (UUID)                                | `cc12e1c1-4845-48a5-aafa-ac93a0d384ca`                                                                             |
| `title`          | Título do anime                                           | `Sousou no Frieren`                                                                                                |
| `japanese_title` | Título original em japonês                                | `葬送のフリーレン`                                                                                                 |
| `synopsis`       | Sinopse do anime                                          | Uma maga élfica revisita, após o fim da jornada heroica, os lugares por onde passou com seus antigos companheiros. |
| `type`           | Tipo (TV, filme, OVA, etc.)                               | `TV`                                                                                                               |
| `episodes`       | Número de episódios                                       | `28`                                                                                                               |
| `status`         | Status de exibição                                        | `Finished Airing`                                                                                                  |
| `aired_from`     | Data de início da exibição                                | `2023-09-29`                                                                                                       |
| `aired_to`       | Data de término da exibição                               | `2024-03-22`                                                                                                       |
| `score`          | Nota média calculada a partir das avaliações dos usuários | `9.25`                                                                                                             |
| `image_source`   | URL/caminho da imagem de capa                             | `/images/anime/cc12e1c1-4845-48a5-aafa-ac93a0d384ca.jpg`                                                           |

### Manga

Representa um título de mangá do catálogo.

| Atributo         | Descrição                                                 | Exemplo                                                                                                |
|------------------|-----------------------------------------------------------|--------------------------------------------------------------------------------------------------------|
| `id` (PK)        | Identificador único (UUID)                                | `84e14398-b607-428c-89d1-6853e66051e4`                                                                 |
| `title`          | Título do mangá                                           | `Sousou no Frieren`                                                                                    |
| `japanese_title` | Título original em japonês                                | `葬送のフリーレン`                                                                                     |
| `synopsis`       | Sinopse do mangá                                          | Uma maga élfica revisita, após o fim da jornada, os lugares onde esteve com seus antigos companheiros. |
| `chapters`       | Número de capítulos                                       | `null` (em publicação)                                                                                 |
| `volumes`        | Número de volumes                                         | `null` (em publicação)                                                                                 |
| `status`         | Status de publicação                                      | `Publishing`                                                                                           |
| `published_from` | Data de início da publicação                              | `2020-04-28`                                                                                           |
| `published_to`   | Data de término da publicação                             | `null`                                                                                                 |
| `score`          | Nota média calculada a partir das avaliações dos usuários | `8.87`                                                                                                 |
| `image_source`   | URL/caminho da imagem de capa                             | `/images/manga/84e14398-b607-428c-89d1-6853e66051e4.jpg`                                               |

### Genre

Representa um gênero que pode ser associado a animes e mangás.

| Atributo      | Descrição                  | Exemplo                                                                                 |
|---------------|----------------------------|-----------------------------------------------------------------------------------------|
| `id` (PK)     | Identificador único (UUID) | `f0840f92-098f-4119-a2da-2bc8f3fd2da3`                                                  |
| `name`        | Nome do gênero             | `Fantasy`                                                                               |
| `description` | Descrição do gênero        | Obras ambientadas em mundos com magia, criaturas fantásticas e elementos sobrenaturais. |

### Studio

Representa um estúdio responsável pela produção de animes.

| Atributo        | Descrição                  | Exemplo                                                                                                 |
|-----------------|----------------------------|---------------------------------------------------------------------------------------------------------|
| `id` (PK)       | Identificador único (UUID) | `bc16759f-e992-496e-ad72-75feb92b0e99`                                                                  |
| `name`          | Nome do estúdio            | `Madhouse`                                                                                              |
| `japanese_name` | Nome original em japonês   | `マッドハウス`                                                                                          |
| `description`   | Descrição do estúdio       | Estúdio de animação japonês fundado em Tóquio, conhecido por produções como Death Note e One Punch Man. |
| `established`   | Data de fundação           | `1972-10-17`                                                                                            |
| `image_source`  | URL/caminho da imagem/logo | `/images/studio/bc16759f-e992-496e-ad72-75feb92b0e99.jpg`                                               |

### Author

Representa um autor responsável pela criação de um mangá.

| Atributo             | Descrição                  | Exemplo                                                   |
|----------------------|----------------------------|-----------------------------------------------------------|
| `id` (PK)            | Identificador único (UUID) | `e521d6f5-748e-4ce3-9d9c-a0887108cdc9`                    |
| `name`               | Primeiro nome              | `Kanehito`                                                |
| `last_name`          | Sobrenome                  | `Yamada`                                                  |
| `japanese_name`      | Primeiro nome em japonês   | `鐘人`                                                    |
| `japanese_last_name` | Sobrenome em japonês       | `山田`                                                    |
| `birthday`           | Data de nascimento         | `null`                                                    |
| `image_source`       | URL/caminho da foto        | `/images/author/e521d6f5-748e-4ce3-9d9c-a0887108cdc9.jpg` |

### Character

Representa um personagem que pode estar vinculado a animes e/ou mangás.

| Atributo             | Descrição                         | Exemplo                                                                                                                                                                |
|----------------------|-----------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `id` (PK)            | Identificador único (UUID)        | `67e4c7ba-8161-4d63-81ac-c7886686801d`                                                                                                                                 |
| `name`               | Primeiro nome                     | `Frieren`                                                                                                                                                              |
| `last_name`          | Sobrenome                         | `null`                                                                                                                                                                 |
| `japanese_name`      | Primeiro nome em japonês          | `フリーレン`                                                                                                                                                           |
| `japanese_last_name` | Sobrenome em japonês              | `null`                                                                                                                                                                 |
| `age`                | Idade do personagem               | `1000+`                                                                                                                                                                |
| `gender`             | Gênero do personagem              | `Female`                                                                                                                                                               |
| `role`               | Papel na obra                     | `Main`                                                                                                                                                                 |
| `description`        | Descrição/biografia do personagem | Maga élfica que participou da jornada para derrotar o Rei Demônio e, séculos depois, passa a refletir sobre os laços que formou com seus antigos companheiros humanos. |
| `image_source`       | URL/caminho da imagem             | `/images/character/67e4c7ba-8161-4d63-81ac-c7886686801d.jpg`                                                                                                           |

## Relacionamentos

### Adicionar à lista

**User ↔ Anime** e **User ↔ Manga** — cardinalidade (0,n) em ambos os lados.
Representa a lista pessoal do usuário.

| Atributo      | Exemplo      |
|---------------|--------------|
| `status`      | `Completed`  |
| `progress`    | `28`         |
| `started_at`  | `2023-09-29` |
| `finished_at` | `2024-03-22` |

> Exemplo: usuário `hzokbe` adicionou o anime `Sousou no Frieren` à lista com status "Completed".

### Avaliar

**User ↔ Anime** e **User ↔ Manga** — cardinalidade (0,n) em ambos os lados.
Representa a avaliação que um usuário dá a um anime/mangá.

| Atributo | Exemplo |
|----------|---------|
| `score`  | `9`     |

> Exemplo: usuário `hzokbe` avaliou o anime `Sousou no Frieren` com nota `9`.

### Favoritar

**User ↔ Anime** e **User ↔ Manga** — cardinalidade (0,n) em ambos os lados.
Representa a marcação de um anime/mangá como favorito, independentemente de estar na lista pessoal. Sem atributos
próprios.

> Exemplo: usuário `hzokbe` favoritou o anime `Sousou no Frieren`.

### Escrever

**Author ↔ Manga** — cardinalidade (1,n) em ambos os lados.
Representa a autoria de um mangá. Sem atributos próprios.

> Exemplo: autor `Kanehito Yamada` escreveu o mangá `Sousou no Frieren`.

### Possuir (gênero)

**Anime ↔ Genre** — cardinalidade (0,n) do lado de Anime e (1,n) do lado de Genre. **Manga ↔ Genre** — cardinalidade
(0,n) do lado de Manga e (1,n) do lado de Genre.
Representa os gêneros associados a um anime/mangá. Sem atributos próprios.

> Exemplo: o anime `Sousou no Frieren` possui o gênero `Fantasy`.

### Possuir (personagem)

**Anime ↔ Character** e **Manga ↔ Character** — cardinalidade (1,n) em ambos os lados.
Representa os personagens que aparecem em um anime/mangá. Sem atributos próprios.

> Exemplo: o anime `Sousou no Frieren` possui o personagem `Frieren`.

### Produzir

**Anime ↔ Studio** — cardinalidade (0,n) do lado de Anime e (1,n) do lado de Studio.
Representa os estúdios responsáveis pela produção de um anime. Sem atributos próprios.

> Exemplo: o estúdio `Madhouse` produziu o anime `Sousou no Frieren`.
