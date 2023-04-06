--
-- PostgreSQL database dump
--

-- Dumped from database version 15.2
-- Dumped by pg_dump version 15.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: favorite_languages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.favorite_languages (
    user_id integer NOT NULL,
    programming_language_id integer NOT NULL
);


ALTER TABLE public.favorite_languages OWNER TO postgres;

--
-- Name: likes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.likes (
    who_liked_user_id integer NOT NULL,
    was_liked_user_id integer NOT NULL
);


ALTER TABLE public.likes OWNER TO postgres;

--
-- Name: programming_languages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.programming_languages (
    programming_language_id integer NOT NULL,
    language_name character varying(10) NOT NULL
);


ALTER TABLE public.programming_languages OWNER TO postgres;

--
-- Name: programming_languages_programming_language_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.programming_languages_programming_language_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.programming_languages_programming_language_id_seq OWNER TO postgres;

--
-- Name: programming_languages_programming_language_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.programming_languages_programming_language_id_seq OWNED BY public.programming_languages.programming_language_id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    age integer NOT NULL,
    name text NOT NULL,
    city text NOT NULL,
    info text,
    image_path text DEFAULT '/images/default.jpg'::text NOT NULL,
    CONSTRAINT positive_age CHECK ((age > 0))
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: users_passwords; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users_passwords (
    user_id integer NOT NULL,
    hashed_pass character varying(512) NOT NULL
);


ALTER TABLE public.users_passwords OWNER TO postgres;

--
-- Name: users_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.users_user_id_seq OWNER TO postgres;

--
-- Name: users_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_user_id_seq OWNED BY public.users.user_id;


--
-- Name: programming_languages programming_language_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.programming_languages ALTER COLUMN programming_language_id SET DEFAULT nextval('public.programming_languages_programming_language_id_seq'::regclass);


--
-- Name: users user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.users_user_id_seq'::regclass);


--
-- Data for Name: favorite_languages; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.favorite_languages (user_id, programming_language_id) FROM stdin;
\.


--
-- Data for Name: likes; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.likes (who_liked_user_id, was_liked_user_id) FROM stdin;
\.


--
-- Data for Name: programming_languages; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.programming_languages (programming_language_id, language_name) FROM stdin;
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (user_id, age, name, city, info, image_path) FROM stdin;
\.


--
-- Data for Name: users_passwords; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users_passwords (user_id, hashed_pass) FROM stdin;
\.


--
-- Name: programming_languages_programming_language_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.programming_languages_programming_language_id_seq', 1, false);


--
-- Name: users_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_user_id_seq', 1, false);


--
-- Name: favorite_languages favorite_languages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.favorite_languages
    ADD CONSTRAINT favorite_languages_pkey PRIMARY KEY (user_id, programming_language_id);


--
-- Name: likes likes_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.likes
    ADD CONSTRAINT likes_pkey PRIMARY KEY (who_liked_user_id, was_liked_user_id);


--
-- Name: programming_languages programming_languages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.programming_languages
    ADD CONSTRAINT programming_languages_pkey PRIMARY KEY (programming_language_id);


--
-- Name: users_passwords users_passwords_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users_passwords
    ADD CONSTRAINT users_passwords_pkey PRIMARY KEY (user_id);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- Name: favorite_languages favorite_languages_programming_language_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.favorite_languages
    ADD CONSTRAINT favorite_languages_programming_language_id_fkey FOREIGN KEY (programming_language_id) REFERENCES public.programming_languages(programming_language_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: favorite_languages favorite_languages_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.favorite_languages
    ADD CONSTRAINT favorite_languages_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: likes likes_was_liked_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.likes
    ADD CONSTRAINT likes_was_liked_user_id_fkey FOREIGN KEY (was_liked_user_id) REFERENCES public.users(user_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: likes likes_who_liked_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.likes
    ADD CONSTRAINT likes_who_liked_user_id_fkey FOREIGN KEY (who_liked_user_id) REFERENCES public.users(user_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: users_passwords users_passwords_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users_passwords
    ADD CONSTRAINT users_passwords_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

