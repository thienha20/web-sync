PGDMP  3                
    {            db_cb    16.1 (Debian 16.1-1.pgdg120+1)    16.0 P    }           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            ~           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    40960    db_cb    DATABASE     p   CREATE DATABASE db_cb WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';
    DROP DATABASE db_cb;
                postgres    false            �            1255    40961    current_timestamp()    FUNCTION     �   CREATE FUNCTION public."current_timestamp"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
NEW.updated_at = now();
RETURN NEW;
END;$$;
 ,   DROP FUNCTION public."current_timestamp"();
       public          postgres    false            �            1255    40962    delete_category()    FUNCTION     �   CREATE FUNCTION public.delete_category() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('category', old.category_id, 'delete', now());
	return old;
END;$$;
 (   DROP FUNCTION public.delete_category();
       public          postgres    false            �            1255    40963    delete_country()    FUNCTION     �   CREATE FUNCTION public.delete_country() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('country', old.country_id, 'delete', now());
	return old;
END;$$;
 '   DROP FUNCTION public.delete_country();
       public          postgres    false            �            1255    40964    delete_post()    FUNCTION     �   CREATE FUNCTION public.delete_post() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('post', old.post_id, 'delete', now());
	return old;
END;$$;
 $   DROP FUNCTION public.delete_post();
       public          postgres    false            �            1255    40965    delete_region()    FUNCTION     �   CREATE FUNCTION public.delete_region() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('region', old.region_id, 'delete', now());
	return old;
END;$$;
 &   DROP FUNCTION public.delete_region();
       public          postgres    false            �            1255    40966    delete_user()    FUNCTION     �   CREATE FUNCTION public.delete_user() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('user', old.user_id, 'delete', now());
	return old;
END;$$;
 $   DROP FUNCTION public.delete_user();
       public          postgres    false            �            1255    40967    insert_country()    FUNCTION       CREATE FUNCTION public.insert_country() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	RAISE NOTICE 'vao nha';
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('country', new.country_id, 'insert', now());
	return new;
END;$$;
 '   DROP FUNCTION public.insert_country();
       public          postgres    false            �           0    0    FUNCTION insert_country()    COMMENT     S   COMMENT ON FUNCTION public.insert_country() IS 'update clone when change country';
          public          postgres    false    229            �            1255    40968    insert_region()    FUNCTION     �   CREATE FUNCTION public.insert_region() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('region', new.region_id, 'insert', now());
	return new;
END;$$;
 &   DROP FUNCTION public.insert_region();
       public          postgres    false            �            1255    40969    update_country()    FUNCTION       CREATE FUNCTION public.update_country() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	RAISE NOTICE 'vao nha';
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('country', old.country_id, 'update', now());
	return new;
END;$$;
 '   DROP FUNCTION public.update_country();
       public          postgres    false            �            1255    40970    update_region()    FUNCTION       CREATE FUNCTION public.update_region() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
	RAISE NOTICE 'vao nha';
	INSERT INTO public.db_queries_logs(object_name, object_id, object_type, timestamps) 
	VALUES('region', old.region_id, 'update', now());
	return new;
END;$$;
 &   DROP FUNCTION public.update_region();
       public          postgres    false            �            1259    40971    db_categories    TABLE     �  CREATE TABLE public.db_categories (
    parent_id integer DEFAULT 0,
    path character varying(1000) DEFAULT ''::character varying,
    name character varying(200) DEFAULT ''::character varying,
    description text DEFAULT ''::text,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    category_id integer NOT NULL
);
 !   DROP TABLE public.db_categories;
       public         heap    postgres    false            �            1259    41484    db_categories_category_id_seq    SEQUENCE     �   CREATE SEQUENCE public.db_categories_category_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 4   DROP SEQUENCE public.db_categories_category_id_seq;
       public          postgres    false    215            �           0    0    db_categories_category_id_seq    SEQUENCE OWNED BY     _   ALTER SEQUENCE public.db_categories_category_id_seq OWNED BY public.db_categories.category_id;
          public          postgres    false    226            �            1259    40983    db_countries    TABLE     �   CREATE TABLE public.db_countries (
    country_name character varying(40),
    region_id integer,
    country_code character(2),
    country_id smallint NOT NULL
);
     DROP TABLE public.db_countries;
       public         heap    postgres    false            �            1259    40986    db_countries_country_id_seq    SEQUENCE     �   CREATE SEQUENCE public.db_countries_country_id_seq
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 2   DROP SEQUENCE public.db_countries_country_id_seq;
       public          postgres    false    216            �           0    0    db_countries_country_id_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public.db_countries_country_id_seq OWNED BY public.db_countries.country_id;
          public          postgres    false    217            �            1259    40987    db_posts    TABLE     `  CREATE TABLE public.db_posts (
    name character varying(500) DEFAULT ''::character varying NOT NULL,
    description text NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    user_id integer,
    category_id integer,
    post_id integer NOT NULL
);
    DROP TABLE public.db_posts;
       public         heap    postgres    false            �            1259    41462    db_posts_post_id_seq    SEQUENCE     �   CREATE SEQUENCE public.db_posts_post_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.db_posts_post_id_seq;
       public          postgres    false    218            �           0    0    db_posts_post_id_seq    SEQUENCE OWNED BY     M   ALTER SEQUENCE public.db_posts_post_id_seq OWNED BY public.db_posts.post_id;
          public          postgres    false    224            �            1259    40996    db_queries_logs    TABLE     �   CREATE TABLE public.db_queries_logs (
    log_id bigint NOT NULL,
    object_name character varying(20),
    object_type character varying(10),
    timestamps timestamp without time zone,
    object_id bigint
);
 #   DROP TABLE public.db_queries_logs;
       public         heap    postgres    false            �            1259    40999    db_queries_logs_log_id_seq    SEQUENCE     �   ALTER TABLE public.db_queries_logs ALTER COLUMN log_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.db_queries_logs_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    219            �            1259    41000 
   db_regions    TABLE     j   CREATE TABLE public.db_regions (
    region_id integer NOT NULL,
    region_name character varying(25)
);
    DROP TABLE public.db_regions;
       public         heap    postgres    false            �            1259    41003    db_users    TABLE     �  CREATE TABLE public.db_users (
    username character varying(200) DEFAULT ''::character varying NOT NULL,
    email character varying(200) DEFAULT ''::character varying NOT NULL,
    full_name character varying(200) DEFAULT ''::character varying NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    country_id smallint NOT NULL,
    user_id integer NOT NULL
);
    DROP TABLE public.db_users;
       public         heap    postgres    false            �            1259    41473    db_users_user_id_seq    SEQUENCE     �   CREATE SEQUENCE public.db_users_user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.db_users_user_id_seq;
       public          postgres    false    222            �           0    0    db_users_user_id_seq    SEQUENCE OWNED BY     M   ALTER SEQUENCE public.db_users_user_id_seq OWNED BY public.db_users.user_id;
          public          postgres    false    225            �            1259    41014    regions_region_id_seq    SEQUENCE     �   CREATE SEQUENCE public.regions_region_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.regions_region_id_seq;
       public          postgres    false    221            �           0    0    regions_region_id_seq    SEQUENCE OWNED BY     R   ALTER SEQUENCE public.regions_region_id_seq OWNED BY public.db_regions.region_id;
          public          postgres    false    223            �           2604    41485    db_categories category_id    DEFAULT     �   ALTER TABLE ONLY public.db_categories ALTER COLUMN category_id SET DEFAULT nextval('public.db_categories_category_id_seq'::regclass);
 H   ALTER TABLE public.db_categories ALTER COLUMN category_id DROP DEFAULT;
       public          postgres    false    226    215            �           2604    41016    db_countries country_id    DEFAULT     �   ALTER TABLE ONLY public.db_countries ALTER COLUMN country_id SET DEFAULT nextval('public.db_countries_country_id_seq'::regclass);
 F   ALTER TABLE public.db_countries ALTER COLUMN country_id DROP DEFAULT;
       public          postgres    false    217    216            �           2604    41463    db_posts post_id    DEFAULT     t   ALTER TABLE ONLY public.db_posts ALTER COLUMN post_id SET DEFAULT nextval('public.db_posts_post_id_seq'::regclass);
 ?   ALTER TABLE public.db_posts ALTER COLUMN post_id DROP DEFAULT;
       public          postgres    false    224    218            �           2604    41017    db_regions region_id    DEFAULT     y   ALTER TABLE ONLY public.db_regions ALTER COLUMN region_id SET DEFAULT nextval('public.regions_region_id_seq'::regclass);
 C   ALTER TABLE public.db_regions ALTER COLUMN region_id DROP DEFAULT;
       public          postgres    false    223    221            �           2604    41474    db_users user_id    DEFAULT     t   ALTER TABLE ONLY public.db_users ALTER COLUMN user_id SET DEFAULT nextval('public.db_users_user_id_seq'::regclass);
 ?   ALTER TABLE public.db_users ALTER COLUMN user_id DROP DEFAULT;
       public          postgres    false    225    222            o          0    40971    db_categories 
   TABLE DATA           p   COPY public.db_categories (parent_id, path, name, description, created_at, updated_at, category_id) FROM stdin;
    public          postgres    false    215   #e       p          0    40983    db_countries 
   TABLE DATA           Y   COPY public.db_countries (country_name, region_id, country_code, country_id) FROM stdin;
    public          postgres    false    216   �e       r          0    40987    db_posts 
   TABLE DATA           l   COPY public.db_posts (name, description, created_at, updated_at, user_id, category_id, post_id) FROM stdin;
    public          postgres    false    218   �f       s          0    40996    db_queries_logs 
   TABLE DATA           b   COPY public.db_queries_logs (log_id, object_name, object_type, timestamps, object_id) FROM stdin;
    public          postgres    false    219   Cg       u          0    41000 
   db_regions 
   TABLE DATA           <   COPY public.db_regions (region_id, region_name) FROM stdin;
    public          postgres    false    221   �g       v          0    41003    db_users 
   TABLE DATA           k   COPY public.db_users (username, email, full_name, created_at, updated_at, country_id, user_id) FROM stdin;
    public          postgres    false    222   �g       �           0    0    db_categories_category_id_seq    SEQUENCE SET     K   SELECT pg_catalog.setval('public.db_categories_category_id_seq', 3, true);
          public          postgres    false    226            �           0    0    db_countries_country_id_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public.db_countries_country_id_seq', 31, true);
          public          postgres    false    217            �           0    0    db_posts_post_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.db_posts_post_id_seq', 4, true);
          public          postgres    false    224            �           0    0    db_queries_logs_log_id_seq    SEQUENCE SET     I   SELECT pg_catalog.setval('public.db_queries_logs_log_id_seq', 68, true);
          public          postgres    false    220            �           0    0    db_users_user_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.db_users_user_id_seq', 5, true);
          public          postgres    false    225            �           0    0    regions_region_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.regions_region_id_seq', 1, false);
          public          postgres    false    223            �           2606    41021    db_countries country_id 
   CONSTRAINT     ]   ALTER TABLE ONLY public.db_countries
    ADD CONSTRAINT country_id PRIMARY KEY (country_id);
 A   ALTER TABLE ONLY public.db_countries DROP CONSTRAINT country_id;
       public            postgres    false    216            �           2606    41023 $   db_queries_logs db_queries_logs_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.db_queries_logs
    ADD CONSTRAINT db_queries_logs_pkey PRIMARY KEY (log_id);
 N   ALTER TABLE ONLY public.db_queries_logs DROP CONSTRAINT db_queries_logs_pkey;
       public            postgres    false    219            �           2606    41493    db_categories pk_category_id 
   CONSTRAINT     c   ALTER TABLE ONLY public.db_categories
    ADD CONSTRAINT pk_category_id PRIMARY KEY (category_id);
 F   ALTER TABLE ONLY public.db_categories DROP CONSTRAINT pk_category_id;
       public            postgres    false    215            �           2606    41472    db_posts pk_post_id 
   CONSTRAINT     V   ALTER TABLE ONLY public.db_posts
    ADD CONSTRAINT pk_post_id PRIMARY KEY (post_id);
 =   ALTER TABLE ONLY public.db_posts DROP CONSTRAINT pk_post_id;
       public            postgres    false    218            �           2606    41483    db_users pk_user_id 
   CONSTRAINT     V   ALTER TABLE ONLY public.db_users
    ADD CONSTRAINT pk_user_id PRIMARY KEY (user_id);
 =   ALTER TABLE ONLY public.db_users DROP CONSTRAINT pk_user_id;
       public            postgres    false    222            �           2606    41027    db_regions regions_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public.db_regions
    ADD CONSTRAINT regions_pkey PRIMARY KEY (region_id);
 A   ALTER TABLE ONLY public.db_regions DROP CONSTRAINT regions_pkey;
       public            postgres    false    221            �           2606    41029    db_countries unique_code 
   CONSTRAINT     [   ALTER TABLE ONLY public.db_countries
    ADD CONSTRAINT unique_code UNIQUE (country_code);
 B   ALTER TABLE ONLY public.db_countries DROP CONSTRAINT unique_code;
       public            postgres    false    216            �           2606    41033    db_users username 
   CONSTRAINT     P   ALTER TABLE ONLY public.db_users
    ADD CONSTRAINT username UNIQUE (username);
 ;   ALTER TABLE ONLY public.db_users DROP CONSTRAINT username;
       public            postgres    false    222            �           1259    41500    fk_category_id    INDEX     J   CREATE INDEX fk_category_id ON public.db_posts USING btree (category_id);
 "   DROP INDEX public.fk_category_id;
       public            postgres    false    218            �           1259    41506 
   fk_user_id    INDEX     B   CREATE INDEX fk_user_id ON public.db_posts USING btree (user_id);
    DROP INDEX public.fk_user_id;
       public            postgres    false    218            �           1259    41034    fki_fk_category_id    INDEX     N   CREATE INDEX fki_fk_category_id ON public.db_posts USING btree (category_id);
 &   DROP INDEX public.fki_fk_category_id;
       public            postgres    false    218            �           1259    41191    fki_fk_country    INDEX     I   CREATE INDEX fki_fk_country ON public.db_users USING btree (country_id);
 "   DROP INDEX public.fki_fk_country;
       public            postgres    false    222            �           1259    41185    fki_fk_region_id    INDEX     N   CREATE INDEX fki_fk_region_id ON public.db_countries USING btree (region_id);
 $   DROP INDEX public.fki_fk_region_id;
       public            postgres    false    216            �           1259    41197    fki_fk_user_id    INDEX     F   CREATE INDEX fki_fk_user_id ON public.db_posts USING btree (user_id);
 "   DROP INDEX public.fki_fk_user_id;
       public            postgres    false    218            �           1259    41035 	   object_id    INDEX     j   CREATE INDEX object_id ON public.db_queries_logs USING btree (object_id) WITH (deduplicate_items='true');
    DROP INDEX public.object_id;
       public            postgres    false    219            �           1259    41036    object_name_type_time    INDEX     �   CREATE INDEX object_name_type_time ON public.db_queries_logs USING btree (object_name, object_type, timestamps) WITH (deduplicate_items='true');
 )   DROP INDEX public.object_name_type_time;
       public            postgres    false    219    219    219            �           1259    41037    parents    INDEX     f   CREATE INDEX parents ON public.db_categories USING btree (parent_id) WITH (deduplicate_items='true');
    DROP INDEX public.parents;
       public            postgres    false    215            �           2620    41038 %   db_categories trigger_delete_category    TRIGGER     �   CREATE TRIGGER trigger_delete_category BEFORE DELETE ON public.db_categories FOR EACH ROW EXECUTE FUNCTION public.delete_category();
 >   DROP TRIGGER trigger_delete_category ON public.db_categories;
       public          postgres    false    228    215            �           2620    41039 #   db_countries trigger_delete_country    TRIGGER     �   CREATE TRIGGER trigger_delete_country AFTER DELETE ON public.db_countries FOR EACH ROW EXECUTE FUNCTION public.delete_country();
 <   DROP TRIGGER trigger_delete_country ON public.db_countries;
       public          postgres    false    216    235            �           2620    41203    db_posts trigger_delete_post    TRIGGER     w   CREATE TRIGGER trigger_delete_post AFTER DELETE ON public.db_posts FOR EACH ROW EXECUTE FUNCTION public.delete_post();
 5   DROP TRIGGER trigger_delete_post ON public.db_posts;
       public          postgres    false    230    218            �           2620    41320     db_regions trigger_delete_region    TRIGGER     ~   CREATE TRIGGER trigger_delete_region BEFORE DELETE ON public.db_regions FOR EACH ROW EXECUTE FUNCTION public.delete_region();
 9   DROP TRIGGER trigger_delete_region ON public.db_regions;
       public          postgres    false    231    221            �           2620    41042    db_users trigger_delete_user    TRIGGER     x   CREATE TRIGGER trigger_delete_user BEFORE DELETE ON public.db_users FOR EACH ROW EXECUTE FUNCTION public.delete_user();
 5   DROP TRIGGER trigger_delete_user ON public.db_users;
       public          postgres    false    232    222            �           2620    41045     db_regions trigger_update_region    TRIGGER     }   CREATE TRIGGER trigger_update_region AFTER UPDATE ON public.db_regions FOR EACH ROW EXECUTE FUNCTION public.update_region();
 9   DROP TRIGGER trigger_update_region ON public.db_regions;
       public          postgres    false    221    234            �           2620    41046    db_countries update_country    TRIGGER     y   CREATE TRIGGER update_country AFTER UPDATE ON public.db_countries FOR EACH ROW EXECUTE FUNCTION public.update_country();
 4   DROP TRIGGER update_country ON public.db_countries;
       public          postgres    false    233    216            �           2620    41047    db_categories update_timestamp    TRIGGER     �   CREATE TRIGGER update_timestamp BEFORE UPDATE ON public.db_categories FOR EACH ROW EXECUTE FUNCTION public."current_timestamp"();
 7   DROP TRIGGER update_timestamp ON public.db_categories;
       public          postgres    false    227    215            �           2620    41048    db_posts update_timestamp    TRIGGER     }   CREATE TRIGGER update_timestamp BEFORE UPDATE ON public.db_posts FOR EACH ROW EXECUTE FUNCTION public."current_timestamp"();
 2   DROP TRIGGER update_timestamp ON public.db_posts;
       public          postgres    false    218    227            �           2620    41049    db_users update_timestamp    TRIGGER     }   CREATE TRIGGER update_timestamp BEFORE UPDATE ON public.db_users FOR EACH ROW EXECUTE FUNCTION public."current_timestamp"();
 2   DROP TRIGGER update_timestamp ON public.db_users;
       public          postgres    false    222    227            �           2606    41495    db_posts fk_category_id    FK CONSTRAINT     �   ALTER TABLE ONLY public.db_posts
    ADD CONSTRAINT fk_category_id FOREIGN KEY (category_id) REFERENCES public.db_categories(category_id) NOT VALID;
 A   ALTER TABLE ONLY public.db_posts DROP CONSTRAINT fk_category_id;
       public          postgres    false    3259    215    218            �           2606    41186    db_users fk_coutry_id    FK CONSTRAINT     �   ALTER TABLE ONLY public.db_users
    ADD CONSTRAINT fk_coutry_id FOREIGN KEY (country_id) REFERENCES public.db_countries(country_id) ON DELETE CASCADE NOT VALID;
 ?   ALTER TABLE ONLY public.db_users DROP CONSTRAINT fk_coutry_id;
       public          postgres    false    216    222    3261            �           2606    41180    db_countries fk_region_id    FK CONSTRAINT     �   ALTER TABLE ONLY public.db_countries
    ADD CONSTRAINT fk_region_id FOREIGN KEY (region_id) REFERENCES public.db_regions(region_id) ON DELETE CASCADE NOT VALID;
 C   ALTER TABLE ONLY public.db_countries DROP CONSTRAINT fk_region_id;
       public          postgres    false    216    3276    221            �           2606    41501    db_posts fk_user_id    FK CONSTRAINT     �   ALTER TABLE ONLY public.db_posts
    ADD CONSTRAINT fk_user_id FOREIGN KEY (user_id) REFERENCES public.db_users(user_id) NOT VALID;
 =   ALTER TABLE ONLY public.db_posts DROP CONSTRAINT fk_user_id;
       public          postgres    false    3279    222    218            o   �   x�}ν�0���<�-��옛�&
�&LC�؀
J&�&�EiR��}p�G���1"C�<q�_w�;�H�dNE��l%��#ʏ���^p�Y�l.�Mc��KyII]�s�od%궘,i`����j�c1Qа�!�/Xi;�      p   �   x��An�0E�N�	�bH�,BAUJ���K-�A�Qڞ���Y��?z�'�� ���j���ʱSK0}rTV�u��6ܭ�HĔ|i��5)���tz������,�Y�=��>b4l�Ҧ�y���;jxl}ܟ����y:�c�[�Ճ���7T�o}�<W�[���k8� �Q�XV���♎��s?^t�!����VU�BD$m��[��3DLOD�%H�      r   |   x�}�1�0��99E.��~v�gaABB��"�:D��_�k�#���8<���da^��l�mT�;��,i@���~�w�$�E�y�:������#�]���R�����t�(�^g�s/9����C      s   S   x�33�,JM������+N-*�4202�54�5�P00�20�2��323�46�4�2���NI�I-IEWmneTmbfajT���� �!�      u   6   x�3�t�M-�LN,�2�t,�L�2���LI�IUpM,.QH�KQpL)������ ]��      v   �   x���;
1 �������O�����1���^�픰�������r�ݜ���֦_��d�ą��>c��e��̈������~�$� bS�m9��c攝7G��l9���f+&qD�3bp�h�RoI\q     