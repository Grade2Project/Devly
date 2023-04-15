create schema if not exists devly

    create table if not exists users
    (
        login      varchar(32) unique not null primary key ,
        birth_date date, 
        name       text,
        city       text,
        info       text,
        resume_path text,
        grade_id int references grades(id),
        contact_id int references contacts(id),
        image_path text default '/images/default.jpg'::text
    )
    create index if not exists user_login_index on users(login)
    
    create table if not exists contacts
    (
        id serial primary key, 
        phone varchar(13),
        email varchar(32)
    )
        
    create table if not exists grades
    (
        id serial primary key,
        grade text 
    )

    create table if not exists users_passwords
    (
        user_login     varchar(32)    not null
            primary key
            references users (login)
                on update cascade on delete cascade,
        hashed_pass varchar(512) not null
    )

    create table if not exists companies
    (
        id           serial primary key,
        company_name text not null,
        info         text
    )

    create table if not exists programming_languages
    (
        id            serial
            primary key,
        language_name varchar(10) not null
    )

    create table if not exists vacancies
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

    create table if not exists users_favorite_languages
    (
        user_login                integer not null
            references users (login)
                on update cascade on delete cascade,
        programming_language_id integer not null
            references programming_languages (id)
                on update cascade on delete cascade,
        primary key (user_login, programming_language_id)
    )

    create table if not exists users_favorite_vacancies
    (
        user_login    integer not null
            references users (login)
                on update cascade on delete cascade,
        vacancy_id integer not null
            references vacancies (id)
                on update cascade on delete cascade,
        primary key (user_login, vacancy_id)
    )

    create table if not exists companies_favorite_users
    (
        company_id integer not null
            references companies (id)
                on update cascade on delete cascade,
        user_login    integer not null
            references users (login)
                on update cascade on delete cascade,
        primary key (company_id, user_login)
    )




