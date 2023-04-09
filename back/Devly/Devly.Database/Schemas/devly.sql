create
database devly;

create schema devly

    create table users
    (
        id         serial primary key,
        login      varchar(32) unique,
        age        integer     not null
            constraint positive_age check (age > 0),
        name       varchar(64) not null,
        city       varchar(64) not null,
        info       text,
        image_path text default '/images/default.jpg'::text not null
    )

    create table users_passwords
    (
        user_id     integer      not null
            primary key
            references users (id)
                on update cascade on delete cascade,
        hashed_pass varchar(512) not null
    )

    create table programming_languages
    (
        id            serial primary key,
        language_name varchar(10) not null
    )

    create table users_favorite_languages
    (
        user_id                 integer not null
            references users (id)
                on update cascade on delete cascade,
        programming_language_id integer not null
            references programming_languages (id)
                on update cascade on delete cascade,
        primary key (user_id, programming_language_id)
    )

    create table likes
    (
        who_liked_user_id integer not null
            references users (id)
                on update cascade on delete cascade,
        was_liked_user_id integer not null
            references users (id)
                on update cascade on delete cascade,
        primary key (who_liked_user_id, was_liked_user_id)
    );

