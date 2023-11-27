PGDMP                  
    {            db_cb    16.1 (Debian 16.1-1.pgdg120+1)    16.0 I    x           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            y           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            z           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            {           1262    16388    db_cb    DATABASE     p   CREATE DATABASE db_cb WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';
    DROP DATABASE db_cb;
                postgres    false            �            1255    16390    current_timestamp()    FUNCTION     �   CREATE FUNCTION public."current_timestamp"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
NEW.updated_at = now();
RETURN NEW;
END;$$;
 ,   DROP FUNCTION public."current_timestamp"();
       public          postgres    false            �            1255    32768    delete_category()    FUNCTION     �   CREATE FUNCTION public.delete_category() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('category', old.country_id, 'delete', now());
	return old;
END;$$;
 (   DROP FUNCTION public.delete_category();
       public          postgres    false            �            1255    16391    delete_country()    FUNCTION     �   CREATE FUNCTION public.delete_country() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('country', old.country_id, 'delete', now());
	return old;
END;$$;
 '   DROP FUNCTION public.delete_country();
       public          postgres    false            �            1255    32770    delete_post()    FUNCTION     �   CREATE FUNCTION public.delete_post() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('post', old.country_id, 'delete', now());
	return old;
END;$$;
 $   DROP FUNCTION public.delete_post();
       public          postgres    false            �            1255    16392    delete_region()    FUNCTION     �   CREATE FUNCTION public.delete_region() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('region', old.region_id, 'delete', now());
	return old;
END;$$;
 &   DROP FUNCTION public.delete_region();
       public          postgres    false            �            1255    32769    delete_user()    FUNCTION     �   CREATE FUNCTION public.delete_user() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('user', old.country_id, 'delete', now());
	return old;
END;$$;
 $   DROP FUNCTION public.delete_user();
       public          postgres    false            �            1255    16393    insert_country()    FUNCTION       CREATE FUNCTION public.insert_country() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	RAISE NOTICE 'vao nha';
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('country', new.country_id, 'insert', now());
	return new;
END;$$;
 '   DROP FUNCTION public.insert_country();
       public          postgres    false            |           0    0    FUNCTION insert_country()    COMMENT     S   COMMENT ON FUNCTION public.insert_country() IS 'update clone when change country';
          public          postgres    false    229            �            1255    16394    insert_region()    FUNCTION       CREATE FUNCTION public.insert_region() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	RAISE NOTICE 'vao nha';
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('region', new.region_id, 'insert', now());
	return new;
END;$$;
 &   DROP FUNCTION public.insert_region();
       public          postgres    false            �            1255    16395    update_country()    FUNCTION       CREATE FUNCTION public.update_country() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	RAISE NOTICE 'vao nha';
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('country', old.country_id, 'update', now());
	return new;
END;$$;
 '   DROP FUNCTION public.update_country();
       public          postgres    false            �            1255    16396    update_region()    FUNCTION       CREATE FUNCTION public.update_region() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	RAISE NOTICE 'vao nha';
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('region', old.region_id, 'update', now());
	return new;
END;$$;
 &   DROP FUNCTION public.update_region();
       public          postgres    false            �            1259    16397    db_categories    TABLE     �  CREATE TABLE public.db_categories (
    parent_id integer DEFAULT 0,
    path character varying(1000) DEFAULT ''::character varying,
    name character varying(200) DEFAULT ''::character varying,
    description text DEFAULT ''::text,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    category_id integer NOT NULL
);
 !   DROP TABLE public.db_categories;
       public         heap    postgres    false            �            1259    16408    db_categories_category_id_seq    SEQUENCE     �   CREATE SEQUENCE public.db_categories_category_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 4   DROP SEQUENCE public.db_categories_category_id_seq;
       public          postgres    false    215            }           0    0    db_categories_category_id_seq    SEQUENCE OWNED BY     _   ALTER SEQUENCE public.db_categories_category_id_seq OWNED BY public.db_categories.category_id;
          public          postgres    false    216            �            1259    16409    db_countries    TABLE     �   CREATE TABLE public.db_countries (
    country_name character varying(40),
    region_id integer,
    country_code character(2),
    country_id smallint NOT NULL
);
     DROP TABLE public.db_countries;
       public         heap    postgres    false            �            1259    16412    db_countries_country_id_seq    SEQUENCE     �   CREATE SEQUENCE public.db_countries_country_id_seq
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 2   DROP SEQUENCE public.db_countries_country_id_seq;
       public          postgres    false    217            ~           0    0    db_countries_country_id_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public.db_countries_country_id_seq OWNED BY public.db_countries.country_id;
          public          postgres    false    218            �            1259    16413    db_posts    TABLE     `  CREATE TABLE public.db_posts (
    post_id integer NOT NULL,
    name character varying(500) DEFAULT ''::character varying NOT NULL,
    description text NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    user_id integer,
    category_id integer
);
    DROP TABLE public.db_posts;
       public         heap    postgres    false            �            1259    16421    db_posts_post_id_seq    SEQUENCE     �   ALTER TABLE public.db_posts ALTER COLUMN post_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.db_posts_post_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    219            �            1259    16422    db_queries_logs    TABLE     �   CREATE TABLE public.db_queries_logs (
    log_id bigint NOT NULL,
    object_name character varying(20),
    object_type character varying(10),
    timestamps timestamp without time zone,
    object_id bigint
);
 #   DROP TABLE public.db_queries_logs;
       public         heap    postgres    false            �            1259    16425    db_queries_logs_log_id_seq    SEQUENCE     �   ALTER TABLE public.db_queries_logs ALTER COLUMN log_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.db_queries_logs_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    221            �            1259    16426 
   db_regions    TABLE     j   CREATE TABLE public.db_regions (
    region_id integer NOT NULL,
    region_name character varying(25)
);
    DROP TABLE public.db_regions;
       public         heap    postgres    false            �            1259    16429    db_users    TABLE     �  CREATE TABLE public.db_users (
    user_id integer NOT NULL,
    username character varying(200) DEFAULT ''::character varying NOT NULL,
    email character varying(200) DEFAULT ''::character varying NOT NULL,
    full_name character varying(200) DEFAULT ''::character varying NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    country_id smallint NOT NULL
);
    DROP TABLE public.db_users;
       public         heap    postgres    false            �            1259    16439    db_users_user_id_seq    SEQUENCE     �   ALTER TABLE public.db_users ALTER COLUMN user_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.db_users_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    224            �            1259    16440    regions_region_id_seq    SEQUENCE     �   CREATE SEQUENCE public.regions_region_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.regions_region_id_seq;
       public          postgres    false    223                       0    0    regions_region_id_seq    SEQUENCE OWNED BY     R   ALTER SEQUENCE public.regions_region_id_seq OWNED BY public.db_regions.region_id;
          public          postgres    false    226            �           2604    16441    db_categories category_id    DEFAULT     �   ALTER TABLE ONLY public.db_categories ALTER COLUMN category_id SET DEFAULT nextval('public.db_categories_category_id_seq'::regclass);
 H   ALTER TABLE public.db_categories ALTER COLUMN category_id DROP DEFAULT;
       public          postgres    false    216    215            �           2604    16442    db_countries country_id    DEFAULT     �   ALTER TABLE ONLY public.db_countries ALTER COLUMN country_id SET DEFAULT nextval('public.db_countries_country_id_seq'::regclass);
 F   ALTER TABLE public.db_countries ALTER COLUMN country_id DROP DEFAULT;
       public          postgres    false    218    217            �           2604    16443    db_regions region_id    DEFAULT     y   ALTER TABLE ONLY public.db_regions ALTER COLUMN region_id SET DEFAULT nextval('public.regions_region_id_seq'::regclass);
 C   ALTER TABLE public.db_regions ALTER COLUMN region_id DROP DEFAULT;
       public          postgres    false    226    223            j          0    16397    db_categories 
   TABLE DATA           p   COPY public.db_categories (parent_id, path, name, description, created_at, updated_at, category_id) FROM stdin;
    public          postgres    false    215   �^       l          0    16409    db_countries 
   TABLE DATA           Y   COPY public.db_countries (country_name, region_id, country_code, country_id) FROM stdin;
    public          postgres    false    217   b_       n          0    16413    db_posts 
   TABLE DATA           l   COPY public.db_posts (post_id, name, description, created_at, updated_at, user_id, category_id) FROM stdin;
    public          postgres    false    219   �`       p          0    16422    db_queries_logs 
   TABLE DATA           b   COPY public.db_queries_logs (log_id, object_name, object_type, timestamps, object_id) FROM stdin;
    public          postgres    false    221   �`       r          0    16426 
   db_regions 
   TABLE DATA           <   COPY public.db_regions (region_id, region_name) FROM stdin;
    public          postgres    false    223   �a       s          0    16429    db_users 
   TABLE DATA           k   COPY public.db_users (user_id, username, email, full_name, created_at, updated_at, country_id) FROM stdin;
    public          postgres    false    224   �a       �           0    0    db_categories_category_id_seq    SEQUENCE SET     K   SELECT pg_catalog.setval('public.db_categories_category_id_seq', 3, true);
          public          postgres    false    216            �           0    0    db_countries_country_id_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public.db_countries_country_id_seq', 31, true);
          public          postgres    false    218            �           0    0    db_posts_post_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.db_posts_post_id_seq', 1, true);
          public          postgres    false    220            �           0    0    db_queries_logs_log_id_seq    SEQUENCE SET     H   SELECT pg_catalog.setval('public.db_queries_logs_log_id_seq', 8, true);
          public          postgres    false    222            �           0    0    db_users_user_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.db_users_user_id_seq', 3, true);
          public          postgres    false    225            �           0    0    regions_region_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.regions_region_id_seq', 1, false);
          public          postgres    false    226            �           2606    16445    db_categories category_id 
   CONSTRAINT     `   ALTER TABLE ONLY public.db_categories
    ADD CONSTRAINT category_id PRIMARY KEY (category_id);
 C   ALTER TABLE ONLY public.db_categories DROP CONSTRAINT category_id;
       public            postgres    false    215            �           2606    16447    db_countries country_id 
   CONSTRAINT     ]   ALTER TABLE ONLY public.db_countries
    ADD CONSTRAINT country_id PRIMARY KEY (country_id);
 A   ALTER TABLE ONLY public.db_countries DROP CONSTRAINT country_id;
       public            postgres    false    217            �           2606    16449 $   db_queries_logs db_queries_logs_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.db_queries_logs
    ADD CONSTRAINT db_queries_logs_pkey PRIMARY KEY (log_id);
 N   ALTER TABLE ONLY public.db_queries_logs DROP CONSTRAINT db_queries_logs_pkey;
       public            postgres    false    221            �           2606    16451    db_posts pk_post_id 
   CONSTRAINT     V   ALTER TABLE ONLY public.db_posts
    ADD CONSTRAINT pk_post_id PRIMARY KEY (post_id);
 =   ALTER TABLE ONLY public.db_posts DROP CONSTRAINT pk_post_id;
       public            postgres    false    219            �           2606    16453    db_regions regions_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public.db_regions
    ADD CONSTRAINT regions_pkey PRIMARY KEY (region_id);
 A   ALTER TABLE ONLY public.db_regions DROP CONSTRAINT regions_pkey;
       public            postgres    false    223            �           2606    16455    db_countries unique_code 
   CONSTRAINT     [   ALTER TABLE ONLY public.db_countries
    ADD CONSTRAINT unique_code UNIQUE (country_code);
 B   ALTER TABLE ONLY public.db_countries DROP CONSTRAINT unique_code;
       public            postgres    false    217            �           2606    16457    db_users user_id 
   CONSTRAINT     S   ALTER TABLE ONLY public.db_users
    ADD CONSTRAINT user_id PRIMARY KEY (user_id);
 :   ALTER TABLE ONLY public.db_users DROP CONSTRAINT user_id;
       public            postgres    false    224            �           2606    16459    db_users username 
   CONSTRAINT     P   ALTER TABLE ONLY public.db_users
    ADD CONSTRAINT username UNIQUE (username);
 ;   ALTER TABLE ONLY public.db_users DROP CONSTRAINT username;
       public            postgres    false    224            �           1259    16460    fki_fk_category_id    INDEX     N   CREATE INDEX fki_fk_category_id ON public.db_posts USING btree (category_id);
 &   DROP INDEX public.fki_fk_category_id;
       public            postgres    false    219            �           1259    16461 	   object_id    INDEX     j   CREATE INDEX object_id ON public.db_queries_logs USING btree (object_id) WITH (deduplicate_items='true');
    DROP INDEX public.object_id;
       public            postgres    false    221            �           1259    16462    object_name_type_time    INDEX     �   CREATE INDEX object_name_type_time ON public.db_queries_logs USING btree (object_name, object_type, timestamps) WITH (deduplicate_items='true');
 )   DROP INDEX public.object_name_type_time;
       public            postgres    false    221    221    221            �           1259    16463    parents    INDEX     f   CREATE INDEX parents ON public.db_categories USING btree (parent_id) WITH (deduplicate_items='true');
    DROP INDEX public.parents;
       public            postgres    false    215            �           2620    32771 %   db_categories trigger_delete_category    TRIGGER     �   CREATE TRIGGER trigger_delete_category BEFORE DELETE ON public.db_categories FOR EACH ROW EXECUTE FUNCTION public.delete_category();
 >   DROP TRIGGER trigger_delete_category ON public.db_categories;
       public          postgres    false    234    215            �           2620    16464 #   db_countries trigger_delete_country    TRIGGER     �   CREATE TRIGGER trigger_delete_country AFTER DELETE ON public.db_countries FOR EACH ROW EXECUTE FUNCTION public.delete_country();
 <   DROP TRIGGER trigger_delete_country ON public.db_countries;
       public          postgres    false    217    233            �           2620    32773    db_posts trigger_delete_post    TRIGGER     x   CREATE TRIGGER trigger_delete_post BEFORE DELETE ON public.db_posts FOR EACH ROW EXECUTE FUNCTION public.delete_post();
 5   DROP TRIGGER trigger_delete_post ON public.db_posts;
       public          postgres    false    219    236            �           2620    16465 !   db_regions trigger_delete_regionT    TRIGGER     �   CREATE TRIGGER "trigger_delete_regionT" AFTER DELETE ON public.db_regions FOR EACH ROW EXECUTE FUNCTION public.delete_region();
 <   DROP TRIGGER "trigger_delete_regionT" ON public.db_regions;
       public          postgres    false    228    223            �           2620    32772    db_users trigger_delete_user    TRIGGER     x   CREATE TRIGGER trigger_delete_user BEFORE DELETE ON public.db_users FOR EACH ROW EXECUTE FUNCTION public.delete_user();
 5   DROP TRIGGER trigger_delete_user ON public.db_users;
       public          postgres    false    224    235            �           2620    16466 #   db_countries trigger_insert_country    TRIGGER     �   CREATE TRIGGER trigger_insert_country AFTER INSERT ON public.db_countries FOR EACH ROW EXECUTE FUNCTION public.insert_country();
 <   DROP TRIGGER trigger_insert_country ON public.db_countries;
       public          postgres    false    229    217            �           2620    16467     db_regions trigger_insert_region    TRIGGER     }   CREATE TRIGGER trigger_insert_region AFTER INSERT ON public.db_regions FOR EACH ROW EXECUTE FUNCTION public.insert_region();
 9   DROP TRIGGER trigger_insert_region ON public.db_regions;
       public          postgres    false    223    230            �           2620    16468     db_regions trigger_update_region    TRIGGER     }   CREATE TRIGGER trigger_update_region AFTER UPDATE ON public.db_regions FOR EACH ROW EXECUTE FUNCTION public.update_region();
 9   DROP TRIGGER trigger_update_region ON public.db_regions;
       public          postgres    false    232    223            �           2620    16469    db_countries update_country    TRIGGER     y   CREATE TRIGGER update_country AFTER UPDATE ON public.db_countries FOR EACH ROW EXECUTE FUNCTION public.update_country();
 4   DROP TRIGGER update_country ON public.db_countries;
       public          postgres    false    231    217            �           2620    16470    db_categories update_timestamp    TRIGGER     �   CREATE TRIGGER update_timestamp BEFORE UPDATE ON public.db_categories FOR EACH ROW EXECUTE FUNCTION public."current_timestamp"();
 7   DROP TRIGGER update_timestamp ON public.db_categories;
       public          postgres    false    227    215            �           2620    16471    db_posts update_timestamp    TRIGGER     }   CREATE TRIGGER update_timestamp BEFORE UPDATE ON public.db_posts FOR EACH ROW EXECUTE FUNCTION public."current_timestamp"();
 2   DROP TRIGGER update_timestamp ON public.db_posts;
       public          postgres    false    219    227            �           2620    16472    db_users update_timestamp    TRIGGER     }   CREATE TRIGGER update_timestamp BEFORE UPDATE ON public.db_users FOR EACH ROW EXECUTE FUNCTION public."current_timestamp"();
 2   DROP TRIGGER update_timestamp ON public.db_users;
       public          postgres    false    224    227            �           2606    16473 %   db_countries countries_region_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.db_countries
    ADD CONSTRAINT countries_region_id_fkey FOREIGN KEY (region_id) REFERENCES public.db_regions(region_id);
 O   ALTER TABLE ONLY public.db_countries DROP CONSTRAINT countries_region_id_fkey;
       public          postgres    false    217    3270    223            �           2606    16478    db_posts fk_category_id    FK CONSTRAINT     �   ALTER TABLE ONLY public.db_posts
    ADD CONSTRAINT fk_category_id FOREIGN KEY (category_id) REFERENCES public.db_categories(category_id) NOT VALID;
 A   ALTER TABLE ONLY public.db_posts DROP CONSTRAINT fk_category_id;
       public          postgres    false    215    3256    219            �           2606    16483    db_users fk_country    FK CONSTRAINT     �   ALTER TABLE ONLY public.db_users
    ADD CONSTRAINT fk_country FOREIGN KEY (country_id) REFERENCES public.db_countries(country_id) NOT VALID;
 =   ALTER TABLE ONLY public.db_users DROP CONSTRAINT fk_country;
       public          postgres    false    224    217    3259            �           2606    16488    db_posts user_id    FK CONSTRAINT     �   ALTER TABLE ONLY public.db_posts
    ADD CONSTRAINT user_id FOREIGN KEY (user_id) REFERENCES public.db_users(user_id) NOT VALID;
 :   ALTER TABLE ONLY public.db_posts DROP CONSTRAINT user_id;
       public          postgres    false    224    219    3272            j   y   x�3��LI��P�}�{i��!g��-
%w-�4202�54�52S00�20�20�322�06�'e�e 3�4Y��37_�$��\��������X��������YƸ�25ѳ0�7�'e����� L5U      l   :  x�5�Mn�@���)8A�L�]�BDiD��1dFM&h2����iՅ������Q� ��Е�y���NU�S�������L �, ��U^E!�0�hC�rcj*�E���[�,5�FX���=��`�̰Ц�kg7	��7g�W]�peb�+K�X�� &ʶd<�2!�H퉗��� ��qu������a�g�Xܵ{*ې�8�AL0W��g���[Sܩo}�8�{1��v'�fsL{K�a�nAq�LK��e �;Sg\|֚�����1O@�Z��&!܃dL����O^� ���?�m�      n   >   x�3�,�,�IU0�L��+I�2���uu���̬��LLM�Jsr��qqq ��      p   �   x���=�@��z�\�������4F�01h
n�XPh�'_^��}�i��1��
B�-s+�P*̅(d��$���:>�4s)�{%�]�VϵH��<B�x�mkQ�2���A���Y�EPF?|d��û���S�(&1q�"� wWQ      r   ?   x�3�t--�/H�2�t�M-�LN,�2�t,�L�2���LI�IUpM,.QH�KQpL)������ �~�      s   R   x�3�,-N-J��K/�L��Kt�M���K���<�ݚ�Pv�9O������X��P��L��������X��������!W� y��     