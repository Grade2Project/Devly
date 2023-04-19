
create schema devly;

create table if not exists devly.grades
(
    id    serial primary key,
    value text
);

create table if not exists devly.contacts
(
    id    serial primary key,
    phone varchar(13),
    email varchar(32)
    );

create table if not exists devly.users_passwords
(
    user_login  varchar(32) unique not null primary key,
    hashed_pass varchar(512)       not null
    );


create table if not exists devly.users
(
    login       varchar(32) references users_passwords (user_login) primary key,
    birth_date  date,
    name        text,
    city        text,
    info        text,
    resume_path text,
    grade_id    int references grades (id),
    contact_id  int references contacts (id),
    image_path  text default '/images/default.jpg'::text
    );

create table if not exists devly.companies
(
    id           serial primary key,
    company_name text not null,
    info         text
);

create table if not exists devly.programming_languages
(
    id            serial primary key,
    language_name varchar(10) not null
    );

create table if not exists devly.vacancies
(
    id                      serial primary key,
    company_id              integer not null
    references companies (id),
    programming_language_id integer not null
    references programming_languages (id),
    salary                  int     not null,
    info                    text    not null
    );

create table if not exists devly.users_favorite_languages
(
    user_login              varchar(32) not null
    references users (login),
    programming_language_id integer     not null
    references programming_languages (id),
    primary key (user_login, programming_language_id)
    );

create table if not exists devly.users_favorite_vacancies
(
    user_login varchar(32) not null
    references users (login),
    vacancy_id integer     not null
    references vacancies (id),
    primary key (user_login, vacancy_id)
    );

create table if not exists devly.companies_favorite_users
(
    company_id integer     not null
    references companies (id),
    user_login varchar(32) not null
    references users (login),
    primary key (company_id, user_login)
    );

