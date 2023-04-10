create database devly;

create schema devly

    create table users
    (
        id         serial
            primary key,
        age        integer                                  not null
            constraint positive_age
                check (age > 0),
        name       text                                     not null,
        city       text                                     not null,
        info       text,
        image_path text default '/images/default.jpg'::text not null
    )

    create table companies
    (
        id           serial primary key,
        company_name text not null,
        info text
    )

    create table vacancies
    (
        id                      serial primary key,
        company_id              integer not null
            references companies (id)
                on update cascade on delete cascade,
        programming_language_id integer not null
            references programming_languages (id)
                on update cascade on delete cascade,
        salary                  int     not null,
        info                    text    not null
    )

    create table users_passwords
    (
        user_id     integer      not null
            primary key
            references users
                on update cascade on delete cascade,
        hashed_pass varchar(512) not null
    )

    create table programming_languages
    (
        id            serial
            primary key,
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

    create table users_favorite_vacancies
    (
        user_id    integer not null
            references users (id)
                on update cascade on delete cascade,
        vacancy_id integer not null
            references vacancies (id)
                on update cascade on delete cascade
    )

    create table companies_favorite_users
    (
        company_id integer not null
            references companies (id)
                on update cascade on delete cascade,
        user_id    integer not null
            references users (id)
                on update cascade on delete cascade
    )


