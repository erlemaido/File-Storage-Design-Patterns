Bakalaureusetöö <b>"Mõned failide salvestamise disainimustrid SQL-andmebaasi kasutavate andmebaasirakenduste jaoks" </b>
raames tehtud rakendused disainilahenduste võrdlemiseks

Erle Maido, 2021

<br>

Näidisrakendus on kuni 10.06.2021 ligipääsetav siit: <a>http://ec2-52-2-103-207.compute-1.amazonaws.com:5000/</a>

<i>Palun näiterakenduses suures mahus tooteid mitte genereerida, sest rakendus on üleval tasuta pilveserveris, mis ei pruugi suurele koormusele vastu pidada. Töö käigus tehtud mõõtmiste läbiviimiseks kasutas autor võimsamaid ja tasulisi servereid.</i>

<br>
<br>

Repositoorium sisaldab nelja erinevat piltide salvestamise disainilahenduse projekti: 


* <b>BLOB</b> - piltide salvestamine andmebaasi suurobjektidena
* <b>Bytea</b> - piltide salvestamine andmebaasi bytea veergu
* <b>Cloud</b> - piltide salvestamine Amazon S3 korvi ja andmebaasis hoitakse viidet pildi asukohale
* <b>Filesystem</b> - piltide salvestamine failisüsteemi ja andmebaasis hoitakse viidet pildi asukohale






<i>Abiks:</i>

Migratsioonid

~~~
dotnet ef migrations add Initial
dotnet ef database update
dotnet ef database drop
~~~

Docker image ehitamiseks

~~~
docker build -t erlemaido/bytea-products .
docker build -t erlemaido/blob-products .
docker build -t erlemaido/cloud-products .
docker build -t erlemaido/filesystem-products .
~~~

Dockeri image pushimiseks 

~~~
docker push erlemaido/bytea-products
docker push erlemaido/blob-products
docker push erlemaido/cloud-products
docker push erlemaido/filesystem-products
~~~
