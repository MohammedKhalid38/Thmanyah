--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5
-- Dumped by pg_dump version 17.5

-- Started on 2025-07-17 22:20:44

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
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
-- TOC entry 218 (class 1259 OID 16396)
-- Name: Locales; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."Locales" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Code" text NOT NULL,
    "Direction" text NOT NULL,
    "IsDefault" boolean NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" uuid,
    "Sort" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ModifiedBy" uuid,
    "ModifiedAt" timestamp with time zone,
    "IsPublished" boolean NOT NULL,
    "PublishedAt" timestamp with time zone,
    "PublishedBy" uuid,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "IsNeedDeleteApprove" boolean NOT NULL,
    "IsActive" boolean NOT NULL,
    "Title" text DEFAULT ''::text NOT NULL
);


ALTER TABLE public."Locales" OWNER TO postgres_user;

--
-- TOC entry 219 (class 1259 OID 16403)
-- Name: Permissions; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."Permissions" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Title" text NOT NULL,
    "IsStatic" boolean NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" uuid,
    "Sort" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ModifiedBy" uuid,
    "ModifiedAt" timestamp with time zone,
    "IsPublished" boolean NOT NULL,
    "PublishedAt" timestamp with time zone,
    "PublishedBy" uuid,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "IsNeedDeleteApprove" boolean NOT NULL,
    "IsActive" boolean NOT NULL
);


ALTER TABLE public."Permissions" OWNER TO postgres_user;

--
-- TOC entry 231 (class 1259 OID 16484)
-- Name: PostCategories; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."PostCategories" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" uuid,
    "Sort" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ModifiedBy" uuid,
    "ModifiedAt" timestamp with time zone,
    "IsPublished" boolean NOT NULL,
    "PublishedAt" timestamp with time zone,
    "PublishedBy" uuid,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "IsNeedDeleteApprove" boolean NOT NULL,
    "IsActive" boolean NOT NULL
);


ALTER TABLE public."PostCategories" OWNER TO postgres_user;

--
-- TOC entry 232 (class 1259 OID 24577)
-- Name: PostCategoryVersions; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."PostCategoryVersions" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" uuid,
    "CreatedAt" timestamp with time zone NOT NULL,
    "IsPublished" boolean NOT NULL,
    "PublishedAt" timestamp with time zone,
    "PublishedBy" uuid,
    "MainVersionId" uuid NOT NULL
);


ALTER TABLE public."PostCategoryVersions" OWNER TO postgres_user;

--
-- TOC entry 233 (class 1259 OID 24584)
-- Name: PostVersions; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."PostVersions" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "Description" text NOT NULL,
    "Link" text NOT NULL,
    "Image" text NOT NULL,
    "Meta" text,
    "Duration" text NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" uuid,
    "CreatedAt" timestamp with time zone NOT NULL,
    "IsPublished" boolean NOT NULL,
    "PublishedAt" timestamp with time zone,
    "PublishedBy" uuid,
    "MainVersionId" uuid NOT NULL
);


ALTER TABLE public."PostVersions" OWNER TO postgres_user;

--
-- TOC entry 229 (class 1259 OID 16457)
-- Name: Posts; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."Posts" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "Description" text NOT NULL,
    "Link" text NOT NULL,
    "Image" text NOT NULL,
    "Meta" text,
    "Duration" text NOT NULL,
    "PostCategoryId" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" uuid,
    "Sort" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ModifiedBy" uuid,
    "ModifiedAt" timestamp with time zone,
    "IsPublished" boolean NOT NULL,
    "PublishedAt" timestamp with time zone,
    "PublishedBy" uuid,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "IsNeedDeleteApprove" boolean NOT NULL,
    "IsActive" boolean NOT NULL,
    "Type" smallint DEFAULT 0 NOT NULL
);


ALTER TABLE public."Posts" OWNER TO postgres_user;

--
-- TOC entry 221 (class 1259 OID 16411)
-- Name: RoleClaims; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."RoleClaims" (
    "Id" integer NOT NULL,
    "RoleId" uuid NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


ALTER TABLE public."RoleClaims" OWNER TO postgres_user;

--
-- TOC entry 220 (class 1259 OID 16410)
-- Name: RoleClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres_user
--

ALTER TABLE public."RoleClaims" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."RoleClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 222 (class 1259 OID 16418)
-- Name: RolePermissions; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."RolePermissions" (
    "Id" uuid NOT NULL,
    "RoleId" uuid NOT NULL,
    "PermissionId" uuid NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" uuid,
    "Sort" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ModifiedBy" uuid,
    "ModifiedAt" timestamp with time zone,
    "IsPublished" boolean NOT NULL,
    "PublishedAt" timestamp with time zone,
    "PublishedBy" uuid,
    "DeletedAt" timestamp with time zone,
    "DeletedBy" uuid,
    "IsNeedDeleteApprove" boolean NOT NULL,
    "IsActive" boolean NOT NULL
);


ALTER TABLE public."RolePermissions" OWNER TO postgres_user;

--
-- TOC entry 223 (class 1259 OID 16423)
-- Name: Roles; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."Roles" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "IsStatic" boolean NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "Name" text,
    "NormalizedName" text,
    "ConcurrencyStamp" text
);


ALTER TABLE public."Roles" OWNER TO postgres_user;

--
-- TOC entry 225 (class 1259 OID 16431)
-- Name: UserClaims; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."UserClaims" (
    "Id" integer NOT NULL,
    "UserId" uuid NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


ALTER TABLE public."UserClaims" OWNER TO postgres_user;

--
-- TOC entry 224 (class 1259 OID 16430)
-- Name: UserClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres_user
--

ALTER TABLE public."UserClaims" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."UserClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 226 (class 1259 OID 16438)
-- Name: UserLogins; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."UserLogins" (
    "Id" uuid NOT NULL,
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text,
    "UserId" uuid NOT NULL
);


ALTER TABLE public."UserLogins" OWNER TO postgres_user;

--
-- TOC entry 227 (class 1259 OID 16445)
-- Name: UserRoles; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."UserRoles" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "RoleId" uuid NOT NULL
);


ALTER TABLE public."UserRoles" OWNER TO postgres_user;

--
-- TOC entry 228 (class 1259 OID 16450)
-- Name: UserTokens; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."UserTokens" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text
);


ALTER TABLE public."UserTokens" OWNER TO postgres_user;

--
-- TOC entry 230 (class 1259 OID 16469)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."Users" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Avatar" uuid,
    "RoleId" uuid,
    "DepartmentId" uuid,
    "IsDeleted" boolean NOT NULL,
    "UserName" text,
    "NormalizedUserName" text,
    "Email" text,
    "NormalizedEmail" text,
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL
);


ALTER TABLE public."Users" OWNER TO postgres_user;

--
-- TOC entry 217 (class 1259 OID 16391)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres_user
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres_user;

--
-- TOC entry 4804 (class 2606 OID 16402)
-- Name: Locales PK_Locales; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."Locales"
    ADD CONSTRAINT "PK_Locales" PRIMARY KEY ("Id");


--
-- TOC entry 4806 (class 2606 OID 16409)
-- Name: Permissions PK_Permissions; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."Permissions"
    ADD CONSTRAINT "PK_Permissions" PRIMARY KEY ("Id");


--
-- TOC entry 4828 (class 2606 OID 16490)
-- Name: PostCategories PK_PostCategories; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."PostCategories"
    ADD CONSTRAINT "PK_PostCategories" PRIMARY KEY ("Id");


--
-- TOC entry 4830 (class 2606 OID 24583)
-- Name: PostCategoryVersions PK_PostCategoryVersions; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."PostCategoryVersions"
    ADD CONSTRAINT "PK_PostCategoryVersions" PRIMARY KEY ("Id");


--
-- TOC entry 4832 (class 2606 OID 24590)
-- Name: PostVersions PK_PostVersions; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."PostVersions"
    ADD CONSTRAINT "PK_PostVersions" PRIMARY KEY ("Id");


--
-- TOC entry 4823 (class 2606 OID 16463)
-- Name: Posts PK_Posts; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."Posts"
    ADD CONSTRAINT "PK_Posts" PRIMARY KEY ("Id");


--
-- TOC entry 4808 (class 2606 OID 16417)
-- Name: RoleClaims PK_RoleClaims; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."RoleClaims"
    ADD CONSTRAINT "PK_RoleClaims" PRIMARY KEY ("Id");


--
-- TOC entry 4810 (class 2606 OID 16422)
-- Name: RolePermissions PK_RolePermissions; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."RolePermissions"
    ADD CONSTRAINT "PK_RolePermissions" PRIMARY KEY ("Id");


--
-- TOC entry 4812 (class 2606 OID 16429)
-- Name: Roles PK_Roles; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."Roles"
    ADD CONSTRAINT "PK_Roles" PRIMARY KEY ("Id");


--
-- TOC entry 4814 (class 2606 OID 16437)
-- Name: UserClaims PK_UserClaims; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."UserClaims"
    ADD CONSTRAINT "PK_UserClaims" PRIMARY KEY ("Id");


--
-- TOC entry 4816 (class 2606 OID 16444)
-- Name: UserLogins PK_UserLogins; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."UserLogins"
    ADD CONSTRAINT "PK_UserLogins" PRIMARY KEY ("Id");


--
-- TOC entry 4818 (class 2606 OID 16449)
-- Name: UserRoles PK_UserRoles; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."UserRoles"
    ADD CONSTRAINT "PK_UserRoles" PRIMARY KEY ("Id");


--
-- TOC entry 4820 (class 2606 OID 16456)
-- Name: UserTokens PK_UserTokens; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."UserTokens"
    ADD CONSTRAINT "PK_UserTokens" PRIMARY KEY ("Id");


--
-- TOC entry 4826 (class 2606 OID 16475)
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- TOC entry 4802 (class 2606 OID 16395)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4821 (class 1259 OID 16481)
-- Name: IX_Posts_PostCategoryId; Type: INDEX; Schema: public; Owner: postgres_user
--

CREATE INDEX "IX_Posts_PostCategoryId" ON public."Posts" USING btree ("PostCategoryId");


--
-- TOC entry 4824 (class 1259 OID 16482)
-- Name: IX_Users_RoleId; Type: INDEX; Schema: public; Owner: postgres_user
--

CREATE INDEX "IX_Users_RoleId" ON public."Users" USING btree ("RoleId");


--
-- TOC entry 4833 (class 2606 OID 16491)
-- Name: Posts FK_Posts_PostCategories_PostCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."Posts"
    ADD CONSTRAINT "FK_Posts_PostCategories_PostCategoryId" FOREIGN KEY ("PostCategoryId") REFERENCES public."PostCategories"("Id") ON DELETE CASCADE;


--
-- TOC entry 4834 (class 2606 OID 16476)
-- Name: Users FK_Users_Roles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: postgres_user
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "FK_Users_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."Roles"("Id");


-- Completed on 2025-07-17 22:20:44

--
-- PostgreSQL database dump complete
--

