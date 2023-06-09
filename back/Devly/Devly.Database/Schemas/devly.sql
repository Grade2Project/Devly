create schema devly;

CREATE OR REPLACE FUNCTION random_between(low INT ,high INT)
    RETURNS INT AS
$$
BEGIN
    RETURN floor(random()* (high-low + 1) + low);
END;
$$ language 'plpgsql' STRICT;

create table if not exists devly.grades
(
    id    serial primary key,
    value text unique
);

insert into devly.grades(value)
values ('Junior'),
       ('Junior+'),
       ('Middle'),
       ('Middle+'),
       ('Senior'),
       ('Lead');

create table if not exists devly.cities
(
    id serial primary key,
    name text
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
    login       varchar(32) references devly.users_passwords (user_login) primary key,
    experience int,
    birth_date  date,
    name        text,
    city_id     int references devly.cities(id) default random_between(1, 1117),
    info        text,
    grade_id    int references devly.grades (id),
    contact_id  int references devly.contacts (id),
    image_path  text default '../photos/users/default.txt'
);

create table if not exists devly.companies
(
    id            serial primary key,
    company_email text unique not null,
    company_name  text        not null,
    image_path    text default '../photos/companies/default.txt',
    info          text
);

create index on devly.companies (company_email);

create table if not exists devly.companies_passwords
(
    company_id  int unique   not null primary key
        references devly.companies (id),
    hashed_pass varchar(512) not null
);

create table if not exists devly.programming_languages
(
    id            serial primary key,
    language_name varchar(10) not null
);
insert into devly.programming_languages(language_name)
values ('C++'),
       ('C#'),
       ('Java'),
       ('JavaScript'),
       ('Python'),
       ('Ruby'),
       ('Haskel'),
       ('Pascal'),
       ('Scratch'),
       ('Go'),
       ('Basic'),
       ('PHP'),
       ('HTML'),
       ('CSS');

create table if not exists devly.vacancies
(
    id                      serial primary key,
    company_id              integer not null
        references devly.companies (id),
    programming_language_id integer not null
        references devly.programming_languages (id),
    grade_id                integer not null
        references devly.grades (id),
    salary                  int     not null,
    city_id int references devly.cities(id) default random_between(1, 1117),
    info                    text    not null
);

create table if not exists devly.users_favorite_languages
(
    user_login              varchar(32) not null
        references devly.users (login),
    programming_language_id integer     not null
        references devly.programming_languages (id),
    primary key (user_login, programming_language_id)
);

create table if not exists devly.users_favorite_vacancies
(
    user_login varchar(32) not null
        references devly.users (login),
    vacancy_id integer     not null
        references devly.vacancies (id),
    primary key (user_login, vacancy_id)
);

create table if not exists devly.companies_favorite_users
(
    company_id integer     not null
        references devly.companies (id),
    user_login varchar(32) not null
        references devly.users (login),
    primary key (company_id, user_login)
);

